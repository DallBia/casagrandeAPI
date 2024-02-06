using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Models
{
    public class FormacaoModel
    {
        /*
areasRelacionadas:'Fisio Padovan,Fonoaudiologia,'
dtConclusao:'01/01/2004'
id:10
idFuncionario:34
instituicao:'34'
nivel:'Pós-Graduação'
nomeFormacao:'teste'
*/
        [Key]
        public int id { get; set; }
        public int idFuncionario { get; set; }
        public string? dtConclusao { get; set; }
        public string nivel { get; set; }
        public string? registro { get; set; }
        public string instituicao { get; set; }
        public string nomeFormacao { get; set; }
        public string areasRelacionadas { get; set; }
    }
}

