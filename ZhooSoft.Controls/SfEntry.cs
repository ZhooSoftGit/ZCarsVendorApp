using Syncfusion.Maui.Toolkit.TextInputLayout;
using System.Windows.Input;

namespace ZhooSoft.Controls
{
    public class SfEntry : SfTextInputLayout
    {
        private readonly Entry _entry;

        public static readonly BindableProperty SfTextProperty =
            BindableProperty.Create(
                nameof(SfText),
                typeof(string),
                typeof(SfEntry),
                string.Empty,
                BindingMode.TwoWay,
                propertyChanged: OnTextChanged);

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(
                nameof(Keyboard),
                typeof(Keyboard),
                typeof(SfEntry),
                Keyboard.Default);

        public static readonly BindableProperty TextChangedCommandProperty =
            BindableProperty.Create(
                nameof(TextChangedCommand),
                typeof(ICommand),
                typeof(SfEntry));

        public static readonly BindableProperty ClearIconViewProperty =
            BindableProperty.Create(
                nameof(ClearIconView),
                typeof(View),
                typeof(SfEntry),
                default(View),
                propertyChanged: OnClearIconViewChanged);

        public string SfText
        {
            get => (string)GetValue(SfTextProperty);
            set => SetValue(SfTextProperty, value);
        }

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public ICommand? TextChangedCommand
        {
            get => (ICommand?)GetValue(TextChangedCommandProperty);
            set => SetValue(TextChangedCommandProperty, value);
        }

        public View? ClearIconView
        {
            get => (View?)GetValue(ClearIconViewProperty);
            set => SetValue(ClearIconViewProperty, value);
        }

        public SfEntry()
        {
            _entry = new Entry
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };

            _entry.SetBinding(Entry.TextProperty, new Binding(nameof(SfText), source: this, mode: BindingMode.TwoWay));
            _entry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this));
            _entry.TextChanged += OnEntryTextChanged;

            Content = _entry;

            ApplyCustomStyle();
        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfEntry sfEntry)
            {
                sfEntry.UpdateClearIconVisibility(newValue as string);
            }
        }

        private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateClearIconVisibility(e.NewTextValue);
            TextChangedCommand?.Execute(e);
        }

        private static void OnClearIconViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfEntry sfEntry && newValue is View icon)
            {
                icon.IsVisible = !string.IsNullOrEmpty(sfEntry.SfText);

                icon.GestureRecognizers.Clear();
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (_, _) => sfEntry.ClearText();
                icon.GestureRecognizers.Add(tapGesture);

                sfEntry.TrailingView = icon;
            }
        }

        private void ClearText()
        {
            SfText = string.Empty;
            UpdateClearIconVisibility(string.Empty);
            TextChangedCommand?.Execute(new TextChangedEventArgs("", ""));
        }

        private void UpdateClearIconVisibility(string? text)
        {
            if (ClearIconView != null)
                ClearIconView.IsVisible = !string.IsNullOrEmpty(text);
        }

        private void ApplyCustomStyle()
        {
            Background = Colors.White;
            ContainerBackground = Colors.White;
            ContainerType = ContainerType.Outlined;
        }
    }
}
