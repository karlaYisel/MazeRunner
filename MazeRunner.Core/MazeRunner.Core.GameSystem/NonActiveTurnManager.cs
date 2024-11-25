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
        private MovementManager MM = MovementManager.MM;
        private AttackManager AM = AttackManager.AM;

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
        public void PerformTurn(NPC nonPlayable)
        {
            Cell initialCell = GM.maze.Grid[nonPlayable.X, nonPlayable.Y];
            List<(Cell cell, int distance)> cells = MM.GetCellsInRange(initialCell, nonPlayable.Speed);
            if (cells.Count == 0) return;
            Cell cell;
            List<Cell> neighbors;
            int delay;
            List<Character> opponents;
            int initialLife;
            //Effects[] initialEffects; //Longitud de Effects.TypeOfEffects enum
            switch (nonPlayable.TypeNPC)
            {
                case TypeOfNPC.Neutral:
                    opponents = GM.GetCharactersInCell(initialCell);
                    opponents.Remove(nonPlayable);
                    foreach (Character character in opponents)
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
                                MM.MoveToken(nonPlayable, cell, delay);
                                initialLife = character.CurrentLife;
                                //initialEffects = ref character.ActualEffects; ???
                                nonPlayable.Attack(character);
                                GM.StabilizeToken(character);
                                if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                {
                                    GM.EventDefetedToken(character, nonPlayable, 0);
                                    return;
                                }
                                if (initialLife > character.CurrentLife)
                                {
                                    GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                                }
                                else if (initialLife < character.CurrentLife)
                                {
                                    GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                                }
                                //compara efectos para saber cuales se anaden
                                //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                                GM.EventChangeInMazeMade();
                                return;
                            }
                        }
                    }
                    opponents = AM.GetPossibleOpponents(nonPlayable);
                    foreach (Character character in opponents)
                    {
                        if(nonPlayable.TargedCharacters is not null && nonPlayable.TargedCharacters.Contains(character)) 
                        {
                            initialLife = character.CurrentLife;
                            //initialEffects = ref character.ActualEffects; ???
                            nonPlayable.Attack(character);
                            GM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.CurrentLife)
                            {
                                GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                            }
                            else if (initialLife < character.CurrentLife)
                            {
                                GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                            }
                            //compara efectos para saber cuales se anaden
                            //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                            GM.EventChangeInMazeMade();
                            MM.MakeRandomMove(nonPlayable);
                            return;
                        }
                    }
                    break;
                case TypeOfNPC.Aggressive:
                    opponents = GM.GetCharactersInCell(initialCell);
                    opponents.Remove(nonPlayable);
                    foreach (Character character in opponents)
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
                            MM.MoveToken(nonPlayable, cell, delay);
                            initialLife = character.CurrentLife;
                            //initialEffects = ref character.ActualEffects; ???
                            nonPlayable.Attack(character);
                            GM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.CurrentLife)
                            {
                                GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                            }
                            else if (initialLife < character.CurrentLife)
                            {
                                GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                            }
                            //compara efectos para saber cuales se anaden
                            //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                            GM.EventChangeInMazeMade();
                            return;
                        }
                    }
                    opponents = AM.GetPossibleOpponents(nonPlayable);
                    foreach (Character character in opponents)
                    {
                        initialLife = character.CurrentLife;
                        //initialEffects = ref character.ActualEffects; ???
                        nonPlayable.Attack(character);
                        GM.StabilizeToken(character);
                        if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                        {
                            GM.EventDefetedToken(character, nonPlayable, 0);
                            return;
                        }
                        if (initialLife > character.CurrentLife)
                        {
                            GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                        }
                        else if (initialLife < character.CurrentLife)
                        {
                            GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                        }
                        //compara efectos para saber cuales se anaden
                        //StabilizeEffects(tokens); //Los efectos dan mensaje del dano que hacen, cuales se quitan
                        GM.EventChangeInMazeMade();
                        MM.MakeRandomMove(nonPlayable);
                        return;
                    }
                    cells = MM.GetCellsInRange(initialCell, nonPlayable.Speed);
                    foreach ((Cell cell, int distance) cellWithDistance in cells)
                    {
                        opponents.AddRange(GM.GetCharactersInCell(cellWithDistance.cell));
                        if (opponents.Count > 0) 
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
                                    MM.MoveToken(nonPlayable, neighbor, cellWithDistance.distance - delay);
                                    Character character = opponents[random.Next(0, opponents.Count)];
                                    initialLife = character.CurrentLife;
                                    //initialEffects = ref character.ActualEffects; ???
                                    nonPlayable.Attack(character);
                                    GM.StabilizeToken(character);
                                    if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                    {
                                        GM.EventDefetedToken(character, nonPlayable, 0);
                                        return;
                                    }
                                    if (initialLife > character.CurrentLife)
                                    {
                                        GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                                    }
                                    else if (initialLife < character.CurrentLife)
                                    {
                                        GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
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