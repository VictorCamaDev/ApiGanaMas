namespace Ganamas.Dominio.Excepciones;

public class ExcepcionValidacion : Exception
{
    public Dictionary<string, string[]> Errores { get; }
    
    public ExcepcionValidacion(string mensaje, Dictionary<string, string[]> errores) : base(mensaje)
    {
        Errores = errores;
    }
}
