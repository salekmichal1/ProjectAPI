using ProjectAPI.Model.DTO;

namespace ProjectAPI.Repozytorium
{
    public interface IAutoryzacjaRepo
    {
        bool IsUniqueUser(string nazwaurzytkownika);
        Task<OdpowiedzLogowaniaDTO> Login(ProsbaLogowaniaDTO prosbaLogowaniaDTO);
        Task<UrzytkownikDTO> Register(ProsbaRejestracjiDTO prosbatDTO);
    }
}
