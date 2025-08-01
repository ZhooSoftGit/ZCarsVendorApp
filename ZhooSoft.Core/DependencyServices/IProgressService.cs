namespace ZhooSoft.Core
{
    #region Interfaces

    public interface IProgressService
    {
        #region Methods

        void HideProgress();

        void ShowAlertWithTextField(string message, string HintText, Action<bool, string> Selection);

        void ShowProgress(string message);

        void ShowToast(string message);

        #endregion
    }

    #endregion
}
