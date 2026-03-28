// Cliente: utiliza el proxy
using System.Diagnostics;

Console.WriteLine("Creando proxy para la base de datos...");
IDatabaseReader databaseReader = new CacheDatabaseReader("Analista");
Stopwatch stopwatch = Stopwatch.StartNew();

stopwatch.Start();

// primera llamada, se obtiene del sujeto real
Console.WriteLine("Primera llamada a GetData... analizando rol: Analista");
string resultQuery = databaseReader.GetData("SELECT * FROM heavy_table");
Console.WriteLine("Resultado: " + resultQuery);
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");

// segunda llamada, se obtiene de la caché
stopwatch.Restart();
Console.WriteLine("\nSegunda llamada a GetData...");
resultQuery = databaseReader.GetData("SELECT * FROM heavy_table");
Console.WriteLine("Resultado: " + resultQuery);
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");

// Tercera llamada, nueva consulta, se obtiene del sujeto real
stopwatch.Restart();
Console.WriteLine("\nTercera llamada a GetData con nueva consulta...");
resultQuery = databaseReader.GetData("SELECT * FROM another_heavy_table");
Console.WriteLine("Resultado: " + resultQuery);
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");

IDatabaseReader adminDatabaseReader = new CacheDatabaseReader("Admin");
stopwatch.Restart();
Console.WriteLine("\nLlamada a GetData con rol Admin...");
resultQuery = adminDatabaseReader.GetData("SELECT * FROM heavy_table");
Console.WriteLine("Resultado: " + resultQuery);
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");

IDatabaseReader sinAccesoDatabaseReader = new CacheDatabaseReader("Usuario");
stopwatch.Restart();
Console.WriteLine("\nLlamada a GetData con rol sin acceso...");
resultQuery = sinAccesoDatabaseReader.GetData("SELECT * FROM heavy_table");
Console.WriteLine("Resultado: " + resultQuery);
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");

// ---------------------------------------
// Interfaz: Contrato común para el sujeto real y el proxy
public interface IDatabaseReader
{
    string GetData(string query);

    string RemoveData(string query);
}

// Sujeto Real: la cuenta
public class HevyDatabaseReader : IDatabaseReader
{
    public string GetData(string query)
    {        // Simula una operación costosa
        System.Threading.Thread.Sleep(5000);
        return "Datos de la base de datos pesada";
    }

    public string RemoveData(string query)
    {
        // Simula una operación costosa
        System.Threading.Thread.Sleep(2000);
        return "Datos eliminados de la base de datos pesada";
    }
}

// Proxy: intercepta las llamadas al sujeto real
public class CacheDatabaseReader : IDatabaseReader
{
    private readonly Lazy<HevyDatabaseReader> _realReader = new Lazy<HevyDatabaseReader>();
    private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

    private const string RolAmin = "Admin";
    private const string RolAnalista = "Analista";

    private readonly string _role;
    public CacheDatabaseReader(string role)
    {
        _role = role;
    }

    public string GetData(string query)
    {
        if (_role != RolAmin && _role != RolAnalista)
        {
            return "Acceso denegado: 403 rol no autorizado";
        }

        if (_role == RolAnalista)
        {
            Console.WriteLine("Interceptando llamada a GetData con query: " + query);
            if (!_cache.TryGetValue(query, out var cachedData))
            {

                // aplicacion legacy, donde no se puede modificar el código del cliente, pero se desea mejorar el rendimiento con caching
                Console.WriteLine("No se encontró en caché, obteniendo datos del sujeto real...");
                var data = _realReader.Value.GetData(query);
                _cache[query] = data; // Almacena en caché
                return data;
            }
            Console.WriteLine("Devolviendo datos desde la caché");
            return cachedData;
        }

        if (_role == RolAmin)
        {
            // aplicacion legacy, donde no se puede modificar el código del cliente, pero se desea mejorar el rendimiento con caching
            Console.WriteLine("No se encontró en caché, obteniendo datos del sujeto real...");
            var data = _realReader.Value.GetData(query);
            _cache[query] = data; // Almacena en caché
            return data;
        }

        return "Acceso denegado: 403 rol no autorizado";

    }

    public string RemoveData(string query)
    {
        Console.WriteLine("Interceptando llamada a RemoveData con query: " + query);
        if (_cache.ContainsKey(query))
        {
            Console.WriteLine("Eliminando datos de la caché");
            _cache.Remove(query);
        }
        return "Datos eliminados de la caché";
    }

}


