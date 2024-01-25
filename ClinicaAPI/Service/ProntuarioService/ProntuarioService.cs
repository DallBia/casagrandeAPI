using ClinicaAPI.DataContext;
using ClinicaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaAPI.Service.ProntuarioService
{
    public class ProntuarioService : IProntuarioInterface
    {
        private readonly ApplicationDbContext _context;
        public ProntuarioService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<ProntuarioModel>>> CreateProntuario(ProntuarioModel novoProntuario)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = new ServiceResponse<List<ProntuarioModel>>();

            try
            {
                if (novoProntuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                _context.Add(novoProntuario);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Prontuarios.ToList();
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


        public async Task<ServiceResponse<ProntuarioModel>> GetProntuariobyId(int Id)
        {
            ServiceResponse<ProntuarioModel> serviceResponse = new ServiceResponse<ProntuarioModel>();

            try
            {
                ProntuarioModel Prontuario = _context.Prontuarios.FirstOrDefault(x => x.IdColab == Id);


                if (Prontuario == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }

                serviceResponse.Dados = Prontuario;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ProntuarioModel>>> GetProntuariobyTipo(string tipo)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = new ServiceResponse<List<ProntuarioModel>>();

            try
            {
                serviceResponse.Dados = _context.Prontuarios.Where(x => x.Tipo == tipo).ToList();
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
        public async Task<ServiceResponse<List<ProntuarioModel>>> GetProntuario()
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = new ServiceResponse<List<ProntuarioModel>>();

            try
            {
                serviceResponse.Dados = _context.Prontuarios.ToList();
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

        public async Task<ServiceResponse<List<ProntuarioModel>>> UpdateProntuario(ProntuarioModel editProntuario)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = new ServiceResponse<List<ProntuarioModel>>();
            try
            {
                ProntuarioModel Prontuario = _context.Prontuarios.AsNoTracking().FirstOrDefault(x => x.Id == editProntuario.Id);


                if (Prontuario == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }


                _context.Prontuarios.Update(editProntuario);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Prontuarios.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<ProntuarioModel>>> DeletePront(int Id)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = new ServiceResponse<List<ProntuarioModel>>();

            try
            {
                ProntuarioModel pront = _context.Prontuarios.AsNoTracking().FirstOrDefault(x => x.Id == Id);


                if (pront == null)
                {
                    serviceResponse.Mensagem = "Usuário não encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }


                _context.Prontuarios.Remove(pront);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Prontuarios.ToList();
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
