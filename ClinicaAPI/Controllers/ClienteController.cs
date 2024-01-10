using ClinicaAPI.Models;
using ClinicaAPI.Service.ClienteService;
using ClinicaAPI.Service.ColaboradorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteInterface _clienteInterface;
        public ClienteController(IClienteInterface clienteInterface)
        {
            _clienteInterface = clienteInterface;
        }


        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<ServiceResponse<ClienteModel>>> GetClientebyId(int Id)
        {
            ServiceResponse<ClienteModel> serviceResponse = await _clienteInterface.GetClientebyId(Id);
            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpGet("Email")]
        public async Task<ActionResult<ServiceResponse<ClienteModel>>> GetClientebyEmail(string Email)
        {
            ServiceResponse<ClienteModel> serviceResponse = await _clienteInterface.GetClientebyEmail(Email);
            return Ok(serviceResponse);
        }
        [HttpGet("Area")]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> GetClientebyArea(string Area)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.GetClientebyArea(Area);
            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpGet("Agenda/{tipo}")]
        public async Task<ActionResult<ServiceResponse<List<TipoModel>>>> GetClientebyAgenda(string tipo)
        {
            ServiceResponse<List<TipoModel>> serviceResponse = await _clienteInterface.GetClientebyAgenda(tipo);
            return Ok(serviceResponse);
        }

        [HttpGet("Nome")]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> GetClientebyNome(string atr, string par, string ret)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.GetClientebyNome(atr, par, ret);
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> GetCliente()
        {
            return Ok("teste por aqui funciona");
            //return Ok(await _clienteInterface.GetCliente());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> CreateCliente(ClienteModel novoCliente)
        {
            return Ok(await _clienteInterface.CreateCliente(novoCliente));
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> UpdateCliente(ClienteModel editCliente)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.UpdateCliente(editCliente);
            return Ok(serviceResponse);
        }

        [HttpPut("Ativa")]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> ActivateCliente(int Id)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.ActivateCliente(Id);
            return Ok(serviceResponse);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> DeleteCliente(int Id)
        {
            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.DeleteCliente(Id);
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet("novoId/{id}")]
        public async Task<ActionResult<ServiceResponse<List<ClienteModel>>>> GetCli(string id)
        {

            ServiceResponse<List<ClienteModel>> serviceResponse = await _clienteInterface.GetCli(id);
            return Ok(serviceResponse);
        }
    }
}
