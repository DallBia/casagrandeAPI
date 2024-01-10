namespace ClinicaAPI.Models
{
    public class InfoModel
    {
        public int id { get; set; }
        public int idFuncAlt { get; set; }
        public string nomeInfo { get; set; }
        public string? subtitulo { get; set; }
        public DateTime dtInicio { get; set; } = new DateTime();
        public DateTime? dtFim { get; set; }
        public string tipoInfo { get; set; }
        public string destinat { get; set; }
    }
}
