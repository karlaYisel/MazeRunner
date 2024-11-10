using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.GameSystem;

namespace MazeRunner.Core.MazeGenerator
{
    public class Maze
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public Player[] Players { get; private set;}
        public bool IsCoopGame { get; private set; }
        public int InitialNumberOfObstacles {get; private set;}
        public int InitialNumberOfTraps {get; private set;}
        public int InitialNumberOfNPC {get; private set;}
        public int ActualNumberOfObstacles {get; private set;}
        public int ActualNumberOfTraps {get; private set;}
        public int ActualNumberOfNPC {get; private set;}
        public int ActualNumberOfInteractives {get; private set;}
        public Cell[,] Grid {get; private set;} 
        private Random random = new Random();
        
        public Maze(int Width, int Height, int numberOfPlayers, bool isCoop, int numberOfObstacles, int numberOfTraps, int numberOfNPC)
        {
            this.Width = Width;
            this.Height = Height;
            this.Players = new Player[numberOfPlayers + 1];
            this.IsCoopGame = isCoop;
            this.InitialNumberOfObstacles = numberOfObstacles;
            this.InitialNumberOfTraps = numberOfTraps;
            this.InitialNumberOfNPC = numberOfNPC;
            this.ActualNumberOfObstacles = 0;
            this.ActualNumberOfTraps = 0;
            this.ActualNumberOfNPC = 0;
            this.ActualNumberOfInteractives = 0;
            this.Grid = new Cell[Width, Height];
            this.InitializeGridAndPlayers();
            this.GenerateMaze();
            this.TryGenerateInteractiveObjects(numberOfObstacles, numberOfTraps, numberOfNPC);
        }
        
