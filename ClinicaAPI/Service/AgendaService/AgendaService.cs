using ClinicaAPI.DataContext;
using ClinicaAPI.Enums;
using ClinicaAPI.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class AgendaService : IAgendaInterface
{
    private readonly ApplicationDbContext _context;

    public AgendaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<AgendaModel>>> GetAgendaByDate(DateOnly dia)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();

        try
        {
            // Consulta no banco de dados para encontrar agendas na data especificada (dia).
            List<AgendaModel> agendas = await _context.Agendas
                .Where(a => a.diaI <= dia && a.diaF >= dia)
                .ToListAsync();

            serviceResponse.Dados = agendas;

            if (agendas.Count == 0)
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

    public async Task<ServiceResponse<List<AgendaModel>>> CreateAgenda(AgendaModel novaAgenda)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();

        AgendaModel agendaExistente = _context.Agendas
                .FirstOrDefault(x => x.diaI <= novaAgenda.diaI
                                    && x.diaF >= novaAgenda.diaF
                                    && x.horario == novaAgenda.horario
                                    && x.sala == novaAgenda.sala
                                    );
        if (agendaExistente != null)
        {
            try
            {                
                agendaExistente.idCliente = novaAgenda.idCliente;
                agendaExistente.idFuncAlt = novaAgenda.idFuncAlt;
                agendaExistente.nome = novaAgenda.nome;
                agendaExistente.dtAlt = DateTime.UtcNow;
                agendaExistente.horario = novaAgenda.horario;
                agendaExistente.sala = novaAgenda.sala;
                agendaExistente.unidade = novaAgenda.unidade;
                agendaExistente.diaI = novaAgenda.diaI;
                agendaExistente.diaF = novaAgenda.diaF;
                agendaExistente.repeticao = novaAgenda.repeticao;
                agendaExistente.subtitulo = novaAgenda.subtitulo;
                agendaExistente.status = novaAgenda.status;
                agendaExistente.historico = novaAgenda.historico;
                agendaExistente.obs = novaAgenda.obs;
                agendaExistente.valor = novaAgenda.valor;
                agendaExistente.profis = novaAgenda.profis;

                _context.Agendas.Update(agendaExistente);
                await _context.SaveChangesAsync();
                var diaI = novaAgenda.diaI;
                var diaF = novaAgenda.diaF;
                List<AgendaModel> agendas = await _context.Agendas
                    .Where(a => a.diaI <= diaI && a.diaF >= diaF)
                    .ToListAsync();
                serviceResponse.Dados = agendas;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
        }
        else
        {
            try
            {
                _context.Agendas.Add(novaAgenda);
                await _context.SaveChangesAsync();
                var diaI = novaAgenda.diaI;
                var diaF = novaAgenda.diaF;
                List<AgendaModel> agendas = await _context.Agendas
                    .Where(a => a.diaI <= diaI && a.diaF >= diaF)
                    .ToListAsync();
                serviceResponse.Dados = agendas;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
        }      
        return serviceResponse;
    }


   public async Task<ServiceResponse<AgendaModel>> UpdateAgenda(int id, AgendaModel agendaAtualizada)
     {
         ServiceResponse<AgendaModel> serviceResponse = new ServiceResponse<AgendaModel>();

         try
         {
             /*var agendaExistente = await _context.Agendas.FindAsync(id);*/
            var agendaExistente = _context.Agendas.AsNoTracking().FirstOrDefault(x => x.id == agendaAtualizada.id);

             if (agendaExistente == null)
             {
                 serviceResponse.Mensagem = "Agenda não encontrada.";
                 return serviceResponse;
             }

             agendaExistente.idCliente = agendaAtualizada.idCliente;
             agendaExistente.idFuncAlt = agendaAtualizada.idFuncAlt;
             agendaExistente.nome = agendaAtualizada.nome;
             agendaExistente.dtAlt= DateTime.UtcNow;
             agendaExistente.horario = agendaAtualizada.horario;
             agendaExistente.sala = agendaAtualizada.sala;
             agendaExistente.unidade = agendaAtualizada.unidade;
             agendaExistente.diaI = agendaAtualizada.diaI;
             agendaExistente.diaF = agendaAtualizada.diaF;
             agendaExistente.repeticao = agendaAtualizada.repeticao;
             agendaExistente.subtitulo = agendaAtualizada.subtitulo;
             agendaExistente.status = agendaAtualizada.status;
             agendaExistente.historico = agendaAtualizada.historico;
             agendaExistente.obs = agendaAtualizada.obs;
             agendaExistente.valor = agendaAtualizada.valor;
             agendaExistente.profis = agendaAtualizada.profis;

            _context.Agendas.Update(agendaExistente);
             await _context.SaveChangesAsync();
             serviceResponse.Dados = agendaExistente;
         }
         catch (Exception ex)
         {
             serviceResponse.Mensagem = ex.Message;
             serviceResponse.Sucesso = false;
         }

         return serviceResponse;
     }

    public async Task<ServiceResponse<AgendaModel>> ValidAgenda(AgendaModel testAgenda)
    {
        ServiceResponse<AgendaModel> serviceResponse = new ServiceResponse<AgendaModel>();
        if (testAgenda.nome != "")
        {
            try
            {
                AgendaModel agendaExistente = _context.Agendas
                .FirstOrDefault(x => x.diaI <= testAgenda.diaI
                                    && x.diaF >= testAgenda.diaF
                                    && x.nome == testAgenda.nome
                                    && x.horario == testAgenda.horario);               
                

                if (agendaExistente != null)
                {
                    if (agendaExistente.configRept == "X")
                    {
                        serviceResponse.Dados = agendaExistente;
                        serviceResponse.Mensagem = "Encontrado.";
                        serviceResponse.Sucesso = true;
                    }
                    else
                    {
                        var rept1 = agendaExistente.configRept.Split('%');
                        var rept2 = testAgenda.configRept.Split('%');
                        if (rept1[0] == "D")
                        {
                            serviceResponse.Dados = agendaExistente;
                            serviceResponse.Mensagem = "Encontrado.";
                            serviceResponse.Sucesso = true;
                        }
                        else
                        {
                            if (rept1[1] == rept2[1])
                            {
                                if (rept1[0] == "S" || rept1[2] == rept2[2])
                                {
                                    serviceResponse.Dados = agendaExistente;
                                    serviceResponse.Mensagem = "Encontrado.";
                                    serviceResponse.Sucesso = true;
                                }
                                else
                                {
                                    serviceResponse.Dados = agendaExistente;
                                    serviceResponse.Mensagem = "Não Encontrado.";
                                    serviceResponse.Sucesso = true;
                                }
                            }
                            else
                            {
                                serviceResponse.Dados = agendaExistente;
                                serviceResponse.Mensagem = "Não Encontrado.";
                                serviceResponse.Sucesso = true;
                            }
                        }                        
                    }                    
                }
                else
                {                    
                    serviceResponse.Dados = agendaExistente;
                    serviceResponse.Mensagem = "Não Encontrado.";
                    serviceResponse.Sucesso = true;
                };

            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = "Erro ao realizar operação: " + ex.Message;
                serviceResponse.Sucesso = false;
            }
        }
        else
        {
            try
            {
                AgendaModel agendaExistente = _context.Agendas
                .FirstOrDefault(x => x.diaI <= testAgenda.diaI
                                    && x.diaF >= testAgenda.diaF
                                    && x.sala == testAgenda.sala
                                    && x.unidade == testAgenda.unidade
                                    && x.horario == testAgenda.horario
                                    && x.status != 0);

                if (agendaExistente != null)
                {
                    
                    serviceResponse.Dados = agendaExistente;
                    serviceResponse.Mensagem = "Indisponível.";
                    serviceResponse.Sucesso = true;
                }
                else
                {
                    serviceResponse.Dados = agendaExistente;
                    serviceResponse.Mensagem = "Disponível.";
                    serviceResponse.Sucesso = true;
                };

            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = "Erro ao realizar operação: " + ex.Message;
                serviceResponse.Sucesso = false;
            }
        }
        

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<AgendaModel>>> GetMultiAgenda(string parametro)
    {
        var param = parametro.Split('֍');
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();
        try
        {
 
            if (param[0] == "nome")
            {
                List<AgendaModel> agendas = await _context.Agendas
                    .Where(a => a.nome == param[1])
                    .ToListAsync();
                
                if (agendas.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                }
                serviceResponse.Dados = agendas;
                serviceResponse.Mensagem = agendas.Count + " substituições feitas";
            }
            else
            {
                List<AgendaModel> agendas = await _context.Agendas
                    .Where(a => a.multi == param[1])
                    .ToListAsync();
                if (agendas.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                }
                serviceResponse.Dados = agendas;
                serviceResponse.Mensagem = agendas.Count + " substituições feitas";
            }
            serviceResponse.Sucesso = true;
        }
        catch (Exception ex)
        {
            serviceResponse.Mensagem = "Ocorreu um erro";
            serviceResponse.Dados = null;
            serviceResponse.Sucesso = false;
        }
        return serviceResponse;
    }

    

    public async Task<ServiceResponse<List<AgendaModel>>> MultiAgenda(int id, string parametro)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();
        
        try
        {
            switch (id)
            {
                case 1:
                    List<AgendaModel> agendas = await _context.Agendas
                    .Where(a => a.nome == parametro
                    && a.status == (StatusEnum)6)
                    .ToListAsync();

                    if (agendas.Count == 0)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = agendas;
                        serviceResponse.Mensagem = agendas.Count + " substituições feitas";
                    }
                    else
                    {
                        foreach (AgendaModel i in agendas)
                        {
                            i.profis = null;
                            i.valor = 0;
                            i.idCliente = 0;
                            i.multi = "";
                            i.nome = "";
                            i.profis = "";
                            i.repeticao = 0;
                            i.status = 0;
                            i.subtitulo = "";
                        }
                        _context.Agendas.UpdateRange(agendas);
                        await _context.SaveChangesAsync();

                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas.Count + " substituições feitas";
                    }
                    break;
                case 2:
                    List<AgendaModel> agendas2 = await _context.Agendas
                    .Where(a => a.multi == parametro
                    && a.status == (StatusEnum)6)
                    .ToListAsync();

                    if (agendas2.Count == 0)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = agendas2;
                        serviceResponse.Mensagem = agendas2.Count + " substituições feitas";
                    }
                    else
                    {
                        foreach (AgendaModel i in agendas2)
                        {
                            i.profis = null;
                            i.valor = 0;
                            i.idCliente = 0;
                            i.multi = "";
                            i.nome = "";
                            i.profis = "";
                            i.repeticao = 0;
                            i.status = 0;
                            i.subtitulo = "";
                        }
                        _context.Agendas.UpdateRange(agendas2);
                        await _context.SaveChangesAsync();

                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas2.Count + " substituições feitas";
                    }
                    break;
                default:
                    List<AgendaModel> agendas3 = await _context.Agendas
                                        .Where(a => a.multi == parametro
                                        && a.status == (StatusEnum)6)
                                        .ToListAsync();

                    if (agendas3.Count == 0)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = agendas3;
                        serviceResponse.Mensagem = agendas3.Count + " substituições feitas";
                    }
                    else
                    {
                        foreach (AgendaModel i in agendas3)
                        {                          
                            i.status = (StatusEnum)2;
                        }
                        _context.Agendas.UpdateRange(agendas3);
                        await _context.SaveChangesAsync();
                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas3.Count + " substituições feitas";
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Mensagem = "Ocorreu um erro";
            serviceResponse.Dados = null;
            serviceResponse.Sucesso = false;
        }
        return serviceResponse;
    }
}