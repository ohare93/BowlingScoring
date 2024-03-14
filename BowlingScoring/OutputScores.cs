using System.Text;

namespace BowlingScoring;



public interface IOutputPlayer
{
    void OutputScores(PlayerGame playerGame);
}

public class PlayerScoresToConsole : IOutputPlayer
{
    public void OutputScores(PlayerGame playerGame)
    {
        string frameOutput = "Frames:               " + String.Join("|", playerGame.Frames.Select(f => f.ToString()));

        if (playerGame.Frames.LastOrDefault()?.IsComplete() == true)
        {
            frameOutput += "|";
        }

        Stack<int> endScores = playerGame.CalculateScores();
        string scoreEachFrame = "Score per Frame:     " + String.Join("|", endScores.Select(s => NumberToPaddedString(s)));
        string runningTotal = "Score Running Total: " + String.Join("|", AccumulateScores(endScores).Select(s => NumberToPaddedString(s)));

        StringBuilder stats = new();
        stats.Append("Stats:     ");
        stats.Append($"Frame {playerGame.CurrentFrameNumber()};");
        //stats.Append($"Total: {playerGame.TotalScore};");

        Console.WriteLine(stats);
        Console.WriteLine(frameOutput);
        Console.WriteLine(scoreEachFrame);
        Console.WriteLine(runningTotal);
    }

    string NumberToPaddedString(int number)
    {
        return $"{number,3}";
    }

    IEnumerable<int> AccumulateScores(IEnumerable<int> scores)
    {
        int runningTotal = 0;
        foreach (int score in scores)
        {
            runningTotal += score;
            yield return runningTotal;
        }
    }
}
