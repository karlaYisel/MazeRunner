using MazeRunner.Core.InteractiveObjects;

namespace MazeRunner.Core.MazeGenerator
{
    public class Maze
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        //Añadir array jugadores los jugadores contienne sus fichas, metodo obtener fichas en casilla
        public int InitialNumberOfObstacles {get; private set;}
        public int InitialNumberOfTraps {get; private set;}
        public int InitialNumberOfNPC {get; private set;}
        public int ActualNumberOfObstacles {get; private set;}
        public int ActualNumberOfTraps {get; private set;}
        public int ActualNumberOfNPC {get; private set;}
        public int ActualNumberOfInteractives {get; private set;}
        public Cell[,] Grid {get; private set;} 
        private Random random = new Random();
        
        public Maze(int Width, int Height, int numberOfObstacles, int numberOfTraps, int numberOfNPC)
        {
            this.Width = Width;
            this.Height = Height;
            this.InitialNumberOfObstacles = numberOfObstacles;
            this.InitialNumberOfTraps = numberOfTraps;
            this.InitialNumberOfNPC = numberOfNPC;
            this.ActualNumberOfObstacles = 0;
            this.ActualNumberOfTraps = 0;
            this.ActualNumberOfNPC = 0;
            this.ActualNumberOfInteractives = 0;
            this.Grid = new Cell[Width, Height];
            this.InitializeGrid();
            this.GenerateMaze();
            this.TryGenerateInteractiveObjects(numberOfObstacles, numberOfTraps, numberOfNPC);
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
            this.ActualNumberOfInteractives = 0;
            this.ActualNumberOfObstacles = 0;
            this.ActualNumberOfTraps = 0;
            this.ActualNumberOfNPC = 0;
            Cell cell;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cell = Grid[x, y];
                    cell.Interactive = null;
                    cell.isVisited = false;
                    cell.Walls["top"] = cell.Walls["right"] = cell.Walls["bottom"] = cell.Walls["left"] = true;
                }
            }

            this.GenerateMaze();
            this.TryGenerateInteractiveObjects(InitialNumberOfObstacles, InitialNumberOfTraps, InitialNumberOfNPC);
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
                    this.BreakWall(current.X, current.Y, neighbor.X, neighbor.Y);
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
                            if (random.Next(0, 2) == 0)
                            {
                                var neighbor = neighbors[random.Next(0, neighbors.Count)];
                                this.BreakWall(current.X, current.Y, neighbor.X, neighbor.Y);
                                continue;
                            }
                        }  
                        break;
                    }
                }
            }
        }

        //Para Maze tambien pero cuando tenga los jugadores
        public List<Character> GetPossibleOpenents(Maze maze)
        {
            return new List<Character>();
        }
        
        public Stack<Cell> GetOptimalPath(int initialX, int initialY, int destinyX, int destinyY, int distance)
        {
            Stack<Cell> cellPath = new Stack<Cell>();
            if (distance > 0 
            && initialX < this.Width && initialX >= 0
            && initialY < this.Height && initialY >= 0
            && destinyX < this.Width && destinyX >= 0
            && destinyY < this.Height && destinyY >= 0
            && (initialX != destinyX || initialY != destinyY)) 
            {
                List<(Cell cell, int distance)> cells = GetCellsInRange(initialX, initialY, distance);
                Cell destinyCell = this.Grid[destinyX, destinyY];
                distance = 0;
                foreach ((Cell cell, int distance) cellWithDistance in cells)
                {
                    if (cellWithDistance.cell.Equals(destinyCell))
                    {
                        distance = cellWithDistance.distance;
                        break;
                    }
                }
                if (distance > 0)
                {
                    cellPath.Push(destinyCell);
                    int delay;
                    List<Cell> neighbors;
                    while (distance > 0)
                    {
                        destinyCell = cellPath.Pop();
                        cellPath.Push(destinyCell);
                        neighbors = GetNeighborsWithoutWall(destinyCell);
                        if (destinyCell.Interactive is Obstacle obstacle && obstacle.ActualState == State.Active)
                        {
                            delay = obstacle.Delay;
                        }
                        else
                        {
                            delay = 1;
                        }
                        distance -= delay;
                        foreach (Cell neighbor in neighbors)
                        {
                            if (cells.Contains((neighbor, distance)))
                            {
                                cellPath.Push(neighbor);
                                break;
                            }
                        }
                    }
                }
            }
            return cellPath;
        }

        public List<(Cell, int)> GetCellsInRange(int cellX, int cellY, int distance)
        {
            List<(Cell cell, int distance)> cells = new List<(Cell, int)>();
            if (distance > 0 
            && cellX < this.Width && cellX >= 0
            && cellY < this.Height && cellY >= 0) 
            { 
                cells.Add((this.Grid[cellX, cellY], 0));
                List<Cell> cellsWithi = new List<Cell>();
                List<Cell> actualCells = new List<Cell>();
                List<Cell> neighbors = new List<Cell>();
                int delay;
                for (int i = 0; i < distance; i++)
                {
                    foreach ((Cell cell, int distance) cellWithDistance in cells)
                    {
                        if (!actualCells.Contains(cellWithDistance.cell)) {actualCells.Add(cellWithDistance.cell); }
                        if (cellWithDistance.distance == i) {cellsWithi.Add(cellWithDistance.cell); }
                    }
                    foreach (Cell cell in cellsWithi)
                    {
                        neighbors = GetNeighborsWithoutWall(cell);
                        foreach (Cell neighbor in neighbors)
                        {
                            //Añadir con metodo para que sea solo si ya hay 2 personajes
                            if (neighbor.Interactive is Character character || (neighbor.Interactive is Obstacle obs && obs.ActualState == State.Active && obs.Delay == 5))
                            {
                                continue;
                            }
                            else if (neighbor.Interactive is Obstacle obstacle && obstacle.ActualState == State.Active)
                            {
                                delay  = obstacle.Delay;
                            }
                            else
                            {
                                delay = 1;
                            }
                            foreach ((Cell cell, int distance) cellWithDistance in cells)
                            {
                                if (cellWithDistance.cell.Equals(neighbor) && i + delay <= distance && cellWithDistance.distance > i + delay)
                                {
                                    cells.Add((neighbor, i + delay));
                                    cells.Remove(cellWithDistance);
                                }
                            }
                        }
                    }
                    cellsWithi.Clear();
                } 
                cells.Remove((this.Grid[cellX, cellY], 0));
            }
            return cells;
        }
    
        private List<Cell> GetUnvisitedNeighbors(Cell cell)
        {
            var neighbors = new List<Cell>();
            if (cell.X > 0 && !Grid[cell.X - 1, cell.Y].isVisited) neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && !Grid[cell.X + 1, cell.Y].isVisited) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && !Grid[cell.X, cell.Y - 1].isVisited) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && !Grid[cell.X, cell.Y + 1].isVisited) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }
    
        private List<Cell> GetNeighborsWithWall(Cell cell)
        {
            var neighbors = new List<Cell>();
            if (cell.X > 0 && cell.Walls["left"])  neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && cell.Walls["right"]) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && cell.Walls["top"]) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && cell.Walls["bottom"]) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }

        private List<Cell> GetNeighborsWithoutWall(Cell cell)
        {
            var neighbors = new List<Cell>();
            if (cell.X > 0 && !cell.Walls["left"])  neighbors.Add(Grid[cell.X - 1, cell.Y]);
            if (cell.X < Width - 1 && !cell.Walls["right"]) neighbors.Add(Grid[cell.X + 1, cell.Y]);
            if (cell.Y > 0 && !cell.Walls["top"]) neighbors.Add(Grid[cell.X, cell.Y - 1]);
            if (cell.Y < Height - 1 && !cell.Walls["bottom"]) neighbors.Add(Grid[cell.X, cell.Y + 1]);
            return neighbors;
        }
    
        public void BreakWall(int cellX, int cellY, int otherCellX, int otherCellY)
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
                    else if (cellY == otherCellY + 1) {cell.Walls["top"] = false; otherCell.Walls["bottom"] = false;}
                    else if (cellY == otherCellY - 1) {cell.Walls["bottom"] = false; otherCell.Walls["top"] = false;}
                }
            }
        }

        public bool TryGenerateInteractiveObjects(int numberOfObstacles, int numberOfTraps, int numberOfNPC)
        {
            int NewActualNumberOfInteractives = ActualNumberOfInteractives + numberOfObstacles + numberOfTraps + numberOfNPC;
            //Considerar adeemas las fichas de los jugadores
            if (NewActualNumberOfInteractives > this.Width*this.Height) 
            {
                return false;
            }
            else
            {
                int numberOfInteractives = NewActualNumberOfInteractives - ActualNumberOfInteractives;
                this.ActualNumberOfObstacles += numberOfObstacles;
                this.ActualNumberOfTraps += numberOfTraps;
                this.ActualNumberOfInteractives = NewActualNumberOfInteractives;
                string[] classes = [];
                List<Cell> emptyCells = [];
                Cell actualCell;
                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        actualCell  = this.Grid[x, y];

                        if (actualCell.Interactive is null) {emptyCells.Add(actualCell);}
                    }
                }
                //foreach TypeOfInteractive?
                //A;adir nuevos tipos
                for ( int i = 0; i < numberOfInteractives; i++)
                {
                    actualCell = emptyCells[random.Next(0, emptyCells.Count)];
                    if (i < numberOfObstacles)
                    {
                        classes = Enum.GetNames(typeof(TypeOfObstacle));
                    }
                    else if (i < numberOfObstacles + numberOfTraps)
                    {
                        classes = Enum.GetNames(typeof(TypeOfTrap));
                    }
                    else
                    {
                        //tener en cuenta de ponerlo con los jugadores NPC
                        classes = Enum.GetNames(typeof(TypeOfNPC));
                        //Darle la posicion de la celda al NPC y quitar la casilla sin asignarle un Interactive
                    }
                    actualCell.Interactive = CreateRandomInteractive(classes);
                    emptyCells.Remove(actualCell);
                }

                return true;
            }
        }

        private Interactive? CreateRandomInteractive(string[] classes)
        {
            string randomClass = classes[random.Next(0, classes.Length)];
    
            string nameClass = $"MazeRunner.Core.InteractiveObjects.{randomClass}";
            Type? typeClass = Type.GetType(nameClass);
    
            if (typeClass != null)
            {
                object[] parameter = [];
    
                switch (randomClass)
                {
                    case "TemporalWall":
                        parameter = [random.Next(1, 6)]; 
                        break;
                    case "SpikeTrap":
                        parameter = [random.Next(4, 11)]; 
                        break;
                    case "Passive" or "Neutral" or  "Aggressive":
                        parameter = [0, 0, random.Next(40, 76), random.Next(5, 9), random.Next(5, 9), random.Next(5, 9), random.Next(2, 6)]; 
                        break;
                    default:
                        break;
                }
    
                return Activator.CreateInstance(typeClass, parameter) as Interactive;
            }
    
            return null;
        }
    }
}