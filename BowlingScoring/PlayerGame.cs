namespace BowlingScoring;

public class PlayerGame
{
    public IList<FrameBase> Frames { get; internal set; }
    private readonly IOutputPlayer outputPlayer;

    public PlayerGame(IOutputPlayer outputPlayer)
    {
        this.outputPlayer = outputPlayer;

        Frames = new List<FrameBase>(10);
    }

    public void OutputScores()
    {
        outputPlayer.OutputScores(this);
    }

    public Stack<int> CalculateScores()
    {
        var endScores = new Stack<int>();
        Stack<int> throwsInReverseOrder = new Stack<int>();

        foreach(var frame in Frames.Reverse())
        {
            int throwAfter = throwsInReverseOrder.ElementAtOrDefault(0);
            int throwAfterThat = throwsInReverseOrder.ElementAtOrDefault(1);

            int frameScore = frame.Score(throwAfter, throwAfterThat);
            endScores.Push(frameScore);

            frame.GetThrows().Reverse().ToList().ForEach(t => throwsInReverseOrder.Push(t.SimpleScore));
        }

        return endScores;
    }

    public int CurrentFrameNumber()
    {
        var latestFrame = Frames.LastOrDefault();

        if (latestFrame == null)
            return 1;

        return Frames.Count + (latestFrame.IsComplete() ? 1 : 0);
    }

    public void AddThrow(int throwScore)
    {
        Throw newThrow = new Throw(throwScore);
        AddThrow(newThrow);
    }

    public void AddThrow(Throw newThrow)
    {
        var latestFrame = Frames.LastOrDefault();

        if (latestFrame == null || latestFrame.IsComplete())
        {
            AddFrame(newThrow);
            return;
        }

        latestFrame.AddThrow(newThrow);
    }

    private void AddFrame(Throw newThrow)
    {
        if (Frames.Count < 9)
        {
            Frames.Add(new Frame(newThrow));
        }
        else if (Frames.Count == 9)
        {
            Frames.Add(new FinalFrame(newThrow));
        }
        else
        {
            throw new Exception($"Frame limit reached. Already have {Frames.Count} frames.");
        }
    }
}

