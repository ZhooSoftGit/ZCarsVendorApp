namespace ZhooSoft.Core
{
    #region Interfaces

    public interface IViewFor
    {
        #region Properties

        object ViewModel { get; set; }

        #endregion
    }

    public interface IViewFor<T> : IViewFor where T : ViewModelBase, new()
    {
        #region Properties

        new T ViewModel { get; set; }

        #endregion
    }

    #endregion
}
