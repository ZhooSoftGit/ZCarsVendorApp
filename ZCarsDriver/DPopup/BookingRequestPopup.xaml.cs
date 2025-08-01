using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;

namespace ZCarsDriver.DPopup;

public partial class BookingRequestPopup : Popup<object>
{

    public BookingRequestPopup()
    {
        InitializeComponent();
        BindingContext = new BookingRequestViewModel();
        if (BindingContext is BookingRequestViewModel vm)
        {
            vm.CurrentPopup = this;
        }
    }

    private async void OnSkipClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is BookingRequestViewModel vm)
        {
            vm.RejectCommand.Execute(e);
            await CloseAsync();
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (BindingContext is BookingRequestViewModel vm)
        {
            vm.AcceptCommand.Execute(e);
            await CloseAsync();
        }
    }
}