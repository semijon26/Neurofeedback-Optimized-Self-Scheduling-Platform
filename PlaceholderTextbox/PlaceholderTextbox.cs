using System.Windows;
using System.Windows.Controls;

namespace PlaceholderTextbox
{
    public class PlaceholderTextbox : TextBox
    {


        public static readonly DependencyProperty PlaceholderProperty = 
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextbox), 
                new PropertyMetadata(string.Empty));
        
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceholderTextbox), new PropertyMetadata(true));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyPropertyKey, value); }
        }

        static PlaceholderTextbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextbox), new FrameworkPropertyMetadata(typeof(PlaceholderTextbox)));
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            IsEmpty = string.IsNullOrEmpty(Text);
            base.OnTextChanged(e);
        }
    }
}
