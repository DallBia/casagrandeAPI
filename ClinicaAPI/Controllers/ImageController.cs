using ClinicaAPI.DataContext;
using ClinicaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using System.Net.Mime;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpPost]
        public async Task<ServiceResponse<List<TipoModel>>> UploadFile(IFormFile file) //, [FromForm] string NomeS
        {

            ServiceResponse<List<TipoModel>> serviceResponse = new ServiceResponse<List<TipoModel>>();

            try
            {


                // Lógica para converter o conteúdo do arquivo em um array de bytes
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    // Crie uma instância do seu modelo de arquivo e preencha os campos necessários
                    var fileModel = new FileModel
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Content = fileBytes // O conteúdo real do arquivo em formato de bytes
                    };

                    // Adicione a entidade ao contexto e salve as alterações no banco de dados
                    _context.Files.Add(fileModel);
                    await _context.SaveChangesAsync();
                }
                List<FileModel> retorno = _context.Files.ToList();
                List<TipoModel> clientes = new List<TipoModel>();

                foreach (var i in retorno)
                {
                    TipoModel tmp = new TipoModel();
                    var dia = i.UploadDate.ToString();
                    tmp.id = i.Id;
                    tmp.nome = i.FileName + "֍" + i.ContentType + "֍" + dia;
                    clientes.Add(tmp);
                }
                serviceResponse.Dados = clientes;
                serviceResponse.Mensagem = "Sucesso!";
                serviceResponse.Sucesso = false;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
                return serviceResponse;    //    5֍teste֍pdf
            }
        }

        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ServiceResponse<List<TipoModel>>> GetFile(int id)
        {
            ServiceResponse<List<TipoModel>> serviceResponse = new ServiceResponse<List<TipoModel>>();
            try
            {

                List<FileModel> retorno = _context.Files.ToList();
                List<TipoModel> resultado = new List<TipoModel>();

                foreach (var i in retorno)
                {
                    var dados = i.FileName.Split('֍');
                    var dadosId = 0;
                    if (int.TryParse(dados[0], out int meuInteiro))                    {
                        dadosId = meuInteiro;
                    }
                    if (dadosId == id && dados[1] == "C")
                    {
                        TipoModel tmp = new TipoModel();
                        var dia = i.UploadDate.ToString();
                        tmp.id = i.Id;
                        tmp.nome = i.FileName + "֍" + i.ContentType + "֍" + dia;
                        resultado.Add(tmp);
                    }
                    
                }
                serviceResponse.Dados = resultado;
                serviceResponse.Mensagem = "Sucesso!";
                serviceResponse.Sucesso = false;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
                return serviceResponse;    //    5֍teste֍pdf
            }
        }

        [Authorize]
        [HttpGet("id2/{id}")]
        public async Task<ServiceResponse<List<TipoModel>>> GetFile2(int id)
        {
            ServiceResponse<List<TipoModel>> serviceResponse = new ServiceResponse<List<TipoModel>>();
            try
            {

                List<FileModel> retorno = _context.Files.ToList();
                List<TipoModel> resultado = new List<TipoModel>();

                foreach (var i in retorno)
                {
                    var dados = i.FileName.Split('֍');
                    var dadosId = 0;
                    if (int.TryParse(dados[0], out int meuInteiro))
                    {
                        dadosId = meuInteiro;
                    }
                    if (dadosId == id && dados[1] == "E")
                    {
                        TipoModel tmp = new TipoModel();
                        var dia = i.UploadDate.ToString();
                        tmp.id = i.Id;
                        tmp.nome = i.FileName + "֍" + i.ContentType + "֍" + dia;
                        resultado.Add(tmp);
                    }

                }
                serviceResponse.Dados = resultado;
                serviceResponse.Mensagem = "Sucesso!";
                serviceResponse.Sucesso = false;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
                return serviceResponse;    //    5֍teste֍pdf
            }
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public IActionResult DownloadFile(int id)
        {
            try
            {
                var fileModel = _context.Files.Find(id);

                if (fileModel == null)
                {
                    return NotFound(); // Retorna 404 se o arquivo não for encontrado
                }

                // Configuração da resposta para download
                var nome = fileModel.FileName.Split('֍');

                var contentDisposition = new ContentDisposition
                {
                    FileName = nome[5],
                    Inline = false, // Define como falso para forçar o download
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                // Retorna o arquivo como um FileStreamResult
                return File(fileModel.Content, fileModel.ContentType);
            }
            catch (Exception ex)
            {
                // Lida com erros e retorna uma resposta adequada
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
