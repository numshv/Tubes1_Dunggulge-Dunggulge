using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Kotak : Bot
{
    static void Main(string[] args)
    {
        new Kotak().Start();
    }

    // Constructor, which loads the bot config file
    Kotak() : base(BotInfo.FromFile("Kotak.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {

        BodyColor = Color.FromArgb(0xA8, 0xD5, 0x8E);   // Soft Light Green
        TurretColor = Color.FromArgb(0x80, 0xC6, 0x66); // Light Olive Green
        RadarColor = Color.FromArgb(0xB4, 0xD8, 0xA1);  // Pastel Green
        BulletColor = Color.FromArgb(0x6A, 0xA1, 0x6E); // Moss Green
        ScanColor = Color.FromArgb(0xA3, 0xC8, 0x87);   // Pale Green
        TracksColor = Color.FromArgb(0x9B, 0xD4, 0x83); // Mint Green
        GunColor = Color.FromArgb(0x7F, 0xB7, 0x6F);    // Fern Green

        // Di awal, langsung mencari posisi di tengah arena
        FindTargetPosition();
        TargetSpeed = 6;

        while (IsRunning)
        {
            // Jika bot sudah berada di pinggir arena, kembali ke tengah arena
            if (X > ArenaWidth * 0.8 || X < ArenaWidth * 0.2 || Y > ArenaHeight * 0.8 || Y < ArenaHeight * 0.2)
            {
                FindTargetPosition();
            }
            GunTurnRate = 15;
            // Bergerak dengan jalur berbentuk kotak
            Forward(200);
            TurnLeft(90);
        }
    }

    private void FindTargetPosition()
    {
        // Posisi target adalah di tengah arena
        double targetX = 0.5 * ArenaWidth;
        double targetY = 0.5 * ArenaHeight;
        
        // Menghitung arah derajat ke posisi target
        double angleToTarget = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        double turnAngle = findAngle(angleToTarget);
        TurnLeft(turnAngle);
        
        // Bergerak ke posisi target
        double distanceToTarget = Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y));
        Forward(distanceToTarget);

        // Meluruskan badan bot
        TurnLeft(360-Direction);
    }

    // Fungsi untuk menemukan seberapa jauh bot harus berputar untuk menuju arah yang diinginkan
    private double findAngle(double angle)
    {
        double turnAngle = angle - Direction;
        while (turnAngle > 180) turnAngle -= 360;
        while (turnAngle < -180) turnAngle += 360;
        return turnAngle;
    }

    // Fungsi ketika berhasil memindai bot
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Menembak apabila energi cukup
        if (checkEnergy()){
            // Kuatnya tembakan berdasarkan jarak bot musuh dan energi bot ini
            var distance = DistanceTo(e.X, e.Y);
            if (distance > 200 || Energy < 30){
                Fire(1);
            } else if (distance > 100 && distance < 200){
                Fire(2);
            } else if (distance < 50){
                Fire(3);
            }
        }
    }

    // Fungsi ketika bot menabrak tembok
    // Walaupun bot didesain untuk terus berada di tengah, bot bisa menabrak tembok
    // apabila bot harus terus menerus ditabrak/menghindar dari bot lain
    public override void OnHitWall(HitWallEvent e)
    {
        // Bergerak mundur lalu kembali ke tengah arena
        Back(100);
        FindTargetPosition();
    }

    // Fungsi ketika bot menabrak bot lain
    public override void OnHitBot(HitBotEvent e)
    {
        // Bergerak mundur
        Back(30);
    }

    // Fungsi ketika terkena tembakan bot lain
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Bergerak mundur
        Back(30);
    }

    // Fungsi untuk mengecek apakah energi bot masih lebih dari 10
    private bool checkEnergy(){
        return Energy > 10;
    }
}

