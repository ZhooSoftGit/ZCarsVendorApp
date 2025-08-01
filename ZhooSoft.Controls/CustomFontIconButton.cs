using System.Windows.Input;

namespace ZhooSoft.Controls
{
    public class CustomFontIconButton : Border
    {
        private readonly Grid _grid;
        private readonly Label _iconLabel;
        private readonly Label _textLabel;

        public CustomFontIconButton()
        {
            _iconLabel = new Label();
            _textLabel = new Label();

            _grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = 8 },
                    new ColumnDefinition { Width = GridLength.Star }
                },
            };

            _grid.Add(_iconLabel, 0, 0);
            _grid.Add(_textLabel, 2, 0);

            Content = _grid;

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (ClickCommand?.CanExecute(CommandParameter) ?? false)
                        ClickCommand.Execute(CommandParameter);
                })
            });
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(CustomFontIconButton), default(string), propertyChanged: OnFontFamilyChanged);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomFontIconButton)bindable;
            control._iconLabel.FontFamily = (string)newValue;
        }

        public static readonly BindableProperty IconTextProperty =
            BindableProperty.Create(nameof(IconText), typeof(string), typeof(CustomFontIconButton), default(string), propertyChanged: OnIconTextChanged);

        public string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }

        private static void OnIconTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomFontIconButton)bindable;
            control._iconLabel.Text = (string)newValue;
        }

        public static readonly BindableProperty ButtonTextProperty =
            BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(CustomFontIconButton), default(string), propertyChanged: OnButtonTextChanged);

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        private static void OnButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomFontIconButton)bindable;
            control._textLabel.Text = (string)newValue;
        }

        public static readonly BindableProperty ClickCommandProperty =
            BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(CustomFontIconButton));

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(CustomFontIconButton), Colors.Black, propertyChanged: OnIconColorChanged);

        public Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomFontIconButton)bindable;
            control._iconLabel.TextColor = (Color)newValue;
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
