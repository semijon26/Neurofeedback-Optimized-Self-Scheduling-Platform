using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaktionslogik für TasktreeOverview.xaml
    /// </summary>
    public partial class TasktreeOverview
    {
        public TasktreeOverview()
        {
            InitializeComponent();
            NavigationService navigationService = Utils.NavigationService.GetInstance();
            TasktreeOverviewViewModel viewModel = new TasktreeOverviewViewModel(navigationService);
            DataContext = viewModel;
        }
        private void MyView_Loaded(object sender, RoutedEventArgs e)
        {
            var firstColumnWidth = firstColumn.ActualWidth;
            scrollRect.Width = firstColumnWidth;
        }
        
        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Eigenschaften der Scrollbar-Übertragung implementieren
            if (ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                // Höhe des MacroGraphen in der linken Spalte
                double leftColumnHeight = macroGraph.ActualHeight;
                // Höhe des sichtbaren Bereichs des Scrollbereichs in der rechten Spalte
                double rightColumnVisibleHeight = ScrollViewer.ViewportHeight;

                // Überprüfen, ob die Höhe des Scrollbereichs größer als 0 ist
                if (rightColumnVisibleHeight > 0)
                {
                    // Verhältnis zwischen der Höhe des sichtbaren Bereichs und der Höhe des Scrollbereichs
                    double heightRatio = leftColumnHeight / ScrollViewer.ExtentHeight;

                    scrollRect.Height = rightColumnVisibleHeight * heightRatio;

                    // Vertikaler Offset im sichtbaren Bereich des Scrollbereichs der rechten Spalte
                    double scrollOffset = ScrollViewer.VerticalOffset;

                    // Position des Rechtecks in der linken Spalte entsprechend umrechnen
                    double rectPosition = (scrollOffset / ScrollViewer.ExtentHeight) * leftColumnHeight;

                    // Überprüfen, ob der Scrollviewer das Ende erreicht hat
                    if (scrollOffset == ScrollViewer.ScrollableHeight)
                    {
                        Canvas.SetTop(scrollRect, leftColumnHeight - scrollRect.Height);
                    }
                    else
                    {
                        Canvas.SetTop(scrollRect, rectPosition);
                    }
                }
            }
        }

        // Tasktree should not receive key events
        private void ScrollViewer_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ScrollViewer_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
