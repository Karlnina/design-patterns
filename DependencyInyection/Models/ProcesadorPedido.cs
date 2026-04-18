using DependencyInyection.Interfaces;

namespace DependencyInyection.Models;

public class ProcesadorPedido
{
    private readonly IConfiguracion _configuracion;
    private readonly IPagoStrategy _pagoStrategy;
    private readonly IEnumerable<INotificador> _notificadores;
    private readonly IHistoricoPedidos _historicoPedidos;

    public ProcesadorPedido(IConfiguracion configuracion, IPagoStrategy pagoStrategy, IEnumerable<INotificador> notificadores, IHistoricoPedidos historico)
    {
        _configuracion = configuracion;
        _pagoStrategy = pagoStrategy;
        _notificadores = notificadores;
        _historicoPedidos = historico;
    }

    public void FinalizarPedido(IPedido pedido)
    {
        decimal total = pedido.CalcularTotal();
        Console.WriteLine($"Procesando pedido para {_configuracion.NombreTienda} con total de {total:C}...");

        _pagoStrategy.ProcesarPago(total);

        string mensaje = $"Pedido finalizado con total de {total:C} en {_configuracion.NombreTienda}";
        foreach (var notificador in _notificadores)
        {
            notificador.Actualizar(mensaje);
        }

        _historicoPedidos.Guardar(mensaje);

        Console.WriteLine("Pedido procesado exitosamente.");
    }

}
