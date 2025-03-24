using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Straight : Bot
{

    // Margin dari tembok agar tidak nabrak
    private const double SAFE_DISTANCE = 25;
    // Titik awal tujuan bot 
    private double targetX, targetY;
    private int waitTicks = 0; // Counter to track waiting time
    private bool movingForward;
    
    static void Main(string[] args)
    {
        new Straight().Start();
    }

    // Constructor
    Straight() : base(BotInfo.FromFile("Straight.json")) { }

    public override void Run()
    {
        GunTurnRate = 20; 
        TargetSpeed = 5;
        // Set colors
        BodyColor = Color.FromArgb(0x73, 0x65, 0x4A);
        TurretColor = Color.FromArgb(0xA4, 0x8D, 0x6A);
        RadarColor = Color.FromArgb(0xBD, 0xA5, 0x83);
        BulletColor = Color.FromArgb(0x52, 0x48, 0x39);
        ScanColor = Color.FromArgb(0xE6, 0xCE, 0xAC);

        DecideFirstPos();

        while (!OnTargetCoord())
        {
            MoveToMiddleLeft();
        }

        while (IsRunning)
        {
            
            // GunTurnRate = 20; 
            // TargetSpeed = 5;

            if (movingForward)
            {
                if (X <= ArenaWidth - SAFE_DISTANCE)
                {
                    // Maju hingga X = ArenaWidth - SAFE_DISTANCE
                    Forward(ArenaWidth-(SAFE_DISTANCE*2)); 
                }
                else
                {
                    movingForward = false; // Ubah jadi gerak mundur
                }
            }
            else
            {
                if (X > SAFE_DISTANCE)
                {
                    // Mundur hingga X = SAFE_DISTANCE
                    Back(ArenaWidth-(SAFE_DISTANCE*2)); // Move in steps to allow checking
                }
                else
                {
                    movingForward = true; // Ubah kembali jadi maju
                }
            }
            
        }
        
    }

    private void DecideFirstPos(){
        targetY = ArenaHeight/2;
        if(X <= (ArenaWidth/2)){
            targetX = SAFE_DISTANCE;
            movingForward = true; // Track movement direction
        }
        else{
            targetX = ArenaWidth - SAFE_DISTANCE;
            movingForward = false;
        }
    }

    //Fungsi untuk ke tengah kiri saat mulai awal round
    private void MoveToMiddleLeft()
    {

        double angleToTarget = Math.Atan2(targetY - Y, targetX - X) * 180 / Math.PI;
        TurnTo(angleToTarget);
        Forward(Math.Sqrt((targetX - X) * (targetX - X) + (targetY - Y) * (targetY - Y)));
        TurnTo(0);
    }

    //Fungsi untuk ke belok ke arah tertentu
    private void TurnTo(double angle)
    {
        double turnAngle = angle - Direction;
        if (turnAngle > 180) turnAngle -= 360;
        else if (turnAngle < -180) turnAngle += 360;

        TurnLeft(turnAngle);
    }

    //Fungsi untuk cek apakah sudah di tengah kiri atau belum
    private bool OnTargetCoord()
    {
        return Math.Abs(X - targetX) < SAFE_DISTANCE &&
            Math.Abs(Y - targetY) < SAFE_DISTANCE; 
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

    // Fungsi untuk melakukan gerakan ketika menabrak bot lain
    public override void OnHitBot(HitBotEvent botHitBotEvent)
    {
        Back(60);  
        waitTicks = 30;  
        Console.WriteLine("rammed" + waitTicks);
    }

    // Fungsi untuk melakukan gerakan yang perlu dianalisis per tick
    public override void OnTick(TickEvent tickEvent)
    {
        if (waitTicks > 0) // If waiting period is active
        {
            waitTicks--;
            Console.WriteLine(waitTicks);
            return; // Do nothing while waiting
        }

        if (waitTicks == 0) // Once 10 ticks have passed
        {
            Forward(60); // Move forward to go back to its original route
            waitTicks = -1; 
        }

        if(Energy <= 5){ // If energi <= 5, then stop shooting
            GunTurnRate = 0;   
        }
    }

}