using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.GameSystem
{
    public delegate void PlayerMessage(Character affectedCharacter, Interactive? modificaterObject, int modificator);
    public delegate void ChangeTurn(int turn);
    public delegate void ChangeMaze();

    public class GameManager
    {
        private static GameManager? _GM;
        private static readonly object _lock = new object();
        private Random random= new Random();
        public GeneratorManager GeM = GeneratorManager.GeM;
        public MovementManager MM = MovementManager.MM;
        public AttackManager AM = AttackManager.AM;
        public NonActiveTurnManager NATM = NonActiveTurnManager.NATM;
        public List<Player> ActivePlayers { get; private set;} = new List<Player>();
        public List<Player> NonActivePlayers { get; private set;} = new List<Player>();
        public event PlayerMessage DefetedToken;
        public event PlayerMessage DemagedToken;
        public event PlayerMessage HealedToken;
        //public event PlayerMessage TokenEffectAdd;
        //public event PlayerMessage TokenEffectSubstract;
        public event ChangeMaze ChangeInMazeMade;
        public event ChangeTurn ChangeInTurnMade;
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
        {}

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
        public void InitializePlayers(int numberOfPlayers, string[] names)
        {
            ActivePlayers.Clear();
            NonActivePlayers.Clear();
            string name;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (i >= names.Length || string.IsNullOrEmpty(names[i])) { name = "Juagador " + (i + 1).ToString(); }
                else { name = names[i]; }
                this.ActivePlayers.Add(new Player(name));
            }
            for (int i = numberOfPlayers; i < 5; i++)
            {
                if (i < 4) 
                {
                    if (i >= names.Length || string.IsNullOrEmpty(names[i])) { name = "Bot " + (i + 1 - numberOfPlayers).ToString(); }
                    else { name = names[i]; }
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
            GeM.TryGenerateInteractiveObjects(numberOfObstacles, numberOfTraps);
            GeM.TryGenerateNPCs(numberOfNPCs);
        }

//Events Manager
        public void EventDefetedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            DefetedToken(affectedCharacter, modificaterObject, modificator);
        }

        public void EventDemagedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            DemagedToken(affectedCharacter, modificaterObject, modificator);
        }

        public void EventHealedToken(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            HealedToken(affectedCharacter, modificaterObject, modificator);
        }

        public void EventChangeInMazeMade()
        {
            ChangeInMazeMade();
        }

        public void EventPassTurn()
        {
            Turn++;
            ChangeInTurnMade(Turn);
        }

    }
}