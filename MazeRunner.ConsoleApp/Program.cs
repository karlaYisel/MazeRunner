using MazeRunner.Core.MazeGenerator;
using MazeRunner.Core.InteractiveObjects;
using MazeRunner.Core.GameSystem;

namespace MazeRunner.ConsoleApp
{
    public class Program
    {
        static GameManager GM = GameManager.GM;
        public static void Main(string[] args)
        {
            GM.DefetedToken += DefetedMessage;
            GM.HealedToken += HealedMessage;
            GM.DemagedToken += DemagedMessage;
            GM.ChangeInMazeMade += RefreshMaze;
            PrincipalMenu();
        }
    
        private static void PrincipalMenu()
        {
            string? option;
            while(true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("MAZE RUNNER");
                Console.WriteLine("--------------");
                Console.WriteLine("1 - Jugar");
                Console.WriteLine("2 - Salir");
                Console.WriteLine();

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 1:
                            PlayerConfiguration();
                            break;
                        case 2:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Opción no encontrada.");
                            Thread.Sleep(500);
                            continue;
                    }
                }
            }
        }

        //Tutorial

        private static void PlayerConfiguration()
        {
            string? option;
            while(true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Bienvenidos a Maze Runner");
                Console.WriteLine("-------------------------");
                Console.WriteLine("¿Cuántos jugadores son?");
                Console.Write("> Somos ");

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una cantidad válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    if (number < 2 || number > 2)
                    {
                        Console.WriteLine("Deben haber 2 jugadores."); //XD
                        Thread.Sleep(500);
                        continue;
                    }
                    else
                    {
                        string?[] names = new string[number];
                        for (int i = 0; i < number; i++)
                        {
                            Console.WriteLine("Ingrese el nombre del jugador {0}.", i+1);
                            Console.WriteLine("Si no escribe nada o es algo no válido se dejará Jugador {0}.", i+1);
                            names[i] = Console.ReadLine();
                        }
                        GM.InitializePlayers(number, names);
                        //
                        Maze maze = new Maze(11, 11);
                        List<Cell> startPoints = new List<Cell>([maze.Grid[10, 9], maze.Grid[0, 0], maze.Grid[9, 10]
                                                               , maze.Grid[0, 1], maze.Grid[10, 9], maze.Grid[1, 0]]);
                        List<Cell> endPoints = new List<Cell>([maze.Grid[5, 5]]);
                        LoadGame(false, false, maze, startPoints, endPoints, 10, 10, 10, 3);
                        return;
                    }
                }
            }
        }

        //Menu de laberintos según cantidad de jugadores
        //Creador de nuevo laberinto
        //Editor de laberinto
        //Borrar laberinto

        private static void LoadGame(bool isCoopGame, bool playWithBots, Maze maze, List<Cell> startPoints, List<Cell> endPoints, int numberOfObstacles, int numberOfTraps, int numberOfNPCs, int numberOfTokens)
        {
            GM.InitializeLevel(isCoopGame, playWithBots, maze, startPoints, endPoints, numberOfObstacles, numberOfTraps, numberOfNPCs, numberOfTokens);
            TokenSelectionMenu();
        }

        private static void TokenSelectionMenu()
        {
            string? option;
            List<TypeOfPlayableCharacter> charactersTypes = new List<TypeOfPlayableCharacter>();
            foreach (Player player in GM.ActivePlayers)
            {
                charactersTypes.Clear();
                while(charactersTypes.Count < GM.NumberOfTokensByPlayers)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Hora de elgir tus fichas, {0}", player.Name);
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("1 - Héroe");
                    Console.WriteLine();
                    Console.Write("> Mi ficha número {0} será ", player.Tokens.Count + 1);
    
                    option = Console.ReadLine();
    
                    if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                    {
                        Console.WriteLine("Debes poner una opción válida.");
                        Thread.Sleep(500);
                        continue;
                    }
                    else
                    {
                        switch (number)
                        {
                            case 1:
                                charactersTypes.Add(TypeOfPlayableCharacter.Hero);
                                break;
                            default:
                                Console.WriteLine("Opción no encontrada.");
                                Thread.Sleep(500);
                                continue;
                        }
                    }
                }
                GM.GeM.TryGenerateTokensForPlayer(player, charactersTypes);
            }
            StartGame();
        }

        private static void StartGame()
        {
            int[,] matriz;
            Player player;
            while(true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Clear();
                    Console.WriteLine("");
                    matriz = GenerateAssosiatedMatriz();
                    PrintMaze(matriz);
                    Console.WriteLine("");

                    if (i < GM.ActivePlayers.Count)
                    {
                        player = GM.ActivePlayers[i];
                        PlayerTurn(player);
                    }
                    else
                    {
                        if (i == 4)
                        {
                            foreach (NPC nonPlayable in GM.NonActivePlayers[4 - GM.ActivePlayers.Count].Tokens)
                            {
                                GM.NATM.TakeTurn(nonPlayable);
                            }
                        }
                        else if (GM.IsCoopGame)
                        {
                            //player = GM.NonActivePlayers[i - GM.ActivePlayers.Count];
                            //foreach(PlayableCharacter playable in player.Tokens)
                            //{
                            //    GM.NATM.TakeTurn(Playable)
                            //}
                        }
                    }
                }
                GM.EventPassTurn();
            }
        }

        private static void PlayerTurn(Player player)
        {
            string? option;
            int[,] matriz;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("");
                matriz = GenerateAssosiatedMatriz();
                PrintMaze(matriz);
                Console.WriteLine("");
                Console.WriteLine(player.Name + ":");
                List<string> options = [..Enum.GetNames(typeof(OptionsByPlayer))];
                for(int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, options[i]);
                }
                Console.WriteLine("0 - Menú");

                option = Console.ReadLine();  
                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 0:
                            GameMenu();
                            break;
                        case 1:
                            TokensMenu(player);
                            break;
                        case 2:
                            return;
                        default:
                            Console.WriteLine("Opción no encontrada.");
                            Thread.Sleep(500);
                            continue;
                    }
                }
            }
        }

        private static void TokensMenu(Player player)
        {
            string? option;
            int[,] matriz;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("");
                matriz = GenerateAssosiatedMatriz();
                PrintMaze(matriz);
                Console.WriteLine("");
                Console.WriteLine("Fichas jugables de " + player.Name + ":");
                for (int i = 0; i < GM.NumberOfTokensByPlayers; i++)
                {
                    Console.WriteLine("{0} - {1} en {2},{3}.", i + 1, player.Tokens[i].GetType().Name, player.Tokens[i].X, player.Tokens[i].Y);
                }
                Console.WriteLine("0 - Regresar");

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 0:
                            return;
                        case > 0:
                            if (number <= GM.NumberOfTokensByPlayers)
                            {
                                TokenTurn(player.Tokens[number - 1] as PlayableCharacter);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Opción no encontrada.");
                                Thread.Sleep(500);
                                continue;
                            }
                        default:
                            Console.WriteLine("Opción no encontrada.");
                            Thread.Sleep(500);
                            continue;
                    }
                }
            }
        }

        private static void TokenTurn(PlayableCharacter? character)
        {
            if (character is null) 
            {
                Console.WriteLine("Personaje no jugable.");
                Thread.Sleep(500);
                return;
            }
            string? option;
            int[,] matriz;
            int turnsTillAbility;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("");
                matriz = GenerateAssosiatedMatriz();
                PrintMaze(matriz);
                Console.WriteLine("");
                Console.WriteLine("Opciones de " + character.GetType().Name + ":");
                List<string> options = [..Enum.GetNames(typeof(OptionsByToken))];
                for(int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, options[i]);
                }
                Console.WriteLine("0 - Regresar");

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 0:
                            return;
                        case 1:
                            if (character.HasMoved)
                            {
                                Console.WriteLine("Este personaje ya se ha movido en el turno actual.");
                                Thread.Sleep(500);
                                continue;
                            }
                            MoveInTurn(character);
                            break;
                        case 2:
                            if (character.HasAttacked)
                            {
                                Console.WriteLine("Este personaje ya ha atacado en el turno actual.");
                                Thread.Sleep(500);
                                continue;
                            }
                            AttackInTurn(character);
                            break;
                        case 3:
                            turnsTillAbility = character.LastTurnUsingAbility + character.AbilityRecoveryTime - GM.Turn;
                            if (turnsTillAbility > 0)
                            {
                                Console.WriteLine("Habilidad desactivada hasta dentro de {0} turnos.", turnsTillAbility);
                                Thread.Sleep(500);
                                continue;
                            }
                            else
                            {
                                UseAbility(character);
                            }
                            break;
                        default:
                            Console.WriteLine("Opción no encontrada.");
                            Thread.Sleep(500);
                            continue;
                    }
                }
            }
        }

        private static void GameMenu()
        {
            string? option;
            while(true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Menú");
                Console.WriteLine("----");
                Console.WriteLine("1 - Regresar a la partida");
                Console.WriteLine("2 - Ir a Menú Principal");
                Console.WriteLine("3 - Salir del Juego");
                Console.WriteLine();

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 1:
                            return;
                        case 2:
                            PrincipalMenu();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Opción no encontrada.");
                            Thread.Sleep(500);
                            continue;
                    }
                }
            }
        }

        private static void MoveInTurn(PlayableCharacter character)
        {
            List<(Cell cell, int distance)> cellsWithDistance = GM.MM.GetCellsInRange(GM.maze.Grid[character.X, character.Y], character.Speed);
            List<Cell> cells = new List<Cell>();
            foreach ((Cell cell, int distance) cell in cellsWithDistance)
            {
                cells.Add(cell.cell);
            }
            GM.MM.ColorCells(cells);
            string? option;
            int[,] matriz;
            Cell destinyCell;
            int destinyDistance = 0;
            int x;
            int y;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                matriz = GenerateAssosiatedMatriz();
                PrintMaze(matriz);
                Console.WriteLine("");
                Console.WriteLine("Opciones:");
                Console.WriteLine("0 - Regresar");
                Console.WriteLine("1 - Ingresar cordenadas");
                Console.WriteLine("");

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 0:
                            GM.MM.CleanColor();
                            return;
                        case 1:
                            Console.Write("Posición X: ");
                            option = Console.ReadLine();
                            if(string.IsNullOrEmpty(option) || !int.TryParse(option, out x))
                            {
                                Console.WriteLine("Debes poner una opción válida.");
                                Thread.Sleep(500);
                                continue;
                            }
                            Console.Write("Posición Y: ");
                            option = Console.ReadLine();
                            if(string.IsNullOrEmpty(option) || !int.TryParse(option, out y))
                            {
                                Console.WriteLine("Debes poner una opción válida.");
                                Thread.Sleep(500);
                                continue;
                            }
                            if (x >= GM.maze.Width || y >= GM.maze.Height || x < 0 || y < 0)
                            {
                                Console.WriteLine("Debes poner una casilla válida.");
                                Thread.Sleep(500);
                                continue;
                            }
                            else
                            {
                                destinyCell = GM.maze.Grid[x,y];
                                foreach ((Cell cell, int distance) cell in cellsWithDistance)
                                {
                                    if (destinyCell.Equals(cell.cell)) 
                                    {
                                        destinyDistance = cell.distance;
                                        break;
                                    }
                                }
                                if (destinyDistance == 0)
                                {
                                    Console.WriteLine("Debes poner una casilla válida.");
                                    Thread.Sleep(500);
                                    continue;
                                }
                                GM.MM.MoveToken(character, destinyCell, destinyDistance);
                                character.Moved();
                                GM.MM.CleanColor();
                                return;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private static void AttackInTurn(PlayableCharacter character)
        {
            List<Character> oponents = GM.AM.GetPossibleOponents(character);
            GM.AM.TargetCharacters(oponents);
            string? option;
            int[,] matriz;
            Character oponent;
            int oponentInitialLife;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                matriz = GenerateAssosiatedMatriz();
                PrintMaze(matriz);
                Console.WriteLine("");
                Console.WriteLine("Opciones:");
                for(int i = 0; i < oponents.Count; i++)
                {
                    Console.WriteLine("{0} - {1} en {2},{3}", i + 1, oponents[i].GetType().Name, oponents[i].X, oponents[i].Y);
                }
                Console.WriteLine("0 - Regresar");
                Console.WriteLine("");

                option = Console.ReadLine();

                if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    switch (number)
                    {
                        case 0:
                            GM.AM.CleanTargets();
                            return;
                        default:
                            if(number > oponents.Count)
                            {
                                Console.WriteLine("Opción no encontrada.");
                                Thread.Sleep(500);
                                continue;
                            }
                            else
                            {
                                oponent = oponents[number - 1];
                                oponentInitialLife = oponent.ActualLife;
                                Console.Write("{0} decide atacar a {1}", character.GetType().Name, oponent.GetType().Name);
                                Thread.Sleep(100);
                                if(character.Attack(oponent))
                                {
                                    Console.WriteLine(" y es efectivo.");
                                    Thread.Sleep(100);
                                    GM.AM.StabilizeToken(oponent);
                                    if ((oponentInitialLife != oponent.MaxLife && oponent.ActualLife == oponent.MaxLife) || oponent.ActualState == State.Inactive) 
                                    {
                                        GM.EventDefetedToken(oponent, character, 0);
                                    }
                                    if (oponentInitialLife > oponent.ActualLife)
                                    {
                                        GM.EventDemagedToken(oponent, character, oponentInitialLife - oponent.ActualLife);
                                    }
                                    else if (oponentInitialLife < oponent.ActualLife)
                                    {
                                        GM.EventHealedToken(oponent, character, oponent.ActualLife - oponentInitialLife);
                                    }
                                    GM.EventChangeInMazeMade();
                                }
                                else
                                {
                                    Console.WriteLine(" pero falla.");
                                    Thread.Sleep(100);
                                }
                                GM.AM.CleanTargets();
                                character.Attacked();
                                return;
                            }
                    }
                }
            }
        }

        private static void UseAbility(PlayableCharacter character)
        {
            switch(character.GetType().Name)
            {
                case "Hero":
                    List<Character> oponents = GM.AM.GetPossibleOponents(character);
                    GM.AM.TargetCharacters(oponents);
                    string? option;
                    int[,] matriz;
                    Character oponent;
                    int oponentInitialLife;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("");
                        matriz = GenerateAssosiatedMatriz();
                        PrintMaze(matriz);
                        Console.WriteLine("");
                        Console.WriteLine("Opciones:");
                        for(int i = 0; i < oponents.Count; i++)
                        {
                            Console.WriteLine("{0} - {1} en {2},{3}", i + 1, oponents[i].GetType().Name, oponents[i].X, oponents[i].Y);
                        }
                        Console.WriteLine("0 - Regresar");
                        Console.WriteLine("");
        
                        option = Console.ReadLine();
        
                        if(string.IsNullOrEmpty(option) || !int.TryParse(option, out int number))
                        {
                            Console.WriteLine("Debes poner una opción válida.");
                            Thread.Sleep(500);
                            continue;
                        }
                        else
                        {
                            switch (number)
                            {
                                case 0:
                                    GM.AM.CleanTargets();
                                    return;
                                default:
                                    if(number > oponents.Count)
                                    {
                                        Console.WriteLine("Opción no encontrada.");
                                        Thread.Sleep(500);
                                        continue;
                                    }
                                    else
                                    {
                                        oponent = oponents[number - 1];
                                        oponentInitialLife = oponent.ActualLife;
                                        Console.WriteLine("{0} decide usar habilidad ''Espada Sagrada'' en {1}.", character.GetType().Name, oponent.GetType().Name);
                                        Thread.Sleep(100);
                                        if(character.ActivateAbility(GM.Turn, oponent))
                                        {
                                            Console.WriteLine(" y es efectivo.");
                                            Thread.Sleep(100);
                                            GM.AM.StabilizeToken(oponent);
                                            if ((oponentInitialLife != oponent.MaxLife && oponent.ActualLife == oponent.MaxLife) || oponent.ActualState == State.Inactive) 
                                            {
                                                GM.EventDefetedToken(oponent, character, 0);
                                            }
                                            if (oponentInitialLife > oponent.ActualLife)
                                            {
                                                GM.EventDemagedToken(oponent, character, oponentInitialLife - oponent.ActualLife);
                                            }
                                            else if (oponentInitialLife < oponent.ActualLife)
                                            {
                                                GM.EventHealedToken(oponent, character, oponent.ActualLife - oponentInitialLife);
                                            }
                                            GM.EventChangeInMazeMade();
                                        }
                                        else
                                        {
                                            Console.WriteLine(" pero falla.");
                                            Thread.Sleep(100);
                                        }
                                        GM.AM.CleanTargets();
                                        return;
                                    }
                            }
                        }
                    }
                default:
                    break;
            }
        }

        //turnos
        //player
        //items
        //tokens

        public static void DefetedMessage(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            if(modificator == 0)
            {
                Console.Write("{0} ha sido derrotado", affectedCharacter.GetType().Name);
                if (modificaterObject is not null) Console.WriteLine(" por {0}.", modificaterObject.GetType().Name);
                else Console.WriteLine(".");
                Thread.Sleep(100);
            }
        }

        public static void HealedMessage(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            if(modificator <= 0)
            {
                Console.WriteLine("{0} se mantiene igual.", affectedCharacter.GetType().Name);
                if (modificaterObject is not null) Console.WriteLine("{0} no efectivo.", modificaterObject.GetType().Name);
                Thread.Sleep(100);
            }
            else
            {
                Console.Write("{0} ha restaurado {1} de vida", affectedCharacter.GetType().Name, modificator);
                if (modificaterObject is not null) Console.WriteLine(" gracias a {0}.", modificaterObject.GetType().Name);
                else Console.WriteLine(".");
                Thread.Sleep(100);
            }
        }

        public static void DemagedMessage(Character affectedCharacter, Interactive? modificaterObject, int modificator)
        {
            if(modificator <= 0)
            {
                Console.WriteLine("{0} se mantiene igual.", affectedCharacter.GetType().Name);
                if (modificaterObject is not null) Console.WriteLine("{0} no efectivo.", modificaterObject.GetType().Name);
                Thread.Sleep(100);
            }
            else
            {
                Console.Write("{0} ha perdido {1} de vida", affectedCharacter.GetType().Name, modificator);
                if (modificaterObject is not null) Console.WriteLine(" debido a {0}.", modificaterObject.GetType().Name);
                else Console.WriteLine(".");
                Thread.Sleep(100);
            }
        }

        public static void RefreshMaze()
        {
            Console.Clear();
            int[,] matriz = GenerateAssosiatedMatriz();
            PrintMaze(matriz);
        }

        public static int[,] GenerateAssosiatedMatriz()
        {
            int[,] matriz = new int[2*GM.maze.Width + 1, 2*GM.maze.Height + 1];
            for (int x = 0; x < GM.maze.Grid.GetLength(0); x++)
            {    
                for (int y = 0; y < GM.maze.Grid.GetLength(1); y++)
                {
                    Cell cell = GM.maze.Grid[x, y];
                    string? typeInteractive = cell.Interactive?.GetType().Name;

                    //Build walls
                    if (cell.X == 0) { matriz[0, 2*cell.Y] = matriz[0, 2*cell.Y + 1] = matriz[0, 2*cell.Y + 2] = 1; }
                    if (cell.Y == 0) { matriz[2*cell.X, 0] = matriz[2*cell.X + 1, 0] = matriz[2*cell.X + 2, 0] = 1; }
                    if (cell.Walls["right"]) { matriz[2*cell.X + 2, 2*cell.Y] = matriz[2*cell.X + 2, 2*cell.Y + 1] = matriz[2*cell.X + 2, 2*cell.Y + 2] = 1; }
                    else { matriz[2*cell.X + 2, 2*cell.Y + 1] = 0; matriz[2*cell.X + 2, 2*cell.Y + 2] = 0; }
                    if (cell.Walls["bottom"]) { matriz[2*cell.X, 2*cell.Y + 2] = matriz[2*cell.X + 1, 2*cell.Y + 2] = matriz[2*cell.X + 2, 2*cell.Y + 2] = 1; }
                    else { matriz[2*cell.X + 1, 2*cell.Y + 2] = 0; }

                    //Determine what the cell contains and build it
                    switch (typeInteractive)
                    {
                        case "TemporalWall":
                            if (cell.Interactive is not null && cell.Interactive.ActualState == State.Active) {matriz[2*cell.X + 1, 2*cell.Y + 1] = 3; }
                            else {matriz[2*cell.X + 1, 2*cell.Y + 1] = 2; }
                            break;
                        default:
                            if (GM.EndPoints.Contains(cell)) {matriz[2*cell.X + 1, 2*cell.Y + 1] = 4; }
                            else {matriz[2*cell.X + 1, 2*cell.Y + 1] = 2; }
                            break;
                    }
                    if (cell.IsColored) matriz[2*cell.X + 1, 2*cell.Y + 1] = 5;
                }
            }
            return matriz;
        }
    
        public static void PrintMaze(int[,] matriz)
        {
            Console.Write("    ");
            int numberOfPlayer;
            List<Character> characters;
            for (int x = 0; x < matriz.GetLength(0); x++)
            {
                if (x < 10) Console.Write(" " + x.ToString() + " ");
                else if (x < 100) Console.Write(" " + x.ToString());
                else Console.Write(x.ToString());
            }
            Console.WriteLine("");
            for (int y = 0; y < matriz.GetLength(1); y++)
            {
                if (y < 10) Console.Write(y.ToString() + "   ");
                else if (y < 100) Console.Write(y.ToString() + "  ");
                else Console.Write(y.ToString() + " ");
    
                for (int x = 0; x < matriz.GetLength(0); x++)
                {
                    if (matriz[x,y] < 2)
                    {
                        switch (matriz[x,y])
                        {
                            case 0:
                                Console.Write("   ");
                                break;
                            case 1:
                                Console.Write("███");
                                break;
                            default:
                                Console.Write("   ");
                                break;
                        }
                    }
                    else
                    {
                        characters = GM.AM.GetCharactersInCell(GM.maze.Grid[x/2, y/2]);
                        if (1 > characters.Count) numberOfPlayer = 0;
                        else 
                        {
                            if(characters[0].IsTargeted) numberOfPlayer = 6;
                            else numberOfPlayer = GetNumberOfPlayerByToken(characters[0]);
                        }
                        switch (numberOfPlayer)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 2: 
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 4: 
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 5: 
                                Console.Write("•");
                                break;
                            case 6:
                                Console.Write("+");
                                break;
                            default:
                                Console.Write(" ");
                                break;
                        }
                        switch (matriz[x,y])
                        {
                            case 2:
                                Console.Write("·");
                                break;
                            case 3:
                                Console.Write("█");
                                break;
                            case 4:
                                Console.Write("X");
                                break;
                            case 5:
                                Console.Write("□");
                                break;
                            default:
                                Console.Write(" ");
                                break;
                        }
                        if (2 > characters.Count) numberOfPlayer = 0;
                        else 
                        {
                            if(characters[1].IsTargeted) numberOfPlayer = 6;
                            else numberOfPlayer = GetNumberOfPlayerByToken(characters[1]);
                        }
                        switch (numberOfPlayer)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 2: 
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 4: 
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("•");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case 5: 
                                Console.Write("•");
                                break;
                            case 6:
                                Console.Write("+");
                                break;
                            default:
                                Console.Write(" ");
                                break;
                        }
                    }
                }
                Console.WriteLine("");
            }
        }

        private static int GetNumberOfPlayerByToken(Character? character)
        {
            if (character is not null)
            {
                foreach(Player player in GM.ActivePlayers)
                {
                    if (player.Tokens.Contains(character)) return GM.ActivePlayers.IndexOf(player) + 1;
                }
                foreach(Player player in GM.NonActivePlayers)
                {
                    if (player.Tokens.Contains(character)) return GM.ActivePlayers.IndexOf(player) + GM.ActivePlayers.Count + 1;
                }
            }
            return 0;
        }
    }
}