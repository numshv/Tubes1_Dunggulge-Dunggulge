using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CircularMG : Bot
{
    // The main method starts our bot
    static void Main(string[] args)
    {
        new CircularMG().Start();
    }

    // Constructor, which loads the bot config file
    CircularMG() : base(BotInfo.FromFile("CircularMG.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        BodyColor = Color.FromArgb(0xAD, 0xD8, 0xE6);   // Light Blue
        TurretColor = Color.FromArgb(0x00, 0x00, 0x00); // Black
        RadarColor = Color.FromArgb(0xAD, 0xD8, 0xE6);  // Light Blue
        BulletColor = Color.FromArgb(0x00, 0x00, 0xFF); // Blue
        ScanColor = Color.FromArgb(0xD8, 0xBF, 0xD8);   // Light Purple
        TracksColor = Color.FromArgb(0x80, 0x00, 0x80); // Purple
        GunColor = Color.FromArgb(0x30, 0x19, 0x34);    // Dark Purple
        while (IsRunning)
        {
            // Keep spinning to scan for enemies
            TurnGunLeft(360);
            TargetSpeed = 4;
            TurnRate = 5;
        }
    }

    // Function to determine the power of the shot based on the enemy's energy
    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine($"Detected bot at ({e.X}, {e.Y})");
        //Menentukan kekuatan tembakan sesuai dengan energi bot musuh
        double firePower;
        if (e.Energy > 50) {
            firePower = 1;
        } else {
            firePower = 3;
        }
        Fire(firePower);
    }

    // When the bot is hit by a bullet -> dodge
    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine($"Hit bot at ({e.X}, {e.Y})");
        TargetSpeed = 4;
        Back(100); 
        TurnRight(45); 
        Forward(100); 
    }

    // When hit the wall -> turn back
    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Hit a wall, turning back!");
        Back(50);
        TurnRight(90);
        for (int i = 0; i < 4; i++) { // Keep spinning to confuse the enemies
            TargetSpeed = 5;
            TurnRate = 20;
        }
    }

    // Saat terkena peluru -> menghindar
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        Console.WriteLine("Hit by a bullet! Dodging...");
        // Keep the information of the bullet direction to determine the movement
        double bulletDirection = e.Bullet.Direction;
        if (bulletDirection > -90 && bulletDirection < 90)
        {
            // If the bullet comes from the front, move to the right side
            TurnRight(90);
            Forward(150);
        }
        else
        {
            // If the bullet comes from the back, move to the left side
            TurnLeft(90);
            Forward(150);
        }
    }
}
