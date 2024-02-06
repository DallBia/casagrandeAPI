using ClinicaAPI.Models;
using ClinicaAPI.Service.FormacaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormacaoController : ControllerBase
    {
        private readonly IFormacaoInterface _FormacaoInterface;
        public FormacaoController(IFormacaoInterface FormacaoInterface)
        {
            _FormacaoInterface = FormacaoInterface;
        }


        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<ServiceResponse<List<FormacaoModel>>>> GetFormacaobyId(int Id)
        {
            ServiceResponse<List<FormacaoModel>> serviceResponse = await _FormacaoInterface.GetFormacaobyId(Id);
            return Ok(serviceResponse);
        }

        [HttpGet("Area")]
        public async Task<ActionResult<ServiceResponse<List<FormacaoModel>>>> GetFormacaobyArea(string Area)
        {
            ServiceResponse<List<FormacaoModel>> serviceResponse = await _FormacaoInterface.GetFormacaobyArea(Area);
            return Ok(serviceResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<FormacaoModel>>>> GetFormacao()
        {
            return Ok(await _FormacaoInterface.GetFormacao());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<FormacaoModel>>>> CreateFormacao(FormacaoModel novoFormacao)
        {
            return Ok(await _FormacaoInterface.CreateFormacao(novoFormacao));
        }

        [Authorize]
        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<FormacaoModel>>>> UpdateFormacao(FormacaoModel editFormacao)
        {
            ServiceResponse<List<FormacaoModel>> serviceResponse = await _FormacaoInterface.UpdateFormacao(editFormacao);
            return Ok(serviceResponse);
        }


    }
}
