namespace MazeRuner.Core.Maze;

public class Maze
{
    internal int  Width;
    internal int Height;
    internal Cell[,] Grid;
    private Random Random = new Random();

    protected Maze(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Grid = new Cell[width, height];
        this.InitializeGrid();
        this.GenerateMaze();
    }
    private void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Grid[x, y] = new Cell(x, y);
            }
        }
    }

    internal void GenerateMaze()
    {
        Cell Start = Grid[0, 0];
        var stack = new Stack<Cell>();
        stack.Push(Start);
        Start.Visited = true;
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                stack.Push(current);
                var neighbor = neighbors[Random.Next(neighbors.Count)];
                this.BreakWall(current.X, current.Y, neighbor.X, neighbor.Y);
                neighbor.Visited = true;
                stack.Push(neighbor);
            }
            else
            {
                //Generates the probability of break walls in the last cell before a backtracking
                while (true)
                {
                    neighbors = GetNeighborsWithWall(current);
                    if (neighbors.Count > 0)
                    {
                        if (Random.Next(0, 1) == 0)
                        {
                            var neighbor = neighbors[Random.Next(0, neighbors.Count - 1)];
                            this.BreakWall(current.X, current.Y, neighbor.X, neighbor.Y);
                            continue;
                        }
                    }  
                    break;
                }
            }
        }
    }

    private List<Cell> GetUnvisitedNeighbors(Cell cell)
    {
        var neighbors = new List<Cell>();
        if (cell.X > 0 && !Grid[cell.X - 1, cell.Y].Visited) neighbors.Add(Grid[cell.X - 1, cell.Y]);
        if (cell.X < Width - 1 && !Grid[cell.X + 1, cell.Y].Visited) neighbors.Add(Grid[cell.X + 1, cell.Y]);
        if (cell.Y > 0 && !Grid[cell.X, cell.Y - 1].Visited) neighbors.Add(Grid[cell.X, cell.Y - 1]);
        if (cell.Y < Height - 1 && !Grid[cell.X, cell.Y + 1].Visited) neighbors.Add(Grid[cell.X, cell.Y + 1]);
        return neighbors;
    }

    private List<Cell> GetNeighborsWithWall(Cell cell)
    {
        var neighbors = new List<string>();
        if (cell.X > 0 && cell.Walls["left"])  neighbors.Add(Grid[cell.X - 1, cell.Y]);
        if (cell.X < Width - 1 && cell.Walls["right"]) neighbors.Add(Grid[cell.X + 1, cell.Y]);
        if (cell.Y > 0 && cell.Walls["top"]) neighbors.Add(Grid[cell.X, cell.Y - 1]);
        if (cell.Y < Height - 1 && cell.Walls["bottom"]) neighbors.Add(Grid[cell.X, cell.Y + 1]);
        return neighbors;
    }

    internal void BreakWall(int cellX, int cellY, int otherCellX, int otherCellY)
    {
        if (cellX < this.Width && cellX >= 0 
        && otherCellX < this.Width && otherCellX >= 0
        && cellY < this.Height && cellY >= 0 
        && otherCellY < this.Height && otherCellY >= 0)
        {
            if (cellX == otherCellX || cellY == otherCellY)
            {
                Cell cell =  this.Grid[cellX, cellY];
                Cell otherCell = this.Grid[otherCellX, otherCellY];

                if (cellX == otherCellX + 1) {cell.Walls["left"] = false; otherCell.Walls["right"] = false;}
                else if (cellX == otherCellX - 1) {cell.Walls["right"] = false; otherCell.Walls["left"] = false;}
                else if (cellY == otherCellY + 1) {cell.Walls["bottom"] = false; otherCell.Walls["top"] = false;}
                else if (cellY == otherCellY - 1) {cell.Walls["top"] = false; otherCell.Walls["bottom"] = false;}
            }
        }
    }
}