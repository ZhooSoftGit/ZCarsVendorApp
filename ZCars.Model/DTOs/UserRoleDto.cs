using ZhooCars.Common;

namespace ZhooCars.Model.DTOs
{
    public class UserRoleDto
    {
        #region Properties

        public DateTime CreatedAt { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public UserRoles RoleId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }

        #endregion
    }
}
