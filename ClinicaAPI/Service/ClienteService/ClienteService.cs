using ClinicaAPI.DataContext;
using ClinicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ClinicaAPI.Service.ClienteService
{
    public class ClienteService : IClienteInterface
    {
        private readonly ApplicationDbContext _context;
        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }


        //==============================//


        //[HttpPut("Ativa")]
        public async Task<ServiceResponse<List<ClienteModel>>> ActivateCliente(int Id)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();
            try
            {
                ClienteModel cliente = _context.Clientes.FirstOrDefault(x => x.Id == Id);

                if (cliente == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                cliente.Ativo = !cliente.Ativo;

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Clientes.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        //[HttpPost]
        public async Task<ServiceResponse<List<ClienteModel>>> CreateCliente(ClienteModel novoCliente)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();

            try
            {
                int n1 = 0;
                int n2 = 0;
                if (novoCliente == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                
                novoCliente.DtInclusao = novoCliente.DtInclusao.ToUniversalTime();
                novoCliente.DtNascim = novoCliente.DtNascim.ToUniversalTime();
                novoCliente.ClienteDesde = novoCliente.ClienteDesde.ToUniversalTime();
                n1 = _context.Clientes.ToList().Count();
                _context.Add(novoCliente);
                await _context.SaveChangesAsync();
                n2 = _context.Clientes.ToList().Count();
                serviceResponse.Dados = _context.Clientes.ToList();
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


        //[HttpGet("id/{id}")]
        public async Task<ServiceResponse<ClienteModel>> GetClientebyId(int Id)
        {
            ServiceResponse<ClienteModel> serviceResponse = new ServiceResponse<ClienteModel>();
            try
            {
                ClienteModel cliente = _context.Clientes.FirstOrDefault(x => x.Id == Id);

                if (cliente == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                serviceResponse.Dados = cliente;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        //[HttpGet]
        public async Task<ServiceResponse<List<ClienteModel>>> GetCliente()
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();

            serviceResponse.Mensagem = "Dados enviados com sucesso";
            serviceResponse.Sucesso = true;
            try
            {

                serviceResponse.Dados = _context.Clientes.ToList();
                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;                
                serviceResponse.Dados = null;
            }
            return serviceResponse;
        }


        //[HttpPut("Editar")]
        public async Task<ServiceResponse<List<ClienteModel>>> UpdateCliente(ClienteModel editCliente)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();
            try
            {
                ClienteModel cliente = _context.Clientes.AsNoTracking().FirstOrDefault(x => x.Id == editCliente.Id);


                if (cliente == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                editCliente.DtInclusao = editCliente.DtInclusao.ToUniversalTime();
                editCliente.DtNascim = editCliente.DtNascim.ToUniversalTime();
                editCliente.ClienteDesde= editCliente.ClienteDesde.ToUniversalTime();


                _context.Clientes.Update(editCliente);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Clientes.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        //[HttpGet("Email")]
        public async Task<ServiceResponse<ClienteModel>> GetClientebyEmail(string Email)
        {
            ServiceResponse<ClienteModel> serviceResponse = new ServiceResponse<ClienteModel>();

            try
            {
                ClienteModel cliente = _context.Clientes.FirstOrDefault(x => x.Email.ToLower() == Email.ToLower());


                if (cliente == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }

                serviceResponse.Dados = cliente;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        //[HttpGet("Area")]
        public async Task<ServiceResponse<List<ClienteModel>>> GetClientebyArea(string Area)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();

            try
            {
                List<ClienteModel> clientes = _context.Clientes
                    .Where(x => x.AreaSession.ToLower().Contains(Area.ToLower()))
                    .ToList();

                if (clientes.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    serviceResponse.Dados = clientes;
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


        // [HttpDelete]
        public async Task<ServiceResponse<List<ClienteModel>>> DeleteCliente(int Id)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();

            try
            {
                ClienteModel cliente = _context.Clientes.AsNoTracking().FirstOrDefault(x => x.Id == Id);


                if (cliente == null)
                {
                    serviceResponse.Mensagem = "Usuário não encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }


                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Clientes.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        //[HttpGet("Nome")]
        public async Task<ServiceResponse<List<ClienteModel>>> GetClientebyNome(string atr, string par, string ret)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();
            object Retorno;
            try
            {
                if (atr  == "")
                {
                    List<ClienteModel> clientes = _context.Clientes.OrderBy(cliente => cliente.Id).ToList();

                    if (clientes.Count == 0)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = null;
                        serviceResponse.Sucesso = false;
                    }
                    else
                    {
                        serviceResponse.Dados = clientes;
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = clientes.Count.ToString();
                        Retorno = clientes;
                    }
                }
                else
                {
                    if (atr == "Nome")
                    {
                        var ret2 = int.Parse(ret);
                        List<ClienteModel> clientes = _context.Clientes
                                        .OrderBy(cliente => cliente.Id)
                                        .Where(x => x.Nome.ToLower().Contains(par.ToLower()) && x.Id > ret2)
                                        .Take(10)
                                        .ToList();
                        if (clientes.Count == 0)
                        {
                            serviceResponse.Mensagem = "Nenhum dado encontrado.";
                            serviceResponse.Dados = null;
                            serviceResponse.Sucesso = false;
                        }
                        else
                        {
                            int? re = clientes.LastOrDefault()?.Id;
                            serviceResponse.Dados = clientes;
                            serviceResponse.Sucesso = true;
                            serviceResponse.Mensagem = clientes.Count.ToString() + "/" + re.ToString();
                            Retorno = clientes;
                        }
                    }
                    if (atr == "Nome da Mãe")
                    {
                        List<ClienteModel> clientes = _context.Clientes
                                        .Where(x => x.Mae.ToLower().Contains(par.ToLower()))
                                        .ToList();
                        if (clientes.Count == 0)
                        {
                            serviceResponse.Mensagem = "Nenhum dado encontrado.";
                            serviceResponse.Dados = null;
                            serviceResponse.Sucesso = false;
                        }
                        else
                        {
                            serviceResponse.Dados = clientes;
                            serviceResponse.Sucesso = true;
                            serviceResponse.Mensagem = clientes.Count.ToString();
                            Retorno = serviceResponse.Mensagem;
                        }
                    }
                    if (atr == "Área")
                    {
                        List<ClienteModel> clientes = _context.Clientes
                                        .Where(x => x.AreaSession.ToLower().Contains(par.ToLower()))
                                        .ToList();
                        if (clientes.Count == 0)
                        {
                            serviceResponse.Mensagem = "Nenhum dado encontrado.";
                            serviceResponse.Dados = null;
                            serviceResponse.Sucesso = false;
                        }
                        else
                        {
                            serviceResponse.Dados = clientes;
                            serviceResponse.Sucesso = true;
                            serviceResponse.Mensagem = clientes.Count.ToString();
                            Retorno = serviceResponse.Mensagem;
                        }
                    }
                    if (atr == "Idade")
                    {
                        var idade = int.Parse(par);
                        
                        DateTime dataReferencia = DateTime.Now.ToLocalTime(); 
                        DateTime dataMinimaOnly = dataReferencia.AddYears(-idade - 1);

                        DateTime dataMaxima = dataReferencia.AddYears(-idade);
                        DateTime dataMaximaOnly = dataReferencia.AddYears(-idade);


                        List<ClienteModel> clientes2 = _context.Clientes.ToList();
                        List<ClienteModel> clientes = new List<ClienteModel>();

                        foreach (var i in clientes2)
                        {
                            
                            if (i.DtNascim >= dataMinimaOnly && i.DtNascim < dataMaximaOnly)
                            {
                                clientes.Add(i);
                            }
                        }
                        if (clientes.Count == 0)
                        {
                            serviceResponse.Mensagem = "Nenhum dado encontrado.";
                            serviceResponse.Dados = null;
                            serviceResponse.Sucesso = false;
                        }
                        else
                        {
                            serviceResponse.Dados = clientes;
                            serviceResponse.Sucesso = true;
                            serviceResponse.Mensagem = clientes.Count.ToString();
                            Retorno = serviceResponse.Mensagem;
                        }
                    }
                }

                
                                
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }


        //[HttpGet("Agenda/{tipo}")]
        public async Task<ServiceResponse<List<TipoModel>>> GetClientebyAgenda(string tipo)
        {
            ServiceResponse<List<TipoModel>> serviceResponse = new ServiceResponse<List<TipoModel>>();
            var DadosList = new List<TipoModel>();
            try
            {
                var Lista = _context.Clientes
                    .OrderBy (x => x.Nome)
                    .ToList();
                foreach (var T in Lista)                
                {
                    int id;
                    string campo;

                    switch (tipo)
                    {
                        case ("nome"):
                            id = T.Id;
                            campo = T.Nome;                           
                            break;
                        case ("idade"):
                            id = T.Id;
                            campo = T.DtNascim.ToString();
                            break;
                        default:
                            id = T.Id;
                            campo = T.Nome + '%' + T.DtNascim.ToString("o");
                            break;
                    }
                    TipoModel novoItem = new TipoModel
                    {
                        id = id,
                        nome = campo
                    };

                    DadosList.Add(novoItem);
                }
                    serviceResponse.Dados = DadosList.ToList();
                    serviceResponse.Mensagem = "Carregado com sucesso.";
                    serviceResponse.Sucesso = true;
                
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        //[HttpGet("novoId/{id}")]
        public async Task<ServiceResponse<List<ClienteModel>>> GetCli(string id)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = new ServiceResponse<List<ClienteModel>>();
            try
            {
                // Dividir a string usando '%' como delimitador
                string[] partes = id.Split('-');

                // Verificar se há pelo menos três partes
                if (partes.Length >= 3)
                {
                    // Extrair valores
                    string tipo = partes[0];
                    string valor = partes[1];
                    string ativo = partes[4];
                    var Dados = new List<ClienteModel>();

                    // Converter a terceira parte para inteiro (índice)
                    if (int.TryParse(partes[2], out int indice))
                    {
                        switch (ativo)
                        {
                            case "A":
                                switch (tipo)
                                {
                                    case "Nome":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Nome.ToLower().Contains(valor.ToLower()) && x.Ativo == true)
                                            .ToList();
                                        break;
                                    case "Nome da Mãe":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Mae.ToLower().Contains(valor.ToLower()) && x.Ativo == true)
                                            .ToList();
                                        break;
                                    case "Área":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.AreaSession.ToLower().Contains(valor.ToLower()) && x.Ativo == true)
                                            .ToList();
                                        break;
                                    case "Idade":
                                        var idade = int.Parse(valor);

                                        DateTime dataReferencia = DateTime.Now.ToLocalTime();
                                        DateTime dataMinima = dataReferencia.AddYears(-idade - 1);
                                        DateTime dataMaxima = dataReferencia.AddYears(-idade);

                                        List<ClienteModel> clientes2 = _context.Clientes
                                            .Where(x => x.Ativo == true)
                                            .ToList();

                                        foreach (var i in clientes2)
                                        {
                                            if (i.DtNascim >= dataMinima && i.DtNascim < dataMaxima)
                                            {
                                                Dados.Add(i);
                                            }
                                        }
                                        break;
                                    default:
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Ativo == true)
                                            .ToList();
                                        break;
                                }
                                break;
                            case "I":
                                switch (tipo)
                                {
                                    case "Nome":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Nome.ToLower().Contains(valor.ToLower()) && x.Ativo == false)
                                            .ToList();
                                        break;
                                    case "Nome da Mãe":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Mae.ToLower().Contains(valor.ToLower()) && x.Ativo == false)
                                            .ToList();
                                        break;
                                    case "Área":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.AreaSession.ToLower().Contains(valor.ToLower()) && x.Ativo == false)
                                            .ToList();
                                        break;
                                    case "Idade":
                                        var idade = int.Parse(valor);

                                        DateTime dataReferencia = DateTime.Now.ToLocalTime();
                                        DateTime dataMinima = dataReferencia.AddYears(-idade - 1);
                                        DateTime dataMaxima = dataReferencia.AddYears(-idade);

                                        List<ClienteModel> clientes2 = _context.Clientes
                                            .Where(x => x.Ativo == false)
                                            .ToList();

                                        foreach (var i in clientes2)
                                        {
                                            if (i.DtNascim >= dataMinima && i.DtNascim < dataMaxima)
                                            {
                                                Dados.Add(i);
                                            }
                                        }
                                        break;
                                    default:
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Ativo == false)
                                            .ToList();
                                        break;
                                }
                                break;
                            default:
                                switch (tipo)
                                {
                                    case "Nome":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Nome.ToLower().Contains(valor.ToLower()))
                                            .ToList();
                                        break;
                                    case "Nome da Mãe":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.Mae.ToLower().Contains(valor.ToLower()))
                                            .ToList();
                                        break;
                                    case "Área":
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .Where(x => x.AreaSession.ToLower().Contains(valor.ToLower()))
                                            .ToList();
                                        break;
                                    case "Idade":
                                        var idade = int.Parse(valor);

                                        DateTime dataReferencia = DateTime.Now.ToLocalTime();
                                        DateTime dataMinima = dataReferencia.AddYears(-idade - 1);
                                        DateTime dataMaxima = dataReferencia.AddYears(-idade);

                                        List<ClienteModel> clientes2 = _context.Clientes.ToList();

                                        foreach (var i in clientes2)
                                        {
                                            if (i.DtNascim >= dataMinima && i.DtNascim < dataMaxima)
                                            {
                                                Dados.Add(i);
                                            }
                                        }
                                        break;
                                    default:
                                        Dados = _context.Clientes
                                            .OrderBy(x => x.Id)
                                            .ToList();
                                        break;
                                }
                                break;
                        }
                            
                        var ListaTmp = new List<ClienteModel>();
                        var Lista = new List<ClienteModel>();
                        List<ClienteModel> DadosList = new List<ClienteModel>();
                        if (partes[3] == "P")
                        {
                            ListaTmp = Dados
                            .OrderBy(x => x.Id)
                            .ToList();

                            Lista = Dados
                            .OrderBy(x => x.Id)
                            .Where(x => x.Id >= indice)
                            .Take(10)
                            .ToList();
                        }
                        else
                        {
                            ListaTmp = Dados
                            .OrderByDescending(x => x.Id)
                            .OrderBy(x => x.Id)
                            .ToList();

                            Lista = Dados
                            .OrderByDescending(x => x.Id)
                            .Where(x => x.Id <= indice)
                            .Take(10)
                            .OrderBy(x => x.Id)
                            .ToList();
                        }
                        
                        var total = ListaTmp.Count;
                        var firstX = ListaTmp.FirstOrDefault()?.Id;
                        var lastX = ListaTmp.LastOrDefault()?.Id;
                        var firstY = Lista.FirstOrDefault()?.Id;
                        var lastY = Lista.LastOrDefault()?.Id;

                        var seletor = "X";
                        if (firstX == firstY && lastX == lastY)
                        {
                            seletor = "A";
                        }
                        else
                        {
                            if (firstX == firstY)
                            {
                                seletor = "I";
                            }
                            if (lastX == lastY)
                            {
                                seletor = "F";
                            }
                        }
                        
                        serviceResponse.Dados = Lista.ToList();
                        serviceResponse.Mensagem = firstY.ToString() + "-" + lastY.ToString() + "-" + seletor + "-" + total.ToString();
                        serviceResponse.Sucesso = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Dados = null;
                        serviceResponse.Mensagem = "Problemas foram encontrados no loop mais interno";
                        serviceResponse.Sucesso = false;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Problemas foram encontrados no loop central";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = "Problemas foram encontrados no loop externo";
                serviceResponse.Sucesso = false;
                return serviceResponse;
            }
        }
    }
}
