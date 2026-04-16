/// <summary>
/// Ejemplo de implementación del patrón Observer en C#. En este ejemplo, una tienda online notifica a sus clientes y al sistema de log cada vez que cambia el precio de un producto. Los clientes y el sistema de log son observadores que implementan la interfaz IObservador, y la tienda online es el sujeto que mantiene una lista de suscriptores y notifica a todos ellos cuando ocurre un cambio relevante.
/// </summary>

Console.WriteLine("=== Patrón Observer en C# ===");
TiendaOnline tienda = new TiendaOnline();
PantallaMonitor pantalla1 = new PantallaMonitor();
PantallaMonitor pantalla2 = new PantallaMonitor();
LogSistema logSistema = new LogSistema();

// Suscribimos a los observadores
tienda.Suscribir(pantalla1);
tienda.Suscribir(pantalla2);
tienda.Suscribir(logSistema);

// Cambiamos el precio de un producto y notificamos a los observadores
await tienda.CambiarPrecio("Laptop", 999.99m);
await tienda.CambiarPrecio("Smartphone", 499.99m);

Console.WriteLine("\n=== Desuscripción ===");

// Desuscribimos a un observador
tienda.Desuscribir(pantalla2);

await tienda.CambiarPrecio("Tablet", 299.99m);

// cambiamos el precio de un producto y notificamos a los observadores restantes
await tienda.CambiarPrecio("Laptop", 199.99m);

// Fin del programa
Console.WriteLine("\n=== Fin del programa ===");

// ===========================================================================

// Pattern Observer

// Contrato: Todo observador deve implementar a interface IObserver
public interface IObservador
{
    Task Actualizar(string producto, decimal precio);
}

// Sujeto: El objeto que sera "observado"
public class TiendaOnline
{
    private List<IObservador> _suscriptores = new List<IObservador>();
    private decimal _precio;
    private string _producto;

    // Permite suscribirse
    public void Suscribir(IObservador observador)
    {
        _suscriptores.Add(observador);
        Console.WriteLine($"Observador suscrito: {observador.GetType().Name} con ID: {observador.GetHashCode()} correctamente.");
    }

    // Permite desuscribirse
    public void Desuscribir(IObservador observador)
    {
        _suscriptores.Remove(observador);
        Console.WriteLine($"Observador desuscrito: {observador.GetType().Name} con ID: {observador.GetHashCode()} correctamente.");
    }

    // Permite cambiar el precio de un producto y notificar a TODOS los suscriptores
    public async Task CambiarPrecio(string producto, decimal nuevoPrecio)
    {
        _producto = producto;
        _precio = nuevoPrecio;
        Console.WriteLine($"El precio del producto '{_producto}' ha cambiado a {_precio:C}.");
        await Notificar();
    }

    // Notificar a la lista de suscriptores sobre el cambio de precio a cada uno de ellos
    private async Task Notificar()
    {
        foreach (var observador in _suscriptores)
        {
            await observador.Actualizar(_producto, _precio);
        }
    }
}

// Observador Concreto: Implementa la interfaz IObserver y define la acción a realizar cuando se recibe una notificación

// Observador que muestra el precio en pantalla
public class PantallaMonitor : IObservador
{
    public async Task Actualizar(string producto, decimal precio)
    {
        await Task.Delay(5000); // Simula un proceso de actualización
        Console.WriteLine($"El cliente ha sido notificado: El precio del producto '{producto}' ha cambiado a {precio:C}.");
    }
}

// Observador que registra los eventos en un log
public class LogSistema : IObservador
{
    public async Task Actualizar(string producto, decimal precio)
    {
        await Task.Delay(500); // Simula un proceso de registro
        Console.WriteLine($"LogSistema: El precio del producto '{producto}' ha cambiado a {precio:C}. Registro actualizado a hora {DateTime.Now}.");
    }
}




