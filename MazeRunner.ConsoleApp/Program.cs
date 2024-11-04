using System;
using MazeRunner.Core;
using MazeRunner.Core.Maze;

namespace MazeRunner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Generar Laberinto: 0");
                string? console = Console.ReadLine();
                if (console == null)
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    continue;
                }
                
                int option = int.Parse(console);
                
                if (option != 0)
                {
                    Console.WriteLine("Debes poner una opción válida.");
                    continue;
                }
                else
                {
                    //Console.Clear();
                    Maze maze = new Maze(10, 10);
                    maze.GenerateMaze();
                    int[,] matriz = GenerateAssosiatedMatriz(maze);
                    PrintMaze(matriz);
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

                    if (cell.X == 0) { matriz[0, 2*(cell.Y)] = matriz[0, 2*(cell.Y) + 1] = matriz[0, 2*(cell.Y) + 2] = 1; }
                    if (cell.Y == 0) { matriz[2*(cell.X), 0] = matriz[2*(cell.X) + 1, 0] = matriz[2*(cell.X) + 2, 0] = 1; }
                    if (cell.Walls["right"]) { matriz[2*(cell.X) + 2, 2*(cell.Y)] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 1] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 1; }
                    else { matriz[2*(cell.X) + 2, 2*(cell.Y) + 1] = 0; matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 0; }
                    if (cell.Walls["bottom"]) { matriz[2*(cell.X), 2*(cell.Y) + 2] = matriz[2*(cell.X) + 1, 2*(cell.Y) + 2] = matriz[2*(cell.X) + 2, 2*(cell.Y) + 2] = 1; }
                    else { matriz[2*(cell.X) + 1, 2*(cell.Y) + 2] = 0; }
                    matriz[2*(cell.X) + 1, 2*(cell.Y) + 1] = 2;
                }
            }
            return matriz;
        }
    
        public static void PrintMaze(int[,] matriz)
        {
            Console.WriteLine("");
            for (int x = 0; x < matriz.GetLength(0); x++)
            {
                string row = "    ";
    
                for (int y = 0; y < matriz.GetLength(1); y++)
                {
                    switch (matriz[x,y])
                    {
                        case 0:
                            row += " ";
                            break;
                        case 1:
                            row += "□";
                            break;
                        case 2:
                            row += "·";
                            break;
                        default:
                            row += " ";
                            break;
                    };
                }
    
                Console.WriteLine(row);
            }
            Console.WriteLine("");
        }
    }
}