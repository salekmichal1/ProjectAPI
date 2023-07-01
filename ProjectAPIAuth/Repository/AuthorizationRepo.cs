using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectAPIAuth.Data;
using ProjectAPIAuth.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjectAPIAuth.Model.DTO;

namespace ProjectAPIAuth.Repository
{
    /// <summary>
    /// Kalas imepletmecujaća interjest trzymający metody dla Autoryzacji użytkowników
    /// </summary>
    public class AuthorizationRepo : IAuthorizationRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Konstrutkor klasy 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public AuthorizationRepo(ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager,  RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Sprawdzanie czy użytkownik już istnieje
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsUniqueUser(string userName)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == userName);

            if (user == null)
                return true;

            return false;
        }

        /// <summary>
        /// Logowanie użytkownika
        /// </summary>
        /// <param name="loginRequestDTO"></param>
        /// <returns></returns>
        public async Task<LoginResposneDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _dbContext.ApplicationUsers.SingleOrDefault(x => x.UserName == loginRequestDTO.UserName);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResposneDTO loginResposneDTO = new()
            {
                User = _mapper.Map<UserDTO>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResposneDTO;
        }


        /// <summary>
        /// Rejestracja użytkownika
        /// </summary>
        /// <param name="registerRequestDTO"></param>
        /// <returns></returns>
        public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser userObject = new()
            {
                UserName = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name,
                NormalizedEmail = registerRequestDTO.UserName.ToUpper(),
                Email = registerRequestDTO.UserName
            };

            try
            {
                var result = await _userManager.CreateAsync(userObject, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(userObject, "admin");

                    var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == registerRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(user);
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
