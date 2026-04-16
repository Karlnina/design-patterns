/// <summary>
/// Ejemplo de implementación del patrón Template Method en C#. En este ejemplo, se define una clase abstracta GenerarReporte que contiene el método template Generar
/// </summary>
Console.WriteLine("=== Patrón Template Method en C# ===");
GenerarReporte reporte;

reporte = new ReportePDF();
await reporte.Generar("ReporteVentas.pdf");

Console.WriteLine("\n=== Generando otro reporte ===");

Task.Delay(2000).Wait(); // Simula un tiempo de espera entre la generación de los reportes

reporte = new ReporteCSV();
await reporte.Generar("ReporteVentas.csv");

Console.WriteLine("\n=== Fin del programa ===");


// ==============================================================
// Patrón Template Method en C#
// ==============================================================

// Clase Base (Abstracta) : Define el esqueleto del algoritmo
public abstract class GenerarReporte
{
    // El metodo template - difine el Orden fijo , no se puede modificar. o sobre escribir
    public async Task Generar(string nombreArchivo)
    {
        Console.WriteLine($"Iniciando generación de reporte {nombreArchivo}...");
        await AbrirArchivo(nombreArchivo); // Paso Fijo
        await ProcesarContenido(); // Paso Variable - lo definen las subclases
        await CerrarArchivo();  // Paso Fijo

        Console.WriteLine($"Reporte {nombreArchivo} generado exitosamente.");
    }

    // Pasos fijos - no se pueden modificar iguales a todos los reportes
    private async Task AbrirArchivo(string nombreArchivo)
    {
        await Task.Delay(1000); // Simula el tiempo de apertura del archivo
        Console.WriteLine($"Archivo {nombreArchivo} abierto.");
    }

    private async Task CerrarArchivo()
    {
        await Task.Delay(1000); // Simula el tiempo de cierre del archivo
        Console.WriteLine("Archivo cerrado.");
    }

    // Paso variable - debe ser implementado por las subclases, cada subclase decide como procesar el contenido del reporte
    protected abstract Task ProcesarContenido();
}

// Implementación Concreta: Solo cambia el paso variable
public class ReportePDF : GenerarReporte
{
    protected override async Task ProcesarContenido()
    {
        await Task.Delay(2000); // Simula el tiempo de procesamiento del contenido para PDF
        Console.WriteLine("Procesando contenido para reporte PDF...");
    }
}

public class ReporteCSV : GenerarReporte
{
    protected override async Task ProcesarContenido()
    {
        await Task.Delay(3000); // Simula el tiempo de procesamiento del contenido para CSV
        Console.WriteLine("Procesando contenido para reporte CSV...");
    }
}
