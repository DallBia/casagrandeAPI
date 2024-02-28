using ClinicaAPI.Models;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAgendaInterface
{
    Task<ServiceResponse<List<AgendaModel>>> GetAgendaByDate(DateTime dia);
    Task<ServiceResponse<AgendaModel>> GetAgendaById(int id);
    Task<ServiceResponse<List<AgendaModel>>> CreateAgenda(AgendaModel novaAgenda);
    Task<ServiceResponse<AgendaModel>> UpdateAgenda(int id, AgendaModel editAgenda);
    Task<ServiceResponse<AgendaModel>> ValidAgenda(AgendaModel testAgenda);
    Task<ServiceResponse<List<AgendaModel>>> GetMultiAgenda(string param);
    Task<ServiceResponse<List<AgendaModel>>> MultiAgenda(int id, string param);
    Task<string> TesteA(int id);    
    Task<ServiceResponse<List<AgendaModel>>> VerAgenda(string v);
    Task<ServiceResponse<List<AgendaModel>>> AgendaByFin(int id, string data);
}