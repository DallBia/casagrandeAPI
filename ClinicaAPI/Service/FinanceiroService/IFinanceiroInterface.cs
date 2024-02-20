using ClinicaAPI.Models;

namespace ClinicaAPI.Service.FinanceiroService
{
    public interface IFinanceiroInterface
    {
        Task<ServiceResponse<List<FinanceiroModel>>> CreateFinanceiro(FinanceiroModel novoFinanceiro);
        Task<ServiceResponse<FinanceiroModel>> GetFinanceirobyId(int Id);
        Task<ServiceResponse<FinanceiroModel>> GetFinanceirobyAgenda(int Id);
        Task<ServiceResponse<List<FinanceiroModel>>> GetFinanceirobyCliente(int idCliente);
        Task<ServiceResponse<List<FinanceiroModel>>> UpdateFinanceiro(FinanceiroModel editFinanceiro);
        Task<ServiceResponse<List<FinanceiroModel>>> DeleteFinanceiro(int Id);
        Task<ServiceResponse<FinanceiroModel>> SaldoAgenda(TipoModel valor);
    }
}
