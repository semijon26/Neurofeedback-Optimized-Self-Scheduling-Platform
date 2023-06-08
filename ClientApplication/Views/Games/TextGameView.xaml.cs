using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class TextGameView : UserControl
{
    public TextGameView()
    {
        InitializeComponent();
        TextGameViewModel textGameViewModel = new TextGameViewModel(NavigationService.GetInstance());
        DataContext = textGameViewModel;
    }
    
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var viewModel = DataContext as TextGameViewModel;

        viewModel.InputText = textBox.Text;

        if (viewModel.IsWordFullyWritten)
        {
            textBox.Text = string.Empty;
        }
        if (!viewModel.IsGameRunning)
        {
            textBox.Text = string.Empty;
        }
    }

    private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // Get the pressed key
        Key key = e.Key;

        // Allow keyboard letters, shift, space, and punctuation
        if (!IsAllowedKey(key, Keyboard.Modifiers))
        {
            // Suppress the key event for disallowed keys
            e.Handled = true;
        }
    }

    private bool IsAllowedKey(Key key, ModifierKeys modifiers)
    {
        // Check if the key is a keyboard letter (A-Z)
        if (key >= Key.A && key <= Key.Z)
        {
            return true;
        }

        // Check if the key is a digit (0-9)
        if (key >= Key.D0 && key <= Key.D9 && modifiers != ModifierKeys.Shift)
        {
            return false;
        }

        // Check if the key is a numpad digit (0-9)
        if (key >= Key.NumPad0 && key <= Key.NumPad9)
        {
            return false;
        }

        // Check if the key is a space or punctuation
        if (key == Key.Space || IsPunctuationKey(key) || key == Key.Back)
        {
            return true;
        }

        // Check if the key is a modifier key (Shift)
        if (key == Key.LeftShift || key == Key.RightShift)
        {
            return true;
        }

        // Check if the key with modifiers produces an allowed character
        if (modifiers.HasFlag(ModifierKeys.Shift))
        {
            if (key == Key.D1 || key == Key.D2)
            {
                return true;
            }
        }

        return false;
    }


    private bool IsAllowedCharacter(char character)
    {
        // Define a list of allowed characters
        string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        allowedCharacters += " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        return allowedCharacters.Contains(character);
    }

    private bool IsPunctuationKey(Key key)
    {
        // Define the punctuation keys
        List<Key> punctuationKeys = new List<Key>
        {
            Key.OemTilde,
            Key.OemMinus,
            Key.OemPlus,
            Key.OemOpenBrackets,
            Key.OemCloseBrackets,
            Key.OemSemicolon,
            Key.OemQuotes,
            Key.OemComma,
            Key.OemPeriod,
            Key.OemQuestion,
            Key.OemBackslash
        };

        return punctuationKeys.Contains(key);
    }
}