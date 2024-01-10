using ClinicaAPI.DataContext;
using ClinicaAPI.DTO;
using ClinicaAPI.Models;
using ClinicaAPI.Service.ClienteService;
using ClinicaAPI.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginDetalhes)
        {
            var user = ValidarUsuario(loginDetalhes);
            if (user != null)
            {
                var tokenString = GerarTokenJWT(user);
                HttpContext.Response.Headers.Add("Baerer", tokenString);
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private UserModel ValidarUsuario(LoginDTO loginDetalhes)
        {
            var smartUser = _context.Users.Where(user =>
                            user.email == loginDetalhes.Usuario && user.senhaHash == loginDetalhes.Senha
                            ).FirstOrDefault();
            var provtUser = _context.Users.Where(user =>
                            user.email == loginDetalhes.Usuario && user.senhaProv == loginDetalhes.Senha
                            ).FirstOrDefault();
            
            if (smartUser != null) {
                smartUser.senhaProv = null;
                return smartUser; }
            else if (provtUser != null) {
                provtUser.senhaHash = provtUser.senhaProv;
                return provtUser; }
            else return null;

            //if (smartUser == null) return null; else return smartUser;
        }
        private string GerarTokenJWT(UserModel smartUser)
        {    

            var _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var Provisoria = smartUser.senhaProv;
            if (Provisoria == null)
            {
                Provisoria = "";
            }
            

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", smartUser.ativo.ToString()));
            permClaims.Add(new Claim("userid", smartUser.id.ToString()));
            permClaims.Add(new Claim("name", smartUser.nome));
            permClaims.Add(new Claim("email", smartUser.email));
            permClaims.Add(new Claim("area", smartUser.areaSession));
            permClaims.Add(new Claim("prov", Provisoria));
            permClaims.Add(new Claim("perfil", smartUser.idPerfil.ToString()));
            permClaims.Add(new Claim("deslig", smartUser.dtDeslig.ToString()));

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                permClaims,
                expires: DateTime.Now.AddMinutes(920), //tempo de duração do token
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        [Authorize]
        [HttpPut("Editar")]
        public async Task<ActionResult<ServiceResponse<List<UserModel>>>> UpdateUser(UserModel editUser)
        {
            ServiceResponse<List<UserModel>> serviceResponse = new ServiceResponse<List<UserModel>>();
            try
            {
                UserModel User = _context.Users.AsNoTracking().FirstOrDefault(x => x.id == editUser.id);


                if (User == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    if (editUser.nome != null)
                    {
                        User.nome = editUser.nome;
                    }
                    if (editUser.celular != null)
                    {
                        User.celular = editUser.celular;
                    }
                    if (editUser.ativo != null)
                    {
                        User.ativo = editUser.ativo;
                    }
                    if (editUser.cpf != null)
                    {
                        User.cpf = editUser.cpf;
                    }
                    if (editUser.idPerfil != null)
                    {
                        User.idPerfil = editUser.idPerfil;
                    }
                    DateOnly dataMinima = new DateOnly(1900, 1, 1);
                    if (editUser.dtDeslig != dataMinima)
                    {
                        User.dtDeslig = editUser.dtDeslig;
                    }
                    if (editUser.dtNasc != null)
                    {
                        User.dtNasc = editUser.dtNasc;
                    }
                    if (editUser.email != null)
                    {
                        User.email = editUser.email;
                    }
                    if (editUser.endereco != null)
                    {
                        User.endereco = editUser.endereco;
                    }
                    if (editUser.rg != null)
                    {
                        User.rg = editUser.rg;
                    }
                    if (editUser.telFixo != null)
                    {
                        User.telFixo = editUser.telFixo;
                    }
                }
                _context.Users.Update(User);
                await _context.SaveChangesAsync();
                foreach (var user in serviceResponse.Dados)
                {

                    user.senhaHash = "secreta";
                }
                serviceResponse.Dados = _context.Users.ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserModel>>>> GetUser()
        {
            ServiceResponse<List<UserModel>> serviceResponse = new ServiceResponse<List<UserModel>>();

            try
            {
                serviceResponse.Dados = _context.Users.ToList();
                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                }
                else
                {
                    foreach (var user in serviceResponse.Dados)
                    {
                        // Modifique o valor da propriedade SenhaHash aqui, se necessário
                        user.senhaHash = "secreta";
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }


        [Authorize]
        [HttpPost("Novo")]
        public async Task<ServiceResponse<List<UserModel>>> CreateUser(UserModel novoUser)
        {
            ServiceResponse<List<UserModel>> serviceResponse = new ServiceResponse<List<UserModel>>();

            try
            {
                if (novoUser == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                _context.Add(novoUser);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Users.ToList();
                if (serviceResponse.Dados.Count == 0)
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
    }
}
