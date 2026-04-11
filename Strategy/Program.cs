Console.WriteLine("Ejemplo del patrón Strategy en C# para aplicar diferentes estrategias de descuento en una tienda.");
var precioOriginal = 100m;
Console.WriteLine($"Precio original: {precioOriginal:C}");
// Aplicar sin descuento
var sinDescuento = new SinDescuentoStrategy();
var precioSinDescuento = sinDescuento.CalcularDescuento(precioOriginal);
Console.WriteLine($"Precio sin descuento: {precioSinDescuento:C}");
// Aplicar descuento del 10%
var descuento10 = new Descuento10Strategy();
var precioConDescuento10 = descuento10.CalcularDescuento(1000m);
Console.WriteLine($"Precio con descuento del 10%: {precioConDescuento10:C}");

/// <summary>
/// ------------------------------------
/// </summary>

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
    {        return precio * 0.9m;
    }
}