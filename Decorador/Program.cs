
// Ejemplo de patrón Decorador en C#
INotificador notificador = new EncriptacionDecorador(
    new LogNotificadorDecorador(
        new NotificadorBase()
    )
);
notificador.Enviar("Enviado mensaje desde un nivel de decorador!");


// 1. Interfaz comun
public interface INotificador
{
    void Enviar(string mensaje);
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