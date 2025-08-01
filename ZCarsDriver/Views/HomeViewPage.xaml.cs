using ZCarsDriver.ViewModel;
using ZCarsDriver.Views.Common;
using ZhooSoft.Core;
using ZhooSoft.Core.NavigationBase;

namespace ZCarsDriver.Views;

public partial class HomeViewPage : BaseContentPage<HomeViewModel>
{
    public HomeViewPage()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        ServiceHelper.GetService<INavigationService>().PushAsync(ServiceHelper.GetService<SliderMenuPage>());
    }
}
