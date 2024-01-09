using ClinicaAPI.Models;
using ClinicaAPI.Service.InfoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly InfoInterface _InfoInterface;
        public InfoController(InfoInterface InfoInterface)
        {
            _InfoInterface = InfoInterface;
        }
        //[Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<ServiceResponse<InfoModel>>> GetInfobyId(int Id)
        {
            ServiceResponse<InfoModel> serviceResponse = await _InfoInterface.GetInfobyId(Id);
            return Ok(serviceResponse);
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<InfoModel>>>> GetInfo()
        {
            return Ok(await _InfoInterface.GetInfo());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<InfoModel>>>> CreateInfo(InfoModel novoInfo)
        {
            return Ok(await _InfoInterface.CreateInfo(novoInfo));
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<InfoModel>>>> UpdateInfo(InfoModel editInfo)
        {
            ServiceResponse<List<InfoModel>> serviceResponse = await _InfoInterface.UpdateInfo(editInfo);
            return Ok(serviceResponse);
        }
    }
}
