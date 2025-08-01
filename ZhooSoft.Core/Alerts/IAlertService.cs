namespace ZhooSoft.Core.Alerts
{
    #region Interfaces

    public interface IAlertService
    {
        #region Methods

        Task ShowAlert(string title, string message, string cancel);

        Task<bool> ShowConfirmation(string title, string message, string accept, string cancel);

        #endregion
    }

    #endregion
}
