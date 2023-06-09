using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class BackTrackView : UserControl
{
    public BackTrackView()
    {
        InitializeComponent();
        var backTrackViewModel = new BackTrackViewModel(NavigationService.GetInstance());
        DataContext = backTrackViewModel;
        IsTypingEnabled(backTrackViewModel);
    }
    
    private void TextBox_PreviewKeyDown(object sender, TextCompositionEventArgs e)
    {
        var success = int.TryParse(e.Text, out var number);
        if (!success)
        {
            e.Handled = true;
        }
        else
        {
            var backTrackViewModel = (BackTrackViewModel)DataContext;
            backTrackViewModel.InvokeNumberInsertedEventHandler(number);
        }
    }

    private void IsTypingEnabled(BackTrackViewModel viewModel)
    {
        viewModel.TypingEnabledEventHandler += (_, isEnabled) =>
        {
            BackTrackTextBox.IsEnabled = isEnabled;
            if(isEnabled) BackTrackTextBox.Focus();
        };
    }
}