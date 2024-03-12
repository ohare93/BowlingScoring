
> The purpose of this challenge is to implement a system that can act as a scoreboard for a bowling
game.
> UI is not a necessity,

# Outstanding tasks

- Populate README with design choices and my resonings for them

- Calculate scoring 


## Scoring thoughts

1. Frame vs throws
    - Up to 2 throws
        - Apart from the final Frame? Or should the extra ones count as bonus frames?
    - Need a final score for the frame
2. Strikes and Spares
    - Final score is calculated via the 
3. Scoring objects
    - Calculate the scoring backwards?
        - Keep a record of the previous two throws
        - When a strike/spare is found, use those in the calculation immediately. No need to fetch anything.
    - Types
        - ScoreResolved: scores that have no ongoing strikes/spares waiting for their final results 
        - ScoreUnresolved: scores that are not yet complete, due to outstanding strikes/spares
        - ScoreOngoing: resolved + unresolved. A temp 'at least' score.
    