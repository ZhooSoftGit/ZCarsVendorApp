using ZhooSoft.Auth.ViewModel;
using ZhooSoft.Core;

namespace ZhooSoft.Auth.Views;

public partial class OTPVerificationPage : ContentPage
{
    private List<Entry> _otpEntries;
    public OTPVerificationPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<OTPVerificationViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is OTPVerificationViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}
