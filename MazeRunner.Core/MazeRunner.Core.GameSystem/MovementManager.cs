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
        public void RandomMove (Character token)
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
            int initialLife;
            //Effects[] initialEffects; //Longitud de Effects.TypeOfEffects enum
            Stack<Cell> optimalPath = GetOptimalPath(GM.maze.Grid[token.X, token.Y], cell, distance);
            Queue<Cell> path = new Queue<Cell>();
            cell = GM.maze.Grid[token.X, token.Y];
            int delay;
            for (int i = 0; i < optimalPath.Count(); i++)
            {
                if (token.ActualState == State.Inactive /*Si contiene tipo congelado no moverte*/) 
                {
                    CleanColor();
                    return;
                }
                ColorCells([cell]);
                path.Enqueue(cell);
                cell = optimalPath.Pop(); 
                if (cell.Interactive is Obstacle obstacle) delay = obstacle.Delay;
                else delay = 1;
                Thread.Sleep(delay*50);
                token.ChangePosition(cell.X, cell.Y);
                GM.EventChangeInMazeMade();
                if (cell.Interactive is Trap trap && (trap.IsStandTrap == false || optimalPath.Count() == 0)) 
                {
                    initialLife = token.ActualLife;
                    //initialEffects = ref token.ActualEffects; ???
                    if (trap.TryToTrigger(token))
                    {
                        GM.AM.StabilizeToken(token);
                        if ((initialLife != token.MaxLife && token.ActualLife == token.MaxLife) || token.ActualState == State.Inactive) 
                        {
                            GM.EventDefetedToken(token, trap, 0);
                            CleanColor();
                            return;
                        }
                        if (initialLife > token.ActualLife)
                        {
                            GM.EventDemagedToken(token, trap, initialLife - token.ActualLife);
                        }
                        else if (initialLife < token.ActualLife)
                        {
                            GM.EventHealedToken(token, trap, token.ActualLife - initialLife);
                        }
                        //compara efectos para saber cuales se anaden
                        //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                        GM.EventChangeInMazeMade();
                    }
                    else
                    {
                        GM.EventDemagedToken(token, trap, 0);
                    }
                }
            }
            CleanMovement(path);
        }

        public Cell TokenInitialPosition(PlayableCharacter token)
        {
            Player? player = null;
            int numberOfPlayer;
            int numberOfToken;
            Cell startCell;
            foreach (Player possiblePlayer in GM.ActivePlayers)
            {
                if (possiblePlayer.Tokens.Contains(token)) 
                {
                    player = possiblePlayer;
                    break;
                }
            }
            if (player is null)
            {
                foreach (Player possiblePlayer in GM.ActivePlayers)
                {
                    if (possiblePlayer.Tokens.Contains(token)) 
                    {
                        player = possiblePlayer;
                        break;
                    }
                }
                if (player is null) return new Cell(0, 0);
                numberOfPlayer = GM.ActivePlayers.Count + GM.NonActivePlayers.IndexOf(player);
            }
            else
            {
                numberOfPlayer = GM.ActivePlayers.IndexOf(player);
            }
            numberOfToken = player.Tokens.IndexOf(token);
            startCell = GM.StartPoints[numberOfPlayer*GM.NumberOfTokensByPlayers + numberOfToken];
            return startCell;
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
                    neighbors = GM.maze.GetNeighborsWithoutWall(cell);
                    foreach (Cell neighbor in neighbors)
                    {
                        if (GM.AM.GetCharactersInCell(neighbor).Count() > 1 || (neighbor.Interactive is Obstacle obs && obs.ActualState == State.Active && obs.Delay == 5))
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
            cells.Remove((initialCell, 0));
            return cells;
        }
    
        public List<Cell> GetCellsAtDistance (Cell initialCell, int distance)
        {
            List<Cell> cells = new List<Cell>();
            if (!GM.maze.IsOfThisMaze(initialCell)) return cells;
            List<(int x, int y)> possibleCells = new List<(int, int)> ();
            int y;
            for (int x = 0; x <= distance; x++)
            {
                y = distance - x;
                possibleCells.Add((initialCell.X + x, initialCell.Y + y));
                if (y > 0) {possibleCells.Add((initialCell.X + x, initialCell.Y - y)); }
                if (x > 0) {possibleCells.Add((initialCell.X - x, initialCell.Y + y)); }
                if (x > 0 && y > 0) {possibleCells.Add((initialCell.X - x, initialCell.Y - y)); }
            }
            foreach ((int x, int y) possibleCell in possibleCells)
            {
                if(possibleCell.x < GM.maze.Width && possibleCell.x >= 0
                && possibleCell.y < GM.maze.Height && possibleCell.y >= 0)
                {
                    cells.Add(GM.maze.Grid[possibleCell.x, possibleCell.y]);
                }
            }
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