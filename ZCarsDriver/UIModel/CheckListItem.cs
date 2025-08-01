﻿using CommunityToolkit.Mvvm.ComponentModel;
using ZCarsDriver.UIHelper;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;

namespace ZCarsDriver.UIModel
{
    public partial class CheckListItem : ObservableObject
    {
        #region Fields

        [ObservableProperty]
        private bool _isCompleted;

        [ObservableProperty]
        private ApprovalStatus? _approvalStatus;

        [ObservableProperty]
        private string _itemName;

        #endregion

        #region Properties

        public string IconName { get; set; }

        public CheckListType CheckListType { get; set; }

        public bool FrontOnly { get; set; }

        public bool IsDocument { get; set; }

        public bool IsForm { get; set; }

        public bool IsNavigation { get; set; }

        public bool IsOptional { get; set; }

        public Page? NavigationPage { get; set; }

        public ActionType CheckListItemStatus { get; set; }

        public DocumentTypes FrontDocType { get; set; }

        public DocumentTypes BackDocType { get; set; }

        #endregion
    }
}
