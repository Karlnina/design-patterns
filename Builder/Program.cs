
Console.WriteLine("Construyendo una computadora para servidor...");
// var servidorBuilder = new ServidorBuilder();
// servidorBuilder.ConfigurarCPU("AMD EPYC 7742");
// servidorBuilder.ConfigurarRAM("512GB");
// servidorBuilder.ConfigurarAlmacenamiento("2TB SSD");
// servidorBuilder.ConfigurarEsGamer(false);
// var computadoraServidor = servidorBuilder.ObtenerComputadora();
// computadoraServidor.MostrarEspecificaciones();

var director = new Director();
var computadoraServidor = director.ConstruirServidor(new ServidorBuilder());
computadoraServidor.MostrarEspecificaciones();

try
{
    Console.WriteLine("\nConstruyendo una computadora de escritorio...");
    var computadoraEscritorio = director.ConstruirComputadora(new EscritorioBuilder());
    computadoraEscritorio.MostrarEspecificaciones();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}


public class Computadora
{
    public string CPU { get; set; } = "Generico";
    public string RAM { get; set; }
    public string Almacenamiento { get; set; } = "HDD";
    public bool EsGamer { get; set; }

    public void MostrarEspecificaciones()
    {
        Console.WriteLine($"CPU: {CPU}");
        Console.WriteLine($"RAM: {RAM}");
        Console.WriteLine($"Almacenamiento: {Almacenamiento}");
        Console.WriteLine($"Es Gamer: {EsGamer}");
    }
}

public interface IComputadoraBuilder
{
    void ConfigurarCPU(string cpu);
    void ConfigurarRAM(string ram);
    void ConfigurarAlmacenamiento(string almacenamiento);
    void ConfigurarEsGamer(bool esGamer);
    Computadora ObtenerComputadora();
}

public class ServidorBuilder : IComputadoraBuilder
{
    private Computadora _computadora = new Computadora();

    public void ConfigurarCPU(string cpu) => _computadora.CPU = cpu;

    public void ConfigurarRAM(string ram) => _computadora.RAM = ram;

    public void ConfigurarAlmacenamiento(string almacenamiento) => _computadora.Almacenamiento = almacenamiento;

    public void ConfigurarEsGamer(bool esGamer)
    {
        _computadora.EsGamer = esGamer;
    }

    public Computadora ObtenerComputadora()
    {
        return _computadora;
    }
}

public class EscritorioBuilder : IComputadoraBuilder
{
    private Computadora _computadora = new Computadora();

    public void ConfigurarCPU(string cpu) => _computadora.CPU = cpu;

    public void ConfigurarRAM(string ram) => _computadora.RAM = ram;

    public void ConfigurarAlmacenamiento(string almacenamiento) => _computadora.Almacenamiento = almacenamiento;

    public void ConfigurarEsGamer(bool esGamer)
    {
        _computadora.EsGamer = esGamer;
    }

    public Computadora ObtenerComputadora()
    {
        if (string.IsNullOrEmpty(_computadora.CPU) || _computadora.CPU == "Generico")
        {
            throw new InvalidOperationException("La CPU debe ser configurada antes de obtener la computadora.");
        }

        return _computadora;
    }
}


public class Director
{
    public Computadora ConstruirComputadora(IComputadoraBuilder builder)
    {
        builder.ConfigurarRAM("32GB");
        builder.ConfigurarAlmacenamiento("1TB SSD");
        builder.ConfigurarEsGamer(true);
        return builder.ObtenerComputadora();
    }

    public Computadora ConstruirServidor(IComputadoraBuilder builder)
    {
        builder.ConfigurarCPU("AMD EPYC 7742");
        builder.ConfigurarRAM("512GB");
        builder.ConfigurarAlmacenamiento("2TB SSD");
        builder.ConfigurarEsGamer(false);
        return builder.ObtenerComputadora();
    }
}