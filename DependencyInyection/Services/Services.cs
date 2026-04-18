using DependencyInyection.Interfaces;

namespace DependencyInyection.Services;

public class TiendaConfiguracion : IConfiguracion
{
    public string NombreTienda => "Mi Tienda Online";
}

// strategy para procesar pagos
public class PagoTarjeta : IPagoStrategy
{
    public void ProcesarPago(decimal monto)
    {
        Console.WriteLine($"Procesando pago con tarjeta por un monto de {monto:C}...");
    }
}

public class PagoPayPal : IPagoStrategy
{
    public void ProcesarPago(decimal monto)
    {
        Console.WriteLine($"Procesando pago con PayPal por un monto de {monto:C}...");
    }
}

// Observador para notificaciones
public class NotificadorEmail : INotificador
{
    public void Actualizar(string mensaje)
    {
        Console.WriteLine($"Enviando notificación por email: {mensaje}");
    }
}

public class NotificadorSMS : INotificador
{
    public void Actualizar(string mensaje)
    {
        Console.WriteLine($"Enviando notificación por SMS: {mensaje}");
    }
}

// Historial de pedidos(Singleton)
public class HistoricoPedidos : IHistoricoPedidos
{
    public List<string> Pedidos { get; } = new List<string>();

    public void Guardar(string mensaje)
    {
        Pedidos.Add(mensaje);
        Console.WriteLine($"Pedido guardado en el histórico: {mensaje}");
    }

    public List<string> ObtenerPedidos()
    {
        return Pedidos;
    }
}