using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
        game.Start();

        }
    }



public class Game
    {
        /// <summary>
        /// Selection item
        /// </summary>
        enum RPS : int
        {
            Rock = 0,
            Paper = 1,
            Scissors = 2,

            Invalid = 100
        }

        /// <summary>
        /// dictionary of winners
        /// </summary>
        Dictionary<RPS, RPS> winners = new Dictionary<RPS, RPS>
        {
            { RPS.Rock, RPS.Scissors },
            { RPS.Scissors, RPS.Paper },
            { RPS.Paper, RPS.Rock }
        };

        int computerWins = 0;
        int playerWins = 0;
        int count = 0;
        string playerName = null;
        int scoreX = 0,
            scoreY = 0;

        static Random r = new Random();

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            playerName = null;
            while (string.IsNullOrEmpty(playerName))
            {
                Console.Clear();
                Console.Write("Welcome to Rock, Paper, Scissors.\r\n\r\nPlease enter your name... ");
                playerName = Console.ReadLine();
            }

            Console.Write($"Welcome {playerName}...\r\n\r\n How many games do you want play? ");
            string gameStr = Console.ReadLine();
            int numOfGames = 0;
            int.TryParse(gameStr, out numOfGames);
            
            while (numOfGames <= 0 || numOfGames <=3)
            {
                Console.Write($"{gameStr} is not a valid input.\r\nPlease enter the amount of games you wish to play. The number you enter must be an odd number... ");
                gameStr = Console.ReadLine();
                int.TryParse(gameStr, out numOfGames);
            }

            SetupGame();
            DrawGameBoard();

            Console.WriteLine("The rules are very simple, you will be asked to make your selection. The computer will choose first, this is so it cant cheat. You can then enter Rock, Paper or Scissors. The winner will be determined against the standard Rock, Paper, Scissors rule book.\r\n\r\nWhen you are ready press [Enter] to start the game.");
            Console.ReadLine();


            for (int i = 0; i < numOfGames; i++)
                if (!PlayRound(i))
                    break;

            DrawEnd();
        }


        /// <summary>
        /// Runs a player round
        /// </summary>
        /// <param name="roundNumber"></param>
        /// <returns></returns>
        bool PlayRound(int roundNumber)
        {
            DrawGameBoard();

            RPS computerChoice = GetComputerRps();
            RPS playerChoice = RPS.Invalid;
            Console.WriteLine($"Round {roundNumber + 1}:");
            bool quit = false;
            while (playerChoice == RPS.Invalid)
            {

                Console.Write("Please make your selection. Rock, Paper, Scissors... ");
                string choice = Console.ReadLine();
                switch (choice.ToLowerInvariant().Trim())
                {
                    case "rock":
                    case "r":
                        playerChoice = RPS.Rock;
                        break;
                    case "paper":
                    case "p":
                        playerChoice = RPS.Paper;
                        break;
                    case "scissors":
                    case "s":
                        playerChoice = RPS.Scissors;
                        break;
                    case "quit":
                    case "q":
                        if(playerWins>computerWins)
                        quit = true;
                        break;

                    default:
                        Console.WriteLine($"{choice} is not a valid selection. Please try again.\r\n");
                        break;
                }
                if (quit)
                    break;
            }

            if (quit)
                return false;

            Console.WriteLine($"The computer chose: {computerChoice}");
            Console.WriteLine($"You chose: {playerChoice}");
            CalculateWinner(playerChoice, computerChoice);

            Console.Write("Press [Enter] to start the next game.");
            Console.ReadLine();
            return true;
        }

        /// <summary>
        /// Calculates the winner
        /// </summary>
        /// <param name="player"></param>
        /// <param name="computer"></param>
        void CalculateWinner(RPS player, RPS computer)
        {
            //tie game
            if (player == computer)
            {
                Console.WriteLine($"The game is a tie.");
                playerWins++;
                computerWins++;
                
            }
            else
            {
                //calculating the winner is simple, simply get the
                //winning combination for the player
                //if the result equals the computers roll then the player wins
                //otherwise the computer wins.
                // such as player calls rock, winners[rock] == scissors. If computer == scissors then 
                // player wins otherwise the computer wins as the only other option is paper 
                // remeber the options of the computer has a rock is negated in the tie selection
                //

                
                var p = winners[player];
                if (p == computer)
                {
                    Console.WriteLine($"Congratulations you won. {player} beats {computer}.");
                    playerWins++;
                }
                else
                {
                    Console.WriteLine($"Computer Wins. {computer} beats {player}.");
                    computerWins++;
                }

                
            }
        }


        /// <summary>
        /// Gets the computer RPS selection
        /// </summary>
        /// <returns></returns>
        RPS GetComputerRps()
        {
            int rng = r.Next(0, 3);

            return (RPS)rng;
        }

        /// <summary>
        /// Sets up a game board
        /// </summary>
        void SetupGame()
        {
            /*  Template
             *  Score: {playerName}: {playerWins, 4 spaces}      Computer: {computerWins, 4 spaces} 
             */

            int requiredWidth = "Score: ".Length +
                               playerName.Length +
                               1 +
                               1 +
                               4 +
                               4 +
                               "Computer".Length +
                               1 +
                               1 +
                               4 +
                               1;

            int right = Console.WindowWidth - requiredWidth;
            int top = 1;
            if (right < "ROCK, PAPER, SCISSORS...".Length)
                top++;

            scoreX = right;
            scoreY = top;
        }

        /// <summary>
        /// Draws the game board header
        /// </summary>
        void DrawGameBoard()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("-");
            Console.Write("ROCK, PAPER, SCISSORS...");
            Console.SetCursorPosition(scoreX, scoreY);
            Console.Write($"Score: {playerName}: {FormatScore(playerWins)}    Computer: {FormatScore(computerWins)}");
            Console.SetCursorPosition(0, scoreY + 1);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("-");

            Console.SetCursorPosition(0, scoreY + 4);
        }


        /// <summary>
        /// Draws the end of the game
        /// </summary>
        void DrawEnd()
        {
            DrawGameBoard();
            Console.WriteLine("Thank you for playing Rock, Paper, Scissors.");
            Console.WriteLine("\r\n\r\nThe final score was:");
            Console.WriteLine($"\r\n{playerName}: {playerWins}");
            Console.WriteLine($"\r\nComputer: {computerWins}");

            //if (playerWins > computerWins)
                if (playerWins>1)
                Console.WriteLine("\r\nCongratulations, you have won this round.");
            else
                Console.WriteLine("\r\nMaybe you will have better luck next time.");

            Console.Write("\r\nPress [Enter] to finish the game.");
            Console.ReadLine();
        }

        /// <summary>
        /// Simple format method to convert the score into a fixed length 
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        string FormatScore(int score)
        {
            string format = "     " + score.ToString();
            return format.Substring(format.Length - 4);
        }

    }

}
