// Ejemplo de patrón Adapter en C#
 var sistemaLegacy = new SistemaLegacy();
 INotificador adaptador = new AlertaAdapter(sistemaLegacy);
adaptador.Enviar("¡Alerta importante!");

// --------------------------------------------------------------

// 1. interfaz moderna que queremos usar
public interface INotificador
{
    void Enviar(string mensaje);
}

// 2. Sistema antiguo(incompatible), que no podemos modificar
public class SistemaLegacy
{
    public void EmitirAlertaAntigua(string texto, int prioridad)
    {
        Console.WriteLine($"Alerta del sistema legacy: {texto} con prioridad {prioridad}");
    }
}

// 3. Adapter: traducir la interfa antiguia a la moderna
public class AlertaAdapter : INotificador
{
    private readonly SistemaLegacy _sistemaLegacy;

    public AlertaAdapter(SistemaLegacy sistemaLegacy)
    {
        _sistemaLegacy = sistemaLegacy;
    }

    public void Enviar(string mensaje)
    {
        // Aquí se puede agregar lógica adicional para determinar la prioridad, etc.
        int prioridad = 1; // Prioridad por defecto
        Console.WriteLine("Adaptando mensaje para el sistema legacy...");
        _sistemaLegacy.EmitirAlertaAntigua(mensaje, prioridad);
    }
}
