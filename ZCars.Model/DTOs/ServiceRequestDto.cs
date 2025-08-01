using System.ComponentModel.DataAnnotations;
using ZhooCars.Common;

namespace ZhooCars.Model.DTOs
{
    public class CreateServiceRequestDto
    {
        #region Properties

        [Required]
        public string City { get; set; }

        [Required]
        public string Description { get; set; }

        // Address Fields
        [Required]
        public string DoorNo { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string IssueDetails { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public bool PickupRequired { get; set; }

        [Required]
        public string Pincode { get; set; }

        public string? Priority { get; set; } = "Medium";// Default Priority

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string Street1 { get; set; }

        public string? Street2 { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public string VehicleType { get; set; }

        #endregion
    }

    public class ServiceRequestAssignmentDto
    {
        #region Properties

        public DateTime? AssignedAt { get; set; }

        public int? AssignedProviderId { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? EstimatedCompletionTime { get; set; }

        public int Id { get; set; }

        public int TicketId { get; set; }

        #endregion
    }

    public class ServiceRequestDetailsDto
    {
        #region Properties

        public ServiceRequestPickupDropDto? RequestPickupDropDto { get; set; }

        public ServiceRequestDto? ServiceRequest { get; set; }

        public ServiceRequestAssignmentDto? ServiceRequestAssignment { get; set; }

        #endregion
    }

    public class ServiceRequestDto
    {
        #region Properties

        public string City { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }

        public string DoorNo { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }

        public string IssueDetails { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public bool PickupRequired { get; set; }

        public string Pincode { get; set; }

        public string Priority { get; set; }

        public string ServiceType { get; set; }

        public int Status { get; set; }

        public string Street1 { get; set; }

        public string? Street2 { get; set; }

        public string Subject { get; set; }

        public int TicketId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? UserId { get; set; }

        public string VehicleType { get; set; }

        #endregion
    }

    public class ServiceRequestPickupDropDto
    {
        #region Properties

        public string? DropAddress { get; set; }

        public string? DropBy { get; set; }

        public decimal? DropLatitude { get; set; }

        public decimal? DropLongitude { get; set; }

        public DateTime? DropTime { get; set; }

        public int Id { get; set; }

        public string? PickupAddress { get; set; }

        public string? PickupBy { get; set; }

        public decimal? PickupLatitude { get; set; }

        public decimal? PickupLongitude { get; set; }

        public bool PickupRequired { get; set; }

        public DateTime? PickupTime { get; set; }

        public int TicketId { get; set; }

        #endregion
    }

    public class UpdateStatusDto
    {
        #region Properties

        public ServiceRequestStatus Status { get; set; }

        #endregion
    }

}
