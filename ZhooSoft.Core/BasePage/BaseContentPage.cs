namespace ZhooSoft.Core
{
    public abstract class BaseContentPage<T> : ContentPage, IViewFor<T> where T : ViewModelBase, new()
    {
        #region Fields

        private T _viewModel;

        #endregion

        #region Constructors

        protected BaseContentPage()
        {
            ViewModel = ServiceHelper.GetService<T>();
        }

        #endregion

        #region Properties

        public T ViewModel
        {
            get
            {
                return _viewModel;
            }

            set
            {
                _viewModel = value;

                BindingContext = _viewModel;
            }
        }

        object IViewFor.ViewModel
        {
            get { return _viewModel; }
            set
            {
                ViewModel = (T)value;
            }
        }

        #endregion

        #region Methods

        protected override void OnAppearing()
        {
            _viewModel?.OnAppearing();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel?.OnDisappearing();
            base.OnDisappearing();
        }

        #endregion
    }
}
