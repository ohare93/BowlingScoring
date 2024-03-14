using System.Text;

namespace BowlingScoring;

public interface IScoreCalculatable
{
    public int Score(int throwAfter, int throwAfterThat);
    public bool IsScoreResolved(int numOfFutureThrows);

}

/// <summary>
/// Base class for all frames.
/// 
/// Knows how to calculate its own score, whether that score is fully resolved, and whether the turn is complete.
/// </summary>
/// <param name="firstThrow"></param>
public abstract partial class FrameBase(Throw firstThrow) : IScoreCalculatable
{

    public Throw FirstThrow { get; } = firstThrow;
    public Throw? SecondThrow { get; internal set; }

    public bool IsStrike => FirstThrow.IsStrike;
    public bool IsSpare => SecondThrow != null && (FirstThrow.SimpleScore + SecondThrow.SimpleScore) == 10;

    public abstract IList<Throw> GetThrows();
    public abstract bool IsComplete();
    public abstract void AddThrow(Throw throwToAdd);
    public abstract int Score(int throwAfter, int throwAfterThat);
    public abstract bool IsScoreResolved(int numOfFutureThrows);
}

/// <summary>
/// A normal Frame in Bowling. May contain 1 or 2 throws.
/// </summary>
public class Frame : FrameBase
{
    public Frame(Throw firstThrow) : base(firstThrow)
    {
    }

    public override bool IsComplete()
    {
        if (SecondThrow != null)
            return true;

        if (FirstThrow.IsStrike)
            return true;

        return false;
    }

    public override void AddThrow(Throw throwToAdd)
    {
        if (IsComplete())
            throw new InvalidOperationException("Cannot add a throw to a completed frame");

        if (FirstThrow.SimpleScore + throwToAdd.SimpleScore > 10)
            throw new InvalidOperationException("Frame adds up to more than 10 pins");

        SecondThrow = throwToAdd;
    }

    public override bool IsScoreResolved(int numOfFutureThrows)
    {
        if (IsComplete())
        {
            if (!IsStrike && !IsSpare)
                return true;

            if (IsSpare)
                return numOfFutureThrows >= 1;

            if (IsStrike)
                return numOfFutureThrows >= 2;
        }

        return false;
    }

    public override int Score(int throwAfter, int throwAfterThat)
    {
        if (IsStrike)
            return 10 + throwAfter + throwAfterThat;

        if (IsSpare)
            return 10 + throwAfter;

        return FirstThrow.SimpleScore + (SecondThrow?.SimpleScore ?? 0);
    }

    public override string ToString()
    {
        if (IsStrike)
        {
            return " X ";
        }

        StringBuilder sb = new();
        sb.Append(FirstThrow);

        if (SecondThrow != null)
        {
            sb.Append(",");

            if (IsSpare)
                sb.Append("/");
            else
                sb.Append(SecondThrow);
        }
        else
            sb.Append("  ");

        return sb.ToString();
    }

    public override IList<Throw> GetThrows()
    {
        var throws = new List<Throw>() { FirstThrow };
        if (SecondThrow != null)
            throws.Add(SecondThrow);
        return throws;
    }
}

/// <summary>
/// The final Frame in Bowling. May contain up to 3 throws. Scoring rules differ
/// </summary>
public class FinalFrame : FrameBase
{
    public Throw? ThirdThrow { get; set; }

    public FinalFrame(Throw firstThrow) : base(firstThrow)
    {
    }

    public override bool IsComplete()
    {
        if (ThirdThrow != null)
            return true;

        if (!FirstThrow.IsStrike && SecondThrow != null)
            return true;

        return false;
    }

    public override void AddThrow(Throw throwToAdd)
    {
        if (IsComplete())
            throw new InvalidOperationException("Cannot add a throw to a completed frame");

        if (SecondThrow == null)
            SecondThrow = throwToAdd;
        else
            ThirdThrow = throwToAdd;
    }

    public override bool IsScoreResolved(int numOfFutureThrows)
    {
        return IsComplete();
    }

    public override int Score(int throwAfter, int throwAfterThat)
    {
        return FirstThrow.SimpleScore
            + (SecondThrow?.SimpleScore ?? 0)
            + (ThirdThrow?.SimpleScore ?? 0);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(FirstThrow);

        if (SecondThrow != null)
            sb.Append(",").Append(SecondThrow);

        if (ThirdThrow != null)
            sb.Append(",").Append(ThirdThrow);

        return sb.ToString();
    }

    public override IList<Throw> GetThrows()
    {
        var throws = new List<Throw>() { FirstThrow };
        if (SecondThrow != null)
            throws.Add(SecondThrow);
        if (ThirdThrow != null)
            throws.Add(ThirdThrow);
        return throws;
    }
}