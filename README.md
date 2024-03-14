
> The purpose of this challenge is to implement a system that can act as a scoreboard for a bowling
game.
> UI is not a necessity

# Implementation notes

### Console Output

Easiest method to be able to exercise the rules in an exam like environment. 

I considered implementing a miriad of unit tests to show the logic works, 
but that left the possibility of there being unwritten tests I had not considered that the examiners would like to have covered.

A small looping console app was simple to write and allows for all types of manual tests to be run. 

Though I did take the opportunity to implement a simple dependency injection pattern for the Console output. See below.

### Dependency Injection of IOutputPlayer into PlayerGame.

```C#
public PlayerGame(IOutputPlayer outputPlayer)
```

PlayerScoresToConsole implements IOutputPlayer, and is injected into PlayerGame.

This setup allows for easy introduction in the future of a different output method (such as a proper UI) with minimal changes to PlayerGame itself.

### FrameBase as an abstract class to base Frame and FinalFrame on

```C#
public abstract partial class FrameBase(Throw firstThrow) : IScoreCalculatable
```

The final frame of Bowling just has such different rules than all the rest:
- Completion rules - since you *can* have three throws if you get a Strike or a Spare
- Scoring - no future throws are considered, since they all exist within this frame

I wanted an implementation that would allow me to encapsulate the differences in one area. 
Thus allowing the rest of the program to treat any frame as the same, due to **Polymorphism**. Frames are all called the same way with `AddThrow`, `IsComplete`, and `Score`, and the Frame knows how to handle them.


### Calculation of Scoring Backwards

Playing through a game of bowling one adds throws in sequence - the new after the latest.

But when calculating the score of each Frame, the score of future frames is necessary. So working forwards would be a bit of a pain.

**The/A Solution:** Calculate the scoring in reverse order, starting with the latest turn. 

```C#
var endScores = new Stack<int>();
Stack<int> throws = new Stack<int>();
```

When a new score is calculated for a Frame (starting with the latest Frame) then that score is added to the Stack `endScores`. It's IEnumerable so it can be used in the IOutputPlayer as a Stack with no issue.

Then `throws` is used to store each Throw from a Frame as the scoring loop progresses. Then the two latest throws are given to each Frame so it may calculate it's own fully resolved score, if possible.
- Side note: We're not in Python, so that variable name is A-OK!


### Scoring as a Permenent Record vs a Recalculated Score

> Why not store the scores permanently directly on the Frames?

Each Frame having a potential dependency on future Frames leads to messy code and potential bugs.

Recalculating it for each display is very simple and this example is not computationally intensive for that to matter.

There are also situations in bowling where mistakes are corrected in previous Frames, manually on the little machine. 
So a method to do a full recalculation is necessary in a full setup.





# Initial Thoughts

Just my initial thoughts/notes when I began the implementation. Left here to facilitate discussion.

## Tasks

- Populate README with design choices and my reasonings for them

- Calculate scoring 

## Scoring

1. Frame vs throws
    - Up to 2 throws
        - Apart from the final Frame? Or should the extra ones count as bonus frames?
    - Need a final score for the frame
2. Strikes and Spares
    - Final score is calculated with the points of the one/two next throws.
3. Scoring objects
    - Calculate the scoring backwards?
        - Keep a record of the previous two throws
        - When a strike/spare is found, use those in the calculation immediately. No need to fetch anything.
    - Types
        - ScoreResolved: scores that have no ongoing strikes/spares waiting for their final results 
        - ScoreUnresolved: scores that are not yet complete, due to outstanding strikes/spares
        - ScoreOngoing: resolved + unresolved. A temp 'at least' score.
    

# Bonus!

My best score at Bowling 😎 

154, baby! My Granddad almost caught up to me with a Turkey in the end and landed 1 point behind! 😱

![](./Wedding%20Feb%2025%202019.jpg)
