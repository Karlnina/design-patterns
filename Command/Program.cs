/// <summary>
/// Ejemplo del patrón Command en C# para controlar una luz con un control remoto.
/// </summary>
Console.WriteLine("Ejemplo del patrón Command en C# para controlar una luz con un control remoto.");
 var controlRemoto = new ControlRemoto();

 // Encender la luz
controlRemoto.SetComando(new EncenderLuzCommand());
controlRemoto.PresionarBoton();

// Apagar la luz
controlRemoto.SetComando(new ApagarLuzCommand());
controlRemoto.PresionarBoton();

// Registrar un log
controlRemoto.SetComando(new RegistrarLogCommand("Se ha realizado una acción en la luz"));
controlRemoto.PresionarBoton();


/// <summary>
/// ------------------------------------
/// </summary>

// Interface base
public interface ICommand
{
    void Ejecutar();
}

// Comando concreto: encender la luz
public class EncenderLuzCommand : ICommand
{
    public void Ejecutar()
    {
        Console.WriteLine("La luz se ha encendido.");
    }
}

// Comando concreto: apagar la luz
public class ApagarLuzCommand : ICommand
{
    public void Ejecutar()
    {
        Console.WriteLine("La luz se ha apagado.");
    }
}

// Comando concreto: registrar logs
public class RegistrarLogCommand : ICommand
{
    private string _mensaje;

    public RegistrarLogCommand(string mensaje)
    {
        _mensaje = mensaje;
    }

    public void Ejecutar()
    {
        Console.WriteLine($"Log: {_mensaje} registrado a horas {DateTime.Now}.");
    }
}

// Invocador: control remoto
public class ControlRemoto
{
    private ICommand _comando;

    public void SetComando(ICommand comando)
    {
        _comando = comando;
    }

    public void PresionarBoton()
    {
        _comando.Ejecutar();
    }
}



