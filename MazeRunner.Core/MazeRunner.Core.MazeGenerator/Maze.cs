namespace MazeRunner.Core.MazeGenerator
{
    public class Maze
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public Cell[,] Grid {get; private set;} 
        private Random random = new Random();
        
        public Maze(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            this.Grid = new Cell[Width, Height];
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

        public void RegenerateMaze()
        {
            CleanCells();
            RegenerateWalls();
            GenerateMaze();
        }

        public void CleanCells()
        {
            Cell cell;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cell = Grid[x, y];
                    cell.Interactive = null;
                    if (cell.IsColored == true) cell.ChangeColorStatus();
                }
            }
        }
        
        public void RegenerateWalls()
        {
            Cell cell;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cell = Grid[x, y];
                    cell.isVisited = false;
                    cell.Walls["top"] = cell.Walls["right"] = cell.Walls["bottom"] = cell.Walls["left"] = true;
                }
            }
        }
    
        public void CleanWalls()
        {
            Cell cell;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cell = Grid[x, y];
                    cell.isVisited = true;
                    cell.Walls["top"] = cell.Walls["right"] = cell.Walls["bottom"] = cell.Walls["left"] = false;
                }
            }
        }

        private void GenerateMaze()
        {
            Cell start = Grid[0, 0];
            var stack = new Stack<Cell>();
            stack.Push(start);
            start.isVisited = true;
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var neighbors = GetUnvisitedNeighbors(current);
                if (neighbors.Count > 0)
                {
                    stack.Push(current);
                    var neighbor = neighbors[random.Next(neighbors.Count)];
                    this.BreakWall(current, neighbor);
                    neighbor.isVisited = true;
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
                            if (random.Next(0, 10) == 0)
                            {
                                var neighbor = neighbors[random.Next(0, neighbors.Count)];
                                this.BreakWall(current, neighbor);
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
            if (!IsOfThisMaze(cell)) return neighbors;
            if (cell.X > 0 && !Grid[cell.X - 1, cell.Y].isVisited) neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && !Grid[cell.X + 1, cell.Y].isVisited) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && !Grid[cell.X, cell.Y - 1].isVisited) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && !Grid[cell.X, cell.Y + 1].isVisited) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }
    
        public List<Cell> GetNeighborsWithWall(Cell cell)
        {
            var neighbors = new List<Cell>();
            if (!IsOfThisMaze(cell)) return neighbors;
            if (cell.X > 0 && cell.Walls["left"])  neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && cell.Walls["right"]) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && cell.Walls["top"]) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && cell.Walls["bottom"]) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }

        public List<Cell> GetNeighborsWithoutWall(Cell cell)
        {
            var neighbors = new List<Cell>();
            if (!IsOfThisMaze(cell)) return neighbors;
            if (cell.X > 0 && !cell.Walls["left"])  neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && !cell.Walls["right"]) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && !cell.Walls["top"]) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && !cell.Walls["bottom"]) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }
    
        public void BreakWall(Cell cell, Cell otherCell)
        {
            if (!IsOfThisMaze(cell) || !IsOfThisMaze(otherCell)) return;
            if (cell.X == otherCell.X || cell.Y == otherCell.Y)
            {
                if (cell.X == otherCell.X + 1) {cell.Walls["left"] = false; otherCell.Walls["right"] = false;}
                else if (cell.X == otherCell.X - 1) {cell.Walls["right"] = false; otherCell.Walls["left"] = false;}
                else if (cell.Y == otherCell.Y + 1) {cell.Walls["top"] = false; otherCell.Walls["bottom"] = false;}
                else if (cell.Y == otherCell.Y - 1) {cell.Walls["bottom"] = false; otherCell.Walls["top"] = false;}
            }
        }

        public bool IsOfThisMaze (Cell cell)
        {
            if (cell.X >= 0 && cell.Y >= 0
            &&  cell.X < this.Width && cell.Y < this.Height
            &&  cell.Equals(this.Grid[cell.X, cell.Y])) return true;
            return false;
        }
    }
}