
Console.WriteLine("Construyendo una computadora para servidor...");
// var servidorBuilder = new ServidorBuilder();
// servidorBuilder.ConfigurarCPU("AMD EPYC 7742");
// servidorBuilder.ConfigurarRAM("512GB");
// servidorBuilder.ConfigurarAlmacenamiento("2TB SSD");
// servidorBuilder.ConfigurarEsGamer(false);
// var computadoraServidor = servidorBuilder.ObtenerComputadora();
// computadoraServidor.MostrarEspecificaciones();

// var director = new Director();
// var computadoraServidor = director.ConstruirServidor(new ServidorBuilder());
// computadoraServidor.MostrarEspecificaciones();

// try
// {
//     Console.WriteLine("\nConstruyendo una computadora de escritorio...");
//     var computadoraEscritorio = director.ConstruirComputadora(new EscritorioBuilder());
//     computadoraEscritorio.MostrarEspecificaciones();
// }
// catch (InvalidOperationException ex)
// {
//     Console.WriteLine($"Error: {ex.Message}");
// }

// var computadora = new Computadora.Builder();
// computadora.ConfigurarCPU("Intel Core i9-11900K");
// // computadora.ConfigurarRAM("32GB");
// computadora.ConfigurarAlmacenamiento("1TB SSD");
// computadora.ConfigurarEsGamer(true);
// Computadora computadoraPersonalizada = computadora.BuilderComputadora();
// computadoraPersonalizada.MostrarEspecificaciones();


var director = new Director();
var computadoraServidor = director.ConstruirServidor(new Computadora.Builder());
computadoraServidor.MostrarEspecificaciones();

try
{
    Console.WriteLine("\nConstruyendo una computadora de escritorio...");
    var computadoraEscritorio = director.ConstruirComputadora(new Computadora.Builder());
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

    public class Builder
    {
        private Computadora computadora = new Computadora();

        public void ConfigurarCPU(string cpu)
        {
            computadora.CPU = cpu;
        }

        public void ConfigurarRAM(string ram)
        {
            computadora.RAM = ram;
        }

        public void ConfigurarAlmacenamiento(string almacenamiento)
        {
            computadora.Almacenamiento = almacenamiento;
        }

        public void ConfigurarEsGamer(bool esGamer)
        {
            computadora.EsGamer = esGamer;
        }

        public Computadora BuilderComputadora()
        {
            if (string.IsNullOrEmpty(computadora.RAM))
            {
                throw new InvalidOperationException("La RAM es un campo obligatorio.");
            }

            return computadora;
        }
    }

    public void MostrarEspecificaciones()
    {
        Console.WriteLine($"CPU: {CPU}");
        Console.WriteLine($"RAM: {RAM}");
        Console.WriteLine($"Almacenamiento: {Almacenamiento}");
        Console.WriteLine($"Es Gamer: {EsGamer}");
    }
}


public class Director
{
    public Computadora ConstruirComputadora(Computadora.Builder builder)
    {
        builder.ConfigurarAlmacenamiento("1TB SSD");
        builder.ConfigurarEsGamer(true);
        return builder.BuilderComputadora();
    }

    public Computadora ConstruirServidor(Computadora.Builder builder)
    {
        builder.ConfigurarCPU("AMD EPYC 7742");
        builder.ConfigurarRAM("512GB");
        builder.ConfigurarAlmacenamiento("2TB SSD");
        builder.ConfigurarEsGamer(false);
        return builder.BuilderComputadora();
    }
}