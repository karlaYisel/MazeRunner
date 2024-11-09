using System;
using MazeRunner.Core;
using MazeRunner.Core.MazeGenerator;
using MazeRunner.Core.InteractiveObjects;
using System.Threading;

namespace MazeRunner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? console;
            Maze maze;
            int[,] matriz;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Salir: 0");
                Console.WriteLine("Generar Laberinto: 1");
                console = Console.ReadLine();
                if (string.IsNullOrEmpty(console))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(1000);
                    continue;
                }

                int option = int.Parse(console);
                
                switch (option)
                {
                    case 0:
                        return;
                    case 1:
                        maze = new Maze(10, 10, 10, 10, 10);
                        break;
                    default:
                        Console.WriteLine("Debes poner una opción válida.");
                        Thread.Sleep(1000);
                        continue;
                }

                break;
            }
            
            while (true)
            {
                Console.Clear();
                
                matriz = GenerateAssosiatedMatriz(maze);
                //Agregar posicion de fichas NPC y de jugadores
                PrintMaze(matriz);

                Console.WriteLine("Salir: 0");
                Console.WriteLine("Regenerar Laberinto: 1");
                Console.WriteLine("Verificar Casilla: 2");
                console = Console.ReadLine();
                if (string.IsNullOrEmpty(console))
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    Thread.Sleep(1000);
                    continue;
                }

                int option = int.Parse(console);
                switch (option)
                {
                    case 0:
                        return;
                    case 1:
                        maze.RegenerateMaze();
                        continue;
                    case 2:
                        while (true)
                        {
                            Console.Write("X: ");
                            string? X  = Console.ReadLine();
                            Console.Write("\nY: ");
                            string? Y = Console.ReadLine();
                            if (string.IsNullOrEmpty(X) || string.IsNullOrEmpty(Y)) 
                            {
                                Console.WriteLine("Debes poner una opción válida.");
                                Thread.Sleep(1000);
                                continue;
                            }
                            
                            int x = int.Parse(X);
                            int y = int.Parse(Y);
                            Cell cell = maze.Grid[x, y];
                            if (cell.Interactive?.GetType() is null)
                            {
                                Console.WriteLine("Está vacía.");
                            }
                            else
                            {
                                Console.WriteLine("Es {0}", cell.Interactive.GetType().Name);
                            }
                            Thread.Sleep(1000);
                            break;
                        }
                        break;
                    default:
                        Console.WriteLine("Debes poner una opción válida.");
                        Thread.Sleep(1000);
                        continue;
                }
            }
        }
    
        public static int[,] GenerateAssosiatedMatriz(Maze maze)
        {
            int[,] matriz = new int[2*(maze.Width) + 1, 2*(maze.Height) + 1];
            for (int x = 0; x < maze.Grid.GetLength(0); x++)
            {    
                for (int y = 0; y < maze.Grid.GetLength(1); y++)
                {
                    Cell cell = maze.Grid[x, y];
                    string? typeInteractive = cell.Interactive?.GetType().Name;

                    //Build walls
                    if (cell.X == 0) { matriz[0, 2*(cell.Y)] = matriz[0, 2*(cell.Y) + 1] = matriz[0, 2*(cell.Y) + 2] = 1; }
                    if (cell.Y == 0) { matriz[2*(cell.X), 0] = matriz[2*(cell.X) + 1, 0] = matriz[2*(cell.X) + 2, 0] = 1; }
                    if (cell.Walls["right"]) { matriz[2*(cell.X) + 2, 2*(cell.Y)] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 1] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 1; }
                    else { matriz[2*(cell.X) + 2, 2*(cell.Y) + 1] = 0; matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 0; }
                    if (cell.Walls["bottom"]) { matriz[2*(cell.X), 2*(cell.Y) + 2] = matriz[2*(cell.X) + 1, 2*(cell.Y) + 2] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 1; }
                    else { matriz[2*(cell.X) + 1, 2*(cell.Y) + 2] = 0; }

                    //Determine what the cell contains and build it
                    switch (typeInteractive)
                    {
                        case "TemporalWall":
                            if (cell.Interactive is not null && cell.Interactive.ActualState == State.Active) {matriz[2*(cell.X) + 1, 2*(cell.Y) + 1] = 3; }
                            else {matriz[2*(cell.X) + 1, 2*(cell.Y) + 1] = 2; }
                            break;
                        case "SpikeTrap":
                            matriz[2*(cell.X) + 1, 2*(cell.Y) + 1] = 4;
                            break;
                        default:
                            matriz[2*(cell.X) + 1, 2*(cell.Y) + 1] = 2;
                            break;
                    }
                }
            }
            return matriz;
        }
    
        public static void PrintMaze(int[,] matriz)
        {
            Console.WriteLine("");
            for (int y = 0; y < matriz.GetLength(1); y++)
            {
                string row = "    ";
    
                for (int x = 0; x < matriz.GetLength(0); x++)
                {
                    switch (matriz[x,y])
                    {
                        case 0:
                            row += "   ";
                            break;
                        case 1:
                            row += "███";
                            break;
                        case 2:
                            row += " · ";
                            break;
                        case 3:
                            row += " ֍ ";
                            break;
                        case 4:
                            row += " ʘ ";
                            break;
                        default:
                            row += "   ";
                            break;
                    };
                }
    
                Console.WriteLine(row);
            }
            Console.WriteLine("");
        }
    }
}