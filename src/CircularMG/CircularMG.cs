using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CircularMG : Bot
{
    static void Main(string[] args)
    {
        new CircularMG().Start();
    }

    CircularMG() : base(BotInfo.FromFile("CircularMG.json")) { }

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
            // Terus berputar untuk mendeteksi musuh
            TurnGunLeft(360);
            TargetSpeed = 4;
            TurnRate = 5;
        }
    }

    // Saat mendeteksi bot lain
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

    // Saat terkena bot lain -> mundur
    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine($"Hit bot at ({e.X}, {e.Y})");
        TargetSpeed = 4;
        Back(100); 
        TurnRight(45); // Berbelok untuk menghindar
        Forward(100); // Bergerak maju untuk menjauh
    }

    // Saat menabrak dinding -> putar balik
    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Hit a wall, turning back!");
        Back(50);
        TurnRight(90);
        for (int i = 0; i < 4; i++) {
            TargetSpeed = 5;
            TurnRate = 20;
        }
    }

    // Saat terkena peluru -> menghindar
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        Console.WriteLine("Hit by a bullet! Dodging...");
        // Ambil arah peluru dan bergerak ke arah berlawanan
        double bulletDirection = e.Bullet.Direction;
        if (bulletDirection > -90 && bulletDirection < 90)
        {
            // Jika peluru datang dari depan, bergerak ke samping
            TurnRight(90);
            Forward(150);
        }
        else
        {
            // Jika peluru datang dari belakang, bergerak ke samping
            TurnLeft(90);
            Forward(150);
        }
    }
}
