﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Models;
using System.Windows.Media;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;
using System.Drawing;

namespace ClientApplication.Views.Games;

public partial class BricketBreakerGame : UserControl
{
    private readonly BricketBreakerViewModel _viewModel;

    public BricketBreakerGame()
    {
        InitializeComponent();
        _viewModel = new BricketBreakerViewModel(NavigationService.GetInstance());
        DataContext = _viewModel;

        Loaded += UserControl_Loaded;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);
        if (parentWindow != null)
        {
            // PreviewKeyDown on parent window ensures that the event will always arrive here
            parentWindow.PreviewKeyDown += OnPreviewKeyDown;
            parentWindow.PreviewKeyUp += OnPreviewKeyUp;
        }
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Check key input left/right arrow key
        if (e.Key == Key.Left)
        {
            double newRectangleX = _viewModel.RectangleX - _viewModel.GetRectangleSpeed();
            if (newRectangleX >= 0) // Check if the new position is within the left boundary
            {
                _viewModel.RectangleX = newRectangleX; // Update bar postition
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Right)
        {
            double newRectangleX = _viewModel.RectangleX + _viewModel.GetRectangleSpeed();
            if (newRectangleX + 200 <= 640) // Check if the new position is within the right boundary
            {
                _viewModel.RectangleX = newRectangleX; // Update bar postition
            }
            e.Handled = true;
        }
    }

    private void OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
        // Event if key is not pressed anymore
        if (e.Key == Key.Left || e.Key == Key.Right)
        {
            _viewModel.RectangleX = _viewModel.RectangleX; // Leave bar position where it is
            e.Handled = true;
        }
    }
}