using CommunityToolkit.Maui.Views;
using ZCarsDriver.DPopupVM;

namespace ZCarsDriver.DPopup;

public partial class TripOptionPopup : Popup<object>
{
	public TripOptionPopup()
	{
		InitializeComponent();
		BindingContext = new TripOptionViewModel();

		if(BindingContext is TripOptionViewModel vm)
		{
			vm.CurrentPopup = this;
		}
    }
}