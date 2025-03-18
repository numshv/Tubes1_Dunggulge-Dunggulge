using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// Corners
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// This bot moves to a corner, then swings the gun back and forth.
// If it gets hit, it moves along the wall to dodge.
// If it dies, it tries a new corner in the next round.
// ------------------------------------------------------------------
public class Smart_Calendar1874 : Bot
{
    int enemies; // Number of enemy bots in the game
    int corner = RandomCorner(); // Which corner we are currently using. Set to random corner
    bool stopWhenSeeEnemy = false; // See GoCorner()
    bool movingAlongBorder = false; // Flag to track if we're currently moving along border
    int currentWall = 0; // Tracks which wall we're currently on (0-3)
    double currentHeading = 0; // Store current heading

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

        // Save # of other bots
        enemies = EnemyCount;

        // Initialize wall based on corner
        currentWall = corner / 90;

        // Move to a corner
        GoCorner();

        // Update current heading
        currentHeading = Direction;

        // Initialize gun turn speed to 3
        int gunIncrement = 3;

        // Spin gun back and forth
        while (IsRunning)
        {
            for (int i = 0; i < 45; i++)
            {
                TurnGunLeft(gunIncrement);
            }
            gunIncrement *= -1;
        }
    }

    // Returns a random corner (0, 90, 180, 270)
    private static int RandomCorner()
    {
        return 90 * new Random().Next(4); // Random number is between 0-3
    }

    // A very inefficient way to get to a corner.
    // Can you do better as an home exercise? :)
    private void GoCorner()
    {
        // We don't want to stop when we're just turning...
        stopWhenSeeEnemy = false;
        // Turn to face the wall towards our desired corner
        TurnLeft(CalcBearing(corner));
        // Ok, now we don't want to crash into any bot in our way...
        stopWhenSeeEnemy = true;
        // Move to that wall
        Forward(5000);
        // Turn to face the corner
        TurnLeft(90);
        // Move to the corner
        Forward(5000);
        // Turn gun to starting point
        TurnGunLeft(90);
        
        // Save our current heading
        currentHeading = Direction;
    }

    // We saw another bot -> stop and fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);

        // Should we stop, or just fire?
        if (stopWhenSeeEnemy)
        {
            // Stop movement
            Stop();
            // Call our custom firing method
            SmartFire(distance);
            // Rescan for another bot
            Rescan();
            // This line will not be reached when scanning another bot.
            // So we did not scan another bot -> resume movement
            Resume();
        }
        else
            SmartFire(distance);
    }

    // When hit by a bullet, we move along the border
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Only respond if we're not already moving along border
        if (!movingAlongBorder)
        {
            // Set flag to prevent recursive calls
            movingAlongBorder = true;
            
            // Execute border movement logic
            MoveAlongBorder();
            
            // Reset flag
            movingAlongBorder = false;
        }
    }

    // Moves the bot along the wall to the middle and then back to corner
    private void MoveAlongBorder()
    {
        // First, temporarily disable stopping when seeing an enemy
        bool oldStopValue = stopWhenSeeEnemy;
        stopWhenSeeEnemy = false;

        // Save the current position for returning later
        double startX = X;
        double startY = Y;
        double startHeading = Direction;

        // Calculate distance to move (about half the wall length)
        double moveDistance = Math.Min(ArenaWidth, ArenaHeight) / 2;

        Console.WriteLine("Moving along border to dodge bullet");
        
        // Stop any existing movement
        Stop();
        
        // Ensure we're facing along the wall
        // Determine which wall we're on based on corner
        switch (currentWall)
        {
            case 0: // Top wall
                TurnLeft(CalcBearing(90)); // Face right
                break;
            case 1: // Right wall
                TurnLeft(CalcBearing(180)); // Face down
                break;
            case 2: // Bottom wall
                TurnLeft(CalcBearing(270)); // Face left
                break;
            case 3: // Left wall
                TurnLeft(CalcBearing(0)); // Face up
                break;
        }

        // Move toward the middle of the wall
        Forward(moveDistance);
        
        // Wait a bit to let any incoming bullets pass
        for (int i = 0; i < 10; i++)
        {
            TurnGunLeft(10); // Keep scanning while waiting
        }
        
        // Go back to the starting position
        // Turn around
        TurnLeft(180);
        
        // Return to position
        Forward(moveDistance);
        
        // Restore original heading
        TurnLeft(CalcBearing(startHeading));
        
        // Restore original stop behavior
        stopWhenSeeEnemy = oldStopValue;
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

    // We died -> figure out if we need to switch to another corner
    public override void OnDeath(DeathEvent e)
    {
        // Well, others should never be 0, but better safe than sorry.
        if (enemies == 0)
            return;

        // If 75% of the bots are still alive when we die, we'll switch corners.
        if (EnemyCount / (double)enemies >= .75)
        {
            corner += 90; // Next corner
            corner %= 360; // Make sure the corner is within 0 - 359
            currentWall = corner / 90; // Update current wall

            Console.WriteLine("I died and did poorly... switching corner to " + corner);
        }
        else
            Console.WriteLine("I died but did well. I will still use corner " + corner);
    }
}