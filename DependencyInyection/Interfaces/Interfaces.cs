namespace DependencyInyection.Interfaces;

public interface IConfiguracion
{
    string NombreTienda { get; }
}

public interface IPagoStrategy
{
    void ProcesarPago(decimal monto);
}

public interface INotificador
{
    void Actualizar(string mensaje);
}

public interface IPedido
{
    decimal CalcularTotal();
}

public interface IHistoricoPedidos
{
    void Guardar(string mensaje);

    List<string> ObtenerPedidos();
}