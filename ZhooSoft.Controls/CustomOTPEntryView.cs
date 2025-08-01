using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZhooSoft.Controls
{
    public class CustomOTPEntryView : HorizontalStackLayout
    {
        private Entry[] _entries;

        public static readonly BindableProperty OtpCodeProperty =
            BindableProperty.Create(
                nameof(OtpCode),
                typeof(string),
                typeof(CustomOTPEntryView),
                string.Empty,
                BindingMode.TwoWay,
                propertyChanged: OnOtpCodeChanged);

        public static readonly BindableProperty LengthProperty =
            BindableProperty.Create(
                nameof(Length),
                typeof(int),
                typeof(CustomOTPEntryView),
                4,
                propertyChanged: OnLengthChanged);

        public static readonly BindableProperty OtpCompletedCommandProperty =
            BindableProperty.Create(
                nameof(OtpCompletedCommand),
                typeof(ICommand),
                typeof(CustomOTPEntryView),
                default);

        public string OtpCode
        {
            get => (string)GetValue(OtpCodeProperty);
            set => SetValue(OtpCodeProperty, value);
        }

        public int Length
        {
            get => (int)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        public ICommand? OtpCompletedCommand
        {
            get => (ICommand?)GetValue(OtpCompletedCommandProperty);
            set => SetValue(OtpCompletedCommandProperty, value);
        }

        public CustomOTPEntryView()
        {
            Spacing = 10;
            HorizontalOptions = LayoutOptions.Center;
            BuildEntries();
        }

        private static void OnOtpCodeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomOTPEntryView)bindable;
            control.UpdateEntriesFromCode(newValue?.ToString());
        }

        private static void OnLengthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomOTPEntryView)bindable;
            control.BuildEntries();
        }

        private void BuildEntries()
        {
            Children.Clear();
            _entries = new Entry[Length];

            for (int i = 0; i < Length; i++)
            {
                var entry = new Entry
                {
                    MaxLength = 1,
                    FontSize = 20,
                    Keyboard = Keyboard.Numeric,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    BackgroundColor = Colors.Transparent,
                    TextColor = Colors.Black,
                };

                int index = i;

                entry.TextChanged += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.NewTextValue))
                    {
                        MoveToNext(index);
                    }

                    UpdateOtpCode();
                };

                entry.Focused += (s, e) => entry.Text = string.Empty;

                _entries[i] = entry;

                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 1,
                    BackgroundColor = Colors.White,
                    WidthRequest = 50,
                    HeightRequest = 50,
                    Padding = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = 6 },
                    Content = entry
                };

                Children.Add(border);
            }
        }

        private void MoveToNext(int index)
        {
            if (index < _entries.Length - 1)
            {
                _entries[index + 1].Focus();
            }
        }

        private void UpdateOtpCode()
        {
            OtpCode = string.Concat(_entries.Select(e => e.Text ?? ""));

            if (OtpCode.Length == Length && OtpCompletedCommand?.CanExecute(OtpCode) == true)
            {
                OtpCompletedCommand.Execute(OtpCode);
            }
        }

        private void UpdateEntriesFromCode(string? code)
        {
            if (string.IsNullOrEmpty(code))
                return;

            for (int i = 0; i < Length && i < code.Length; i++)
            {
                _entries[i].Text = code[i].ToString();
            }
        }

        public void Clear()
        {
            foreach (var entry in _entries)
                entry.Text = string.Empty;

            _entries[0].Focus();
        }
    }
}
