namespace ZhooCars.Common
{
    #region Enums

    public enum MobileModule
    {
        Driver,
        Vendor,
        ActingDriver,
        ServiceProvider,
        SparParts,
        BuyAndSell,
        RequestForm,
        CustomerSupport
    }

    public enum ApprovalStatus
    {
        //Only for UI
        NotUploaded = -2,
        NotSubmitted = 0,              // Uploaded, not submitted

        //DB and API
        Submitted = 1,             // Submitted for admin review
        UnderReview = 2,           // Admin reviewing
        Approved = 3,              // Verified and accepted
        Rejected = 4,              // Rejected; user must reupload
        Resubmitted = 5,           // Reupload after rejection
        Expired = 6               // (Optional) for time-bound docs
    }

    public enum UserRoles
    {
        User = 1,
        Driver = 2,
        Admin = 3,
        Vendor = 4,
        ServiceProvider = 5,
        SparPartsDistributor = 6,
        Owner = 7,
        BuyAndSell = 8
    }

    public enum RegsitrationType
    {
        BasicDetails,
        DriverApplication,
        VendorApplication,
        VechicleDetails,
        ServiceProviderApplication,
        SparPartsApplication,
        BuyForm,
        SellingForm
    }

    public enum DocumentTypes
    {
        AadharFront = 1,
        AadharBack = 2,
        Pan = 3,
        RcBookFront = 4,
        RcBookBack = 5,
        LicenseFront = 6,
        LicenseBack = 7,
        Insurance = 8,
        Gstin = 9,
        BankDetails = 10,
        ProfilePhoto = 11,
        FitnessCertificate = 12,
        PollutionCertificate = 13,
        Permit = 14,
        AgreementDocument = 15,
        ChequeLeaf = 16
    }

    public enum DriverStatus
    {
        CheckIn = 1,
        Idle = 2,
        InRide = 3,
        Break,
        CheckOut,
        LoggedOut
    }

    public enum FuelType
    {
        Petrol,
        Diseal,
        Electronic
    }

    public enum VehicleStatus
    {
        Active = 1,
        InActive,
        Maintenance
    }

    public enum RideStatus
    {
        Requested,       // Ride just requested
        Scheduled,       // Ride is scheduled for a future time
        Pending,         // Searching for driver (optional)
        Assigned,        // Driver assigned
        Reached,         // Driver reached pickup point
        Started,         // Ride in progress
        Completed,       // Ride completed successfully
        Cancelled,       // Ride cancelled by user or driver
        Failed,          // System/payment/route failure
        NoResponse,      // No driver accepted in time
        Rejected// Driver explicitly rejected
    }

    public enum RideTypeEnum
    {
        Local = 0,
        Rental = 1,
        Outstation = 2
    }

    public enum VehicleTypeEnum
    {
        Hatchback = 1,      // Compact small cars
        Sedan,          // Medium-sized cars with a separate trunk
        SUV,            // Sport Utility Vehicles
        MPV,            // Multi-Purpose Vehicles
        EV,             // Electric Vehicles
        Luxury,         // High-end luxury cars
        AutoRickshaw,   // Three-wheeler city ride
        BikeTaxi
    }

    public enum BidStatus
    {
        Requested,           // The ride request is sent to the driver
        CancelledByDriver,   // The driver has canceled the request
        MissedByDriver,      // The driver didn't accept/respond in time
        AllottedToOther,     // The request was assigned to another driver
        Accepted
    }

    public enum ServiceRequestStatus
    {
        Pending,
        Open,
        Assigned,
        InProgress,
        OnHold,
        Escalated,
        Completed,
        Rejected,
        Cancelled,
        Failed,
        Closed
    }

    public enum ActionType
    {
        New,
        View,
        Edit
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum DocumentHttpMethod
    {
        /// <summary>
        /// The GET HTTP verb.
        /// </summary>
        GET,
        /// <summary>
        /// The HEAD HTTP verb.
        /// </summary>
        HEAD,
        /// <summary>
        /// The PUT HTTP verb.
        /// </summary>
        PUT,
        /// <summary>
        /// The DELETE HTTP verb.
        /// </summary>
        DELETE
    }

    #endregion

}
