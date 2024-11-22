using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.GameSystem
{
    public class NonActiveTurnManager
    {
        public static NonActiveTurnManager? _NATM;
        public static readonly object _lock = new object();
        private Random random= new Random();
        private GameManager GM = GameManager.GM;

        private NonActiveTurnManager()
        {}

        public static NonActiveTurnManager NATM
        {
            get
            {
                if (_NATM is null)
                {
                    lock (_lock)
                    {
                        if (_NATM is null)
                        {
                            _NATM = new NonActiveTurnManager();
                        }
                    }
                }
                return _NATM;
            }
        }

//Non Active Turn token's methods
        public void TakeTurn(NPC nonPlayable)
        {
            Cell initialCell = GM.maze.Grid[nonPlayable.X, nonPlayable.Y];
            List<(Cell cell, int distance)> cells = GM.MM.GetCellsInRange(initialCell, nonPlayable.Speed);
            if (cells.Count == 0) return;
            Cell cell;
            List<Cell> neighbors;
            int delay;
            List<Character> oponents;
            int initialLife;
            //Effects[] initialEffects; //Longitud de Effects.TypeOfEffects enum
            switch (nonPlayable.TypeNPC)
            {
                case TypeOfNPC.Neutral:
                    oponents = GM.AM.GetCharactersInCell(initialCell);
                    oponents.Remove(nonPlayable);
                    foreach (Character character in oponents)
                    {
                        if(nonPlayable.TargedCharacters is not null && nonPlayable.TargedCharacters.Contains(character)) 
                        {
                            neighbors = GM.maze.GetNeighborsWithoutWall(initialCell);
                            while (neighbors.Count > 0)
                            {
                                cell = neighbors[random.Next(0, neighbors.Count)];
                                if (cell.Interactive is Obstacle obstacle)
                                {
                                    delay = obstacle.Delay;
                                }
                                else 
                                {
                                    delay = 1;
                                }
                                if (delay > nonPlayable.Speed)
                                {
                                    neighbors.Remove(cell);
                                    continue;
                                }
                                GM.MM.MoveToken(nonPlayable, cell, delay);
                                initialLife = character.ActualLife;
                                //initialEffects = ref character.ActualEffects; ???
                                nonPlayable.Attack(character);
                                GM.AM.StabilizeToken(character);
                                if ((initialLife != character.MaxLife && character.ActualLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                {
                                    GM.EventDefetedToken(character, nonPlayable, 0);
                                    return;
                                }
                                if (initialLife > character.ActualLife)
                                {
                                    GM.EventDemagedToken(character, nonPlayable, initialLife - character.ActualLife);
                                }
                                else if (initialLife < character.ActualLife)
                                {
                                    GM.EventHealedToken(character, nonPlayable, character.ActualLife - initialLife);
                                }
                                //compara efectos para saber cuales se anaden
                                //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                                GM.EventChangeInMazeMade();
                                return;
                            }
                        }
                    }
                    oponents = GM.AM.GetPossibleOponents(nonPlayable);
                    foreach (Character character in oponents)
                    {
                        if(nonPlayable.TargedCharacters is not null && nonPlayable.TargedCharacters.Contains(character)) 
                        {
                            initialLife = character.ActualLife;
                            //initialEffects = ref character.ActualEffects; ???
                            nonPlayable.Attack(character);
                            GM.AM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.ActualLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.ActualLife)
                            {
                                GM.EventDemagedToken(character, nonPlayable, initialLife - character.ActualLife);
                            }
                            else if (initialLife < character.ActualLife)
                            {
                                GM.EventHealedToken(character, nonPlayable, character.ActualLife - initialLife);
                            }
                            //compara efectos para saber cuales se anaden
                            //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                            GM.EventChangeInMazeMade();
                            GM.MM.RandomMove(nonPlayable);
                            return;
                        }
                    }
                    break;
                case TypeOfNPC.Aggressive:
                    oponents = GM.AM.GetCharactersInCell(initialCell);
                    oponents.Remove(nonPlayable);
                    foreach (Character character in oponents)
                    {
                        neighbors = GM.maze.GetNeighborsWithoutWall(initialCell);
                        while (neighbors.Count > 0)
                        {
                            cell = neighbors[random.Next(0, neighbors.Count)];
                            if (cell.Interactive is Obstacle obstacle)
                            {
                                delay = obstacle.Delay;
                            }
                            else 
                            {
                                delay = 1;
                            }
                            if (delay > nonPlayable.Speed)
                            {
                                neighbors.Remove(cell);
                                continue;
                            }
                            GM.MM.MoveToken(nonPlayable, cell, delay);
                            initialLife = character.ActualLife;
                            //initialEffects = ref character.ActualEffects; ???
                            nonPlayable.Attack(character);
                            GM.AM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.ActualLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.ActualLife)
                            {
                                GM.EventDemagedToken(character, nonPlayable, initialLife - character.ActualLife);
                            }
                            else if (initialLife < character.ActualLife)
                            {
                                GM.EventHealedToken(character, nonPlayable, character.ActualLife - initialLife);
                            }
                            //compara efectos para saber cuales se anaden
                            //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                            GM.EventChangeInMazeMade();
                            return;
                        }
                    }
                    oponents = GM.AM.GetPossibleOponents(nonPlayable);
                    foreach (Character character in oponents)
                    {
                        initialLife = character.ActualLife;
                        //initialEffects = ref character.ActualEffects; ???
                        nonPlayable.Attack(character);
                        GM.AM.StabilizeToken(character);
                        if ((initialLife != character.MaxLife && character.ActualLife == character.MaxLife) || character.ActualState == State.Inactive) 
                        {
                            GM.EventDefetedToken(character, nonPlayable, 0);
                            return;
                        }
                        if (initialLife > character.ActualLife)
                        {
                            GM.EventDemagedToken(character, nonPlayable, initialLife - character.ActualLife);
                        }
                        else if (initialLife < character.ActualLife)
                        {
                            GM.EventHealedToken(character, nonPlayable, character.ActualLife - initialLife);
                        }
                        //compara efectos para saber cuales se anaden
                        //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                        GM.EventChangeInMazeMade();
                        GM.MM.RandomMove(nonPlayable);
                        return;
                    }
                    cells = GM.MM.GetCellsInRange(initialCell, nonPlayable.Speed);
                    foreach ((Cell cell, int distance) cellWithDistance in cells)
                    {
                        oponents.AddRange(GM.AM.GetCharactersInCell(cellWithDistance.cell));
                        if (oponents.Count > 0) 
                        {
                            if (cellWithDistance.cell.Interactive is Obstacle obstacle)
                            {
                                delay = obstacle.Delay;
                            }
                            else
                            {
                                delay = 1;
                            }
                            neighbors = GM.maze.GetNeighborsWithoutWall(cellWithDistance.cell);
                            foreach (Cell neighbor in neighbors)
                            {
                                if (cells.Contains((neighbor, cellWithDistance.distance - delay)))
                                {
                                    GM.MM.MoveToken(nonPlayable, neighbor, cellWithDistance.distance - delay);
                                    Character character = oponents[random.Next(0, oponents.Count)];
                                    initialLife = character.ActualLife;
                                    //initialEffects = ref character.ActualEffects; ???
                                    nonPlayable.Attack(character);
                                    GM.AM.StabilizeToken(character);
                                    if ((initialLife != character.MaxLife && character.ActualLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                    {
                                        GM.EventDefetedToken(character, nonPlayable, 0);
                                        return;
                                    }
                                    if (initialLife > character.ActualLife)
                                    {
                                        GM.EventDemagedToken(character, nonPlayable, initialLife - character.ActualLife);
                                    }
                                    else if (initialLife < character.ActualLife)
                                    {
                                        GM.EventHealedToken(character, nonPlayable, character.ActualLife - initialLife);
                                    }
                                    //compara efectos para saber cuales se anaden
                                    //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                                    GM.EventChangeInMazeMade();
                                    return;
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //public void PlayableTokenTurn(PlayableCharacter token) {}
    }
}