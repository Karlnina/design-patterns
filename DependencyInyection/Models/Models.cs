using DependencyInyection.Interfaces;

namespace DependencyInyection.Models;

// Pedido base
public class Pedido : IPedido
{
    public decimal Monto { get; set; }

    public decimal CalcularTotal()
    {
        return Monto;
    }
}

// Decorador: Envio prioritario
public class PedidoPrioritario : IPedido
{
    private readonly IPedido _pedidoBase;
    private readonly decimal _costoAdicional = 20;

    public PedidoPrioritario(IPedido pedidoBase)
    {
        _pedidoBase = pedidoBase;
    }

    public decimal CalcularTotal()
    {
        return _pedidoBase.CalcularTotal() + _costoAdicional; // Costo adicional por envío prioritario
    }
}
