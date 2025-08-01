using ZhooCars.Model.DTOs;
using ZhooCars.Model.Response;

namespace ZCars.Model.Response
{
    public class OTPResponse
    {
        public TokenResponse TokenResponse { get; set; }

        public UserDetailDto? Userdetails { get; set; }

        public List<UserRoleDto> UserRoles { get; set; }
    }
}
