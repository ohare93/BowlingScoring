
using BowlingScoring;

namespace BowlingGame
{
    public class BowlingScoring
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Let's bowl!");

            var player1 = new PlayerGame(new PlayerScoresToConsole());

            do
            {
                string input = Console.ReadLine();
                int throwScore;
                if (!Int32.TryParse(input, out throwScore))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                try
                {
                    player1.AddThrow(throwScore);

                    player1.OutputScores();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    if (e.Message.StartsWith("Frame limit reached"))
                    {
                        Console.WriteLine("Start new game? Enter any key");
                        Console.ReadLine();
                        Console.WriteLine("Let's bowl!");
                        player1.Frames.Clear();
                    }
                }

            } while (true);
        }
    }
}