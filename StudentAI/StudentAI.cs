using GameAI.GamePlaying.Core;
using GameAI.GamePlaying.ExampleAI; // Remove after completing exercise

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        private int max;
        private int min;
        private int lookAheadDepth;

        // Constructor; creates a new Minimax AI agent behavior
        public StudentAI() {
            max = 0;
            min = 0;
            lookAheadDepth = 0;
        }

        // Implementation of Minimax. Determines the best move for the player specified by color on the given 
        // board, looking ahead the number of steps indicated by lookAheadDepth. This method will not be called
        // unless there is at least one possible move for the player specified by color.
        public ComputerMove Run(int color, Board board, int lookAheadDepth) {
            max = color;
            min = -color;
            this.lookAheadDepth = lookAheadDepth;
            return Minimax(color, board, -10000, 10000, 0);
        }

        private ComputerMove Minimax(int player, Board board, int alpha, int beta, int currDepth) {
            ComputerMove bestMove = null;
            int depth = currDepth;
            Board boardState = new Board();
            boardState.Copy(board);
            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    if (board.IsValidMove(player, row, col)) {
                        ComputerMove move = new ComputerMove(row, col);
                        boardState.MakeMove(player, row, col);
                        if (boardState.IsTerminalState() || depth == lookAheadDepth) {
                            move.rank = ExampleAI.MinimaxExample.EvaluateTest(boardState);
                            //move.rank = Evaluate(boardState);
                        }
                        else {
                            move.rank = Minimax(GetNextPlayer(player, boardState), boardState, alpha, beta, depth + 1).rank;
                        }

                        if (bestMove == null || move.rank > bestMove.rank) {
                            bestMove = move;
                            if (player == max && bestMove.rank > alpha) {
                                alpha = bestMove.rank;
                            }
                            else if (player == min && bestMove.rank < beta) {
                                beta = bestMove.rank;
                            }
                            if (alpha >= beta) {
                                return bestMove;
                            }
                        }
                    }
                }
            }
            return bestMove;
        }

        // Returns a strategic value estimate for board based on position and color of pieces. It is recommended that 
        // students implement a method with this functionality to evaluate board states at the lookahead depth cutoff.
        private int Evaluate(Board board) {
            return 0;
        }

        private int GetNextPlayer(int player, Board board) {
            Board boardState = new Board();
            boardState.Copy(board);
            if (boardState.HasAnyValidMove(-player)) return -player;
            else return player;
        }
    }
}
