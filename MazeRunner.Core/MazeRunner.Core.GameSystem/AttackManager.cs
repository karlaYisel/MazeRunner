using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.GameSystem
{
    public class AttackManager
    {
        public static AttackManager? _AM;
        public static readonly object _lock = new object();
        private GameManager GM = GameManager.GM;

        private AttackManager()
        {}

        public static AttackManager AM
        {
            get
            {
                if (_AM is null)
                {
                    lock (_lock)
                    {
                        if (_AM is null)
                        {
                            _AM = new AttackManager();
                        }
                    }
                }
                return _AM;
            }
        }

//Attack's methods
        public List<Character> GetPossibleOpponents(Character actualCharacter)
        {
            List<Character> opponents = new List<Character>();
            List<Character> possibleOpponents = new List<Character>();
            List<Cell> cells;
            Cell actualCell;
            Player[] Players = new Player[5];
            int i = 0;
            foreach (Player allPlayer in GM.ActivePlayers)
            {
                Players[i] = allPlayer;
                i++;
            }
            foreach (Player allPlayer in GM.NonActivePlayers)
            {
                Players[i] = allPlayer;
                i++;
            }
            Player? player = null;
            TypeOfAttack typeOfAttack;
            Cell initialCell = GM.maze.Grid[actualCharacter.X, actualCharacter.Y];
            if (actualCharacter is PlayableCharacter playableCharacter)
            {
                typeOfAttack = playableCharacter.TypeOfAttack;
            }
            else 
            {
                typeOfAttack = TypeOfAttack.Large;
            }

            switch (typeOfAttack)
            {
                case TypeOfAttack.Short:
                    possibleOpponents.AddRange(GM.GetCharactersInCell(initialCell));
                    cells = GM.maze.GetNeighborsWithoutWall(initialCell);
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Large: 
                    Queue<Cell> waitingCells = new Queue<Cell>([..GM.maze.GetNeighborsWithoutWall(initialCell)]);
                    cells = GM.maze.GetNeighborsWithoutWall(initialCell);
                    while (waitingCells.Count > 0)
                    {
                        actualCell = waitingCells.Dequeue();
                        foreach (Cell neigbour in GM.maze.GetNeighborsWithoutWall(actualCell))
                        {
                            if(!initialCell.Equals(neigbour) && !cells.Contains(neigbour)) cells.Add(neigbour);
                        }
                    }
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Distance:
                    cells = GetCellsAtDistance(initialCell, 2);
                    cells.AddRange(GetCellsAtDistance(initialCell, 3));
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                default:
                break;
            }
            foreach (Player possiblePlayer in Players)
            {
                if (possiblePlayer.Tokens.Contains(actualCharacter)) 
                {
                    player = possiblePlayer;
                    break;
                }
            }
            if (player is null) return opponents;
            foreach (Character character in possibleOpponents)
            {
                if (this.GM.IsCoopGame)
                {
                    switch (Array.IndexOf(Players, player))
                    {
                        case 0 or 2:
                            if (!Players[0].Tokens.Contains(character) && !Players[2].Tokens.Contains(character))
                            {
                                opponents.Add(character);
                            }
                            break;
                        case 1 or 3:
                            if (!Players[1].Tokens.Contains(character) && !Players[3].Tokens.Contains(character))
                            {
                                opponents.Add(character);
                            }
                            break;
                        default:
                            opponents.Add(character);
                            break;
                    }
                }
                else if ((!actualCharacter.Equals(character) && player == Players[4]) || !player.Tokens.Contains(character))
                {
                    opponents.Add(character);
                    continue;
                }
            }
            
            return opponents;
        }

        public List<Character> GetPossibleFriends(Character actualCharacter)
        {
            List<Character> opponents = new List<Character>();
            List<Character> possibleOpponents = new List<Character>();
            List<Cell> cells;
            Cell actualCell;
            Player[] Players = new Player[5];
            int i = 0;
            foreach (Player allPlayer in GM.ActivePlayers)
            {
                Players[i] = allPlayer;
                i++;
            }
            foreach (Player allPlayer in GM.NonActivePlayers)
            {
                Players[i] = allPlayer;
                i++;
            }
            Player? player = null;
            TypeOfAttack typeOfAttack;
            Cell initialCell = GM.maze.Grid[actualCharacter.X, actualCharacter.Y];
            if (actualCharacter is PlayableCharacter playableCharacter)
            {
                typeOfAttack = playableCharacter.TypeOfAttack;
            }
            else 
            {
                typeOfAttack = TypeOfAttack.Large;
            }

            switch (typeOfAttack)
            {
                case TypeOfAttack.Short:
                    possibleOpponents.AddRange(GM.GetCharactersInCell(initialCell));
                    cells = GM.maze.GetNeighborsWithoutWall(initialCell);
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Large: 
                    Queue<Cell> waitingCells = new Queue<Cell>([..GM.maze.GetNeighborsWithoutWall(initialCell)]);
                    cells = GM.maze.GetNeighborsWithoutWall(initialCell);
                    while (waitingCells.Count > 0)
                    {
                        actualCell = waitingCells.Dequeue();
                        foreach (Cell neigbour in GM.maze.GetNeighborsWithoutWall(actualCell))
                        {
                            if(!initialCell.Equals(neigbour) && !cells.Contains(neigbour)) cells.Add(neigbour);
                        }
                    }
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Distance:
                    cells = GetCellsAtDistance(initialCell, 2);
                    cells.AddRange(GetCellsAtDistance(initialCell, 3));
                    foreach (Cell cell in cells)
                    {
                        possibleOpponents.AddRange(GM.GetCharactersInCell(cell));
                    }
                    break;
                default:
                break;
            }
            foreach (Player possiblePlayer in Players)
            {
                if (possiblePlayer.Tokens.Contains(actualCharacter)) 
                {
                    player = possiblePlayer;
                    break;
                }
            }
            if (player is null) return opponents;
            foreach (Character character in possibleOpponents)
            {
                if (this.GM.IsCoopGame)
                {
                    switch (Array.IndexOf(Players, player))
                    {
                        case 0 or 2:
                            if (Players[0].Tokens.Contains(character) || Players[2].Tokens.Contains(character))
                            {
                                opponents.Add(character);
                            }
                            break;
                        case 1 or 3:
                            if (Players[1].Tokens.Contains(character) || Players[3].Tokens.Contains(character))
                            {
                                opponents.Add(character);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (player.Tokens.Contains(character))
                {
                    opponents.Add(character);
                    continue;
                }
            }
            
            return opponents;
        }

        //public void StabilizeEffects(Character token) {}
    
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

        public async Task TargetCharacters(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                if(character.IsTargeted == false) character.ChangeTargetStatus();
            }
            await GM.EventChangeInMazeMade();
        }

        public async Task CleanTargets()
        {
            List<Player> players = [.. GM.ActivePlayers, .. GM.NonActivePlayers];
            foreach(Player player in players)
            {
                foreach(Character token in player.Tokens)
                {
                    if(token.IsTargeted == true) token.ChangeTargetStatus();
                }
            }
            await GM.EventChangeInMazeMade();
        }
    }
}