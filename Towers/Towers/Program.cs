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
        static int terrainHeight = 58;
        static int terrainWidth = 150;
        static int FirstTowerAngle = 45;
        static int SecondTowerAngle = 45;
        static int firstTowerVelocity;
        static int secondTowerVelocity;
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
            BallMovement(12, 30, false);
            //PrintOnPosition(149, 69, 'N');
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
            terrain = new char[Console.WindowHeight , Console.WindowWidth];
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
                startingPointX = firstTowerCoordinates[1] + 1;
                startingPointY = firstTowerCoordinates[0] - 5;
            }
            else if (activePlayer == false)
            {
                startingPointX = secondTowerCoordinates[1] - 2;
                startingPointY = secondTowerCoordinates[0] - 5;
            }
            int oldX = 0;
            int oldY = 0;
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
                    if (terrain[y, x] == '#')
                    {
                        HitTerrain(y, x);
                        return;
                    }
                    if (terrain[y, x] == '2')
                    {
                        HitTower(y, x);
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
                    if (terrain[y, x] == '#')
                    {
                        HitTerrain(y, x);
                        return;
                    }
                    if (terrain[y, x] == '1')
                    {
                        HitTower(y, x);
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
                    if (terrain[row, col] == '#' && (row != terrainHeight -1 || col != terrainHeight - 1))
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
                    else if (row != terrainHeight - 1 || col != terrainHeight - 1)
                    {
                        terrainBuilder.Append(" ");
                    }
                }
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(terrainBuilder.ToString());

            //Draw Towers
            Console.CursorVisible = false;
            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '1')
                    {
                        PrintOnPosition(col, row, terrain[row, col], ConsoleColor.Red);
                    }
                    else if (terrain[row, col] == '2')
                    {
                        PrintOnPosition(col, row, terrain[row, col], ConsoleColor.Blue);
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

        static void ModifyAngle(ConsoleKeyInfo key)
        {
            const int sencitivity = 5;
            const int maxAngle = 90;
            const int minAngle = -45;

            if (key.Key == ConsoleKey.UpArrow && activePlayer == true && FirstTowerAngle < maxAngle)
            {
                FirstTowerAngle += sencitivity;
            }
            else if (key.Key == ConsoleKey.DownArrow && activePlayer == true && FirstTowerAngle > minAngle)
            {
                FirstTowerAngle -= sencitivity;
            }
            else if (key.Key == ConsoleKey.W && activePlayer == false && SecondTowerAngle < maxAngle)
            {
                SecondTowerAngle += sencitivity;
            }
            else if (key.Key == ConsoleKey.S && activePlayer == false && SecondTowerAngle > minAngle)
            {
                SecondTowerAngle -= sencitivity;
            }
        }

        static void ModifyVelocity(ConsoleKeyInfo key)
        {
            const int sencitivity = 5;
            const int maxVelocity = 100;
            const int minVelocity = 0;

            if (key.Key == ConsoleKey.RightArrow && activePlayer == true && firstTowerVelocity < maxVelocity)
            {
                firstTowerVelocity += sencitivity;
            }
            else if (key.Key == ConsoleKey.LeftArrow && activePlayer == true && firstTowerVelocity > minVelocity)
            {
                firstTowerVelocity -= sencitivity;
            }
            else if (key.Key == ConsoleKey.D && activePlayer == false && secondTowerVelocity < maxVelocity)
            {
                firstTowerVelocity += sencitivity;
            }
            else if (key.Key == ConsoleKey.A && activePlayer == false && secondTowerVelocity > minVelocity)
            {
                firstTowerVelocity -= sencitivity;
            }
        }

        static void PrintPanel()
        { 
        
        }

    }
}
