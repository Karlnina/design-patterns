// Ejemplo de patrón Adapter en C#
// var sistemaLegacy = new SistemaLegacy();
// INotificador adaptador = new AlertaAdapter(sistemaLegacy);
// adaptador.Enviar("¡Alerta importante!");

// var servicioEmailAntiguo = new ServicioEmailAntiguo();
// INotificador emailAdaptador = new EmailAdapter(servicioEmailAntiguo);
// emailAdaptador.Enviar("¡Email importante! desde el adaptador! nuevo ejemplo");


// Colaboración entre patrones: Adapter + Decorator
INotificador notificador = new EncriptacionDecorador(
    new LogNotificadorDecorador(
        new NotificadorBase()
    )
);
notificador.Enviar("Enviado mensaje desde un nivel de decorador!");



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
// Nuevo: EmailAdapter

public class ServicioEmailAntiguo
{
    public ServicioEmailAntiguo()
    {

    }

    public void EnviarEmail(string destinatario, string asunto, string cuerpo)
    {
        Console.WriteLine($"Enviando email a {destinatario} con asunto '{asunto}' y cuerpo '{cuerpo}' usando el sistema antiguo.");
    }
}

public class EmailAdapter : INotificador
{
    private readonly ServicioEmailAntiguo _servicioEmailAntiguo;

    public EmailAdapter(ServicioEmailAntiguo servicioEmailAntiguo)
    {
        _servicioEmailAntiguo = servicioEmailAntiguo;
    }

    public void Enviar(string mensaje)
    {
        // Aquí se puede agregar lógica adicional para determinar el destinatario, asunto, etc.
        string destinatario = "destinatario@example.com";
        string asunto = "Asunto del email";
        string cuerpo = mensaje;
        _servicioEmailAntiguo.EnviarEmail(destinatario, asunto, cuerpo);
    }
}

// 2. Componente base
public class NotificadorBase : INotificador
{
    public void Enviar(string mensaje)
    {
        Console.WriteLine($"Enviando mensaje: {mensaje}");
    }
}

// 3. Decorador base(abstracto)
public abstract class NotificadorDecorador : INotificador
{
    protected INotificador _envoltorio;

    public NotificadorDecorador(INotificador envoltorio)
    {
        _envoltorio = envoltorio;
    }

    public virtual void Enviar(string mensaje)
    {
        _envoltorio.Enviar(mensaje);
    }
}

// 4. Decorador concreto: Log
public class LogNotificadorDecorador : NotificadorDecorador
{
    public LogNotificadorDecorador(INotificador envoltorio) : base(envoltorio) { }

    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"[LOG] Enviando mensaje: {mensaje}");
        base.Enviar(mensaje);
    }
}

// 5. Decorador concreto: Encriptacion
public class EncriptacionDecorador : NotificadorDecorador
{
    public EncriptacionDecorador(INotificador envoltorio) : base(envoltorio) { }

    public override void Enviar(string mensaje)
    {
        string mensajeEncriptado = Encriptar(mensaje);
        base.Enviar(mensajeEncriptado);
    }

    private string Encriptar(string mensaje)
    {
        // Lógica de encriptación (ejemplo simple)
        char[] caracteres = mensaje.ToCharArray();
        Array.Reverse(caracteres);
        return new string(caracteres);
    }
}