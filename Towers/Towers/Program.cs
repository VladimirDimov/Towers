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
        static int terrainHeight = 40;
        static int terrainWidth = 80;
        static int FirstTowerAngle = 45;
        static int SecondTowerAngle = 45;
        static bool activePlayer;
        static int[] firstTowerCoordinates = new int[2];
        static int[] secondTowerCoordinates = new int[2];
        static char[,] terrain;

        static void Main()
        {
            SetConsole();
            BuildRandomTerrain();
            PrintFirstTower(10);
            PrintSecondTower(terrainWidth - 10);
            DrawTerrain();
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
            int minHeight = 0;
            int maxStep = 2;
            int maxHeight = 20;
            int currentHeight = 20;
            int nextHeight;
            Random rnd = new Random();

            for (int col = 0; col < terrainWidth; col++)
            {
                do
                {
                    nextHeight = rnd.Next(currentHeight - maxStep, currentHeight + maxStep+1);
                } while (!(minHeight <= nextHeight && nextHeight <= maxHeight));
                currentHeight = nextHeight;
                for (int row = 0; row <= currentHeight; row++)
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
            for (int row = 0; row < terrainHeight; row++)
            {
                if (terrain[row, x] !='#')
                {
                    firstTowerCoordinates[0] = row;
                    break;
                }
            }
            //print first Tower
            for (int row = firstTowerCoordinates[0]; row < firstTowerCoordinates[0]+towerHeight; row++)
            {
                for (int col = x - 1; col < x + 1; col++)
                {
                    terrain[row,col] = '1';
                }
            }
        }

        static void PrintSecondTower(int x)
        {
            int towerHeight = 5;
            //set first tower coordinates
            secondTowerCoordinates[1] = x;
            for (int row = 0; row < terrainHeight; row++)
            {
                if (terrain[row, x] != '#')
                {
                    secondTowerCoordinates[0] = row;
                    break;
                }
            }
            //print first Tower
            for (int row = secondTowerCoordinates[0]; row < secondTowerCoordinates[0] + towerHeight; row++)
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
        }

        static void DrawTerrain()
        {
            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '#')
                    {
                        PrintOnPosition(col, terrainHeight - 1 - row, terrain[row, col], ConsoleColor.Green);
                    }
                    else if (terrain[row, col] == '1')
                    {
                        PrintOnPosition(col, terrainHeight - 1 - row, terrain[row, col], ConsoleColor.Red);
                    }
                    if (terrain[row, col] == '2')
                    {
                        PrintOnPosition(col, terrainHeight - 1 - row, terrain[row, col], ConsoleColor.Blue);
                    }
                }
            }
        }

        static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x,y);
            Console.Write(c);
        }
    }
}
