namespace BowlingScoring;

public class Throw
{
    public Throw (int simpleScore) {
        if (!validateScore(simpleScore)) {
            throw new ArgumentOutOfRangeException(nameof(simpleScore), "Score for a throw must be between 0 and 10");
        }

        SimpleScore = simpleScore;
    }

    private bool validateScore(int score) {
        return score >= 0 && score <= 10;
    }

    public int SimpleScore { get; }

    /// Possible alternate way to track Throws: exactly which pins were knocked down
    /// Useful for metrics and for the little bowling animations they show on the screen!
    // public int[] complexScore { get; set; }

    public bool IsStrike => SimpleScore == 10;

    public override string ToString()
    {
        if (IsStrike) {
            return "X";
        } else {
            return SimpleScore.ToString();
        }
    }
}
