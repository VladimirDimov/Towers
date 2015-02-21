using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Towers
{
    class Program
    {
        static int terrainHeight = 70;
        static int terrainWidth = 150;
        static int FirstTowerAngle = 45;
        static int SecondTowerAngle = 45;
        static bool activePlayer;
        static int[] firstTowerCoordinates = new int[2];
        static int[] secondTowerCoordinates = new int[2];
        static char[,] terrain;
        private static int d1;

        static void Main()
        {
            SetConsole();
            BuildRandomTerrain();
            PrintFirstTower(10);
            PrintSecondTower(terrainWidth - 10);
            DrawTerrain();
            BallMovement(7, 84, false);
            PrintOnPosition(5, 47, 'N');
            //BuildRandomTerrain();
            while (true)
            {
                Thread.Sleep(150);
            }
        }

        static void SetConsole()
        {
            Console.BufferHeight = Console.WindowHeight = terrainHeight;
            Console.BufferWidth = Console.WindowWidth = terrainWidth;
            terrain = new char[Console.WindowHeight, Console.WindowWidth];
        }

        static void SetGame()
        {

        }

        static void Menu()
        {

        }

        static void BuildTerrainFromFile(char[,] terrain, string file)
        {

        }

        static void BuildRandomTerrain()
        {
            int minHeight = 35;
            int maxStep = 2;
            int maxHeight = 60;
            int currentHeight = 45;
            int nextHeight;
            Random rnd = new Random();

            for (int col = 0; col < terrainWidth; col++)
            {
                do
                {
                    nextHeight = rnd.Next(currentHeight - maxStep, currentHeight + maxStep + 1);
                } while (!(minHeight <= nextHeight && nextHeight <= maxHeight));
                currentHeight = nextHeight;
                for (int row = currentHeight; row < terrainHeight; row++)
                {
                    terrain[row, col] = '#';
                }
            }
        }

        static void PrintFirstTower(int x)
        {
            //set first tower coordinates
            int towerHeight = 5;
            firstTowerCoordinates[1] = x;
            for (int row = terrainHeight - 1; row >= 0; row--)
            {
                if (terrain[row, x] != '#')
                {
                    firstTowerCoordinates[0] = row;
                    break;
                }
            }
            //print first Tower
            for (int row = firstTowerCoordinates[0]; row > firstTowerCoordinates[0] - towerHeight; row--)
            {
                for (int col = x - 1; col < x + 1; col++)
                {
                    terrain[row, col] = '1';
                }
            }
        }

        static void PrintSecondTower(int x)
        {
            int towerHeight = 5;
            //set second tower coordinates
            secondTowerCoordinates[1] = x;
            for (int row = terrainHeight - 1; row >= 0; row--)
            {
                if (terrain[row, x] != '#')
                {
                    secondTowerCoordinates[0] = row;
                    break;
                }
            }
            //print second Tower
            for (int row = secondTowerCoordinates[0]; row > secondTowerCoordinates[0] - towerHeight; row--)
            {
                for (int col = x - 1; col < x + 1; col++)
                {
                    terrain[row, col] = '2';
                }
            }
        }

        static void ActivePlayer(bool activePlayer)
        {
            //  return shooting parameters (velocity, angle, ...);

        }

        static void HitTerrain(int hitX, int hitY)
        {

        }

        static void HitTower(int hitX, int hitY)
        {

        }

        static void Impact(int hitX, int hitY)
        {

        }

        static void BallMovement(int velocity, int angle, bool activePlayer)
        {
            //return hitX and hitY;
            int startingPointX = 0;
            int startingPointY = 0;
            if (activePlayer == true)
            {
                startingPointX = firstTowerCoordinates[1];
                startingPointY = firstTowerCoordinates[0] - 5;
            }
            else if (activePlayer == false)
            {
                startingPointX = secondTowerCoordinates[1];
                startingPointY = secondTowerCoordinates[0] - 5;
            }
            int oldX = startingPointX;
            int oldY = startingPointY;
            int x;
            int y;
            int g = 1;
            double angleInRadians = angle * Math.PI / 180;

            if (activePlayer == true)
            {
                for (float time = 0; time < 2000; time += 0.1f)
                {
                    x = startingPointX + (int)(velocity * time * Math.Cos(angleInRadians));
                    y = (int)(startingPointY - (velocity * time * Math.Sin(angleInRadians) - (g * Math.Pow(time, 2)) / 2));
                    if (x > terrainWidth - 1 || y < 0 || x < 0 || y > terrainHeight - 1)
                    {
                        return;
                    }
                    if (terrain[y + 2, x] == '#')
                    {
                        HitTerrain(y + 2, x);
                        return;
                    }
                    if (terrain[y + 2, x] == '2')
                    {
                        HitTower(y + 2, x);
                        return;
                    }
                    if ((x != oldX && y != oldY) || (x > oldX + 3))
                    {
                        PrintOnPosition(x, y, '.', ConsoleColor.White);
                        oldX = x;
                        oldY = y;
                    }
                }
            }
            else if (activePlayer == false)
            {
                for (float time = 0; time < 2000; time += 0.1f)
                {
                    x = startingPointX - (int)(velocity * time * Math.Cos(angleInRadians));
                    y = (int)(startingPointY - (velocity * time * Math.Sin(angleInRadians) - (g * Math.Pow(time, 2)) / 2));
                    if (x > terrainWidth - 1 || y < 0 || x < 0 || y > terrainHeight - 1)
                    {
                        return;
                    }
                    if (terrain[y + 2, x] == '#')
                    {
                        HitTerrain(y + 2, x);
                        return;
                    }
                    if (terrain[y + 2, x] == '1')
                    {
                        HitTower(y + 2, x);
                        return;
                    }
                    if ((x != oldX && y != oldY) || (x < oldX - 3))
                    {
                        PrintOnPosition(x, y, '.', ConsoleColor.White);
                        oldX = x;
                        oldY = y;
                    }
                }
            }
        }

        static void DrawTerrain()
        {
            //Draw terrain
            StringBuilder terrainBuilder = new StringBuilder();

            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '#')
                    {
                        terrainBuilder.Append("#");
                    }
                    else if (terrain[row, col] == '1')
                    {
                        terrainBuilder.Append("1");
                    }
                    else if (terrain[row, col] == '2')
                    {
                        terrainBuilder.Append("2");
                    }
                    else
                    {
                        terrainBuilder.Append(" ");
                    }
                }
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(terrainBuilder.ToString());

            //Draw Towers
            Console.CursorVisible = false;
            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '1')
                    {
                        PrintOnPosition(col, row - 2, terrain[row, col], ConsoleColor.Red);
                    }
                    else if (terrain[row, col] == '2')
                    {
                        PrintOnPosition(col, row - 2, terrain[row, col], ConsoleColor.Blue);
                    }
                }
            }
        }

        static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
        }
    }
}
