using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectAPI.Data;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectAPI.Repozytorium
{
    /// <summary>
    /// Kalas imepletmecujaća interjest trzymający metody dla Autoryzacji użytkowników
    /// </summary>
    public class AutoryzacjaRepo : IAutoryzacjaRepo
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
        public AutoryzacjaRepo(ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager,  RoleManager<IdentityRole> roleManager)
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
        /// <param name="nazwaurzytkownika"></param>
        /// <returns></returns>
        public bool IsUniqueUser(string nazwaurzytkownika)
        {
            var urzytkownik = _dbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == nazwaurzytkownika);

            if (urzytkownik == null)
                return true;

            return false;
        }

        /// <summary>
        /// Logowanie użytkownika
        /// </summary>
        /// <param name="prosbaLogowaniaDTO"></param>
        /// <returns></returns>
        public async Task<OdpowiedzLogowaniaDTO> Login(ProsbaLogowaniaDTO prosbaLogowaniaDTO)
        {
            var urzytkownik = _dbContext.ApplicationUsers.SingleOrDefault(x => x.UserName == prosbaLogowaniaDTO.NazwaUrzytkownika);
            bool isValid = await _userManager.CheckPasswordAsync(urzytkownik, prosbaLogowaniaDTO.Haslo);

            if (urzytkownik == null || isValid == false)
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(urzytkownik);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, urzytkownik.Name),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            OdpowiedzLogowaniaDTO odpowiedzLogowaniaDTO = new()
            {
                Urzytkownik = _mapper.Map<UrzytkownikDTO>(urzytkownik),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return odpowiedzLogowaniaDTO;
        }


        /// <summary>
        /// Rejestracja użytkownika
        /// </summary>
        /// <param name="prosbaRejestracjiDTO"></param>
        /// <returns></returns>
        public async Task<UrzytkownikDTO> Register(ProsbaRejestracjiDTO prosbaRejestracjiDTO)
        {
            ApplicationUser urzytkownikObj = new()
            {
                UserName = prosbaRejestracjiDTO.NazwaUrzytkownika,
                Name = prosbaRejestracjiDTO.Nazwa,
                NormalizedEmail = prosbaRejestracjiDTO.NazwaUrzytkownika.ToUpper(),
                Email = prosbaRejestracjiDTO.NazwaUrzytkownika
            };

            try
            {
                var rezultat = await _userManager.CreateAsync(urzytkownikObj, prosbaRejestracjiDTO.Haslo);
                if (rezultat.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(urzytkownikObj, "admin");

                    var urzytkownik = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == prosbaRejestracjiDTO.NazwaUrzytkownika);
                    return _mapper.Map<UrzytkownikDTO>(urzytkownik);
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
