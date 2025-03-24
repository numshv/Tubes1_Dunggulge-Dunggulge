using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CircularMG : Bot
{
    // Main method untuk menjalankan bot
    static void Main(string[] args)
    {
        new CircularMG().Start();
    }

    // Konstruktor, yang memuat file konfigurasi bot
    CircularMG() : base(BotInfo.FromFile("CircularMG.json")) { }

    // Melakukan inisialisasi pergerakan bot ketika permainan dimulai
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
            // Selalu bergerak dalam lingkaran untuk mendeteksi musuh agar sulit dideteksi oleh musuh
            TurnGunLeft(360);
            TargetSpeed = 4;
            TurnRate = 5;
        }
    }

    // Fungsi untuk mendeteksi musuh di sekitar bot
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Menentukan tingkat kekuatan tembakan berdasarkan energi musuh
        double firePower;
        if (e.Energy > 50) { // Jika energi musuh di atas 50, gunakan firePower 1
            firePower = 1;
        } else { // Jika energi musuh di bawah 50, gunakan firePower 3  
            firePower = 3;
        }
        Fire(firePower);
    }

    // Jika bot terkena dengan bot lain maka melakukan pergerakan dengan bot lain
    public override void OnHitBot(HitBotEvent e)
    {
        TargetSpeed = 4;
        Back(100); 
        TurnRight(45); 
        Forward(100); 
    }

    // Jika bot terkena tembok maka melakukan pergerakan untuk mundur dan berputar
    public override void OnHitWall(HitWallEvent e)
    {
        Back(50);
        TurnRight(90);
        for (int i = 0; i < 4; i++) { // melanjutkan pola lingkaran untuk membingungkan musuh
            TargetSpeed = 5;
            TurnRate = 20;
        }
    }

    // Saat bot terkena tembakan dari musuh, maka bot akan melakukan pergerakan untuk menghindari tembakan
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Menyimpan informasi arah pergerakan peluru untuk menentukan pergerakan bot selanjutnya
        double bulletDirection = e.Bullet.Direction;
        if (bulletDirection > -90 && bulletDirection < 90)
        {
            // Jika peluru datang dari depan, maka bot akan bergerak ke kanan
            TurnRight(90);
            Forward(150);
        }
        else
        {
            // Jika peluru datang dari belakang, maka bot akan bergerak ke kiri
            TurnLeft(90);
            Forward(150);
        }
    }
}
