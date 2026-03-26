// Programa de ejemplo para el patrón Abstract Factory
var fabricaModerna = new ModernosFactory();
var cliente1 = new Cliente(fabricaModerna);
cliente1.MostrarMuebles();

var fabricaVictoriana = new VictorianaFactory();
var cliente2 = new Cliente(fabricaVictoriana);
cliente2.MostrarMuebles();


// -----------------------
// interfaces para productos
public interface ISilla
{
    void Describir();
}

public interface IMesa
{
    void Describir();
}

// familia moderna
public sealed class SillaModerna : ISilla
{
    public void Describir()
    {
        Console.WriteLine("Soy una silla moderna");
    }
}

public sealed class MesaModerna : IMesa
{
    public void Describir()
    {
        Console.WriteLine("Soy una mesa moderna");
    }
}


// familia victoriana
public sealed class SillaVictoriana : ISilla
{
    public void Describir()
    {
        Console.WriteLine("Soy una silla victoriana");
    }
}

public sealed class MesaVictoriana : IMesa
{
    public void Describir()
    {
        Console.WriteLine("Soy una mesa victoriana");
    }
}


// fabrica abstracta
public interface IMueblesFactory
{
    ISilla CrearSilla();
    IMesa CrearMesa();
}

// fabrica concreta para muebles modernos
public sealed class ModernosFactory : IMueblesFactory
{
    public ISilla CrearSilla()
    {
        return new SillaModerna();
    }

    public IMesa CrearMesa()
    {
        return new MesaModerna();
    }
}

// fabrica concreta para muebles victorianos
public sealed class VictorianaFactory : IMueblesFactory
{
    public ISilla CrearSilla()
    {
        return new SillaVictoriana();
    }

    public IMesa CrearMesa()
    {
        return new MesaVictoriana();
    }
}

// fabrica de muebles personalizada
// public sealed class PersonalizadaFactory : IMueblesFactory
// {
//     public ISilla CrearSilla()
//     {        return new SillaPersonalizada();
//     }

//     public IMesa CrearMesa()
//     {        return new MesaPersonalizada();
//     }
// }

// cliente
public class Cliente
{
    private readonly ISilla _silla;
    private readonly IMesa _mesa;

    public Cliente(IMueblesFactory fabrica)
    {
        _silla = fabrica.CrearSilla();
        _mesa = fabrica.CrearMesa();
    }

    public void MostrarMuebles()
    {
        Console.WriteLine("Muebles creados:");
        _silla.Describir();
        _mesa.Describir();
    }

}