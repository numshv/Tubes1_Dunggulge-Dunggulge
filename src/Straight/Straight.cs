using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Straight : Bot
{

    private const double SAFE_DISTANCE = 25; 
    private double targetX, targetY;
    

    // The main method starts our bot
    static void Main(string[] args)
    {
        new Straight().Start();
    }

    // Constructor, which loads the bot config file
    Straight() : base(BotInfo.FromFile("Straight.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        targetX = SAFE_DISTANCE;
        targetY = ArenaHeight/2;
        // Set colors
        BodyColor = Color.Red;
        TurretColor = Color.Black;
        RadarColor = Color.Yellow;
        BulletColor = Color.Green;
        ScanColor = Color.Green;

        bool movingForward = true; // Track movement direction

        while (!OnTargetCoord())
        {
            MoveToMiddleLeft();
        }

        int i=0;

        while (IsRunning)
        {
            GunTurnRate = 20;
            TargetSpeed = 5;
            if (movingForward)
            {
                // Move forward until reaching X = ArenaWidth - SAFE_DISTANCE
                if (X <= ArenaWidth - SAFE_DISTANCE)
                {
                    i++;
                    if(i%2==0){
                        BodyColor = Color.Yellow;
                    }
                    else{
                        BodyColor = Color.Yellow;
                    }
                    Forward(ArenaWidth-(SAFE_DISTANCE*2)); // Move in steps to allow checking
                }
                else
                {
                    movingForward = false; // Switch to moving backward
                }
            }
            else
            {
                // Move backward until reaching X = SAFE_DISTANCE
                if (X > SAFE_DISTANCE)
                {
                    i++;
                    if(i%2==0){
                        BodyColor = Color.Yellow;
                    }
                    else{
                        BodyColor = Color.Yellow;
                    }
                    Back(ArenaWidth-(SAFE_DISTANCE*2)); // Move in steps to allow checking
                }
                else
                {
                    movingForward = true; // Switch back to moving forward
                }
            }

            // Continuously rotate gun 360° while moving
            
        }
        
    }


    private void MoveToMiddleLeft()
    {
        // Calculate the angle to move towards (25, ArenaHeight / 2)
        double angleToTarget = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        
        // Turn towards the target position
        TurnTo(angleToTarget);

        // Move to (25, ArenaHeight / 2)
        Forward(Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y)));

        // Once there, turn left (which is 0°)
        TurnTo(0);
    }

    private void TurnTo(double angle)
    {
        double turnAngle = angle - Direction;
        if (turnAngle > 180) turnAngle -= 360;
        else if (turnAngle < -180) turnAngle += 360;

        TurnLeft(turnAngle);
    }

    private bool OnTargetCoord()
    {
        return Math.Abs(X - targetX) < SAFE_DISTANCE &&
            Math.Abs(Y - targetY) < SAFE_DISTANCE; 
    }


    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);

        SmartFire(distance);
    }

    private void SmartFire(double distance)
    {
        if (distance > 200 || Energy < 15)
            Fire(1);
        else if (distance > 50)
            Fire(2);
        else
            Fire(3);
    }

}