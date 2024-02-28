using ClinicaAPI.Models;
using ClinicaAPI.Service.DonoSalaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AgendaController : ControllerBase
{
    private readonly IAgendaInterface _agendaInterface;

    public AgendaController(IAgendaInterface agendaInterface)
    {
        _agendaInterface = agendaInterface;
    }

    [HttpGet("AgendaByDate/{dia}")]
    public async Task<ActionResult<ServiceResponse<List<AgendaModel>>>> GetAgendaByDate(string dia)
    {
        /*if (!DateTime.TryParseExact(dia, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return BadRequest("Formato de data inválido. Use o formato yyyy-MM-dd.");
        }

        DateOnly dateOnly = DateOnly.FromDateTime(parsedDate);*/
        DateTime dia0 = DateTime.Parse(dia);
        var dia1 = dia0.ToUniversalTime();

        ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.GetAgendaByDate(dia1);
        return Ok(serviceResponse);
    }
    [HttpGet("Id/{Id}")]
    public async Task<ActionResult<ServiceResponse<AgendaModel>>> GetAgendaId(int id)
    {
        
        ServiceResponse<AgendaModel> serviceResponse = await _agendaInterface.GetAgendaById(id);
        return Ok(serviceResponse);
    }

    [HttpPost("CreateAgenda")]
    public async Task<ActionResult<ServiceResponse<List<AgendaModel>>>> CreateAgenda([FromBody] AgendaModel novaAgenda)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.CreateAgenda(novaAgenda);
        return Ok(serviceResponse);
    }
    [HttpPost("ValidAgenda")]
    public async Task<ActionResult<ServiceResponse<AgendaModel>>> ValidAgenda([FromBody] AgendaModel testAgenda)
    {
        ServiceResponse<AgendaModel> serviceResponse = await _agendaInterface.ValidAgenda(testAgenda);
        return Ok(serviceResponse);
    }
    
    [HttpGet("Multi/{param}")]
    public async Task<ActionResult<ServiceResponse<AgendaModel>>> GetMultiAgenda(string param)
    {
        ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.GetMultiAgenda(param);
        return Ok(serviceResponse);
    }

    [HttpPut("UpdateAgenda/{id}")]
    public async Task<ActionResult<ServiceResponse<AgendaModel>>> UpdateAgenda(int id, [FromBody] AgendaModel agendaAtualizada)
    {
        ServiceResponse<AgendaModel> serviceResponse = await _agendaInterface.UpdateAgenda(id, agendaAtualizada);
        return Ok(serviceResponse);
    }

    [Authorize]
    [HttpPut("MultiAgenda/{id}")]
    public async Task<ActionResult<ServiceResponse<AgendaModel>>> MultiAgenda(int id, [FromBody] string par)
    {

         /*var quebra = par.Split('֍');
         var num = int.Parse(quebra[0]);*/
         ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.MultiAgenda(id, par);
         return Ok(serviceResponse);
    }
        

    [HttpPost("Teste/{id}")]
    public async Task<string> Teste(int id)
    {
        string resposta = await _agendaInterface.TesteA(id);
        return (resposta);
    }

    [HttpPost("Verif")]
    public async Task<ActionResult<ServiceResponse<List<AgendaModel>>>> VerAgenda([FromBody] TipoModel t)
    {
        var v = t.nome;
        ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.VerAgenda(v);
        return (serviceResponse);
    }

    [HttpPost("AgendaByFin")]
    public async Task<ActionResult<ServiceResponse<List<AgendaModel>>>> AgendaByFin([FromBody] TipoModel t)
    {
        var v = t.nome;
        ServiceResponse<List<AgendaModel>> serviceResponse = await _agendaInterface.AgendaByFin(t.id, v);

        return (serviceResponse);
    }
}