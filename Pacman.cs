using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace CSLight
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            bool isPlaying = true;
            Random random = new Random();
            int pacmanX, pacmanY;
            int pacmanDX = 0, pacmanDY = 1;
            int alldots=0;
            bool isAlive=true;

            int ghostX;
            int ghostY;
            int ghostDY = 1;
                int ghostDX=0;

            int collectdots = 0;
            char[,] map = Readmap("Map1", out pacmanX, out pacmanY, out ghostX,out ghostY, ref alldots, ref collectdots);

            DrawMap(map);
            while (isPlaying)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($" Собрано: {collectdots }/{ alldots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            pacmanDX = -1; pacmanDY = 0;
                            break;
                        case ConsoleKey.DownArrow:
                            pacmanDX = 1; pacmanDY = 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            pacmanDX = 0; pacmanDY = -1;
                            break;
                        case ConsoleKey.RightArrow:
                            pacmanDX = 0; pacmanDY = 1;
                            break;

                    }

                }
                if (map[pacmanX + pacmanDX, pacmanY + pacmanDY] != '#')
                {
                    CollectDots(map, pacmanX, pacmanY, ref collectdots);
                    Move(map,' ','@',ref pacmanX, ref pacmanY, pacmanDX, pacmanDY);
                    
                }
                if (map[ghostX + ghostDX, ghostY + ghostDY] != '#')
                {
                    Move(map,'.','$', ref ghostX, ref ghostY, ghostDX, ghostDY);
                   
                }
                else
                {
                    ChangeDirection(random,ref ghostDX, ref ghostDY);
                }
                
                

                System.Threading.Thread.Sleep(17);
                if (ghostX==pacmanX&& ghostY==pacmanY)
                {
                    isAlive = false;
                        isPlaying = false; ;
                }
                if (collectdots==alldots)
                {
                    isPlaying = false;
                }

            }
            if (!isAlive)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Nikita Gundone");
            }
            if (collectdots == alldots)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Nikita Pidor");
            }

        }
        static void ChangeDirection(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1; DY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1; DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0; DY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0; DY = 1;
                    break;

            }
        }

        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int ghostDir = random.Next(1, 5);
            switch (ghostDir)
            {
                case 1:
                    DX = -1; DY = 0;
                    break;
                case 2:
                    DX = 1; DY = 0;
                    break;
                case 3:
                    DX = 0; DY = -1;
                    break;
                case 4:
                    DX = 0; DY = 1;
                    break;

            }
        }

        static void CollectDots(char[,] map ,int pacmanX, int pacmanY,ref int collectdots)
        {
            if (map[pacmanX, pacmanY] == '.')
            {
                collectdots++;
                map[pacmanX, pacmanY] = ' ';
            }
        }

        static void Move(char[,] map,char prev,char symbol,ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X,Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        static char[,] Readmap(string mapName, out int pacmanX, out int pacmanY,out int ghostX, out int ghostY, ref int  alldots, ref int collectdots)
        {
            pacmanY = 0;
            pacmanX = 0;
            ghostX = 0;
            ghostY = 0;
            string[] newfile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newfile.Length, newfile[0].Length];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newfile[i][j];

                    if (map[i, j] == '@')
                    {
                        pacmanX = i;
                        pacmanY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i,j]=='$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i,j] == ' ')
                    {
                        map[i, j] = '.';
                        alldots++;
                    }
                }
            }
            return map;
        }
    }
}