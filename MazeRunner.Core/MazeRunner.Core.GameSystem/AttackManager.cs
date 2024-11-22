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
        public List<Character> GetPossibleOponents(Character actualCharacter)
        {
            List<Character> opponents = new List<Character>();
            List<Character> possibleOponents = new List<Character>();
            List<Cell> cells;
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
                    possibleOponents.AddRange(GetCharactersInCell(initialCell));
                    cells = GM.maze.GetNeighborsWithoutWall(initialCell);
                    foreach (Cell cell in cells)
                    {
                        possibleOponents.AddRange(GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Large: 
                    Stack<Cell> path;
                    cells = GM.MM.GetCellsAtDistance(initialCell, 1);
                    cells.AddRange(GM.MM.GetCellsAtDistance(initialCell, 2));
                    foreach (Cell cell in cells)
                    {
                        path = GM.MM.GetOptimalPath(initialCell, cell, 8);
                        if (path.Count < 1 || path.Count > 2) continue;
                        possibleOponents.AddRange(GetCharactersInCell(cell));
                    }
                    break;
                case TypeOfAttack.Distance:
                    cells = GM.MM.GetCellsAtDistance(initialCell, 2);
                    cells.AddRange(GM.MM.GetCellsAtDistance(initialCell, 3));
                    foreach (Cell cell in cells)
                    {
                        possibleOponents.AddRange(GetCharactersInCell(cell));
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
            foreach (Character character in possibleOponents)
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

        public List<Character> GetCharactersInCell(Cell cell)
        {
            List<Character> characters = new List<Character>();
            if (!GM.maze.IsOfThisMaze(cell)) return characters;
            foreach (Player player in GM.ActivePlayers)
            {
                foreach (Character character in player.Tokens)
                {
                    if (character.X == cell.X && character.Y == cell.Y) {characters.Add(character); }
                }
            }
            foreach (Player player in GM.NonActivePlayers)
            {
                foreach (Character character in player.Tokens)
                {
                    if (character.ActualState == State.Active && character.X == cell.X && character.Y == cell.Y) {characters.Add(character); }
                }
            }
            return characters;
        }    

        public void StabilizeToken(Character token)
        {
            if (token.ActualLife > token.MaxLife) 
            {
                token.ActualLife = token.MaxLife;
                return;
            }
            if (token.ActualLife <= 0) 
            {
                if (token is PlayableCharacter playable) 
                {
                    Cell startCell = GM.MM.TokenInitialPosition(playable);
                    if (!GM.maze.IsOfThisMaze(startCell)) return;
                    token.ChangePosition(startCell.X, startCell.Y);
                    foreach(NPC nonPlayable in GM.NonActivePlayers[GM.NonActivePlayers.Count - 1].Tokens)
                    {
                        if(nonPlayable.TargedCharacters is not null && nonPlayable.TargedCharacters.Contains(token))
                        {
                            nonPlayable.TargedCharacters.Remove(token);
                        }
                    }
                    /*foreach(Effect effect in token.ActualEffects)
                    {
                        effect.Duration = 0;
                    }
                    */
                }
                else
                {
                    token.ChangeState();
                    GM.NonActivePlayers[GM.NonActivePlayers.Count - 1].Tokens.Remove(token);
                }
            }
        }

        //public void StabilizeEffects(Character token) {}

        public void TargetCharacters(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                if(character.IsTargeted == false) character.ChangeTargetStatus();
            }
            GM.EventChangeInMazeMade();
        }

        public void CleanTargets()
        {
            List<Player> players = [.. GM.ActivePlayers, .. GM.NonActivePlayers];
            foreach(Player player in players)
            {
                foreach(Character token in player.Tokens)
                {
                    if(token.IsTargeted == true) token.ChangeTargetStatus();
                }
            }
            GM.EventChangeInMazeMade();
        }
    }
}