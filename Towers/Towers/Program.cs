using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Towers
{
    class Program
    {
        //Keys
        static ConsoleKey firstVelocityUpKey = ConsoleKey.D;
        static ConsoleKey firstVelocityDownKey = ConsoleKey.A;
        static ConsoleKey firstAngleUpKey = ConsoleKey.W;
        static ConsoleKey firstAngleDownKey = ConsoleKey.S;
        static ConsoleKey firstShootKey = ConsoleKey.Spacebar;

        static ConsoleKey secondVelocityUpKey = ConsoleKey.RightArrow;
        static ConsoleKey secondVelocityDownKey = ConsoleKey.LeftArrow;
        static ConsoleKey secondAngleUpKey = ConsoleKey.UpArrow;
        static ConsoleKey secondAngleDownKey = ConsoleKey.DownArrow;
        static ConsoleKey secondShootKey = ConsoleKey.Enter;

        static Random rnd = new Random();
        static int wind = 0;
        static string firstPlayerName = "Player 1";
        static string secondPlayerName = "Player 2";
        static int firstPlayerScore = 0;
        static int secondPlayerScore = 0;
        static int terrainHeight = 70;
        static int terrainWidth = 150;
        static int firstTowerAngle = 45;
        static int secondTowerAngle = 45;
        static int firstTowerVelocity = 0;
        static int secondTowerVelocity = 0;
        static int firstPlayerLivePoints = 100;
        static int secondPlayerLivePoints = 100;
        static bool activePlayer = true;
        static int[] firstTowerCoordinates = new int[2];
        static int[] secondTowerCoordinates = new int[2];
        static char[,] terrain;
        static byte menuChoice = 1;
        static string gameName = "## T - O - W - E - R - S ##";


        static void Main()
        {
            SetGame();
            while (true)
            {
                Game();
            }
        }

        static void SetConsole()
        {
            Console.BufferHeight = Console.WindowHeight = terrainHeight + 7;
            Console.BufferWidth = Console.WindowWidth = terrainWidth;
            Console.Title = "TOWERS2015MadeByHornedDemons";
        }

        static void SetGame()
        {
            SetConsole();
            Menu();
        }

        static void Game()
        {
            BuildRandomTerrain();
            PrintFirstTower(10);
            PrintSecondTower(terrainWidth - 10);
            RestoreLivePoints();

            while (firstPlayerLivePoints > 0 && secondPlayerLivePoints > 0)
            {
                DrawTerrain();
                PrintPanel();
                SetWind();
                try
                {
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            KeyPress(Console.ReadKey());
                            PrintPanel();
                        }
                    }
                }
                catch (Exception)
                {
                    Shoot();
                }
            }
            CheckForWinner();
        }

        static void Menu()
        {
            byte enterFlag = 0;
            while (true)
            {
                DrawMenu();
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    menuChoice += 1;
                }
                if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    menuChoice -= 1;
                }
                if (pressedKey.Key == ConsoleKey.Enter)
                {
                    enterFlag = 1;
                }

                if (menuChoice > 3)
                {
                    menuChoice = 1;
                }
                if (menuChoice < 1)
                {
                    menuChoice = 3;
                }

                switch (menuChoice)
                {
                    case 1:
                        if (enterFlag == 1)
                        {
                            return;
                        }
                        break;
                    case 2:
                        if (enterFlag == 1)
                        {
                            SetPlayersNames();
                            enterFlag = 0;
                            return;
                        }
                        break;
                    case 3:
                        if (enterFlag == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
                Console.Clear();
            }
        }

        private static void DrawMenu()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length - 6, gameName.Length);

            Console.WriteLine(gameName);

            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length, gameName.Length + 2);
            if (menuChoice == 1)
            {
                ColorLine(">New Game<");
            }
            else
            {
                Console.WriteLine("New Game");
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length, gameName.Length + 4);
            if (menuChoice == 2)
            {
                ColorLine(">Settings<");
            }
            else
            {
                Console.WriteLine("Settings");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length, gameName.Length + 6);
            if (menuChoice == 3)
            {
                ColorLine(">Quit Game<");
            }
            else
            {
                Console.WriteLine("Quit Game");
            }

        }

        static void ColorLine(string value)
        {
            // This method writes an entire line to the console with the string.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            // Reset the color.
            Console.ResetColor();
        }

        static void RestoreLivePoints()
        {
            firstPlayerLivePoints = 100;
            secondPlayerLivePoints = 100;
        }

        static void SetPlayersNames()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length, gameName.Length + 9);
            Console.Write("Enter Player One Name (default 'PLayer 1'): ");
            string playerOneName = Console.ReadLine().Trim();

            if (playerOneName.Length % 2 == 1)
            {
                playerOneName += " ";
            }

            if (playerOneName.Length == 0)
            {
                firstPlayerName = "Player 1";
            }
            else
            {
                firstPlayerName = playerOneName;
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 - gameName.Length, gameName.Length + 11);
            Console.Write("Enter Player Two Name (default 'PLayer 2'): ");
            string playerTwoName = Console.ReadLine().Trim();

            if (playerTwoName.Length % 2 == 1)
            {
                playerTwoName += " ";
            }

            if (playerTwoName.Length == 0)
            {
                secondPlayerName = "Player 2";
            }
            else
            {
                secondPlayerName = playerTwoName;
            }

        }

        static void BuildTerrainFromFile(char[,] terrain, string file)
        {
            //TODO
        }

        static void BuildRandomTerrain()
        {
            terrain = new char[terrainHeight, terrainWidth];
            int minHeight = 35;
            int maxStep = 2;
            int maxHeight = 60;
            int currentHeight = 45;
            int nextHeight;

            for (int col = 0; col < terrain.GetLength(1); col++)
            {
                do
                {
                    nextHeight = rnd.Next(currentHeight - maxStep, currentHeight + maxStep + 1);
                } while (!(minHeight <= nextHeight && nextHeight <= maxHeight));
                currentHeight = nextHeight;
                for (int row = currentHeight; row < terrain.GetLength(0); row++)
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

        static void CheckForWinner()
        {
            string winner = string.Empty;
            if (firstPlayerLivePoints <= 0)
            {
                winner = String.Format("{0} wins!!!", secondPlayerName);
                ++secondPlayerScore;
            }
            else if (secondPlayerLivePoints <= 0)
            {
                winner = String.Format("{0} wins!!!", firstPlayerName);
                ++firstPlayerScore;
            }

            PrintOnPosition(72, 30, winner, ConsoleColor.Cyan);
            Thread.Sleep(3000);
        }

        static void Shoot()
        {
            if (activePlayer == true)
            {
                BallMovement((firstTowerVelocity + wind) / 2.0, firstTowerAngle, activePlayer);
            }
            else
            {
                BallMovement((secondTowerVelocity-wind) / 2.0, secondTowerAngle, activePlayer);
            }
            activePlayer = !activePlayer;
        }

        static void HitTerrain(int hitX, int hitY)
        {
            //hit only terrain
            if (hitX < terrain.GetLength(0) - 1 && hitY < terrain.GetLength(1) - 1 && hitY > 0)
            {
                StringBuilder hitLine = new StringBuilder("###");
                //print explosion
                PrintOnPosition(hitY - 1, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 8, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX + 1; i++)
                {
                    for (int j = hitY - 1; j <= hitY + 1; j++)
                    {
                        if (terrain[i, j] != '1' && terrain[i, j] != '2')
                        {
                            terrain[i, j] = ' ';
                        }
                        else //take damage
                        {
                            if (terrain[i, j] == '1')
                            {
                                firstPlayerLivePoints -= 20;
                            }
                            else if (terrain[i, j] == '2')
                            {
                                secondPlayerLivePoints -= 20;
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
            //hit terrain left
            else if (hitX < terrain.GetLength(0) - 1 && hitY == 0)
            {
                StringBuilder hitLine = new StringBuilder("##");
                //print explosion
                PrintOnPosition(hitY, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY, hitX + 8, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX + 1; i++)
                {
                    for (int j = hitY; j <= hitY + 1; j++)
                    {
                        terrain[i, j] = ' ';
                    }
                }
                Thread.Sleep(1000);
            }
            //hit terrain right
            else if (hitX < terrain.GetLength(0) - 1 && hitY == terrain.GetLength(1) - 1)
            {
                StringBuilder hitLine = new StringBuilder("##");
                //print explosion
                PrintOnPosition(hitY - 1, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 8, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX + 1; i++)
                {
                    for (int j = hitY - 1; j <= hitY; j++)
                    {
                        terrain[i, j] = ' ';
                    }
                }
                Thread.Sleep(1000);
            }
            //hit terrain left and down
            else if (hitX == terrain.GetLength(0) - 1 && hitY == 0)
            {
                StringBuilder hitLine = new StringBuilder("##");
                //print explosion
                PrintOnPosition(hitY, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX; i++)
                {
                    for (int j = hitY; j <= hitY + 1; j++)
                    {
                        terrain[i, j] = ' ';
                    }
                }
                Thread.Sleep(1000);
            }
            //hit terrain right and down
            else if (hitX == terrain.GetLength(0) - 1 && hitY == terrain.GetLength(1) - 1)
            {
                StringBuilder hitLine = new StringBuilder("##");
                //print explosion
                PrintOnPosition(hitY - 1, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX; i++)
                {
                    for (int j = hitY - 1; j <= hitY; j++)
                    {
                        terrain[i, j] = ' ';
                    }
                }
                Thread.Sleep(1000);
            }
            //hit terrain down
            else if (hitX == terrain.GetLength(0) - 1 && hitY < terrain.GetLength(1) - 1 && hitY > 0)
            {
                StringBuilder hitLine = new StringBuilder("###");
                //print explosion
                PrintOnPosition(hitY - 1, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
                PrintOnPosition(hitY - 1, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
                //change terrain
                for (int i = hitX - 1; i <= hitX; i++)
                {
                    for (int j = hitY - 1; j <= hitY + 1; j++)
                    {
                        terrain[i, j] = ' ';
                    }
                }
                Thread.Sleep(1000);
            }
        }

        static void HitTower(int hitX, int hitY)
        {
            StringBuilder hitLine = new StringBuilder("###");
            //print explosion
            PrintOnPosition(hitY - 1, hitX + 6, hitLine.ToString(), ConsoleColor.Yellow);
            PrintOnPosition(hitY - 1, hitX + 7, hitLine.ToString(), ConsoleColor.Yellow);
            PrintOnPosition(hitY - 1, hitX + 8, hitLine.ToString(), ConsoleColor.Yellow);
            //take damage
            for (int i = hitX - 1; i <= hitX + 1; i++)
            {
                for (int j = hitY - 1; j <= hitY + 1; j++)
                {
                    if (terrain[i, j] == '1')
                    {
                        firstPlayerLivePoints -= 20;
                    }
                    else if (terrain[i, j] == '2')
                    {
                        secondPlayerLivePoints -= 20;
                    }
                }
            }
            Thread.Sleep(1000);
        }

        static void Impact(int hitX, int hitY)
        {
            if (terrain[hitX, hitY] == '#')
            {
                HitTerrain(hitX, hitY);
            }
            else if (terrain[hitX, hitY] == '1' || terrain[hitX, hitY] == '2')
            {
                HitTower(hitX, hitY);
            }
        }

        static void BallMovement(double velocity, int angle, bool activePlayer)
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
                        continue;
                    }
                    if (terrain[y, x] == '#' || terrain[y, x] == '2' || terrain[y,x] == '1')
                    {
                        Impact(y, x);
                        return;
                    }

                    Thread.Sleep(20);
                    PrintOnPosition(x, y + 7, "*", ConsoleColor.White);
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
                        continue;
                    }
                    if (terrain[y, x] == '#' || terrain[y, x] == '1' || terrain[y, x] == '2')
                    {
                        Impact(y, x);
                        return;
                    }

                    Thread.Sleep(20);
                    PrintOnPosition(x, y + 7, "*", ConsoleColor.White);
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
                    if (terrain[row, col] == '#' && (row != terrainHeight - 1 || col != terrainHeight - 1))
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

            Console.SetCursorPosition(0, 7);
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
                        PrintOnPosition(col, row + 7, terrain[row, col].ToString(), ConsoleColor.Red);
                    }
                    else if (terrain[row, col] == '2')
                    {
                        PrintOnPosition(col, row + 7, terrain[row, col].ToString(), ConsoleColor.Blue);
                    }
                }
            }
        }

        static void PrintOnPosition(int x, int y, string c, ConsoleColor color = ConsoleColor.White)
        {
            //Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void ModifyAngle(ConsoleKeyInfo key)
        {
            const int sencitivity = 5;
            const int maxAngle = 90;
            const int minAngle = 0;

            if (key.Key == firstAngleUpKey && activePlayer == true && firstTowerAngle < maxAngle)
            {
                firstTowerAngle += sencitivity;
            }
            else if (key.Key == firstAngleDownKey && activePlayer == true && firstTowerAngle > minAngle)
            {
                firstTowerAngle -= sencitivity;
            }
            else if (key.Key == secondAngleUpKey && activePlayer == false && secondTowerAngle < maxAngle)
            {
                secondTowerAngle += sencitivity;
            }
            else if (key.Key == secondAngleDownKey && activePlayer == false && secondTowerAngle > minAngle)
            {
                secondTowerAngle -= sencitivity;
            }
        }

        static void ModifyVelocity(ConsoleKeyInfo key)
        {
            const int sencitivity = 1;
            const int maxVelocity = 40;
            const int minVelocity = 0;

            if (key.Key == firstVelocityUpKey && activePlayer == true && firstTowerVelocity < maxVelocity)
            {
                firstTowerVelocity += sencitivity;
            }
            else if (key.Key == firstVelocityDownKey && activePlayer == true && firstTowerVelocity > minVelocity)
            {
                firstTowerVelocity -= sencitivity;
            }
            else if (key.Key == secondVelocityDownKey && activePlayer == false && secondTowerVelocity < maxVelocity)
            {
                secondTowerVelocity += sencitivity;
            }
            else if (key.Key == secondVelocityUpKey && activePlayer == false && secondTowerVelocity > minVelocity)
            {
                secondTowerVelocity -= sencitivity;
            }
        }

        static void PrintPanel()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('=', terrainWidth);
            builder.Append(firstPlayerName);
            builder.Append(new string(' ', terrainWidth - firstPlayerName.Length - secondPlayerName.Length));
            builder.Append(secondPlayerName);
            //
            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            //Write current scores
            string currentResult = string.Format("{0}  {1}:{2}  {3}", firstPlayerName, firstPlayerScore, secondPlayerScore, secondPlayerName);
            builder.Append(' ', (terrainWidth - currentResult.Length) / 2);
            builder.Append(currentResult);
            builder.Append(' ', (terrainWidth - currentResult.Length) / 2 + 1);
            //builder.Append("\n");

            //Print players live points
            string firstPlayerLiveString = string.Format("Health: {0}", firstPlayerLivePoints);
            string secondPlayerLiveString = string.Format("Health: {0}", secondPlayerLivePoints);
            builder.Append(firstPlayerLiveString);
            builder.Append(' ', terrainWidth - firstPlayerLiveString.Length - secondPlayerLiveString.Length);
            builder.Append(secondPlayerLiveString);

            //Prin players shooting angles
            string firstTowerAngleString = string.Format("Angle: {0}", firstTowerAngle);
            string secondTowerAngleString = string.Format("Angle: {0}", secondTowerAngle);
            builder.Append(firstTowerAngleString);
            builder.Append(' ', terrainWidth - firstTowerAngleString.Length - secondTowerAngleString.Length);
            builder.Append(secondTowerAngleString);

            //Print players shooting velocities
            string firstTowerVelocityString = string.Format("Power: {0}", firstTowerVelocity);
            string secondTowerVelocityString = string.Format("Power: {0}", secondTowerVelocity);
            builder.Append(firstTowerVelocityString);
            builder.Append(' ', terrainWidth - firstTowerVelocityString.Length - secondTowerVelocityString.Length);
            builder.Append(secondTowerVelocityString);
            //Draw line
            builder.Append('=', terrainWidth);

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(builder.ToString());

            //highlight active player name
            if (activePlayer)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                PrintOnPosition((terrainWidth - currentResult.Length) / 2, 2, firstPlayerName, ConsoleColor.White);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                PrintOnPosition((terrainWidth - currentResult.Length) / 2 + 5 
                    + firstPlayerScore.ToString().Length 
                    + secondPlayerScore.ToString().Length 
                    + firstPlayerName.Length, 2, secondPlayerName, ConsoleColor.White);
            }
        }

        static void KeyPress(ConsoleKeyInfo keyPressed)
        {

            if (keyPressed.Key == secondAngleUpKey || keyPressed.Key == secondAngleDownKey ||
                keyPressed.Key == firstAngleUpKey || keyPressed.Key == firstAngleDownKey)
            {
                ModifyAngle(keyPressed);
            }

            if (keyPressed.Key == secondVelocityDownKey || keyPressed.Key == secondVelocityUpKey ||
                keyPressed.Key == firstVelocityUpKey || keyPressed.Key == firstVelocityDownKey)
            {
                ModifyVelocity(keyPressed);
            }

            if ((keyPressed.Key == firstShootKey && activePlayer == true) || (keyPressed.Key == secondShootKey && activePlayer == false))
            {
                throw new Exception();
            }
        }

        static void SetWind()
        {
            wind = rnd.Next(-10, 11);
            string windInfo = string.Empty;
            if (wind < 0)
            {
                windInfo = String.Format("Wind: <== {0}", -wind);
            }
            else if (wind == 0)
            {
                windInfo = String.Format("Wind: {0}", wind);
            }
            else
            {
                windInfo = String.Format("Wind: ==> {0}", wind);
            }

            PrintOnPosition(135, 8, windInfo, ConsoleColor.White);
        }
    }
}
