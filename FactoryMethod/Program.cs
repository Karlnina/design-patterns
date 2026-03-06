Console.WriteLine("Sistema de Logistica de entrega");

Logistica logistica = new Logistica("Empresa XYZ");
logistica.EjecutarEntrega("camion");
logistica.EjecutarEntrega("barco");

public class Logistica
{
    public string NombreEmpresa { get; set; }

    public Logistica(string nombreEmpresa)
    {
        NombreEmpresa = nombreEmpresa;
    }

    public void EjecutarEntrega(string tipoTransporte)
    {
        Transporte transporte = CrearTransporte(tipoTransporte);
        transporte.Entregar();
    }

    private Transporte CrearTransporte(string tipoTransporte)
    {
        switch (tipoTransporte.ToLower())
        {
            case "camion":
                return new Camion();
            case "barco":
                return new Barco();
            default:
                throw new ArgumentException("Tipo de transporte no válido");
        }
    }
}

public abstract class Transporte
{
    public abstract void Entregar();
}

public class Camion : Transporte
{
    public override void Entregar()
    {
        Console.WriteLine("Entregando por tierra");
    }
}

public class Barco : Transporte
{
    public override void Entregar()
    {
        Console.WriteLine("Entregando por mar");
    }
}
