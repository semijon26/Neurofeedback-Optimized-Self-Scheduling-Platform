using System;
using System.Threading.Tasks;
using System.Windows;
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
    }
    
    private void TextBox_PreviewKeyDown(object sender, TextCompositionEventArgs e)
    {
        // Überprüfen, ob die eingegebene Eingabe eine Zahl ist
        if (!IsNumericInput(e.Text))
        {
            // Wenn es sich nicht um eine Zahl handelt, die Eingabe ignorieren
            e.Handled = true;
        }
    }
    
    private bool IsNumericInput(string input)
    {
        // Überprüfen, ob die Eingabe eine Zahl ist
        return int.TryParse(input, out _);
    }
    
    private void textBox_Loaded(object sender, RoutedEventArgs e)
    {
        // Den Fokus auf die TextBox setzen
        BackTrackTextBox.Focus();
        // Die Eingabe deaktivieren
        BackTrackTextBox.IsEnabled = false;
        Task.Run(() =>
        {
            // Eine Wartezeit von 5 Sekunden einfügen
            Task.Delay(TimeSpan.FromSeconds(5));
            // Die Eingabe wieder aktivieren
            BackTrackTextBox.IsEnabled = true;
        });

    }
}