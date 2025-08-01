using System.Windows.Input;

namespace ZhooSoft.Controls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty TextChangedCommandProperty =
       BindableProperty.Create(
           nameof(TextChangedCommand),
           typeof(ICommand),
           typeof(CustomEntry),
           null);

        public ICommand TextChangedCommand
        {
            get => (ICommand)GetValue(TextChangedCommandProperty);
            set => SetValue(TextChangedCommandProperty, value);
        }

        public CustomEntry()
        {
            this.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChangedCommand?.CanExecute(e) == true)
            {
                TextChangedCommand.Execute(e);
            }
        }
    }
}
