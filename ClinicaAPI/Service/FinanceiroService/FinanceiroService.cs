using ClinicaAPI.DataContext;
using ClinicaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaAPI.Service.FinanceiroService
{
    public class FinanceiroService : IFinanceiroInterface
    {
        private readonly ApplicationDbContext _context;
        public FinanceiroService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<FinanceiroModel>>> CreateFinanceiro(FinanceiroModel novoFinanceiro)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = new ServiceResponse<List<FinanceiroModel>>();

            try
            {
                if (novoFinanceiro == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                _context.Add(novoFinanceiro);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Financeiros.ToList();
                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";

                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }
 

        public async Task<ServiceResponse<List<FinanceiroModel>>> DeleteFinanceiro(int Id)
        {
            var response = new ServiceResponse<List<FinanceiroModel>>();

            try
            {
                var valorParaExcluir = await _context.Financeiros.FindAsync(Id);

                if (valorParaExcluir == null)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Valor não encontrado para exclusão.";
                    return response;
                }

                _context.Financeiros.Remove(valorParaExcluir);
                await _context.SaveChangesAsync();

                response.Sucesso = true;
                response.Dados = _context.Financeiros.ToList();
                response.Mensagem = "Valor excluído com sucesso.";
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagem = $"Erro ao excluir o valor: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<FinanceiroModel>> GetFinanceirobyAgenda(int Id)
        {
            ServiceResponse<FinanceiroModel> serviceResponse = new ServiceResponse<FinanceiroModel>();

            try
            {
                string id = Id.ToString();
                FinanceiroModel financeiro = _context.Financeiros.FirstOrDefault(x => x.refAgenda == id);


                if (financeiro == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }

                serviceResponse.Dados = financeiro;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FinanceiroModel>>> GetFinanceirobyCliente(int id)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = new ServiceResponse<List<FinanceiroModel>>();

            try
            {
                List<FinanceiroModel> financeiros = _context.Financeiros
                    .Where(x => x.idCliente == id)
                    .ToList();

                if (financeiros.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    serviceResponse.Dados = financeiros;
                    serviceResponse.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<FinanceiroModel>> GetFinanceirobyId(int Id)
        {
            ServiceResponse<FinanceiroModel> serviceResponse = new ServiceResponse<FinanceiroModel>();

            try
            {
                FinanceiroModel financeiro = _context.Financeiros.FirstOrDefault(x => x.id == Id);


                if (financeiro == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }

                serviceResponse.Dados = financeiro;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FinanceiroModel>>> UpdateFinanceiro(FinanceiroModel editFinanceiro)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = new ServiceResponse<List<FinanceiroModel>>();
            try
            {
                FinanceiroModel financeiro = _context.Financeiros.AsNoTracking().FirstOrDefault(x => x.id == editFinanceiro.id);


                if (financeiro == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }


                _context.Financeiros.Update(editFinanceiro);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Financeiros.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }
    }
}
