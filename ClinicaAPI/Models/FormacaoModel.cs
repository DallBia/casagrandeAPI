using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class FormacaoModel
    {
        [Key]
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public string? DtConclusao { get; set; }
        public string Nivel { get; set; }
        public string? Registro { get; set; }
        public string Instituicao { get; set; }
        public string NomeFormacao { get; set; }
        public string AreasRelacionadas { get; set; }
    }
}
