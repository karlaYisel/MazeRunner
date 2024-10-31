namespace MazeRuner.Core.Maze;

public class Cell
{
    internal int X;
    internal int Y;
    internal bool Visited;
    internal Dictionary<string, bool> Walls = new Dictionary<string, bool>
    {
        { "top", true },
        { "right", true },      
        { "bottom", true },
        { "left", true }
    };

    //TODO: La clase de interactuables en lugar de usar objetos
    internal object? Interactive;

    internal Cell(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        this.Visited = false;
        this.Interactive = null;
    }
}
