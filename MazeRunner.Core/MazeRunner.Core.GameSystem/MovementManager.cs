using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;
using System.Threading;

namespace MazeRunner.Core.GameSystem
{
    public class MovementManager
    {
        private static MovementManager? _MM;
        private static readonly object _lock = new object();
        private Random random= new Random();
        private GameManager GM = GameManager.GM;
        private MovementManager()
        {}
        public static MovementManager MM
        {
            get
            {
                if (_MM is null)
                {
                    lock (_lock)
                    {
                        if (_MM is null)
                        {
                            _MM = new MovementManager();
                        }
                    }
                }
                return _MM;
            }
            
        }

//Movement's methods
        public void MakeRandomMove (Character token)
        {
            List<(Cell cell, int distance)> cells = GetCellsInRange(GM.maze.Grid[token.X, token.Y], token.Speed);
            if (random.Next(0, cells.Count + 1) == 0)
            {
                return;
            }
            else
            {
                (Cell cell, int distance) cellWithDistance = cells[random.Next(0, cells.Count)];
                MoveToken(token, cellWithDistance.cell, cellWithDistance.distance);
            }
        }

        public void MoveToken(Character token, Cell cell, int distance = 10)
        {
            int initialLife = token.CurrentLife;
            int initialBurn;
            int initialIce;
            int initialPoison;
            Stack<Cell> optimalPath = GetOptimalPath(GM.maze.Grid[token.X, token.Y], cell, distance);
            distance = optimalPath.Count;
            Queue<Cell> path = new Queue<Cell>();
            cell = GM.maze.Grid[token.X, token.Y];
            int delay;
            CleanColor();
            for (int i = 0; i < distance; i++) 
            {
                if (token.ActualState == State.Inactive || token.RemainingTurnsIced > 0) 
                {
                    CleanColor();
                    return;
                }
                ColorCells([cell]);
                path.Enqueue(cell);
                cell = optimalPath.Pop(); 
                if (cell.Interactive is Obstacle obstacle) delay = obstacle.Delay;
                else delay = 1;
                Thread.Sleep(delay*100);
                token.ChangePosition(cell.X, cell.Y);
                GM.EventChangeInMazeMade();
                token.CurrentLife -= random.Next(0, 6);
                GM.StabilizeToken(token);
                if ((initialLife != token.MaxLife && token.CurrentLife == token.MaxLife) || token.ActualState == State.Inactive) 
                {
                    GM.EventDefetedToken(token, null, 0);
                    CleanColor();
                    return;
                }
                if (initialLife > token.CurrentLife)
                {
                    GM.EventDemagedToken(token, null, initialLife - token.CurrentLife);
                }
                else if (initialLife < token.CurrentLife)
                {
                    GM.EventHealedToken(token, null, token.CurrentLife - initialLife);
                }
                GM.EventChangeInMazeMade();
                if (cell.Interactive is Trap trap && (trap.IsStandTrap == false || optimalPath.Count() == 0)) 
                {
                    switch(trap.GetType().Name)
                    {
                        case "PrisonTrap":
                            if (trap.TryToTrigger(token))
                            {
                                GM.EventMessageToken(token, trap, 1);
                                CleanColor();
                                CleanMovement(path);
                                return;
                            }
                            else
                            {
                                GM.EventMessageToken(token, trap, 0);
                            }
                            break;
                        case "SpikeTrap":
                            initialLife = token.CurrentLife;
                            if (trap.TryToTrigger(token))
                            {
                                GM.StabilizeToken(token);
                                if ((initialLife != token.MaxLife && token.CurrentLife == token.MaxLife) || token.ActualState == State.Inactive) 
                                {
                                    GM.EventDefetedToken(token, trap, 0);
                                    CleanColor();
                                    return;
                                }
                                if (initialLife > token.CurrentLife)
                                {
                                    GM.EventDemagedToken(token, trap, initialLife - token.CurrentLife);
                                }
                                else if (initialLife < token.CurrentLife)
                                {
                                    GM.EventHealedToken(token, trap, token.CurrentLife - initialLife);
                                }
                                GM.EventChangeInMazeMade();
                            }
                            else
                            {
                                GM.EventDemagedToken(token, trap, 0);
                            }
                            break;
                        case "FireTrap" or "IceTrap" or "PoisonTrap":
                            initialLife = token.CurrentLife;
                            initialBurn = token.RemainingStepsBurned;
                            initialIce = token.RemainingTurnsIced;
                            initialPoison = token.RemainingTurnsPoisoned;
                            if (trap.TryToTrigger(token))
                            {
                                GM.StabilizeToken(token);
                                if ((initialLife != token.MaxLife && token.CurrentLife == token.MaxLife) || token.ActualState == State.Inactive) 
                                {
                                    GM.EventDefetedToken(token, trap, 0);
                                    CleanColor();
                                    return;
                                }
                                if (initialLife > token.CurrentLife)
                                {
                                    GM.EventDemagedToken(token, trap, initialLife - token.CurrentLife);
                                }
                                else if (initialLife < token.CurrentLife)
                                {
                                    GM.EventHealedToken(token, trap, token.CurrentLife - initialLife);
                                }
                                if(token.RemainingStepsBurned != initialBurn)
                                {
                                    if(initialBurn == 0)
                                    {
                                        GM.EventStateAddToken(token, trap, 0);
                                    }
                                    GM.EventMessageToken(token, trap, token.RemainingStepsBurned - initialBurn);
                                }
                                if(token.RemainingTurnsIced != initialIce)
                                {
                                    GM.EventStateAddToken(token, trap, 0);
                                    GM.EventMessageToken(token, trap, token.RemainingTurnsIced - initialIce);
                                    CleanColor();
                                    return;
                                }
                                if(token.RemainingTurnsPoisoned != initialPoison)
                                {
                                    if(initialPoison == 0)
                                    {
                                        GM.EventStateAddToken(token, trap, 0);
                                    }
                                    GM.EventMessageToken(token, trap, token.RemainingTurnsPoisoned - initialPoison);
                                }
                                GM.EventChangeInMazeMade();
                            }
                            else
                            {
                                GM.EventDemagedToken(token, trap, 0);
                            }
                            break;
                    }
                }
                if(GM.EndPoints.Contains(cell))
                {
                    if (token is not NPC)
                    {
                        foreach(Player player in GM.ActivePlayers)
                        {
                            if(player.Tokens.Contains(token)) 
                            {
                                GM.EventPlayerWon(player);
                                CleanColor();
                                CleanMovement(path);
                                return;
                            }
                        }
                        foreach(Player player in GM.NonActivePlayers)
                        {
                            if(player.Tokens.Contains(token)) 
                            {
                                GM.EventPlayerWon(player);
                                CleanColor();
                                CleanMovement(path);
                                return;
                            }
                        }
                    }
                }
            }
            CleanMovement(path);
        }

