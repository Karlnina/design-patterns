Console.WriteLine("Demostración del patrón Singleton en C#.");
// LoggerManager logger1 = LoggerManager.Instance;
// LoggerManager logger2 = LoggerManager.Instance;
// logger1.EscribirLog("Este es un mensaje de log desde logger1.");
// logger2.EscribirLog("Este es un mensaje de log desde logger2.");

// Console.WriteLine($"fecha de creación de logger1: {logger1.FechaCreacion}");
// Console.WriteLine($"fecha de creación de logger2: {logger2.FechaCreacion}");
// Console.WriteLine($"¿logger1 y logger2 son la misma instancia? {(ReferenceEquals(logger1, logger2) ? "Sí" : "No")}");
// logger1.MostrarHistorial();
// logger1.LimpiarHistorial();
// logger1.MostrarHistorial();

Console.WriteLine("\nRealizando prueba de estrés concurrente...");
LoggerManager.PruebaEstresConcurrente(100);

Console.WriteLine("\nPresiona cualquier tecla para salir...");



public sealed class LoggerManager
{
   private static Lazy<LoggerManager> _instance = new Lazy<LoggerManager>(() => new LoggerManager());

    public static LoggerManager Instance
    {
        get
        {
            return _instance.Value;
        }
    }

    private readonly List<string> _historial = new List<string>();
    public DateTime FechaCreacion { get; } = DateTime.Now;


    private LoggerManager()
    {
        Console.WriteLine("LoggerManager ha sido instanciado.");
    }

    public void EscribirLog(string message)
    {
        var timestamp = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        Console.WriteLine($"[LOG] {timestamp}: {message}");
        _historial.Add($"[LOG] {timestamp}: {message}");
        // Aquí podrías agregar lógica para escribir en un archivo o enviar a un sistema de logging externo.
    }

    public void MostrarHistorial()
    {
        Console.WriteLine("\nHistorial de Logs:");
        Console.WriteLine("Total de logs registrados: " + _historial.Count);
        foreach (var log in _historial)
        {
            Console.WriteLine(log);
        }
    }

    public void LimpiarHistorial()
    {
        _historial.Clear();
        Console.WriteLine("Historial de logs ha sido limpiado.");
    }

    public static void PruebaEstresConcurrente(int cantadadHilos)
    {
        Console.WriteLine($"\nIniciando prueba de estrés concurrente con {cantadadHilos} hilos...");
        Parallel.For(0, cantadadHilos, i =>
        {
            var logger = Instance;

            logger.EscribirLog($"Mensaje de log desde hilo {i} fecha {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

            Console.WriteLine($"instancia del logger en hilo {i}: {logger.GetHashCode()}");
            Console.WriteLine($"comparación de instancia en hilo {i}: {(ReferenceEquals(logger, Instance) ? "Sí" : "No")}   ");

        });

        Console.WriteLine("Prueba de estrés concurrente finalizada.");
        LoggerManager.Instance.MostrarHistorial();
    }
}