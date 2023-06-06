using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Models;
using ClientApplication.Models.GameState;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public sealed class BricketBreakerViewModel : AbstractGameViewModel<BricketBreakerGameState>
{
    private ObservableCollection<Brick> _bricks;

    private DispatcherTimer timer;
    private int timeLeft;

    private const double RectangleSpeed = 10;
    private const double BallSpeed = 3;

    private const double InitialRectangleX = 220;
    private const double InitialBallX = 320;
    private const double InitialBallY = 170;

    private double ballDirectionX = 1;
    private double ballDirectionY = 1;

    private int _bricksBroken = 0;

    private double rectangleX;

    private double ballX;
    private double ballY;

    public BricketBreakerViewModel(INavigationService navigationService) : base(navigationService, GameType.BricketBraker)
    {
        Bricks = new ObservableCollection<Brick>();
        AddBricks();

        RectangleX = InitialRectangleX;
        BallX = InitialBallX;
        BallY = InitialBallY;
    }

    public override void StartGame(TaskDifficulty taskDifficulty, BricketBreakerGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
           
        }
        // Create a new DispatcherTimer with a 10-second interval
        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        // Subscribe to the Tick event
        timer.Tick += Timer_Tick;

        Logging.LogGameEvent("Timer starts: 60 seconds");
        // Start the timer
        timer.Start();

        // Set the time left to 10 seconds
        TimeLeft = 60;
        IsGameRunning = true;

        MoveBall();
    }

    public override void StopGame()
    {
        IsGameRunning = false;
        BricksBroken = 0;
        TimeLeft = 60;
        RectangleX = InitialRectangleX;
        BallX = InitialBallX;
        BallY = InitialBallY;
    }

    public override BricketBreakerGameState GetGameState()
    {
       return new BricketBreakerGameState();
    }

    public double RectangleX
    {
        get { return rectangleX; }
        set
        {
            rectangleX = value;
            OnPropertyChanged(nameof(RectangleX));
        }
    }

    public double BallX
    {
        get { return ballX; }
        set
        {
            ballX = value;
            OnPropertyChanged(nameof(BallX));
        }
    }
 
    public double BallY
    {
        get { return ballY; }
        set
        {
            ballY = value;
            OnPropertyChanged(nameof(BallY));
        }
    }

    public double GetRectangleSpeed()
    {
        return RectangleSpeed;
    }

    public double GetBallSpeed()
    {
        return BallSpeed;
    }

    public int TimeLeft
    {
        get { return timeLeft; }
        set
        {
            timeLeft = value;
            Logging.LogGameEvent($"Text game time left: {timeLeft}");
            OnPropertyChanged(nameof(TimeLeft));
        }
    }

    public int BricksBroken
    {
        get { return _bricksBroken; }
        set
        {
            _bricksBroken = value;
            OnPropertyChanged(nameof(BricksBroken));
        }
    }

    public ObservableCollection<Brick> Bricks
    {
        get => _bricks;
        set
        {
            if (_bricks != value)
            {
                _bricks = value;
                OnPropertyChanged(nameof(Bricks));
            }
        }
    }

    private void AddBricks()
    {
        // Add bricks to the collection with desired positions
        Bricks.Add(new Brick() { X = 10, Y = 20 });
        Bricks.Add(new Brick() { X = 150, Y = 20 });
        Bricks.Add(new Brick() { X = 290, Y = 20 });
        Bricks.Add(new Brick() { X = 430, Y = 20 });
        Bricks.Add(new Brick() { X = 570, Y = 20 });
    }

    private void MoveBall()
    {
        Task.Run(async () =>
        {
            DateTime previousTime = DateTime.Now;
            while (IsGameRunning)
            {
                // Move the ball horizontally
                BallX += ballDirectionX * BallSpeed;//GetBallSpeed();

                // Move the ball vertically
                BallY += ballDirectionY * BallSpeed;//GetBallSpeed();

                CollisionDetection();

                // Ensure the ball stays within the frame
                BallX = Math.Max(0, Math.Min(640 - 20, BallX)); // Assuming game width is 640 and ball width is 20
                BallY = Math.Max(0, Math.Min(340 - 20, BallY)); // Assuming game height is 340 and ball height is 20

                await Task.Delay(16); // Adjust the delay as per your requirement
            }
        });
    }

    private void CollisionDetection()
    {
        // Get the dimensions of the ball and the rectangle/bar
        double ballLeft = BallX;
        double ballRight = BallX + 20; // Assuming ball width is 20
        double ballTop = BallY;
        double ballBottom = BallY + 20; // Assuming ball height is 20

        double barLeft = RectangleX;
        double barRight = RectangleX + 200; // Assuming bar width is 200
        double barTop = 276; // Assuming bar top position
        double barBottom = barTop + 20; // Assuming bar height is 20

        // Check collision with the bar
        if (ballBottom >= barTop && ballTop <= barBottom && ballRight >= barLeft && ballLeft <= barRight)
        {
            // Ball collided with the bar
            ballDirectionY = -ballDirectionY;
        }

        // Check collision with the walls
        if (ballLeft <= 0 || ballRight >= 640) // Assuming game width is 640
        {
            // Ball collided with the left or right wall
            ballDirectionX = -ballDirectionX;
        }

        if (ballTop <= 0)
        {
            // Ball collided with the top wall
            ballDirectionY = -ballDirectionY;
        }

        if (ballBottom >= 308)
        {
            // Ball misses the bar
            RemoveActiveTask();
            timer.Stop();
            MessageBox.Show("You loose!");
        }

        //var (brickHit, isBrickHit) = CheckBrickCollision();
        //if (isBrickHit)
        //{
        //    //foreach (var hitBrick in brickHit)
        //    Bricks.Remove(brickHit);
        //}

    }

    //private (Brick?, bool) CheckBrickCollision()
    //{
    //    //List<Brick> hitBricks = new();

    //    double ballLeft = ballX;
    //    double ballRight = ballX + 10;
    //    double ballTop = ballY;
    //    double ballBottom = ballY + 10;

    //    foreach (var brick in Bricks)
    //    {
    //        double brickLeft = brick.X;
    //        double brickRight = brick.X + 128;
    //        double brickTop = brick.Y;
    //        double brickBottom = brick.Y + 30;

    //        if (ballRight >= brickLeft && ballLeft <= brickRight && ballTop <= brickBottom)
    //        {
    //            // Collision detected
    //            //hitBricks.Add(brick);
    //            return (brick, true);
    //        }
    //    }

    //    // No collision detected
    //    return (null, false);
    //}

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Decrement the time left
        timeLeft--;
        TimeLeft = timeLeft;

        // Check if the timer has run out
        if (TimeLeft == 0)
        {
            // Stop the timer
            timer.Stop();
            RemoveActiveTask();
            MessageBox.Show("You have survived 60 seconds! You win");
        }
    }
}