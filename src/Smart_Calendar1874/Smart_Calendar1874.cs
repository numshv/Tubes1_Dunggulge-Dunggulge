using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Smart_Calendar1874 : Bot
{

    private const double SAFE_DISTANCE = 25; 
    private double targetX, targetY;

    // The main method starts our bot
    static void Main(string[] args)
    {
        new Smart_Calendar1874().Start();
    }

    // Constructor, which loads the bot config file
    Smart_Calendar1874() : base(BotInfo.FromFile("Smart_Calendar1874.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = Color.Red;
        TurretColor = Color.Black;
        RadarColor = Color.Yellow;
        BulletColor = Color.Green;
        ScanColor = Color.Green;


        FindNearestWall();

        while (!OnTargetWallCoord())
        {
            MoveToNearestWall();
        }

        BodyColor = Color.Yellow;
        TurnLeft(90);
        // Spin gun back and forth
        while (IsRunning)
        {
            TurnGunRight(180);
            TurnGunLeft(180);
        }
    }

    private void FindNearestWall()
    {
        double distanceToNorth = ArenaHeight - Y;
        double distanceToSouth = Y;
        double distanceToEast = ArenaWidth - X;
        double distanceToWest = X;

        double minDistance = Math.Min(Math.Min(distanceToNorth, distanceToSouth), Math.Min(distanceToEast, distanceToWest));

        if (minDistance == distanceToNorth)
        {
            targetX = X;
            targetY = ArenaHeight - SAFE_DISTANCE;
        }
        else if (minDistance == distanceToSouth)
        {
            targetX = X;
            targetY = SAFE_DISTANCE;
        }
        else if (minDistance == distanceToEast)
        {
            targetX = ArenaWidth - SAFE_DISTANCE;
            targetY = Y;
        }
        else
        {
            targetX = SAFE_DISTANCE;
            targetY = Y;
        }
    }

    private void MoveToNearestWall()
    {
        double angleToWall = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        double oppositeAngle = (angleToWall + 180) % 360; // Exact opposite direction
        
        TurnTo(oppositeAngle);
        Back(Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y)));
    }

    private void TurnTo(double angle)
    {
        double turnAngle = angle - Direction;
        if (turnAngle > 180) turnAngle -= 360;
        else if (turnAngle < -180) turnAngle += 360;

        TurnLeft(turnAngle);
    }

    private bool OnTargetWallCoord()
    {
        return Math.Abs(X - targetX) < SAFE_DISTANCE / 2 && Math.Abs(Y - targetY) < SAFE_DISTANCE / 2; 
    }


    // We saw another bot -> stop and fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);

        SmartFire(distance);
    }

    // Custom fire method that determines firepower based on distance.
    // distance: The distance to the bot to fire at.
    private void SmartFire(double distance)
    {
        if (distance > 200 || Energy < 15)
            Fire(1);
        else if (distance > 50)
            Fire(2);
        else
            Fire(3);
    }


    private void MoveWhenHit()
    {
        // 1. Move forward 100 units when hit
        Forward(100);

        // 2. If near a corner, turn right 90°
        if (IsNearCorner())
        {
            Console.WriteLine("Near corner! Turning right 90°...");
            TurnRight(90);
        }
    }

    // Helper function to check if bot is near a corner
    private bool IsNearCorner()
    {
        return X < SAFE_DISTANCE || X > (ArenaWidth - SAFE_DISTANCE) ||
            Y < SAFE_DISTANCE || Y > (ArenaHeight - SAFE_DISTANCE);
    }

    // Event: When the bot gets hit by a bullet
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        Console.WriteLine("⚠ Got hit! Moving to avoid more shots...");
        MoveWhenHit();
    }

}
