using ClinicaAPI.Controllers;
using ClinicaAPI.DataContext;
using ClinicaAPI.Models;

using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
/*
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;
using Microsoft.Exchange.WebServices.Data;
using Google.Apis.Drive.v3.Data;
using System.Security.Cryptography;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ClinicaAPI.Service.EmailService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
*/


namespace ClinicaAPI.Service.ColaboradorService
{
    public class ColaboradorService : IColaboradorInterface

    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly GoogleDriveService _googleDriveService;
        public ColaboradorService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            string credentialsFilePath = "../ClinicaAPI/Service/Drive.json";
            string applicationName = "GoogleDrive";

            //_googleDriveService = new GoogleDriveService(credentialsFilePath, applicationName);

        }



        public async Task<ServiceResponse<List<UserModel>>> CreateColaborador(UserModel novoColaborador)
        {
            ServiceResponse<List<UserModel>> serviceResponse = new ServiceResponse<List<UserModel>>();

            try
            {
                if (novoColaborador == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
                _context.Add(novoColaborador);
                await _context.SaveChangesAsync();
                EmailRequest emailRequest = new EmailRequest();
                emailRequest.Destinatario = novoColaborador.email;
                emailRequest.Assunto = "Cadastro de usuário";
                emailRequest.Corpo = "<p><strong><span style='color: blue; font-size: 24px;'> Olá, "
   + novoColaborador.nome + "!</span></strong></p>"
   + "<p>Você acaba de ser cadastrado como funcionário da <b>Clínica Casagrande. </b></p>"
   + "<p>Seu login é o e-mail <strong><span style='color: blue; font-size: 18px;'>"
   + novoColaborador.email + ".</span></strong></p>"
   + "<p>Sua senha <b>provisória</b> é <strong><span style='color: blue; font-size: 18px;'>"
   + novoColaborador.senhaHash + "</span></strong></p>"
   + "<p>Acesse o link do <a href='http://34.123.211.220'>aplicativo</a> e <b>altere a senha. </b>"
    + "Coloque uma senha que seja fácil para você decorar.</p>"
    + "<p>Para sua comodidade, salve o link na sua guia de marcadores favoritos.</p>"
    + "Depois, vá na guia <b>Cadastro de Equipe</b>, selecione seu nome através do filtro e "
    + "complete as informações do seu cadastro.";
                //ServiceResponse<List<EmailRequest>> emailResponse = await _emailService.EnviarEmailAsync(emailRequest);
                try
                {
                    var email = new MimeMessage();


                    email.From.Add(MailboxAddress.Parse(_configuration["EmailConfig:FromEmail"]));
                    email.Subject = emailRequest.Assunto;
                    email.Body = new TextPart("html")
                    {
                        Text = emailRequest.Corpo
                    };
                    using var smtp = new SmtpClient();

                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    var usuario = _configuration["EmailConfig:FromEmail"];
                    //var usuario = "966741631742-sjsrqt2f8251f7qc0b48i2qcnrsi7tk2.apps.googleusercontent.com";
                    var hash = "sknr xkhy mjzf twul";
                    //var hash = _configuration["EmailConfig:Password"];
                    email.To.Add(MailboxAddress.Parse(emailRequest.Destinatario));
                    smtp.Authenticate(usuario, hash);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    serviceResponse.Mensagem = "mensagem enviada";
                    serviceResponse.Sucesso = true;
                }
                catch
                {
                    serviceResponse.Mensagem = "erro no envio do e-mail";
                    serviceResponse.Sucesso = false;
                }
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



        //[HttpPost("Altera")]
        public async Task<ServiceResponse<UserModel>> AlterarSenha(string email)
        {
            ServiceResponse<UserModel> serviceResponse = new ServiceResponse<UserModel>();
            var variavel = email.Split('|');
            
            try
            {
                if (variavel[0] == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados...";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
               
                EmailRequest emailRequest = new EmailRequest();
                emailRequest.Destinatario = variavel[0];
                emailRequest.Assunto = "Alteração de Senha";
                emailRequest.Corpo = variavel[1];

                //ServiceResponse<List<EmailRequest>> emailResponse = await _emailService.EnviarEmailAsync(emailRequest);
                try
                {
                    var enviar = new MimeMessage();
                    UserModel User = _context.Users.AsNoTracking().FirstOrDefault(x => x.email == variavel[0]);
                    if (User == null)
                    {
                        serviceResponse.Mensagem = "Nenhum dado encontrado.";
                        serviceResponse.Dados = null;
                        serviceResponse.Sucesso = false;
                    }
                    else
                    {
                        User.senhaProv = variavel[2];
                        _context.Users.Update(User);
                        await _context.SaveChangesAsync();
                        serviceResponse.Mensagem = "Usuário encontrado.";
                        serviceResponse.Dados = null;
                        serviceResponse.Sucesso = true;
                    }
                    if (serviceResponse.Sucesso == true)
                    {
                        enviar.From.Add(MailboxAddress.Parse(_configuration["EmailConfig:FromEmail"]));
                        enviar.Subject = emailRequest.Assunto;
                        enviar.Body = new TextPart("html")
                        {
                            Text = emailRequest.Corpo
                        };
                        using var smtp = new SmtpClient();

                        smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        var usuario = _configuration["EmailConfig:FromEmail"];
                        //var usuario = "966741631742-sjsrqt2f8251f7qc0b48i2qcnrsi7tk2.apps.googleusercontent.com";
                        var hash = "sknr xkhy mjzf twul";
                        //var hash = _configuration["EmailConfig:Password"];
                        enviar.To.Add(MailboxAddress.Parse(emailRequest.Destinatario));
                        smtp.Authenticate(usuario, hash);
                        smtp.Send(enviar);
                        smtp.Disconnect(true);    

                        serviceResponse.Mensagem = "mensagem enviada";
                        serviceResponse.Sucesso = true;
                        serviceResponse.Dados = null;
                    }
                    
                }
                catch
                {
                    serviceResponse.Mensagem = "erro no envio do e-mail";
                    serviceResponse.Sucesso = false;
                    serviceResponse.Dados = null;
                }                
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
                serviceResponse.Dados = null;
            }
            return serviceResponse;
        }

        Task<ServiceResponse<List<UserModel>>> IColaboradorInterface.GetColaborador()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<UserModel>>> UpdateColaborador(UserModel editUser)
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
                    if (editUser.nome != "")
                    {
                        User.nome = editUser.nome;
                    }
                    if (editUser.celular != "")
                    {
                        User.celular = editUser.celular;
                    }
                    if (editUser.ativo != null)
                    {
                        User.ativo = editUser.ativo;
                    }
                    if (editUser.cpf != "")
                    {
                        User.cpf = editUser.cpf;
                    }
                    if (editUser.foto != "")
                    {
                        User.foto = editUser.foto;
                        //User.foto = await _googleDriveService.UploadImageToGoogleDrive(editUser.foto, editUser.id);

                    }
                    if (editUser.idPerfil < 4)
                    {
                        User.idPerfil = editUser.idPerfil;
                    }

                    DateOnly dataMinima = new DateOnly(1900, 1, 1);
                    if (editUser.dtDeslig != null)
                    {
                        DateTime dMaxima = (DateTime)editUser.dtDeslig;
                        int ano = dMaxima.Year;
                        int mes = dMaxima.Month;
                        int dia = dMaxima.Day;
                        DateOnly dataMax = new DateOnly(ano, mes, dia);
                        if (dataMax != dataMinima)
                        {
                            User.dtDeslig = editUser.dtDeslig;
                        }
                    }
                    if (editUser.dtNasc != null)
                    {
                        DateTime dMaxima = editUser.dtNasc;
                        int ano = dMaxima.Year;
                        int mes = dMaxima.Month;
                        int dia = dMaxima.Day;
                        DateOnly dataMax = new DateOnly(ano, mes, dia);
                        if (dataMax != dataMinima)
                        {
                            User.dtNasc = editUser.dtNasc;
                        }
                    }
                    if (editUser.dtAdmis != null)
                    {
                        DateTime dMaxima = editUser.dtAdmis;
                        int ano = dMaxima.Year;
                        int mes = dMaxima.Month;
                        int dia = dMaxima.Day;
                        DateOnly dataMax = new DateOnly(ano, mes, dia);
                        if (dataMax != dataMinima)
                        {
                            User.dtAdmis = editUser.dtAdmis;
                        }
                    }
                    if (editUser.email != "")
                    {
                        User.email = editUser.email;
                    }
                    if (editUser.endereco != "")
                    {
                        User.endereco = editUser.endereco;
                    }
                    if (editUser.rg != "")
                    {
                        User.rg = editUser.rg;
                    }
                    if (editUser.areaSession != "")
                    {
                        User.areaSession = editUser.areaSession;
                    }
                    if (editUser.telFixo != "")
                    {
                        User.telFixo = editUser.telFixo;
                    }
                }
                _context.Users.Update(User);
                await _context.SaveChangesAsync();
                serviceResponse.Dados = _context.Users.ToList();
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

        public async Task<ServiceResponse<List<TipoModel>>> GetColabbyAgenda(string tipo)
        {
            ServiceResponse<List<TipoModel>> serviceResponse = new ServiceResponse<List<TipoModel>>();
            var DadosList = new List<TipoModel>();
            try
            {
                var Lista = _context.Users
                    .OrderBy(x => x.nome)
                    .ToList();
                foreach (var T in Lista)
                {
                    int id;
                    string campo;

                    switch (tipo)
                    {
                        case ("nome"):
                            id = T.id;
                            campo = T.nome;
                            break;
                        case ("area"):
                            id = T.id;
                            campo = T.nome + '%' + T.areaSession;
                            break;
                        default:
                            id = T.id;
                            campo = T.nome + '%' + T.dtNasc.ToString("o");
                            break;
                    }
                    TipoModel novoItem = new TipoModel
                    {
                        id = id,
                        nome = campo
                    };

                    DadosList.Add(novoItem);
                }
                serviceResponse.Dados = DadosList.ToList();
                serviceResponse.Mensagem = "Carregado com sucesso.";
                serviceResponse.Sucesso = true;

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserModel>> GetColaboradorbyId(int Id)
        {
            ServiceResponse<UserModel> serviceResponse = new ServiceResponse<UserModel>();

            try
            {
                UserModel user = _context.Users.FirstOrDefault(x => x.id == Id);


                if (user == null)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado.";
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                }

                serviceResponse.Dados = user;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }




        //[HttpGet("novoId/{id}")]
        public async Task<ServiceResponse<List<UserModel>>> GetColab(string id)
        {
            ServiceResponse<List<UserModel>> serviceResponse = new ServiceResponse<List<UserModel>>();
            try
            {
                // Dividir a string usando '|' como delimitador
                string[] partes = id.Split('|');

                // Verificar se há pelo menos três partes
                if (partes.Length >= 3)
                {
                    // Extrair valores
                    string tipo = partes[0];
                    string valor = partes[1];


                    // Converter a terceira parte para inteiro (índice)
                    if (int.TryParse(partes[2], out int indice))
                    {
                       
                        var ListaTmp = new List<UserModel>();
                        var Lista = new List<UserModel>();
                        List<UserModel> DadosList = new List<UserModel>();

                            switch (tipo)
                        {
                            case "nome":
                                if (partes[3] == "P")
                                {
                                    ListaTmp = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.nome.ToLower().Contains(valor.ToLower()))
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.nome.ToLower().Contains(valor.ToLower()) && x.id >= indice)
                                    .Take(10)
                                    .ToList();
                                }
                                else
                                {
                                    ListaTmp = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.nome.ToLower().Contains(valor.ToLower()))
                                    .OrderBy(x => x.id)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.nome.ToLower().Contains(valor.ToLower()) && x.id <= indice)
                                    .Take(10)
                                    .OrderBy(x => x.id)
                                    .ToList();
                                }

                                break;

                            case "area":
                                if (partes[3] == "P")
                                {
                                    ListaTmp = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.areaSession.ToLower().Contains(valor.ToLower()))
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.areaSession.ToLower().Contains(valor.ToLower()) && x.id >= indice)
                                    .Take(10)
                                    .ToList();
                                }
                                else
                                {
                                    ListaTmp = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.areaSession.ToLower().Contains(valor.ToLower()))
                                    .OrderBy(x => x.id)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.areaSession.ToLower().Contains(valor.ToLower()) && x.id <= indice)
                                    .Take(10)
                                    .OrderBy(x => x.id)
                                    .ToList();
                                }

                                break;
                            case "perfil":
                                var perf = int.Parse(valor);
                                if (partes[3] == "P")
                                {
                                    ListaTmp = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.idPerfil == perf)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.idPerfil == perf && x.id >= indice)
                                    .Take(10)
                                    .ToList();
                                }
                                else
                                {
                                    ListaTmp = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.idPerfil == perf)
                                    .OrderBy(x => x.id)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.idPerfil == perf && x.id <= indice)
                                    .Take(10)
                                    .OrderBy(x => x.id)
                                    .ToList();
                                }

                                break;
                            default:
                                if (partes[3] == "P")
                                {
                                    ListaTmp = _context.Users
                                    .OrderBy(x => x.id)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderBy(x => x.id)
                                    .Where(x => x.id >= indice)
                                    .Take(10)
                                    .ToList();
                                }
                                else
                                {
                                    ListaTmp = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .OrderBy(x => x.id)
                                    .ToList();

                                    Lista = _context.Users
                                    .OrderByDescending(x => x.id)
                                    .Where(x => x.id <= indice)
                                    .Take(10)
                                    .OrderBy(x => x.id)
                                    .ToList();
                                }

                                break;
                        }
                        var firstX = ListaTmp.FirstOrDefault()?.id;
                        var lastX = ListaTmp.LastOrDefault()?.id;
                        var firstY = Lista.FirstOrDefault()?.id;
                        var lastY = Lista.LastOrDefault()?.id;

                        var seletor = "X";
                        if (firstX == firstY && lastX == lastY)
                        {
                            seletor = "A";
                        }
                        else
                        {
                            if (firstX == firstY)
                            {
                                seletor = "I";
                            }
                            if (lastX == lastY)
                            {
                                seletor = "F";
                            }
                        }
                        foreach (var T in Lista)
                        {
                            UserModel novoItem = new UserModel
                            {
                                id = T.id,
                                nome = T.nome,
                                foto = T.foto,
                                dtNasc = T.dtNasc,
                                rg = T.rg,
                                cpf = T.cpf,
                                endereco = T.endereco,
                                telFixo = T.telFixo,
                                celular = T.celular,
                                email = T.email,
                                dtAdmis = T.dtAdmis,
                                dtDeslig = T.dtDeslig,
                                idPerfil = T.idPerfil,
                                ativo = T.ativo,
                                areaSession = T.areaSession,
                                senhaHash = "secreta",
                                senhaProv = "secreta"
                            };

                            DadosList.Add(novoItem);
                        }
                        serviceResponse.Dados = DadosList.ToList();
                        serviceResponse.Mensagem = firstY.ToString() + "|" + lastY.ToString() + "|" + seletor + "|" + ListaTmp.Count;
                        serviceResponse.Sucesso = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Dados = null;
                        serviceResponse.Mensagem = "Problemas foram encontrados no loop mais interno";
                        serviceResponse.Sucesso = false;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Problemas foram encontrados no loop central";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = "Problemas foram encontrados no loop externo";
                serviceResponse.Sucesso = false;
                return serviceResponse;
            }


        }

        
    }
    
}
