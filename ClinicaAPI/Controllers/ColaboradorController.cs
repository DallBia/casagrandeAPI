using ClinicaAPI.DataContext;
using ClinicaAPI.DTO;
using ClinicaAPI.Models;
using ClinicaAPI.Service.ColaboradorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;


namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorInterface _colaboradorInterface;
        private readonly ApplicationDbContext _context;
        public ColaboradorController(IColaboradorInterface colaboradorInterface, ApplicationDbContext context)
        {
            _colaboradorInterface = colaboradorInterface;
            _context = context;
        }
        


        [Authorize]
        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<UserModel>>>> UpdateColaborador(UserModel editUser)
        {
            ServiceResponse<List<UserModel>> serviceResponse = await _colaboradorInterface.UpdateColaborador(editUser);
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpPost("AltSen")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDetalhes)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var xId = int.Parse(loginDetalhes.Usuario);
                var novaSenha = loginDetalhes.Senha;
                var User = _context.Users.AsNoTracking().FirstOrDefault(x => x.id == xId);
                if (User == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = "fracasso";
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    if (novaSenha != "")
                    {
                        User.senhaHash = novaSenha;
                        User.senhaProv = null;
                    }

                    if (User.dtDeslig.ToString() == "1900-01-01" || User.dtDeslig.ToString() == "01/01/1900")
                    {
                        User.dtDeslig = null;
                        User.ativo = true;
                        User.senhaProv = null;
                    }

                }
                _context.Users.Update(User);
                await _context.SaveChangesAsync();
                User.senhaHash = "secreta";
                serviceResponse.Dados = "sucesso";

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Dados = "fracasso";
                serviceResponse.Sucesso = false;
            }
            return Ok(serviceResponse);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<UserModel>>>> CreateColaborador(UserModel novoColaborador)
        {
            return Ok(await _colaboradorInterface.CreateColaborador(novoColaborador));
        }

        [Authorize]
        [HttpPost("Altera")]
        //public async Task<ActionResult<ServiceResponse<UserModel>>> AlterarSenha(string email)
            public async Task<IActionResult> AlterarSenha([FromBody] string email)
        {
            ServiceResponse<UserModel> serviceResponse = await _colaboradorInterface.AlterarSenha(email);
            return Ok(serviceResponse);
        }

        [HttpGet("Agenda")]
        public async Task<ActionResult<ServiceResponse<List<TipoModel>>>> GetColaboradorbyAgenda()
        {
            ServiceResponse<List<TipoModel>> serviceResponse = await _colaboradorInterface.GetColabbyAgenda();
            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<ServiceResponse<UserModel>>> GetColaboradorbyId(int id)
        {
            ServiceResponse<UserModel> serviceResponse = await _colaboradorInterface.GetColaboradorbyId(id);
            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpGet("novoId/{id}")]
        public async Task<ActionResult<ServiceResponse<List<UserModel>>>> GetColab(string id)
        {

            ServiceResponse<List<UserModel>> serviceResponse = await _colaboradorInterface.GetColab(id);
            return Ok(serviceResponse);
        }
    }
}
