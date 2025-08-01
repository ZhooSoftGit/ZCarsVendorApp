using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZCarsDriver.Services;
using Button = Android.Widget.Button;
using Color = Android.Graphics.Color;
using View = Android.Views.View;

namespace ZCarsDriver.Platforms
{
    public class OverlayService : IOverlayService
    {
        private IWindowManager _windowManager;
        private View _overlayView;

        public void ShowOverlay()
        {
            var context = Platform.CurrentActivity ?? Android.App.Application.Context;
            if (_overlayView != null)
                return;

            _windowManager = context.GetSystemService(Context.WindowService)
                                    .JavaCast<IWindowManager>();

            var layoutParams = new WindowManagerLayoutParams(
                WindowManagerLayoutParams.WrapContent,
                WindowManagerLayoutParams.WrapContent,
                Build.VERSION.SdkInt >= BuildVersionCodes.O
                    ? WindowManagerTypes.ApplicationOverlay
                    : WindowManagerTypes.Phone,
                WindowManagerFlags.NotFocusable,
                Format.Translucent);

            layoutParams.Gravity = GravityFlags.Top | GravityFlags.End;
            layoutParams.X = 20;
            layoutParams.Y = 200;

            // Create overlay layout
            LinearLayout overlayLayout = new LinearLayout(context)
            {
                Orientation = Orientation.Horizontal
            };
            overlayLayout.SetBackgroundColor(Color.Argb(200, 0, 0, 0));
            overlayLayout.SetPadding(30, 20, 30, 20);

            // Text
            TextView textView = new(context)
            {
                Text = "Ride in progress...",
                TextSize = 16
            };
            textView.SetTextColor(Color.White);

            //// Close button
            //Button closeButton = new Button(context)
            //{
            //    Text = "X",
            //    TextSize = 14
            //};
            //closeButton.Click += (s, e) => RemoveOverlay();

            overlayLayout.SetOnClickListener(new SimpleClickListener(() =>
            {
                Intent intent = new Intent(context, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop);
                context.StartActivity(intent);
                RemoveOverlay();
            }));

            overlayLayout.AddView(textView);
            // overlayLayout.AddView(closeButton);

            _overlayView = overlayLayout;
            _windowManager.AddView(_overlayView, layoutParams);
        }

        public void RemoveOverlay()
        {
            if (_overlayView != null)
            {
                _windowManager.RemoveView(_overlayView);
                _overlayView = null;
            }
        }
    }


    public class SimpleClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Action _onClick;

        public SimpleClickListener(Action onClick)
        {
            _onClick = onClick;
        }

        public void OnClick(View v)
        {
            _onClick?.Invoke();
        }
    }
}