        public Stack<Cell> GetOptimalPath(Cell cell, Cell destiny, int distance = 10)
        {
            Stack<Cell> cellPath = new Stack<Cell>();
            if (distance > 0 
            &&  GM.maze.IsOfThisMaze(cell) && GM.maze.IsOfThisMaze(destiny)
            && !cell.Equals(destiny)) 
            {
                List<(Cell cell, int distance)> cells = GetCellsInRange(cell, distance);
                distance = 0;
                foreach ((Cell cell, int distance) cellWithDistance in cells)
                {
                    if (cellWithDistance.cell.Equals(destiny))
                    {
                        distance = cellWithDistance.distance;
                        break;
                    }
                }
                if (distance > 0)
                {
                    cellPath.Push(destiny);
                    int delay;
                    List<Cell> neighbors;
                    while (distance > 0)
                    {
                        destiny = cellPath.Pop();
                        cellPath.Push(destiny);
                        neighbors = GM.maze.GetNeighborsWithoutWall(destiny);
                        if (destiny.Interactive is Obstacle obstacle && obstacle.ActualState == State.Active)
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

        public List<(Cell, int)> GetCellsInRange(Cell initialCell, int distance)
        {
            List<(Cell cell, int distance)> cells = new List<(Cell, int)>();
            if (!GM.maze.IsOfThisMaze(initialCell)) return cells;
            cells.Add((initialCell, 0));
            List<Cell> cellsWithi = new List<Cell>();
            List<Cell> actualCells = new List<Cell>();
            List<Cell> neighbors;
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
                    neighbors = GM.maze.GetNeighborsWithoutWall(cell);
                    foreach (Cell neighbor in neighbors)
                    {
                        if (GM.GetCharactersInCell(neighbor).Count() > 1 || (neighbor.Interactive is Obstacle obs && obs.ActualState == State.Active && obs.Delay == 5))
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
                        if (actualCells.Contains(neighbor))
                        {
                            foreach ((Cell cell, int distance) cellWithDistance in cells)
                            {
                                if (cellWithDistance.cell.Equals(neighbor) && cellWithDistance.distance > i + delay)
                                {
                                    cells.Add((neighbor, i + delay));
                                    cells.Remove(cellWithDistance);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if(distance >= i + delay)
                            {
                                cells.Add((neighbor, i + delay));
                                actualCells.Add(neighbor);
                            }
                        }
                    }
                }
                cellsWithi.Clear();
            } 
            cells.Remove((initialCell, 0));
            return cells;
        }

        public void ColorCells(List<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                if (cell.IsColored == false) cell.ChangeColorStatus();
            }
            GM.EventChangeInMazeMade();
        }

        public void CleanColor()
        {
            Cell cell;
            for (int x = 0; x < GM.maze.Width; x++)
            {
                for (int y = 0; y < GM.maze.Height; y++)
                {
                    cell = GM.maze.Grid[x, y];
                    if (cell.IsColored == true) cell.ChangeColorStatus();
                }
            }
            GM.EventChangeInMazeMade();
        }

        private void CleanMovement(Queue<Cell> path)
        {
            Cell cell;
            for (int i = 0; i < path.Count; i++)
            {
                Thread.Sleep(50);
                cell = path.Dequeue();
                if (cell.IsColored == true) cell.ChangeColorStatus();
                GM.EventChangeInMazeMade();
            }
        }
    }
}