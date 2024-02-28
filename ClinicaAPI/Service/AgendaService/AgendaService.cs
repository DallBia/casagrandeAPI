using ClinicaAPI.DataContext;
using ClinicaAPI.Enums;
using ClinicaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Security.Cryptography;

public class AgendaService : IAgendaInterface
{
    private readonly ApplicationDbContext _context;

    public AgendaService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<AgendaModel>> GetAgendaById(int id)
    {
        ServiceResponse<AgendaModel> serviceResponse = new ServiceResponse<AgendaModel>();

        try
        {
            var agendaExistente = _context.Agendas.AsNoTracking().FirstOrDefault(x => x.id == id);
            serviceResponse.Mensagem = "Ok";
            serviceResponse.Sucesso = true;
            serviceResponse.Dados = agendaExistente;

            if (agendaExistente == null)
            {
                serviceResponse.Mensagem = "Nenhum dado encontrado.";
                serviceResponse.Dados = null;
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
    public async Task<ServiceResponse<List<AgendaModel>>> GetAgendaByDate(DateTime dia)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();

        try
        {
            // Consulta no banco de dados para encontrar agendas na data especificada (dia).
            List<AgendaModel> agendas = await _context.Agendas
                .Where(a => a.diaI.ToUniversalTime() <= dia.ToUniversalTime() && a.diaF.ToUniversalTime() >= dia.ToUniversalTime())
                .OrderBy(a => a.id)
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

    public async Task<string> TesteA(int id)
    {
        try
        {
            var agendaExistente = _context.Agendas.AsNoTracking().FirstOrDefault(x => x.id == id);

            if (agendaExistente == null)
            {
                return "Dados não encontrados";

            }
            else
            {
                return agendaExistente.nome;
            }
        }
        catch
        {
            return "Erro";
        }
    }
   

    public async Task<ServiceResponse<List<AgendaModel>>> CreateAgenda(AgendaModel novaAgenda)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();

        try
        {
            serviceResponse.Mensagem = "Não foi possível adicionar";
            novaAgenda.diaI = novaAgenda.diaI.ToUniversalTime();
            novaAgenda.diaF = novaAgenda.diaF.ToUniversalTime();
            List<AgendaModel> agendasTmp = await _context.Agendas
                .Where(a => a.diaI.ToUniversalTime() <= novaAgenda.diaI.ToUniversalTime()
                        && a.diaF.ToUniversalTime().AddHours(3) >= novaAgenda.diaF.ToUniversalTime()
                        && a.horario == novaAgenda.horario
                        && a.sala == novaAgenda.sala
                        )
                        .ToListAsync();
            var Idata = novaAgenda.diaI.ToString().Substring(0, 10);
            var Fdata = novaAgenda.diaF.ToString().Substring(0, 10);
            foreach ( var agenda in agendasTmp)
            {
                var Idia = agenda.diaI.ToString().Substring(0,10);
                var Fdia = agenda.diaF.ToString().Substring(0, 10);
                if(Idia == Idata && Fdia == Fdata)
                {
                    agenda.status = (StatusEnum)8;
                    agenda.diaI = agenda.diaI.ToUniversalTime();
                    agenda.diaF = agenda.diaF.ToUniversalTime();
                    agenda.dtAlt = DateTime.UtcNow;
                }
                _context.Agendas.Update(agenda);
            }            
            _context.Agendas.Add(novaAgenda);
            await _context.SaveChangesAsync();

            var diaI = novaAgenda.diaI;
            var diaF = novaAgenda.diaF;
            serviceResponse.Mensagem = "Não foi possível trazer de volta";
            List<AgendaModel> agendas = await _context.Agendas
                .Where(a => a.diaI.ToUniversalTime() <= diaI.ToUniversalTime()
                        && a.status != (StatusEnum)8
                        && a.diaF.ToUniversalTime().AddHours(3) >= diaI.ToUniversalTime())
                        .ToListAsync();
            serviceResponse.Dados = agendas;
            serviceResponse.Mensagem = "Agenda Salva";
            serviceResponse.Sucesso = true;
        }
        catch (Exception ex)
        {
            
            serviceResponse.Sucesso = false;
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
                 var reso = await CreateAgenda(agendaAtualizada);
                serviceResponse.Mensagem = "Agenda criada.";
                 return serviceResponse;
                
            }

             agendaExistente.idCliente = agendaAtualizada.idCliente;
             agendaExistente.idFuncAlt = agendaAtualizada.idFuncAlt;
             agendaExistente.nome = agendaAtualizada.nome;
             agendaExistente.dtAlt= DateTime.UtcNow;
             agendaExistente.horario = agendaAtualizada.horario;
             agendaExistente.sala = agendaAtualizada.sala;
             agendaExistente.unidade = agendaAtualizada.unidade;
                
            if(agendaAtualizada.diaI <= agendaAtualizada.diaF)
            {
                agendaExistente.diaI = agendaAtualizada.diaI;
            }            
             agendaExistente.diaF = agendaAtualizada.diaF;

            if (agendaAtualizada.repeticao != ReptEnum.Cancelar)
            {
                agendaExistente.repeticao = agendaAtualizada.repeticao;
            }
             
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
                List<AgendaModel> agendaExistente = _context.Agendas
                .Where(x => x.diaI.ToUniversalTime() <= testAgenda.diaI.ToUniversalTime().AddHours(3)
                                    && x.diaF.ToUniversalTime().AddHours(3) >= testAgenda.diaI.ToUniversalTime()
                                    && x.nome == testAgenda.nome
                                    && x.status != (StatusEnum)8
                                    && x.horario == testAgenda.horario
                                    )
                .ToList();          
                

                if (agendaExistente != null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Não Encontrado..";
                    serviceResponse.Sucesso = false;
                    foreach (AgendaModel i in agendaExistente )
                    if (i.configRept == "X")
                    {
                        serviceResponse.Dados = i;
                        serviceResponse.Mensagem = "Encontrado.";
                        serviceResponse.Sucesso = true;
                    }
                    else
                    {
                        var rept1 = i.configRept.Split('%');
                        var rept2 = testAgenda.configRept.Split('%');
                        if (rept1[0] == "D")
                        {
                            serviceResponse.Dados = i;
                            serviceResponse.Mensagem = "Encontrado.";
                            serviceResponse.Sucesso = true;
                        }
                        else
                        {
                            if (rept1[1] == rept2[1])
                            {
                                if (rept1[0] == "S" || rept1[2] == rept2[2])
                                {
                                    serviceResponse.Dados = i;
                                    serviceResponse.Mensagem = "Encontrado.";
                                    serviceResponse.Sucesso = true;
                                }
                                /*else
                                {
                                    serviceResponse.Dados = i;
                                    serviceResponse.Mensagem = "Não Encontrado.";
                                    serviceResponse.Sucesso = true;
                                }*/
                            }
                            /*else
                            {
                                serviceResponse.Dados = i;
                                serviceResponse.Mensagem = "Não Encontrado.";
                                serviceResponse.Sucesso = true;
                            }*/
                        }                        
                    }                    
                }
                else
                {                    
                    serviceResponse.Dados = null;
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
                .FirstOrDefault(x => x.diaI.ToUniversalTime() <= testAgenda.diaI.ToUniversalTime()
                                    && x.diaF.ToUniversalTime() >= testAgenda.diaI.ToUniversalTime()
                                    && x.sala == testAgenda.sala
                                    && x.horario == testAgenda.horario
                                    && x.status != (StatusEnum)8
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
        var param = parametro.Split('|');
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
                            i.status = (StatusEnum)0;
                            i.subtitulo = "";
                            i.diaF = i.diaF.ToUniversalTime();
                            i.diaI = i.diaI.ToUniversalTime();
                            i.dtAlt = i.dtAlt.ToUniversalTime();
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
                            i.status = (StatusEnum)0;
                            i.subtitulo = "";
                            i.diaF = i.diaF.ToUniversalTime();
                            i.diaI = i.diaI.ToUniversalTime();
                            i.dtAlt = i.dtAlt.ToUniversalTime();
                        }
                        _context.Agendas.UpdateRange(agendas2);
                        await _context.SaveChangesAsync();

                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas2.Count + " substituições feitas";
                    }
                    break;
                case 3:
                    List<AgendaModel> agendas3 = await _context.Agendas
                    .Where(a => a.multi == parametro
                    && a.status == (StatusEnum)2)
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
                            i.status = (StatusEnum)6;
                            i.diaF = i.diaF.ToUniversalTime();
                            i.diaI = i.diaI.ToUniversalTime();
                            i.dtAlt = i.dtAlt.ToUniversalTime();
                        }
                        _context.Agendas.UpdateRange(agendas3);
                        await _context.SaveChangesAsync();

                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas3.Count + " substituições feitas";
                    }
                    break;
                default:
                    List<AgendaModel> agendas10 = await _context.Agendas
                                        .Where(a => a.multi == parametro
                                        && a.status == (StatusEnum)6)
                                        .ToListAsync();

                    if (agendas10.Count == 0)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = agendas10;
                        serviceResponse.Mensagem = agendas10.Count + " substituições feitas";
                    }
                    else
                    {
                        foreach (AgendaModel i in agendas10)
                        {                          
                            i.status = (StatusEnum)2;
                            i.diaF = i.diaF.ToUniversalTime();
                            i.diaI = i.diaI.ToUniversalTime();
                            i.dtAlt = i.dtAlt.ToUniversalTime();
                        }
                        _context.Agendas.UpdateRange(agendas10);
                        await _context.SaveChangesAsync();
                        serviceResponse.Dados = _context.Agendas.ToList();
                        serviceResponse.Sucesso = true;
                        serviceResponse.Mensagem = agendas10.Count + " substituições feitas";
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

    public async Task<ServiceResponse<List<AgendaModel>>> VerAgenda(string v)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();
        var p = v.Split('%');
        var sala = int.Parse(p[0]);
        var horario = p[1];
        var id = int.Parse(p[2]);
        DateTime diaI = DateTime.Parse(p[3]);
        List<AgendaModel> agendas = await _context.Agendas
             .Where(x => x.diaF.ToUniversalTime() >= diaI.ToUniversalTime()
                        && x.status != (StatusEnum)8)
                    .ToListAsync();

        /* List<AgendaModel> agendas = await _context.Agendas
             .Where(x => x.sala == sala 
             && x.horario == horario
             && x.diaF.ToUniversalTime() >= diaI.ToUniversalTime())
                    .ToListAsync();*/
        serviceResponse.Dados = agendas;
        serviceResponse.Mensagem = agendas.Count.ToString() + " Dados Carregados";
        serviceResponse.Sucesso = true;
        if (agendas.Count == 0)
        {
            serviceResponse.Mensagem = "Nenhum dado";
            serviceResponse.Sucesso = false;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<AgendaModel>>> AgendaByFin(int id, string data)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = new ServiceResponse<List<AgendaModel>>();
        var dados = data.Split("|");
        //Cliente | Pagtos | dataI | dataF | Pendentes | Pagos
        DateTime diaI = DateTime.Parse(dados[2]);
        DateTime diaF = DateTime.Parse(dados[3]);
        List<AgendaModel> agendas = new();
        if (dados[0] == "T")
        {
            agendas = await _context.Agendas
             .Where(x => (x.diaI.ToUniversalTime() <= diaF.ToUniversalTime() && x.diaF.ToUniversalTime() >= diaI.ToUniversalTime())
                 && x.idCliente == id && x.status != (StatusEnum)8 &&
                 (x.status == (StatusEnum)5 || x.status == (StatusEnum)3 || x.status == (StatusEnum)2))
                    .ToListAsync();
        }
        else
        {
            agendas = await _context.Agendas
             .Where(x => (x.diaI.ToUniversalTime() <= diaF.ToUniversalTime() && x.diaF.ToUniversalTime() >= diaI.ToUniversalTime())
                 && x.status != (StatusEnum)8 &&
                 (x.status == (StatusEnum)5 || x.status == (StatusEnum)3 || x.status == (StatusEnum)2))
                    .ToListAsync();
        }
        

        /* List<AgendaModel> agendas = await _context.Agendas
             .Where(x => x.sala == sala 
             && x.horario == horario
             && x.diaF.ToUniversalTime() >= diaI.ToUniversalTime())
                    .ToListAsync();*/
        serviceResponse.Dados = agendas;
        serviceResponse.Mensagem = agendas.Count.ToString() + " Dados Carregados";
        serviceResponse.Sucesso = true;
        if (agendas.Count == 0)
        {
            serviceResponse.Mensagem = "Nenhum dado";
            serviceResponse.Sucesso = false;
        }
        return serviceResponse;
    }

    
}