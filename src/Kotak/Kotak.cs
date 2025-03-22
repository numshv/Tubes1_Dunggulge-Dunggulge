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


        FindTargetPosition();
        TargetSpeed = 6;

        while (IsRunning)
        {

            if (X > ArenaWidth * 0.8 || X < ArenaWidth * 0.2 || Y > ArenaHeight * 0.8 || Y < ArenaHeight * 0.2)
            {
                FindTargetPosition();
            }
            GunTurnRate = 15;
            Forward(200);
            TurnLeft(90);
        }
    }

    private void FindTargetPosition()
    {
        double targetX = 0.5 * ArenaWidth;
        double targetY = 0.5 * ArenaHeight;
        
        // Calculate angle to the target position
        double angleToTarget = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        double turnAngle = findAngle(angleToTarget);
        TurnLeft(turnAngle);
        
        // Move towards the target position
        double distanceToTarget = Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y));
        Forward(distanceToTarget);

        TurnLeft(360-Direction);
    }

    private double findAngle(double angle)
    {
        double turnAngle = angle - Direction;
        while (turnAngle > 180) turnAngle -= 360;
        while (turnAngle < -180) turnAngle += 360;
        return turnAngle;
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (checkEnergy()){
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

    public override void OnHitWall(HitWallEvent e)
    {
        Back(100);
        FindTargetPosition();
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Back(30);
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        Back(30);
    }

    private bool checkEnergy(){
        return Energy > 10;
    }
}

