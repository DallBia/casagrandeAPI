using ClinicaAPI.Models;

namespace ClinicaAPI.Service.ProntuarioService
{
    public interface IProntuarioInterface
    {
        Task<ServiceResponse<List<ProntuarioModel>>> GetProntuario();
        Task<ServiceResponse<List<ProntuarioModel>>> CreateProntuario(ProntuarioModel novoProntuario);
        Task<ServiceResponse<ProntuarioModel>> GetProntuariobyId(int Id);
        Task<ServiceResponse<List<ProntuarioModel>>> GetProntuariobyTipo(string tipo);
        Task<ServiceResponse<List<ProntuarioModel>>> UpdateProntuario(ProntuarioModel editProntuario);
        Task<ServiceResponse<List<ProntuarioModel>>> DeletePront(int Id);
    }
}
