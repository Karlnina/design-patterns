
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


// var director = new Director();
// var computadoraServidor = director.ConstruirServidor(new Computadora.Builder());
// computadoraServidor.MostrarEspecificaciones();

// try
// {
//     Console.WriteLine("\nConstruyendo una computadora de escritorio...");
//     var computadoraEscritorio = director.ConstruirComputadora(new Computadora.Builder());
//     computadoraEscritorio.MostrarEspecificaciones();
// }
// catch (InvalidOperationException ex)
// {
//     Console.WriteLine($"Error: {ex.Message}");
// }

var computadora = new ComputadoraRecord.Builder();
computadora.ConfigurarCPU("Intel Core i9-11900K");
computadora.ConfigurarRAM("32GB");
computadora.ConfigurarAlmacenamiento("1TB SSD");
ComputadoraRecord computadoraPersonalizada = computadora.BuilderComputadora();
computadoraPersonalizada.MostrarEspecificaciones();

public record ComputadoraRecord(string CPU, string RAM, string Almacenamiento, bool EsGamer)
{
    public void MostrarEspecificaciones()
    {
        Console.WriteLine($"CPU: {CPU}");
        Console.WriteLine($"RAM: {RAM}");
        Console.WriteLine($"Almacenamiento: {Almacenamiento}");
        Console.WriteLine($"Es Gamer: {(EsGamer ? "Sí" : "No")}");
    }

    public ComputadoraRecord(string CPU, string RAM, string Almacenamiento) : this(CPU ?? throw new ArgumentNullException("CPU debe ser configurado"), RAM, Almacenamiento, false)
    {
    }

    public class Builder
    {
        private string _cpu;
        private string _ram;
        private string _almacenamiento;
        private bool _esGamer;

        public Builder ConfigurarCPU(string cpu)
        {
            _cpu = cpu;
            return this;
        }

        public Builder ConfigurarRAM(string ram)
        {
            _ram = ram;
            return this;
        }

        public Builder ConfigurarAlmacenamiento(string almacenamiento)
        {
            _almacenamiento = almacenamiento;
            return this;
        }

        public Builder ConfigurarEsGamer(bool esGamer)
        {
            _esGamer = esGamer;
            return this;
        }

        public ComputadoraRecord BuilderComputadora()
        {
            if (string.IsNullOrEmpty(_cpu))
                throw new InvalidOperationException("CPU debe ser configurado");

            return new ComputadoraRecord(_cpu, _ram, _almacenamiento, _esGamer);
        }
    }
}
