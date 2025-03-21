using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Kotak : Bot
{
    int turnCounter;
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Kotak().Start();
    }

    // Constructor, which loads the bot config file
    Kotak() : base(BotInfo.FromFile("Kotak.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {

        BodyColor = Color.FromArgb(0xFF, 0x8C, 0x00);   // Dark Orange
        TurretColor = Color.FromArgb(0xFF, 0xA5, 0x00); // Orange
        RadarColor = Color.FromArgb(0xFF, 0xD7, 0x00);  // Gold
        BulletColor = Color.FromArgb(0xFF, 0x45, 0x00); // Orange-Red
        ScanColor = Color.FromArgb(0xFF, 0xFF, 0x00);   // Bright Yellow 
        TracksColor = Color.FromArgb(0x99, 0x33, 0x00); // Dark   Brownish-Orange
        GunColor = Color.FromArgb(0xCC, 0x55, 0x00);    // Medium Orange

        // Repeat while the bot is running
		turnCounter = 0;

        FindTargetPosition();
        
        // AdjustGunForBodyTurn = false;  // Gun does not turn with body
        // AdjustRadarForBodyTurn = false;  // Radar does not turn with body

        while (IsRunning)
        {
            //TurnRate = 10;
            GunTurnRate = 15;
            
            TargetSpeed = 6;
            Forward(300);
            TurnLeft(90);
            // TurnGunRight(360);
            Forward(300);
            TurnLeft(90);
            // TurnGunRight(360);
            Forward(300);
            TurnLeft(90);
            // TurnGunRight(360);
            Forward(300);
            TurnLeft(90);
            // TurnGunRight(360);

        }
    }

    private void FindTargetPosition()
    {
        double targetY = 0.75 * ArenaHeight;
        double targetX = 0.75 * ArenaWidth;
        
        Forward(Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y)));

        TurnLeft(360-Direction);
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);
        if (distance > 200 || Energy < 25){
            Fire(1);
        } else if (distance > 50){
            Fire(2);
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        FindTargetPosition();
    }

    public override void OnHitBot(HitBotEvent e)
    {
        // var bearing = BearingTo(e.X, e.Y);
        // if (bearing > -10 && bearing < 10)
        // {
        //     var dir = DirectionTo(e.X, e.Y);
        //     TurnGunRight(dir);
        //     TurnRadarRight(dir);
        //     Fire(3);
        // }
        if (e.IsRammed)
        {
            Back(10);
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // double dir = e.Bullet.Direction;
        // TurnRadarRight(dir);
        // Fire(1);
        TargetSpeed = 8;
    }

}

