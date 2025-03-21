using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Satu : Bot
{
    int turnCounter;
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Satu().Start();
    }

    // Constructor, which loads the bot config file
    Satu() : base(BotInfo.FromFile("Satu.json")) { }

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
      
// YANG KURAAANAG BGT ADALAH DODGE BULLET. NAIKIN ITU, DAN BOT INI BAKAL JAUH LEBIH BAGUS


        // Repeat while the bot is running
		turnCounter = 0;
        // AdjustGunForBodyTurn = false;  // Gun does not turn with body
        // AdjustRadarForBodyTurn = false;  // Radar does not turn with body

        while (IsRunning)
        {
            TurnRate = 10;
            GunTurnRate = 15;
            if (turnCounter % 100 == 0) {
                GunTurnRate = 15;
                // TurnGunRight(1);
                // TurnRadarRight(1);
                // SetForward(100);
				TargetSpeed = -5;
			}
            if (turnCounter % 100 == 50) {
                GunTurnRate = -15;
                // TurnGunRight(-1);
                // TurnRadarRight(-1);
                // SetForward(100);
                TargetSpeed = 5;
			}

			turnCounter++;
			Go();

            // Forward(100);
            // TurnLeft(90);
            // // TurnGunRight(360);
            // Forward(100);
            // TurnLeft(90);
            // // TurnGunRight(360);
            // Forward(100);
            // TurnLeft(90);
            // // TurnGunRight(360);
            // Forward(100);
            // TurnLeft(90);
            // // TurnGunRight(360);

        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing < -90 && bearing > 90)
        {
            Back(100);
        } else {
            Fire(1);
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        TurnRight(180);
        Forward(200);
        TargetSpeed = 7;
    }

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -10 && bearing < 10)
        {
            var dir = DirectionTo(e.X, e.Y);
            TurnGunRight(dir);
            TurnRadarRight(dir);
            Fire(3);
        }
        if (e.IsRammed)
        {
            TurnRight(10);
            Back(10);
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // double dir = e.Bullet.Direction;
        // TurnRadarRight(dir);
        // Fire(1);
        TurnRight(10);
    }

}

