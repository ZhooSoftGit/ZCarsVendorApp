namespace ZCars.Model
{
    public class RefreshTokenDto
    {
        #region Properties

        public string? DeviceKey { get; set; }

        public string? IPAddress { get; set; }

        public required string RefreshToken { get; set; }

        public required int UserId { get; set; }

        #endregion
    }
}
