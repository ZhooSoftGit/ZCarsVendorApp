using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

namespace ZhooSoft.Controls
{
    public partial class CustomFontIcon : Border
    {
        private readonly Label _label;

        public CustomFontIcon()
        {
            Stroke = Colors.Gray;
            StrokeThickness = 1;
            StrokeShape = new RoundRectangle { CornerRadius = 6 };
            Padding = 10;
            BackgroundColor = Colors.Transparent;
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            _label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            Content = _label;

            // Tap Handler
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                if (Command?.CanExecute(CommandParameter) ?? false)
                    Command.Execute(CommandParameter);
            };

            GestureRecognizers.Add(tap);
        }

        // ====== TextColor ======
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomFontIcon), Colors.Black, propertyChanged: OnTextColorChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomFontIcon)bindable)._label.TextColor = (Color)newValue;
        }

        // ====== FontSize ======
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(CustomFontIcon), 20.0, propertyChanged: OnFontSizeChanged);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomFontIcon)bindable)._label.FontSize = (double)newValue;
        }

        // ========== Bindable Properties ==========

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomFontIcon), string.Empty, propertyChanged: OnTextChanged);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomFontIcon)bindable)._label.Text = (string)newValue;
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(CustomFontIcon), null, propertyChanged: OnFontFamilyChanged);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomFontIcon)bindable)._label.FontFamily = (string)newValue;
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomFontIcon));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomFontIcon));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}