        private void InitializeGridAndPlayers()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Grid[x, y] = new Cell(x, y);
                }
            }
            string name;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                name = "Jugador " + (i + 1).ToString();
                if (i == this.Players.Count() - 1) { name = "NPCs"; }
                this.Players[i] = new Player(name);
            }
        }

        public void RegenerateMaze()
        {
            this.ActualNumberOfInteractives = 0;
            this.ActualNumberOfObstacles = 0;
            this.ActualNumberOfTraps = 0;
            this.ActualNumberOfNPC = 0;
            this.Players[this.Players.Count() - 1].ChangeTokens(new List<Character>());
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
                            if (random.Next(0, 4) == 0)
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

        public List<Character> GetPossibleOponents(int x, int y, Player player, TypeOfAttack typeOfAttack)
        {
            List<Character> opponents = new List<Character>();
            if(x < this.Width && x >= 0
            && y < this.Height && y >= 0)
            {
                List<Character> possibleOponents = new List<Character>();
                List<(Cell cell, int distance)> cells;
                List<Cell> Cells = new List<Cell>();

                switch (typeOfAttack)
                {
                    case TypeOfAttack.Short:
                        Cell initialCell = this.Grid[x, y];
                        possibleOponents.AddRange(GetCharactersInCell(initialCell));
                        cells = GetCellsInRange(x, y, 1);
                        foreach ((Cell cell, int distance) cell in cells)
                        {
                            possibleOponents.AddRange(GetCharactersInCell(cell.cell));
                        }
                        break;
                    case TypeOfAttack.Large:
                        cells = GetCellsInRange(x, y, 2);
                        foreach ((Cell cell, int distance) cell in cells)
                        {
                            possibleOponents.AddRange(GetCharactersInCell(cell.cell));
                        }
                        break;
                    case TypeOfAttack.Distance:
                        Cells.AddRange(GetCellsAtDistance(x, y, 2));
                        Cells.AddRange(GetCellsAtDistance(x, y, 3));
                        foreach (Cell cell in Cells)
                        {
                            possibleOponents.AddRange(GetCharactersInCell(cell));
                        }
                        break;
                    default:
                    break;
                }
                foreach (Character character in possibleOponents)
                {
                    if (this.IsCoopGame)
                    {
                        switch (Array.IndexOf(this.Players, player))
                        {
                            case 0 or 2:
                                if (!(this.Players[0].Tokens.Contains(character)) && !(this.Players[2].Tokens.Contains(character)))
                                {
                                    opponents.Add(character);
                                }
                                break;
                            case 1 or 3:
                                if (!(this.Players[1].Tokens.Contains(character)) && !(this.Players[3].Tokens.Contains(character)))
                                {
                                    opponents.Add(character);
                                }
                                break;
                            default:
                                opponents.Add(character);
                                break;
                        }
                    }
                    else if (player == this.Players[(this.Players.Count() - 1)] || !(player.Tokens.Contains(character)))
                    {
                        opponents.Add(character);
                        continue;
                    }
                }
            }
            return opponents;
        }

        public List<Character> GetCharactersInCell(Cell cell)
        {
            List<Character> characters = new List<Character>();
            foreach (Player player in this.Players)
            {
                if (player is not null)
                {
                    foreach (Character character in player.Tokens)
                    {
                        if (character is not null && character.X == cell.X && character.Y == cell.Y) {characters.Add(character); }
                    }
                }
            }
            return characters;
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
                            if (GetCharactersInCell(neighbor).Count() > 1 || (neighbor.Interactive is Obstacle obs && obs.ActualState == State.Active && obs.Delay == 5))
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

        public List<Cell> GetCellsAtDistance (int cellX, int cellY, int distance)
        {
            List<Cell> cells = new List<Cell>();
            if(cellX < this.Width && cellX >= 0
            && cellY < this.Height && cellY >= 0)
            {
                List<(int x, int y)> possibleCells = new List<(int, int)> ();
                int y;
                for (int x = 0; x <= distance; x++)
                {
                    y = distance - x;
                    possibleCells.Add((cellX + x, cellY + y));
                    if (y > 0) {possibleCells.Add((cellX + x, cellY - y)); }
                    if (x > 0) {possibleCells.Add((cellX - x, cellY + y)); }
                    if (x > 0 && y > 0) {possibleCells.Add((cellX - x, cellY - y)); }
                }
                foreach ((int x, int y) possibleCell in possibleCells)
                {
                    if(possibleCell.x < this.Width && possibleCell.x >= 0
                    && possibleCell.y < this.Height && possibleCell.y >= 0)
                    {
                        cells.Add(this.Grid[possibleCell.x, possibleCell.y]);
                    }
                }
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
            int numberOfTokens = 0;
            foreach (Player player in this.Players)
            {
                if (player != this.Players[this.Players.Count() - 1]) {numberOfTokens += player.Tokens.Count(); }
            }
            if (NewActualNumberOfInteractives + numberOfTokens > this.Width*this.Height) 
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

                        if (actualCell.Interactive is null && GetCharactersInCell(actualCell).Count() == 0) {emptyCells.Add(actualCell);}
                    }
                }
                foreach (string typeName in Enum.GetNames(typeof(TypeOfInteractive)))
                {
                    int number;
                    string? enumName;
                    switch (typeName)
                    {
                        case "Obstacle":
                            number = numberOfObstacles;
                            enumName = "MazeRunner.Core.InteractiveObjects.TypeOfObstacle";
                            break;
                        case "Trap":
                            number = numberOfTraps;
                            enumName = "MazeRunner.Core.InteractiveObjects.TypeOfTrap";
                            break;
                        case "Character":
                            number = numberOfNPC;
                            enumName = "MazeRunner.Core.InteractiveObjects.TypeOfNPC";
                            break;
                        default:
                            number = 0;
                            enumName = "";
                            break;
                    }
                    Type? enumType = Type.GetType(enumName);
                    if (enumType is null) {continue; }
                    classes = Enum.GetNames(enumType);
                    if (classes is null) {continue; }
                    for (int i = 0; i < number; i++)
                    {
                        actualCell = emptyCells[random.Next(0, emptyCells.Count())];
                        actualCell.Interactive = CreateRandomInteractive(classes);
                        if (actualCell.Interactive is NPC npc)
                        {
                            npc.ChangePosition(actualCell.X, actualCell.Y);
                            List<Character> newTokens = this.Players[Players.Count() - 1].Tokens;
                            newTokens.Add(npc);
                            this.Players[Players.Count() - 1].ChangeTokens(newTokens);
                            actualCell.Interactive = null;
                        }
                        emptyCells.Remove(actualCell);
                    }
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