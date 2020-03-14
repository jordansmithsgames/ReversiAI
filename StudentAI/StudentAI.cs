using GameAI.GamePlaying.Core;

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        // Constructor; creates a new Minimax AI agent behavior
        public StudentAI() {

        }

        // Implementation of Minimax. Determines the best move for the player specified by color on the given 
        // board, looking ahead the number of steps indicated by lookAheadDepth. This method will not be called
        // unless there is at least one possible move for the player specified by color.
        public ComputerMove Run(int color, Board board, int lookAheadDepth) {

        }

        // Returns a strategic value estimate for board based on position and color of pieces. It is recommended that 
        // students implement a method with this functionality to evaluate board states at the lookahead depth cutoff.
        private int Evaluate(Board board) {
            
        }
    }
}
