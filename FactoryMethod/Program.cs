Console.WriteLine("Sistema de Logistica de entrega");

Logistica logisticaTerreste = new LogisticaCamion("Empresa XYZ");
logisticaTerreste.EjecutarEntrega();
Logistica logisticaMaritima = new LogisticaBarco("Empresa XYZ");
logisticaMaritima.EjecutarEntrega();

public abstract class Logistica
{
    public string NombreEmpresa { get; set; }

    public Logistica(string nombreEmpresa)
    {
        NombreEmpresa = nombreEmpresa;
    }

    public void EjecutarEntrega()
    {
        Console.WriteLine($"Iniciando proceso de entrega para {NombreEmpresa}");
        ITransporte transporte = CrearTransporte();
        transporte.Entregar();
    }

    protected abstract ITransporte CrearTransporte();

}


public class Camion : ITransporte
{
    public void Entregar()
    {
        Console.WriteLine("Entregando por tierra");
    }
}

public class Barco : ITransporte
{
    public void Entregar()
    {
        Console.WriteLine("Entregando por mar");
    }
}

public interface ITransporte
{
    void Entregar();
}

public class LogisticaCamion : Logistica
{
    public LogisticaCamion(string nombreEmpresa) : base(nombreEmpresa)
    {
    }

    protected override ITransporte CrearTransporte()
    {
        return new Camion();
    }
}

public class LogisticaBarco : Logistica
{
    public LogisticaBarco(string nombreEmpresa) : base(nombreEmpresa)
    {
    }

    protected override ITransporte CrearTransporte()
    {
        return new Barco();
    }
}
