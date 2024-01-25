using ClinicaAPI.Models;
using ClinicaAPI.Service.ClienteService;
using ClinicaAPI.Service.ProntuarioService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProntuarioController : ControllerBase
    {
        private readonly IProntuarioInterface _prontuarioInterface;
        public ProntuarioController(IProntuarioInterface prontuarioInterface)
        {
            _prontuarioInterface = prontuarioInterface;
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<ServiceResponse<ProntuarioModel>>> GetProntuariobyId(int Id)
        {
            ServiceResponse<ProntuarioModel> serviceResponse = await _prontuarioInterface.GetProntuariobyId(Id);
            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpGet("tipo")]
        public async Task<ActionResult<ServiceResponse<List<ProntuarioModel>>>> GetProntuariobyTipo(string tipo)
        {
            ServiceResponse<ProntuarioModel> serviceResponse = await _prontuarioInterface.GetProntuariobyTipo(tipo);
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProntuarioModel>>>> GetProntuario()
        {
            return Ok(await _prontuarioInterface.GetProntuario());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProntuarioModel>>>> CreateProntuario(ProntuarioModel novoProntuario)
        {
            return Ok(await _prontuarioInterface.CreateProntuario(novoProntuario));
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<ProntuarioModel>>>> UpdateProntuario(ProntuarioModel editProntuario)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = await _prontuarioInterface.UpdateProntuario(editProntuario);
            return Ok(serviceResponse);
        }


        [Authorize]
        [HttpDelete("id/{id}")]
        public async Task<ActionResult<ServiceResponse<List<ProntuarioModel>>>> DeletePront(int Id)
        {
            ServiceResponse<List<ProntuarioModel>> serviceResponse = await _prontuarioInterface.DeletePront(Id);
            return Ok(serviceResponse);
        }
    }
}
