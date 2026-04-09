/// <summary>
/// Ejemplo de patrón Composite en C# para representar un sistema de archivos con carpetas y archivos.
/// </summary>

Carpeta raiz = new Carpeta("C:\\");
Carpeta misDocumentos = new Carpeta("MisDocumentos");
Carpeta fotos = new Carpeta("Fotos");

misDocumentos.Agregar(new Archivo("Curriculum.pdf", 200));
misDocumentos.Agregar(new Archivo("Proyecto.docx", 500));
fotos.Agregar(new Archivo("Vacaciones.jpg", 1500));

raiz.Agregar(misDocumentos);
raiz.Agregar(fotos);
raiz.Agregar(new Archivo("Config.sys", 3));

Console.WriteLine("====== Estructura del sistema de archivos:");

raiz.Mostrar();

Console.WriteLine($"\nTamaño total del sistema de archivos: {raiz.GetTamaño()} KB");

//------------------------------------------------------------------------------------------

// 1. Interface comun (componente)
public interface INodoSistema
{
    void Mostrar(int profundidad = 0);
    int GetTamaño();
}

// 2. Componente hoja: archivo
public class Archivo : INodoSistema
{
    private string nombre;
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    private int tamaño;
    public int Tamaño
    {
        get { return tamaño; }
        set { tamaño = value; }
    }

    public Archivo(string nombre, int tamaño)
    {
        this.nombre = nombre;
        this.tamaño = tamaño;
    }

    public int GetTamaño()
    {
        return tamaño;
    }

    public void Mostrar(int profundidad = 0)
    {
       Console.WriteLine($"{new string('-', profundidad)} Archivo: {nombre} (Tamaño: {tamaño} KB)");
    }
}

// 3. Componente compuesto: carpeta
public class Carpeta : INodoSistema
{
    private string nombre;
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    private readonly List<INodoSistema> _hijos = new List<INodoSistema>();

    public Carpeta(string nombre)
    {
        this.nombre = nombre;
    }

    public void Agregar(INodoSistema nodo)
    {
        if (nodo is Carpeta carpeta && carpeta == this)
        {
            throw new InvalidOperationException("No se puede agregar una carpeta a sí misma.");
        }

        _hijos.Add(nodo);
    }

    public void Mostrar(int profundidad = 0)
    {
        Console.WriteLine($"{new string('-', profundidad)} Carpeta: {nombre}");
        foreach (var nodo in _hijos)
        {
            nodo.Mostrar(profundidad + 2);
        }
    }

    public int GetTamaño()
    {
        // int tamañoTotal = 0;
        // foreach (var nodo in _hijos)
        // {
        //     tamañoTotal += nodo.GetTamaño();
        // }
        int combo = _hijos.Sum(nodo => nodo.GetTamaño());
        // int descuento = (int)(combo * 0.1); // Aplicar un descuento del 10% al tamaño total
        // combo -= descuento; // Restar el descuento al tamaño total
        return combo;
    }
}




