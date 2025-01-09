using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.GameSystem
{
    public class GeneratorManager
    {
        private static GeneratorManager? _GeM;
        private static readonly object _lock = new object();
        private Random random= new Random();
        private GameManager GM = GameManager.GM;
        private GeneratorManager()
        {}
        public static GeneratorManager GeM
        {
            get
            {
                if (_GeM is null)
                {
                    lock (_lock)
                    {
                        if (_GeM is null)
                        {
                            _GeM = new GeneratorManager();
                        }
                    }
                }
                return _GeM;
            }
            
        }

//Generate maze's objects
        public bool TryGenerateInteractiveObjects(int numberOfObstacles, int numberOfTraps)
        {
            int NewActualNumberOfInteractives = GM.ActualNumberOfInteractives + numberOfObstacles + numberOfTraps;
            int numberOfTokens = GM.NumberOfPlayableTokens + GM.NumberOfNonPlayableTokens;
            if (NewActualNumberOfInteractives + numberOfTokens + GM.EndPoints.Count > GM.maze.Width*GM.maze.Height) 
            {
                return false;
            }
            else
            {
                GM.ActualNumberOfObstacles += numberOfObstacles;
                GM.ActualNumberOfTraps += numberOfTraps;
                GM.ActualNumberOfInteractives = NewActualNumberOfInteractives;
                string[] classes = [];
                List<Cell> emptyCells = [];
                Cell actualCell;
                for (int x = 0; x < GM.maze.Width; x++)
                {
                    for (int y = 0; y < GM.maze.Height; y++)
                    {
                        actualCell  = GM.maze.Grid[x, y];
                        if (actualCell.Interactive is null && !GM.StartPoints.Contains(actualCell) && !GM.EndPoints.Contains(actualCell)) {emptyCells.Add(actualCell);}
                    }
                }
                if (numberOfObstacles + numberOfTraps > emptyCells.Count) return false;
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
                        default:
                            number = 0;
                            enumName = "";
                            break;
                    }
                    Type? enumType = Type.GetType(enumName);
                    if (enumType is null) {return false; }
                    classes = Enum.GetNames(enumType);
                    if (classes is null) {return false; }
                    for (int i = 0; i < number; i++)
                    {
                        actualCell = emptyCells[random.Next(0, emptyCells.Count())];
                        actualCell.Interactive = CreateRandomInteractive(classes);
                        emptyCells.Remove(actualCell);
                        if (actualCell.Interactive is TemporalWall temporalWall)
                        {
                            GM.ChangeInTurnMade += temporalWall.StabilizeWall;
                        }
                        if (actualCell.Interactive is DelayObstacle delayObstacle)
                        {
                            GM.ChangeInTurnMade += delayObstacle.StabilizeWall;
                        }
                    }
                }
                return true;
            }
        }

        public bool TryGenerateNPCs(int numberOfNPCs)
        {
            List<Cell> emptyCells = new List<Cell>();
            Cell actualCell;
            for (int x = 0; x < GM.maze.Width; x++)
            {
                for (int y = 0; y < GM.maze.Height; y++)
                {
                    actualCell  = GM.maze.Grid[x, y];
                    if (actualCell.Interactive is null && !GM.StartPoints.Contains(actualCell) 
                    && !GM.EndPoints.Contains(actualCell) && GM.GetCharactersInCell(actualCell).Count < 2) {emptyCells.Add(actualCell);}
                }
            }
            if (numberOfNPCs > emptyCells.Count) return false;
            for (int i = 0; i < numberOfNPCs; i++)
            {
                actualCell = emptyCells[random.Next(0, emptyCells.Count())];
                actualCell.Interactive = CreateRandomInteractive(["NPC"]);
                if (actualCell.Interactive is NPC NonPlayable)
                {
                    NonPlayable.ChangePosition(actualCell.X, actualCell.Y);
                    GM.NonActivePlayers[GM.NonActivePlayers.Count - 1].Tokens.Add(NonPlayable);
                    actualCell.Interactive = null;
                }
                if (GM.GetCharactersInCell(actualCell).Count > 1) emptyCells.Remove(actualCell);
            }
            return true;
        }

        public bool TryGenerateTokensForPlayer (Player player, List<TypeOfPlayableCharacter> charactersTypes)
        { 
            if (charactersTypes.Count != GM.NumberOfTokensByPlayers) return false;
            PlayableCharacter? token;
            List<Character> tokens = new List<Character>();
            for (int i = 0; i < GM.NumberOfTokensByPlayers; i++)
            {
                token = CreateRandomInteractive([charactersTypes[i].ToString()]) as PlayableCharacter;
                if (token is null) return false;
                tokens.Add(token);
            }
            player.ChangeTokens(tokens);
            foreach (Character playable in player.Tokens)
            {
                token = playable as PlayableCharacter;
                if (token is null) return false;
                Cell startCell = GM.GetTokenInitialPosition(token);
                if (!GM.maze.IsOfThisMaze(startCell)) return false;
                playable.ChangePosition(startCell.X, startCell.Y);
                GM.ChangeInTurnMade += token.NewTurn;
            }
            return true;
        }

        public Interactive? CreateRandomInteractive(string[] classes)
        {
            string randomClass = classes[random.Next(0, classes.Length)];
    
            string nameClass = $"MazeRunner.Core.InteractiveObjects.{randomClass}";
            Type? typeClass = Type.GetType(nameClass);
    
            if (typeClass is not null)
            {
                object[] parameter = [];
    
                switch (randomClass)
                {
                    case "TemporalWall":
                        parameter = [random.Next(1, 6)]; 
                        break;
                    case "DelayObstacle":
                        parameter = [random.Next(1, 4), random.Next(2, 5)]; 
                        break;
                    case "PermanentDelayObstacle":
                        parameter = [random.Next(2, 5)]; 
                        break;
                    case "SpikeTrap":
                        parameter = [random.Next(4, 11)]; 
                        break;
                    case "PrisonTrap":
                        parameter = []; 
                        break;
                    case "FireTrap" or "IceTrap" or "PoisonTrap":
                        parameter = [random.Next(4, 11), random.Next(0, 3), random.Next(1, 6)]; 
                        break;
                    case "NPC":
                        string value = random.Next(0, Enum.GetValues(typeof(TypeOfNPC)).Length).ToString();
                        TypeOfNPC typeOfNPC = (TypeOfNPC)Enum.Parse(typeof(TypeOfNPC), value);
                        parameter = [0, 0, typeOfNPC, random.Next(40, 76), random.Next(5, 9), random.Next(5, 9), random.Next(5, 9), random.Next(2, 6)]; 
                        break;
                    case "Hero":
                        parameter = [0, 0, random.Next(60, 101), random.Next(5, 9), random.Next(5, 9), random.Next(5, 9), random.Next(2, 6), random.Next(1, 4)];
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