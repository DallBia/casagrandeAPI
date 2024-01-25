using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class FileModel
    {
        public int Id { get; set; } // Id da entidade, geralmente usado como chave primária
        public string FileName { get; set; } // Nome do arquivo
        public string ContentType { get; set; } // Tipo de conteúdo do arquivo (por exemplo, 'image/jpeg', 'application/pdf', etc.)
        public byte[] Content { get; set; } // Conteúdo real do arquivo armazenado como array de bytes
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;// Data de upload do arquivo, se desejado

    }
}
