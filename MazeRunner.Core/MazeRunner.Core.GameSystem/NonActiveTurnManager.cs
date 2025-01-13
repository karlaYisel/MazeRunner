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
        public async Task PerformTurn(NPC nonPlayable) //Improve this
        {
            if(nonPlayable.RemainingTurnsIced > 0 || nonPlayable.ActualState == State.Inactive) return;
            Cell initialCell = GM.maze.Grid[nonPlayable.X, nonPlayable.Y];
            List<(Cell cell, int distance)> cells = MM.GetCellsInRange(initialCell, nonPlayable.Speed);
            if (cells.Count == 0) return;
            Cell cell;
            List<Cell> neighbors;
            int delay;
            List<Character> opponents;
            int initialLife;
            switch (nonPlayable.TypeNPC)
            {
                case TypeOfNPC.Passive:
                    await MM.MakeRandomMove(nonPlayable);
                    break;
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
                                await MM.MoveToken(nonPlayable, cell, delay);
                                if(!cell.Equals(GM.maze.Grid[nonPlayable.X, nonPlayable.Y]) || nonPlayable.ActualState == State.Inactive) 
                                {
                                    await GM.EventChangeInMazeMade();
                                    return;
                                }
                                initialLife = character.CurrentLife;
                                nonPlayable.Attack(character);
                                await GM.StabilizeToken(character);
                                if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                {
                                    await GM.EventDefetedToken(character, nonPlayable, 0);
                                }
                                if (initialLife > character.CurrentLife)
                                {
                                    await GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                                }
                                else if (initialLife < character.CurrentLife)
                                {
                                    await GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                                }
                                await GM.EventChangeInMazeMade();
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
                            nonPlayable.Attack(character);
                            await GM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                await GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.CurrentLife)
                            {
                                await GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                            }
                            else if (initialLife < character.CurrentLife)
                            {
                                await GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                            }
                            await GM.EventChangeInMazeMade();
                            await MM.MakeRandomMove(nonPlayable);
                            return;
                        }
                    }
                    await MM.MakeRandomMove(nonPlayable);
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
                            await MM.MoveToken(nonPlayable, cell, delay);
                            if(!cell.Equals(GM.maze.Grid[nonPlayable.X, nonPlayable.Y]) || nonPlayable.ActualState == State.Inactive) 
                            {
                                await GM.EventChangeInMazeMade();
                                return;
                            }
                            initialLife = character.CurrentLife;
                            nonPlayable.Attack(character);
                            await GM.StabilizeToken(character);
                            if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                            {
                                await GM.EventDefetedToken(character, nonPlayable, 0);
                                return;
                            }
                            if (initialLife > character.CurrentLife)
                            {
                                await GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                            }
                            else if (initialLife < character.CurrentLife)
                            {
                                await GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                            }
                            await GM.EventChangeInMazeMade();
                            return;
                        }
                    }
                    opponents = AM.GetPossibleOpponents(nonPlayable);
                    foreach (Character character in opponents)
                    {
                        initialLife = character.CurrentLife;
                        nonPlayable.Attack(character);
                        await GM.StabilizeToken(character);
                        if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                        {
                            await GM.EventDefetedToken(character, nonPlayable, 0);
                            return;
                        }
                        if (initialLife > character.CurrentLife)
                        {
                            await GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                        }
                        else if (initialLife < character.CurrentLife)
                        {
                            await GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                        }
                        await GM.EventChangeInMazeMade();
                        await MM.MakeRandomMove(nonPlayable);
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
                                    await MM.MoveToken(nonPlayable, neighbor, cellWithDistance.distance - delay);
                                    if(!neighbor.Equals(GM.maze.Grid[nonPlayable.X, nonPlayable.Y]) || nonPlayable.ActualState == State.Inactive) 
                                    {
                                        await GM.EventChangeInMazeMade();
                                        return;
                                    }
                                    Character character = opponents[random.Next(0, opponents.Count)];
                                    initialLife = character.CurrentLife;
                                    nonPlayable.Attack(character);
                                    await GM.StabilizeToken(character);
                                    if ((initialLife != character.MaxLife && character.CurrentLife == character.MaxLife) || character.ActualState == State.Inactive) 
                                    {
                                        await GM.EventDefetedToken(character, nonPlayable, 0);
                                        return;
                                    }
                                    if (initialLife > character.CurrentLife)
                                    {
                                        await GM.EventDemagedToken(character, nonPlayable, initialLife - character.CurrentLife);
                                    }
                                    else if (initialLife < character.CurrentLife)
                                    {
                                        await GM.EventHealedToken(character, nonPlayable, character.CurrentLife - initialLife);
                                    }
                                    await GM.EventChangeInMazeMade();
                                    return;
                                }
                            }
                        }
                    }
                    await MM.MakeRandomMove(nonPlayable);
                    break;
                default:
                    break;
            }
        }

        //public void PlayableTokenTurn(PlayableCharacter token) {}
    }
}