using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class DocumentUploadViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDocumentService _documentService;

        private readonly IVehicleDetailsService _vehicleDetailsService;

        [ObservableProperty] private ImageSource? _backDocImage;

        [ObservableProperty] private ApprovalStatus _backStatus = ApprovalStatus.NotUploaded;

        private FileResult _backFileResult;

        [ObservableProperty] private string _backFileSize;

        [ObservableProperty] private bool _bothSide;

        private CheckListItem _checklistItem;

        [ObservableProperty] private string _documentNo;

        [ObservableProperty] private string _documentText;

        [ObservableProperty] private ImageSource? _frontDocImage;

        [ObservableProperty] private ApprovalStatus _frontStatus = ApprovalStatus.NotUploaded;

        private FileResult _frontFileResult;

        [ObservableProperty] private string _frontFileSize;

        [ObservableProperty] private string _instructions;

        [ObservableProperty] private bool _isBackUploaded;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty] private bool _isFrontUploaded;

        [ObservableProperty] private bool _isSaveEnabled;

        #endregion

        #region Constructors

        public DocumentUploadViewModel()
        {
            UploadFrontCommand = new AsyncRelayCommand<string>(UploadFrontAsync);
            UploadBackCommand = new AsyncRelayCommand<string>(UploadBackAsync);
            RemoveFrontCommand = new Command(RemoveFront);
            RemoveBackCommand = new Command(RemoveBack);
            SaveCommand = new AsyncRelayCommand(SaveDocument);
            OnTextChangedCmd = new RelayCommand(OnTextChange);

            _vehicleDetailsService = ServiceHelper.GetService<IVehicleDetailsService>();
            _documentService = ServiceHelper.GetService<IDocumentService>();
        }

        private void OnTextChange()
        {
            UpdateSaveButtonState();
        }

        #endregion

        #region Properties

        public ICommand RemoveBackCommand { get; }

        public ICommand RemoveFrontCommand { get; }

        public IAsyncRelayCommand SaveCommand { get; }
        public RelayCommand OnTextChangedCmd { get; }
        public IAsyncRelayCommand<string> UploadBackCommand { get; }

        public IAsyncRelayCommand<string> UploadFrontCommand { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            if (NavigationParams != null && NavigationParams.TryGetValue("checklist", out object? value) && value is CheckListItem item)
            {
                _checklistItem = item;
                UpdatePageData();
                BothSide = !item.FrontOnly;
                DocumentText = item.ItemName;
                if (item.FrontDocType == DocumentTypes.RcBookFront)
                {
                    DocumentText = item.ItemName + "(Ex: TN78AS2343)";
                }
                await LoadData();
            }

            await Task.Delay(100);
            IsBusy = false;
        }

        private void LoadBackData(DocumentDto document)
        {
            BackDocImage = AppHelper.GetDocumentSource(document.DocumentUrl);
            IsBackUploaded = true;
            UpdateSaveButtonState();
        }

        private async Task LoadData()
        {
            var docs = AppHelper.GetDocuments();

            var documents = docs?.FirstOrDefault(x => x.DocumentTypeId == _checklistItem.FrontDocType);

            if (documents != null)
            {
                LoadFrontData(documents);
                if (BothSide)
                {
                    var backsideDoc = docs?.FirstOrDefault(x => x.DocumentTypeId == _checklistItem.BackDocType);
                    if (backsideDoc != null)
                    {
                        LoadBackData(backsideDoc);
                    }
                }
            }
            else
            {
                FrontDocImage = BackDocImage = "defaultimage.png";
                IsEditable = true;
            }
        }

        private void LoadFrontData(DocumentDto document)
        {
            FrontDocImage = AppHelper.GetDocumentSource(document.DocumentUrl);
            IsEditable = document.ApprovalStatus == ApprovalStatus.Rejected;
            FrontStatus = BackStatus = document.ApprovalStatus;
            DocumentNo = document.DocumentNo;
            IsFrontUploaded = true;
            UpdateSaveButtonState();
        }

        private void RemoveBack()
        {
            BackDocImage = null;
            IsBackUploaded = false;
            UpdateSaveButtonState();
        }

        private void RemoveFront()
        {
            FrontDocImage = null;
            IsFrontUploaded = false;
            UpdateSaveButtonState();
        }

        private async Task SaveDocument()
        {
            if (!IsSaveEnabled) return;

            IsBusy = true;
            var newdocs = new List<DocumentUploadRequest>();
            if (_frontFileResult != null)
            {
                var docrequest = new DocumentUploadRequest
                {
                    UploadDocumentRequest = new UploadDocumentRequest { ContentType = _frontFileResult.ContentType, DocumentNo = DocumentNo, DocumentType = _checklistItem.FrontDocType, FileName = _frontFileResult.FileName, HttpMethod = DocumentHttpMethod.PUT },
                    DocStream = await UIHelper.UIHelper.GetStreamFromResult(_frontFileResult)
                };

                newdocs.Add(docrequest);
            }

            if (_backFileResult != null)
            {
                var docrequest = new DocumentUploadRequest
                {
                    UploadDocumentRequest = new UploadDocumentRequest { ContentType = _backFileResult.ContentType, DocumentNo = DocumentNo, DocumentType = _checklistItem.BackDocType, FileName = _backFileResult.FileName, HttpMethod = DocumentHttpMethod.PUT },
                    DocStream = await UIHelper.UIHelper.GetStreamFromResult(_backFileResult)
                };

                newdocs.Add(docrequest);
            }

            if (newdocs.Count > 0)
            {
                if (_checklistItem.FrontDocType == DocumentTypes.RcBookFront)
                {
                    var result = await _vehicleDetailsService.RegisterVehicleDetailsAsync(newdocs, DocumentNo);

                    if (result.IsSuccess)
                    {
                        var doc = AppHelper.GetDocuments();
                        var frontDoc = doc.FirstOrDefault(x => x.DocumentTypeId == _checklistItem.FrontDocType);
                        _checklistItem.ApprovalStatus = frontDoc?.ApprovalStatus;
                    }
                }
                else
                {
                    var result = await _documentService.UploadDocuments(newdocs);

                    if (result.IsSuccess)
                    {
                        var doc = AppHelper.GetDocuments();
                        var frontDoc = doc.FirstOrDefault(x => x.DocumentTypeId == _checklistItem.FrontDocType);
                        _checklistItem.ApprovalStatus = frontDoc?.ApprovalStatus;
                    }
                }

            }

            IsBusy = false;
            await _navigationService.PopAsync();
        }

        private void UpdatePageData()
        {
            PageTitleName = _checklistItem.ItemName + " Document";
            Instructions = $"Upload clear pictures of {_checklistItem.ItemName}. \n Documents should be less than 5MB." +
                $"\n Documents should be JPEG, PNG, or PDF format";
        }

        private void UpdateSaveButtonState()
        {
            if (_checklistItem != null && _checklistItem.FrontOnly)
            {
                IsSaveEnabled = IsFrontUploaded && IsEditable && !string.IsNullOrEmpty(DocumentNo);
            }
            else
            {
                IsSaveEnabled = IsFrontUploaded && IsBackUploaded && IsEditable && !string.IsNullOrEmpty(DocumentNo);
            }
        }

        private async Task UploadBackAsync(string obj)
        {
            if (obj == "Camera")
            {
                _backFileResult = await UIHelper.UIHelper.TakePhoto();
                if (_backFileResult != null)
                {
                    BackDocImage = await UIHelper.UIHelper.GetImageSource(_backFileResult) ?? BackDocImage;
                    BackFileSize = UIHelper.UIHelper.GetFileSizeAsync(_backFileResult).ToString() + " kb";
                    IsBackUploaded = true;
                    BackStatus = ApprovalStatus.NotSubmitted;
                    UpdateSaveButtonState();
                }
            }
            else
            {
                _backFileResult = await UIHelper.UIHelper.PickFile();
                if (_backFileResult != null)
                {
                    BackStatus = ApprovalStatus.NotSubmitted;
                    BackDocImage = await UIHelper.UIHelper.GetImageSource(_backFileResult) ?? BackDocImage;
                    BackFileSize = UIHelper.UIHelper.GetFileSizeAsync(_backFileResult).ToString() + " kb";
                    IsBackUploaded = true;
                    UpdateSaveButtonState();
                }
            }
        }

        private async Task UploadFrontAsync(string obj)
        {
            if (obj == "Camera")
            {
                _frontFileResult = await UIHelper.UIHelper.TakePhoto();
                if (_frontFileResult != null)
                {
                    FrontDocImage = await UIHelper.UIHelper.GetImageSource(_frontFileResult) ?? FrontDocImage;
                    FrontFileSize = UIHelper.UIHelper.GetFileSizeAsync(_frontFileResult).ToString() + " kb";
                    IsFrontUploaded = true;
                    FrontStatus = ApprovalStatus.NotSubmitted;
                    UpdateSaveButtonState();
                }
            }
            else
            {
                _frontFileResult = await UIHelper.UIHelper.PickFile();
                if (_frontFileResult != null)
                {
                    FrontDocImage = await UIHelper.UIHelper.GetImageSource(_frontFileResult) ?? FrontDocImage;
                    FrontFileSize = UIHelper.UIHelper.GetFileSizeAsync(_frontFileResult).ToString() + " kb";
                    IsFrontUploaded = true;
                    FrontStatus = ApprovalStatus.NotSubmitted;
                    UpdateSaveButtonState();
                }
            }
        }

        #endregion
    }
}
