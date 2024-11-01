namespace MazeRunner;

using System;

using MazeRunner.Core;

//TODO: Vincular a la solución
public class ConsoleMazeRunner
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Generar Laberinto: 0");
            int? option = int.Parse(Console.ReadLine());

            if (option != 0)
            {
                Console.WriteLine("Debes poner una opción válida.");
                continue;
            }
            else
            {
                Console.Clear();
                Maze maze = new Maze(10, 10);
                maze.GenerateMaze();
                int[,] matriz = GenerateAsosiatedMatriz(maze);
                PrintMaze(matriz);
            }
        }
    }

    public int[,] GenerateAsosiatedMatriz(Maze maze)
    {
        int[,] matriz = new int[2(maze.Width) + 1, 2(maze.Height) + 1];
        foreach (Cell cell in maze)
        {    
            if (cell.X == 0) { matriz[0, 2(cell.Y)] = matriz[0, 2(cell.Y) + 1] = matriz[0, 2(cell.Y) + 2] = 1; }
            if (cell.Y == 0) { matriz[2(cell.X), 0] = matriz[2(cell.X) + 1, 0] = matriz[2(cell.X) + 2, 0] = 1; }
            if (cell.Walls["right"]) { matriz[2(cell.X) + 2, 2(cell.Y)] = matriz[2(cell.X) + 2, 2(cell.Y) + 1] = matriz[2(cell.X) + 2, 2(cell.Y) + 2] = 1; }
            else { matriz[2(cell.X) + 2, 2(cell.Y) + 1] = 0; matriz[2(cell.X) + 2, 2(cell.Y) + 2] = 0; }
            if (cell.Walls["bottom"]) { matriz[2(cell.X), 2(cell.Y) + 2] = matriz[2(cell.X) + 1, 2(cell.Y) + 2] = matriz[2(cell.X) + 2, 2(cell.Y) + 2] = 1; }
            else { matriz[2(cell.X) + 1, 2(cell.Y) + 2] = 0; }
            matriz[2(cell.X) + 1, 2(cell.Y) + 1] = 2;
        }
        
    }

    public void PrintMaze(int[,] matriz)
    {
        Console.WriteLine("");
        for (int x = 0; x < matriz.Count[0]; x++)
        {
            string row = "    ";

            for (int y = 0; y < matriz.Count[1]; y++)
            {
                switch (matriz[x,y])
                {
                    case 0:
                        row += " ";
                        break;
                    case 1:
                        row += "□";
                        break
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
    }
}