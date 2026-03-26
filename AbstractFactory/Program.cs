// Programa de ejemplo para el patrón Abstract Factory

string configuracion = "AWS"; // Cambia a "Azure" para usar Azure

Console.WriteLine("=== Cliente usando AWS ===");
ICloudFactory awsFactory = new AWSFactory();
Console.WriteLine("=== Cliente usando Azure ===");
ICloudFactory azureFactory = new AzureFactory();

if(configuracion == "AWS")
{
    ClienteCloud clienteAWS = new ClienteCloud(awsFactory);
    clienteAWS.Ejecutar("datos_aws.txt");
}
else
{
    ClienteCloud clienteAzure = new ClienteCloud(azureFactory);
    clienteAzure.Ejecutar("datos_azure.txt");
}

// ClienteCloud clienteAWS = new ClienteCloud(awsFactory);
// clienteAWS.Ejecutar("datos_aws.txt");

// ClienteCloud clienteAzure = new ClienteCloud(azureFactory);
// clienteAzure.Ejecutar("datos_azure.txt");

// -----------------------

// Productos/servicios abstractos
public interface IAlmacenamiento
{
    void GuardarArchivo(string nombre);
}

public interface IComputacion
{
    void EjecutarTarea();
}

// Familia AWS
public sealed class AWSAlmacenamiento : IAlmacenamiento
{
    public void GuardarArchivo(string nombre)
    {
        Console.WriteLine($"Guardando {nombre} en AWS S3bucket.");
    }
}

public sealed class AWSComputacion : IComputacion
{
    public void EjecutarTarea()
    {
        Console.WriteLine("Ejecutando tarea en AWS EC2(maquina virtual).");
    }
}

// Familia Azure
public sealed class AzureAlmacenamiento : IAlmacenamiento
{
    public void GuardarArchivo(string nombre)
    {
        Console.WriteLine($"Guardando {nombre} en Azure Blob Storage.");
    }
}

public sealed class AzureComputacion : IComputacion
{
    public void EjecutarTarea()
    {
        Console.WriteLine("Ejecutando tarea en Azure Virtual Machine.");
    }
}

// Fábrica abstracta
public interface ICloudFactory
{
    IAlmacenamiento CrearAlmacenamiento();
    IComputacion CrearComputacion();
}

// Fábricas concretas
public sealed class AWSFactory : ICloudFactory
{
    public IAlmacenamiento CrearAlmacenamiento()
    {
        return new AWSAlmacenamiento();
    }

    public IComputacion CrearComputacion()
    {
        return new AWSComputacion();
    }
}

// Fábrica concreta Azure
public sealed class AzureFactory : ICloudFactory
{
    public IAlmacenamiento CrearAlmacenamiento()
    {
        return new AzureAlmacenamiento();
    }

    public IComputacion CrearComputacion()
    {
        return new AzureComputacion();
    }
}

// Cliente
public class ClienteCloud
{
    private readonly IAlmacenamiento _almacenamiento;
    private readonly IComputacion _computacion;

    public ClienteCloud(ICloudFactory factory)
    {
        _almacenamiento = factory.CrearAlmacenamiento();
        _computacion = factory.CrearComputacion();
    }

    public void Ejecutar(string nombreArchivo)
    {
        _almacenamiento.GuardarArchivo(nombreArchivo);
        _computacion.EjecutarTarea();
    }
}

