using ClinicaAPI.Models;
using ClinicaAPI.Service.ClienteService;
using ClinicaAPI.Service.FinanceiroService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceiroController : ControllerBase
    {
        private readonly IFinanceiroInterface _financeiroInterface;
        public FinanceiroController(IFinanceiroInterface financeiroInterface)
        {
            _financeiroInterface = financeiroInterface;
        }



        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<ServiceResponse<FinanceiroModel>>> GetFinanceirobyId(int Id)
        {
            ServiceResponse<FinanceiroModel> serviceResponse = await _financeiroInterface.GetFinanceirobyId(Id);
            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpGet("Agenda/{id}")]
        public async Task<ActionResult<ServiceResponse<FinanceiroModel>>> GetFinanceirobyAgenda(int Id)
        {
            ServiceResponse<FinanceiroModel> serviceResponse = await _financeiroInterface.GetFinanceirobyAgenda(Id);
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet("Cliente/{id}")]
        public async Task<ActionResult<ServiceResponse<List<FinanceiroModel>>>> GetFinanceirobyCliente(int id)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = await _financeiroInterface.GetFinanceirobyCliente(id);
            return Ok(serviceResponse);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<FinanceiroModel>>>> CreateFinanceiro(FinanceiroModel novoFinanceiro)
        {
            return Ok(await _financeiroInterface.CreateFinanceiro(novoFinanceiro));
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<FinanceiroModel>>>> UpdateFinanceiro(FinanceiroModel editFinanceiro)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = await _financeiroInterface.UpdateFinanceiro(editFinanceiro);
            return Ok(serviceResponse);
        }
        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<FinanceiroModel>>>> DeleteFinanceiro(int Id)
        {
            ServiceResponse<List<FinanceiroModel>> serviceResponse = await _financeiroInterface.DeleteFinanceiro(Id);
            return Ok(serviceResponse);
        }
        [HttpPut("Saldo")]
            public async Task<ActionResult<ServiceResponse<FinanceiroModel>>> SaldoAgenda(TipoModel valor)
            {
                ServiceResponse<FinanceiroModel> serviceResponse = await _financeiroInterface.SaldoAgenda(valor);
                return Ok(serviceResponse);
            }


    }
    
}
