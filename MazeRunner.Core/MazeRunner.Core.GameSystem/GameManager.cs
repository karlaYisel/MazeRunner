using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.GameSystem
{
    public delegate void PlayerMessage(Character affectedCharacter, Interactive? modificaterObject, int modificator);
    public delegate void PlayerVictory(Player player);
    public delegate void ChangeTurn(int turn);
    public delegate void ChangeMaze();

    public class GameManager
    {
        private static GameManager? _GM;
        private static readonly object _lock = new object();
        public List<Player> ActivePlayers { get; private set;} = new List<Player>();
        public List<Player> NonActivePlayers { get; private set;} = new List<Player>();
        public string[] colors = new string[5]{"blue", "red", "green", "yellow", "gray"}; // Maybe edit this to allow the players choose the colors
        public event PlayerMessage? DefetedToken;
        public event PlayerMessage? DemagedToken;
        public event PlayerMessage? HealedToken;
        public event PlayerMessage? MessageToken;
        public event PlayerMessage? TokenEffectAdd;
        public event PlayerMessage? TokenEffectSubstract;
        public event PlayerVictory? PlayerWon;
        public event ChangeMaze? ChangeInMazeMade;
        public event ChangeTurn? ChangeInTurnMade;
        public bool IsCoopGame { get; private set; } 
        public bool PlayWithBots { get; private set; } 
        public Maze maze { get; private set; } = new Maze(11, 11);
        public List<Cell> StartPoints { get; private set; } = new List<Cell>();
        public List<Cell> EndPoints { get; private set; } = new List<Cell>();
        public int InitialNumberOfObstacles {get; private set;}
        public int InitialNumberOfTraps {get; private set;}
        public int InitialNumberOfNPCs {get; private set;}
        public int ActualNumberOfObstacles {get; internal set;}
        public int ActualNumberOfTraps {get; internal set;}
        public int ActualNumberOfInteractives {get; internal set;}
        public int NumberOfTokensByPlayers {get; private set;}
        public int NumberOfPlayableTokens {get { return NumberOfTokensByPlayers*ActivePlayers.Count; } }
        public int NumberOfNonPlayableTokens {
            get{   
                int numberOfTokens = NonActivePlayers[NonActivePlayers.Count - 1].Tokens.Count;
                if (PlayWithBots) {numberOfTokens += NumberOfTokensByPlayers*(NonActivePlayers.Count - 1); }
                return numberOfTokens;
            }
        }
        public int Turn { get; private set; }

        private GameManager()
        {
        }

        public static GameManager GM
        {
            get
            {
                if (_GM is null)
                {
                    lock (_lock)
                    {
                        if (_GM is null)
                        {
                            _GM = new GameManager();
                        }
                    }
                }
                return _GM;
            }
            
        }

//Initialize properties of GM
        public void InitializePlayers(int numberOfPlayers, string?[] names)
        {
            ActivePlayers.Clear();
            NonActivePlayers.Clear();
            string? name;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                name = null;
                if (i < names.Length) name = names[i];
                if (name is null) name = "Jugador " + (i + 1).ToString();
                this.ActivePlayers.Add(new Player(name));
            }
            for (int i = numberOfPlayers; i < 5; i++)
            {
                if (i < 4) 
                {
                    name = null;
                    if (i < names.Length) name = names[i];
                    if (name is null) name = "Bot " + (i + 1 - numberOfPlayers).ToString();
                }
                else
                {
                    name = "NPCs";
                }
                this.NonActivePlayers.Add(new Player(name));
            }
        }

        public void InitializeLevel(bool isCoopGame, bool playWithBots, Maze maze, List<Cell> startPoints, List<Cell> endPoints, int numberOfObstacles, int numberOfTraps, int numberOfNPCs, int numberOfTokens)
        {
            IsCoopGame = isCoopGame;
            PlayWithBots = playWithBots;
            this.maze = maze;
            StartPoints = startPoints;
            EndPoints = endPoints;
            InitialNumberOfObstacles = numberOfObstacles;
            InitialNumberOfTraps = numberOfTraps;
            InitialNumberOfNPCs = numberOfNPCs;
            ActualNumberOfObstacles = 0;
            ActualNumberOfTraps = 0;
            ActualNumberOfInteractives = 0;
            NumberOfTokensByPlayers = numberOfTokens;
            Turn = 0;
        }

//Events Manager
        public void EventDefetedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            DefetedToken?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventDemagedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            DemagedToken?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventHealedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            HealedToken?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventStateAddToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            TokenEffectAdd?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventStateSubstractToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            TokenEffectSubstract?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventMessageToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            MessageToken?.Invoke(affectedCharacter, modificaterObject, modificator);
        }

        public void EventPlayerWon(Player player)
        {
            PlayerWon?.Invoke(player);
        }

        public void EventChangeInMazeMade()
        {
            ChangeInMazeMade?.Invoke();
        }

        public void EventPassTurn()
        {
            Turn++;
            StabilizeEffects();
            ChangeInTurnMade?.Invoke(Turn);
        }

//General methods
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

        public Cell GetTokenInitialPosition(PlayableCharacter token)
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

        public void StabilizeToken(Character token)
        {
            if (token.CurrentLife > token.MaxLife) 
            {
                token.CurrentLife = token.MaxLife;
                return;
            }
            if (token.CurrentLife <= 0) 
            {
                if (token is PlayableCharacter playable) 
                {
                    Cell startCell = GM.GetTokenInitialPosition(playable);
                    if (!GM.maze.IsOfThisMaze(startCell)) return;
                    token.ChangePosition(startCell.X, startCell.Y);
                    foreach(NPC nonPlayable in GM.NonActivePlayers[GM.NonActivePlayers.Count - 1].Tokens)
                    {
                        if(nonPlayable.TargedCharacters is not null && nonPlayable.TargedCharacters.Contains(token))
                        {
                            nonPlayable.TargedCharacters.Remove(token);
                        }
                    }
                    token.CurrentLife = token.MaxLife;
                    token.ChangeStates(Int32.MinValue, Int32.MinValue, Int32.MinValue);
                }
                else
                {
                    token.ChangeState();
                    GM.NonActivePlayers[GM.NonActivePlayers.Count - 1].Tokens.Remove(token);
                }
                EventDefetedToken(token, null, 0);
            }
        }

        public int GetNumberOfPlayerByToken(Character? character)
        {
            if (character is not null)
            {
                foreach(Player player in ActivePlayers)
                {
                    if (player.Tokens.Contains(character)) return ActivePlayers.IndexOf(player) + 1;
                }
                foreach(Player player in NonActivePlayers)
                {
                    if (player.Tokens.Contains(character)) return NonActivePlayers.IndexOf(player) + ActivePlayers.Count + 1;
                }
            }
            return 0;
        }

        private void StabilizeEffects()
        {
            List<Player> players = new List<Player>();
            foreach(Player player in ActivePlayers)
            {
                players.Add(player);
            }
            foreach(Player player in NonActivePlayers)
            {
                players.Add(player);
            }
            foreach (Player player in players)
            {
                foreach (Character token in player.Tokens)
                {
                    if(token.RemainingTurnsIced == 1)
                    {
                        EventStateSubstractToken(token, null, 1);
                    }
                    if(token.RemainingTurnsPoisoned > 0) 
                    {
                        token.CurrentLife -= 8;
                        EventDemagedToken(token, null, 8);
                    }
                    if(token.RemainingTurnsPoisoned == 1)
                    {
                        EventStateSubstractToken(token, null, 2);
                    }
                    token.ChangeStates(0, -1, -1);
                }
            }
        }
    }
}