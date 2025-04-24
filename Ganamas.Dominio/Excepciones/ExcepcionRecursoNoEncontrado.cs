namespace Ganamas.Dominio.Excepciones;

public class ExcepcionRecursoNoEncontrado : Exception
{
    public ExcepcionRecursoNoEncontrado() : base() { }
    
    public ExcepcionRecursoNoEncontrado(string mensaje) : base(mensaje) { }
    
    public ExcepcionRecursoNoEncontrado(string mensaje, Exception excepcionInterna) : base(mensaje, excepcionInterna) { }
}
