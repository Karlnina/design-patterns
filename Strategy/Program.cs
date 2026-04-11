Console.WriteLine("Ejemplo del patrón Strategy en C# para aplicar diferentes estrategias de descuento en una tienda.");
// var precioOriginal = 100m;
// Console.WriteLine($"Precio original: {precioOriginal:C}");
// // Aplicar sin descuento
// var sinDescuento = new SinDescuentoStrategy();
// var precioSinDescuento = sinDescuento.CalcularDescuento(precioOriginal);
// Console.WriteLine($"Precio sin descuento: {precioSinDescuento:C}");
// // Aplicar descuento del 10%
// var descuento10 = new Descuento10Strategy();
// var precioConDescuento10 = descuento10.CalcularDescuento(1000m);
// Console.WriteLine($"Precio con descuento del 10%: {precioConDescuento10:C}");

// Contexto: Procesar un pedido utilizando estrategias de descuento y comandos para realizar acciones adicionales

// Caso 1: Procesar un pedido sin descuento
var procesadorSinDescuento = new ProcesadorPedidos(new SinDescuentoStrategy());
procesadorSinDescuento.SetComando(new EnviarEmailCommand("cliente@example.com"));
procesadorSinDescuento.SetComando(new GenerarFacturaCommand("FAC-001"));
procesadorSinDescuento.SetComando(new RegistrarLogCommand("Pedido sin descuento procesado"));
procesadorSinDescuento.ProcesarPedido(100m);

Console.WriteLine("\n-----------------------------------\n");

// Caso 2: Procesar un pedido con descuento del 10%
var procesadorConDescuento10 = new ProcesadorPedidos(new Descuento10Strategy());
procesadorConDescuento10.SetComando(new EnviarEmailCommand("cliente2@example.com"));
procesadorConDescuento10.SetComando(new GenerarFacturaCommand("FAC-002"));
procesadorConDescuento10.SetComando(new RegistrarLogCommand("Pedido con descuento del 10% procesado"));
procesadorConDescuento10.ProcesarPedido(200m);

Console.WriteLine("\n-----------------------------------\n");

// =================================
// 1. Patrón Strategy: Estrategias de descuento
// =================================

// Interface de estrategia
public interface IDescuentoStrategy
{
    decimal CalcularDescuento(decimal precio);
}

// Estrategia concreta: Sin descuento
public class SinDescuentoStrategy : IDescuentoStrategy
{
    public decimal CalcularDescuento(decimal precio)
    {
        return precio;
    }
}

// Estrategia concreta: Descuento del 10%
public class Descuento10Strategy : IDescuentoStrategy
{
    public decimal CalcularDescuento(decimal precio)
    {
        return precio * 0.9m;
    }
}

// =================================
// 2. Patrón Comand: Comandos para realizar acciones
// =================================

// Interface de comando
public interface IOrdenCommand
{
    void Ejecutar();
}

// Comando concreto: Enviar email
public class EnviarEmailCommand(string destinatario) : IOrdenCommand
{

    public void Ejecutar()
    {
        Console.WriteLine($"Email enviado al cliente: {destinatario}");
    }
}

// Comando concreto: Generar factura
public class GenerarFacturaCommand(string codigoFactura) : IOrdenCommand
{
    public void Ejecutar()
    {
        Console.WriteLine($"Factura generada para el cliente. Código de factura: {codigoFactura}");
    }
}

// Comando concreto: registrar log
public class RegistrarLogCommand : IOrdenCommand
{    private string _mensaje;
    public RegistrarLogCommand(string mensaje)
    {
        _mensaje = mensaje;
    }

    public void Ejecutar()
    {
        Console.WriteLine($"Log: {_mensaje} registrado a horas {DateTime.Now}.");
    }
}

// =================================
// 3. Contexto e Invocador: Procesador de pedidos que utiliza estrategias y comandos
// =================================

public class ProcesadorPedidos
{
    private IDescuentoStrategy _descuentoStrategy;
    private List<IOrdenCommand> _comandos;

    public ProcesadorPedidos(IDescuentoStrategy descuentoStrategy)
    {
        _descuentoStrategy = descuentoStrategy;
        _comandos = new List<IOrdenCommand>();
    }

    public void SetComando(IOrdenCommand comando)
    {
        _comandos.Add(comando);
    }

    public void ProcesarPedido(decimal monto)
    {
        Console.WriteLine($"Procesando pedido con monto original: {monto:C}");
        var precioFinal = _descuentoStrategy.CalcularDescuento(monto);
        Console.WriteLine($"Precio final después del descuento: {precioFinal:C}");
        foreach (var comando in _comandos)
        {
            comando.Ejecutar();
        }
        Console.WriteLine("Pedido procesado exitosamente.");
    }
}
