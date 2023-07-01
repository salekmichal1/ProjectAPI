using ProjectAPIAuth.Model.DTO;

namespace ProjectAPIAuth.Repository
{
    public interface IAuthorizationRepo
    {
        bool IsUniqueUser(string userName);
        Task<LoginResposneDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
