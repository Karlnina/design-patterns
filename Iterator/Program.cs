/// <summary>
/// Ejemplo de patrón Iterator para recorrer una colección de canciones en una playlist.
/// </summary>

var playlist = new Playlist();
playlist.AddSong(new Song("Bohemian Rhapsody", "Rock"));
playlist.AddSong(new Song("Imagine", "Pop"));
playlist.AddSong(new Song("Smells Like Teen Spirit", "Grunge"));

Console.WriteLine("Recorrido normal:");
foreach (var song in playlist)
{
    Console.WriteLine(song);
}

playlist.Reverse();

Console.WriteLine("\nRecorrido inverso:");
var reverseIter = playlist.GetReverseEnumerator();
while (reverseIter.MoveNext())
    Console.WriteLine($"{reverseIter.Current} (Iterador Inverso)");

Console.WriteLine("\nRecorrido filtrado por genero 'Pop':");
var genreIter = playlist.GetGenreFilterEnumerator("Pop");
while (genreIter.MoveNext())
    Console.WriteLine($"{genreIter.Current} (Iterador Filtrado por Genero)");



// Modelo de cancion
public class Song
{
    public string Title { get; set; }
    public string Genre { get; set; }

    public Song(string title, string genre)
    {
        Title = title;
        Genre = genre;
    }

    public override string ToString()
    {
        return $"{Title} ({Genre})";
    }
}

// Coleccion: Playlist
public class Playlist : IEnumerable<Song>
{
    private List<Song> songs = new List<Song>();

    public void AddSong(Song song)
    {
        songs.Add(song);
    }

    public int Count
    {
        get { return songs.Count; }
    }

    // Metodo recorrido por IEnumerable
    public IEnumerator<Song> GetEnumerator() => new PlaylistIterator(this);

    // Implementacion para compatibilidad con IEnumerable(no generico)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Metodos adaicionales para manipular la playlist, para iteradores personalizados
    public IEnumerator<Song> GetReverseEnumerator() => new ReverseIterator(this);
    public IEnumerator<Song> GetGenreFilterEnumerator(string genre) => new GenreFilterIterator(this, genre);

    internal Song GetSongAt(int index)
    {
        return songs[index];
    }
}


// Iterador Base: Recorrido normal
public class PlaylistIterator : IEnumerator<Song>
{
    private readonly Playlist _playlist;
    private int position = -1;

    public Song Current => _playlist.GetSongAt(position);

    object System.Collections.IEnumerator.Current => Current;

    public PlaylistIterator(Playlist playlist)
    {
        this._playlist = playlist;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _playlist.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    public void Dispose()
    {
    }
}


// iterador inverso
public class ReverseIterator : IEnumerator<Song>
{
    private readonly Playlist _playlist;
    private int position;

    public Song Current => _playlist.GetSongAt(position);

    object System.Collections.IEnumerator.Current => Current;

    public ReverseIterator(Playlist playlist)
    {
        this._playlist = playlist;
        this.position = _playlist.Count; // Iniciar al final de la lista
    }

    public bool MoveNext()
    {
        position--;
        return (position >= 0);
    }

    public void Reset()
    {
        position = _playlist.Count; // Reiniciar al final de la lista
    }

    public void Dispose()
    {
    }
}

// Iterador  con filtro por genero
public class GenreFilterIterator : IEnumerator<Song>
{
    private readonly Playlist _playlist;
    private readonly string _genre;
    private int position = -1;

    public Song Current => _playlist.GetSongAt(position);

    object System.Collections.IEnumerator.Current => Current;

    public GenreFilterIterator(Playlist playlist, string genre)
    {
        this._playlist = playlist;
        this._genre = genre;
    }

    public bool MoveNext()
    {
        do
        {
            position++;
            if (position >= _playlist.Count)
                return false;
        } while (_playlist.GetSongAt(position).Genre != _genre);

        return true;
    }

    public void Reset()
    {
        position = -1;
    }

    public void Dispose()
    {
    }
}
