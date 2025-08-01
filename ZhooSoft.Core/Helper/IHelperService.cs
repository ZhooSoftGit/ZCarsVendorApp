namespace ZhooSoft.Core
{
    #region Interfaces

    public interface IHelperService
    {
        #region Methods

        Task ShowAlert(string title, string message, string cancel);

        Task<bool> ShowConfirmation(string title, string message, string accept, string cancel);

        #endregion
    }

    #endregion
}
