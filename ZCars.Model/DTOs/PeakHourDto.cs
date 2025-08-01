using System.ComponentModel.DataAnnotations;

namespace ZhooCars.Model.DTOs
{
    public class CreateUpdatePeakHourDto
    {
        #region Properties

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsHoliday { get; set; } = false;

        public bool IsWeekend { get; set; } = false;

        [Required]
        public string PeakDay { get; set; }

        [Range(1.0, 10.0, ErrorMessage = "Multiplier must be between 1.0 and 10.0")]
        public decimal PeakMultiplier { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        #endregion
    }

    public class PeakHourDto
    {
        #region Properties

        public TimeOnly EndTime { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsHoliday { get; set; }

        public bool IsWeekend { get; set; }

        public string PeakDay { get; set; }

        public decimal PeakMultiplier { get; set; }

        public TimeOnly StartTime { get; set; }

        #endregion
    }
}
