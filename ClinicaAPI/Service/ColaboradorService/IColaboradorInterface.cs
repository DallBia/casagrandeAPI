using ClinicaAPI.Models;

namespace ClinicaAPI.Service.ColaboradorService
{
    public interface IColaboradorInterface
    {
        Task<ServiceResponse<List<UserModel>>> GetColaborador();
        Task<ServiceResponse<List<UserModel>>> CreateColaborador(UserModel novoColaborador);
        Task<ServiceResponse<UserModel>> GetColaboradorbyId(int Id);
        //Task<ServiceResponse<List<UserModel>>> GetColaboradorbyArea(string Area);
        Task<ServiceResponse<List<UserModel>>> UpdateColaborador(UserModel editUser);
        //Task<ServiceResponse<List<UserModel>>> GetColaboradorbyNome(string Nome);
        //Task<ServiceResponse<UserModel>> AltSenha(dados);
        Task<ServiceResponse<List<TipoModel>>> GetColabbyAgenda(string tipo);
        Task<ServiceResponse<List<UserModel>>> GetColab(string id);

        Task<ServiceResponse<UserModel>> AlterarSenha(string email);
        Task<ServiceResponse<List<UserModel>>> DeleteColaborador(int Id);
        Task<ServiceResponse<string>> Alt(string email, string corpo, string senha);
    }
}
