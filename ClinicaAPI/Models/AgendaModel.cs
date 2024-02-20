using ClinicaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class AgendaModel
    {
        [Key]
        public int id { get; set; }
        public int? idCliente { get; set; }
        public string nome { get; set; }
        public int idFuncAlt { get; set; }
        public DateTime dtAlt { get; set; } = DateTime.UtcNow;
        public string horario { get; set; }
        public int sala { get; set; }
        public double unidade { get; set; } // usarei para definir: quanto falta ser pago. Começa com o valor cheio e vai abatendo de acordo com as entradas de pagto.
        public string configRept { get; set; }
        public DateTime diaI { get; set; }
        public DateTime diaF { get; set; }
        public string diaDaSemana
        {
            get
            {
                return DiaDaSemanaParaPortugues(diaI.DayOfWeek);
            }
        }
        private string DiaDaSemanaParaPortugues(DayOfWeek dia1)
        {
            switch (dia1)
            {
                case DayOfWeek.Sunday: return "DOM";
                case DayOfWeek.Monday: return "SEG";
                case DayOfWeek.Tuesday: return "TER";
                case DayOfWeek.Wednesday: return "QUA";
                case DayOfWeek.Thursday: return "QUI";
                case DayOfWeek.Friday: return "SEX";
                case DayOfWeek.Saturday: return "SÁB";
                default: throw new ArgumentException("dia inválido");
            }
        }
        public ReptEnum repeticao { get; set; }
        public string? subtitulo { get; set; }
        public StatusEnum status { get; set; } = 0;
        public string? historico { get; set; }
        public string? obs { get; set; }
        public double? valor { get; set; } = 0;
        public string? multi { get; set; }
        public string? profis { get; set; }
    }
}
