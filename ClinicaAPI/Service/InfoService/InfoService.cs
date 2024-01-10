using ClinicaAPI.DataContext;
using ClinicaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaAPI.Service.InfoService
{
    public class InfoService : InfoInterface
    {
        private readonly ApplicationDbContext _context;
        public InfoService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<InfoModel>>> CreateInfo(InfoModel novoInfo)
        {
            ServiceResponse<List<InfoModel>> serviceResponse = new ServiceResponse<List<InfoModel>>();

            try
            {
                if (novoInfo == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                _context.Add(novoInfo);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Infos.ToList();
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

        public async Task<ServiceResponse<List<InfoModel>>> GetInfo()
        {
            ServiceResponse<List<InfoModel>> serviceResponse = new ServiceResponse<List<InfoModel>>();

            try
            {
                serviceResponse.Dados = _context.Infos.ToList();
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

        public async Task<ServiceResponse<InfoModel>> GetInfobyId(int Id)
        {
            ServiceResponse<InfoModel> serviceResponse = new ServiceResponse<InfoModel>();

            try
            {
                InfoModel Info = _context.Infos.FirstOrDefault(x => x.id == Id);


                if (Info == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                                

                serviceResponse.Dados = Info;

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<InfoModel>>> UpdateInfo(InfoModel editInfo)
        {
            ServiceResponse<List<InfoModel>> serviceResponse = new ServiceResponse<List<InfoModel>>();
            try
            {
                InfoModel Info = _context.Infos.AsNoTracking().FirstOrDefault(x => x.id == editInfo.id);


                if (Info == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }


                _context.Infos.Update(editInfo);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Infos.ToList();
                serviceResponse.Mensagem = "Nova informação inserida com sucesso.";
                serviceResponse.Sucesso = true;
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
