using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class BricketBreakerGame : UserControl
{
    private BricketBreakerViewModel bricketBreakerViewModel;

    public BricketBreakerGame()
    {
        InitializeComponent();
        bricketBreakerViewModel = new BricketBreakerViewModel(NavigationService.GetInstance());
        DataContext = bricketBreakerViewModel;

        PreviewKeyDown += OnPreviewKeyDown;
        PreviewKeyUp += OnPreviewKeyUp;

        Loaded += (sender, e) => Keyboard.Focus(this);
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            double newRectangleX = bricketBreakerViewModel.RectangleX - 1 * bricketBreakerViewModel.GetRectangleSpeed();
            if (newRectangleX >= 0) // Check if the new position is within the left boundary
            {
                bricketBreakerViewModel.RectangleX = newRectangleX;
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Right)
        {
            double newRectangleX = bricketBreakerViewModel.RectangleX + 1 * bricketBreakerViewModel.GetRectangleSpeed();
            if (newRectangleX + 200 <= 640) // Check if the new position is within the right boundary
            {
                bricketBreakerViewModel.RectangleX = newRectangleX;
            }
            e.Handled = true;
        }
    }

    private void OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left || e.Key == Key.Right)
        {
            bricketBreakerViewModel.RectangleX = bricketBreakerViewModel.RectangleX;
            e.Handled = true;
        }
    }
}