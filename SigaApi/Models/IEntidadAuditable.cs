namespace SigaApi.Models;

public interface IEntidadAuditable
{
    string Estado { get; set; }
    Guid? IdRegistroSustituto { get; set; }
}
