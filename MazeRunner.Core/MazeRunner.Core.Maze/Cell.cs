namespace MazeRunner.Core.Maze
{
    public class Cell
    {
        public int X {get; private set;} 
        public int Y {get; private set;}
        internal bool isVisited;
        public Dictionary<string, bool> Walls = new Dictionary<string, bool>
        {
            { "top", true },
            { "right", true },      
            { "bottom", true },
            { "left", true }
        };
    
        //TODO: La clase de interactuables en lugar de usar objetos
        internal object? Interactive {get; private set;} 
    
        public Cell(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            this.isVisited = false;
            this.Interactive = null;
        }
    }
}
