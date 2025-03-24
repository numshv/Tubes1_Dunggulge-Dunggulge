using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Smart_Calendar1874 : Bot
{
    // Margin dari tembok agar tidak nabrak
    private const double SAFE_DISTANCE = 25; 
    // Titik awal tujuan bot 
    private double targetX, targetY;

    private double minDistance; 
    

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
        BodyColor = Color.FromArgb(0x4A, 0x24, 0x80);
        TurretColor = Color.FromArgb(0xC5, 0x3A, 0x9D);
        RadarColor = Color.FromArgb(0xFF, 0x8E, 0x80);
        BulletColor = Color.FromArgb(0x05, 0x1F, 0x39);
        ScanColor = Color.FromArgb(0xFF, 0x8E, 0x80);

        // Cari koordinat yang diingingkan dulu
        FindNearestWall();
        while (!OnTargetWallCoord())
        {
            // Jalan ke koordinat yang diinginkan
            MoveToNearestWall();
        }

        // Posisikan badan
        TurnLeft(90);

        while (IsRunning)
        {
            // Terus muter dan nembak kalau dapet bot
            TurnGunRight(360);
        }
        
    }

    // Fungsi untuk mencari koordinat dinding terdekat dari spawn point
    private void FindNearestWall()
    {
        double distanceToNorth = ArenaHeight - Y;
        double distanceToSouth = Y;
        double distanceToEast = ArenaWidth - X;
        double distanceToWest = X;

        //mindistance = shortest distance to any of the 4 walls
        minDistance = Math.Min(Math.Min(distanceToNorth, distanceToSouth), Math.Min(distanceToEast, distanceToWest));

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

    // Fungsi untuk menggerakkan bot ke targetX, targetY
    private void MoveToNearestWall()
    {
        double angleToWall = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        double oppositeAngle = (angleToWall + 180) % 360; // Exact opposite direction
        
        TurnTo(oppositeAngle);
        Back(Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y)));
    }

    //Fungsi untuk ke belok ke arah tertentu
    private void TurnTo(double angle)
    {
        double turnAngle = angle - Direction;
        if (turnAngle > 180) turnAngle -= 360;
        else if (turnAngle < -180) turnAngle += 360;

        TurnLeft(turnAngle);
    }

    //Fungsi untuk cek apakah sudah di targetX, targetY atau belum
    private bool OnTargetWallCoord()
    {
        return Math.Abs(X - targetX) < SAFE_DISTANCE / 2 && Math.Abs(Y - targetY) < SAFE_DISTANCE / 2; 
    }

    //Event based function yang akan menembak ketika memindai bot
    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);

        DorDor(distance);
    }

    //Fungsi untuk mengatur kekuatan tembakan sesuai jarak bot target
    private void DorDor(double distance)
    {
        if (distance > 200 || Energy < 15)
            Fire(1);
        else if (distance > 50)
            Fire(2);
        else
            Fire(3);
    }

    // Fungsi untuk bergerak ketika bot terkena peluru
    private void MoveWhenHit()
    {
        
        // Check distance based on the bot's current facing direction
        Console.WriteLine(Direction);
        if (Direction == 180) 
        {
            minDistance = X - SAFE_DISTANCE; // Distance to left wall
        }
        else if (Direction == 270) 
        {
            minDistance = Y - SAFE_DISTANCE; // Distance to top wall
        }
        else if (Direction == 0) 
        {
            minDistance = ArenaWidth - SAFE_DISTANCE - X; // Distance to right wall
        }
        else if (Direction == 90) 
        {
            minDistance = ArenaHeight - SAFE_DISTANCE - Y; // Distance to bottom wall
        }

         Console.WriteLine(minDistance);

        // Adjust movement if near a wall
        int moveDistance = (minDistance < 100 + SAFE_DISTANCE) 
            ? (int)(minDistance - SAFE_DISTANCE) 
            : 100;

        

        // If near a corner, turn 90° before moving
        if (minDistance < (SAFE_DISTANCE + 5))
        {
            Console.WriteLine($"rotate, minDistance: {minDistance}");
            TurnRight(90);
            Forward(110);
        }

        // Move forward by adjusted distance (ensure it's not negative)
        Forward(Math.Max(moveDistance, 0));
    }

    // Fungsi event based yang akan menjalankan movewhenhit() ketika tertembaks
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        MoveWhenHit();
    }
        

}