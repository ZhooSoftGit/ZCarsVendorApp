using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ZhooCars.Common;
using ZhooCars.Model.Request;

namespace ZhooCars.Model.DTOs
{
    public class InsuranceUpdateDto
    {
        #region Properties

        public DateTime InsuranceExpiryDate { get; set; }

        public string InsuranceNumber { get; set; } = string.Empty;

        #endregion
    }

    public class RegisterDriverOrVehicleDto
    {
        #region Properties

        public required string VehicleNumber { get; set; }

        public required List<UploadDocumentRequest> UploadDocumentRequests { get; set; }

        #endregion
    }

    public class VehicleDto : INotifyPropertyChanged
    {
        #region Properties

        public ApprovalStatus ApprovalStatus { get; set; }

        public string? Color { get; set; }

        public string? FuelType { get; set; }

        public DateTime? InsuranceExpiryDate { get; set; }

        public string? InsuranceNumber { get; set; }

        public string? RCNumber { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public int? SeatingCapacity { get; set; }

        public bool? ShowRegistrationOption { get; set; }

        public VehicleStatus? Status { get; set; }

        public int? VehicleId { get; set; }

        public string? VehicleMake { get; set; }

        public string? VehicleModel { get; set; }

        public required string VehicleRegistrationNumber { get; set; }

        public int? VehicleType { get; set; }

        public int? VehicleYear { get; set; }


        private bool? _isSelected { get; set; }
        [JsonIgnore]
        public bool? IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
