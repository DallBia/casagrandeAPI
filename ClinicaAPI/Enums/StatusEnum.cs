using System.Text.Json.Serialization;

namespace ClinicaAPI.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusEnum
    {
        Vago,       //0
        Bloqueado,  //1  
        Pendente,   //2
        Realizado,  //3
        Desmarcado, //4
        Falta,      //5
        Reservado,  //6
        Sala,       //7
        Inativo     //8 
    }
}
