using ClinicaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class ValorModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public double valor { get; set; }
        public DateTime data { get; set; }
        public Boolean selecionada { get; set; }

    }
}
