using GameAI.GamePlaying.Core;
using GameAI.GamePlaying.ExampleAI; // Remove after completing exercise

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        private int max;
        private int min;
        private int maxDepth;

        // Constructor; creates a new Minimax AI agent behavior
        public StudentAI() {
            max = 0;
            min = 0;
            maxDepth = 0;
        }

        // Implementation of Minimax. Determines the best move for the player specified by color on the given 
        // board, looking ahead the number of steps indicated by lookAheadDepth. This method will not be called
        // unless there is at least one possible move for the player specified by color.
        public ComputerMove Run(int color, Board board, int lookAheadDepth) {
            max = color;
            min = -color;
            maxDepth = lookAheadDepth;
            return Minimax(color, board, /*-10000, 10000,*/ 0);
        }

        private ComputerMove Minimax(int player, Board board, /*int alpha, int beta,*/ int depth) {
            ComputerMove bestMove = null;
            Board boardState = new Board();

            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    if (board.IsValidMove(player, row, col)) {
                        ComputerMove move = new ComputerMove(row, col);
                        boardState.Copy(board);
                        boardState.MakeMove(player, row, col);

                        if (boardState.IsTerminalState() || depth == maxDepth) 
                            move.rank = ExampleAI.MinimaxExample.EvaluateTest(boardState);
                            //move.rank = Evaluate(boardState);
                        else
                            move.rank = Minimax(GetNextPlayer(player, boardState), boardState, /*alpha, beta,*/ depth + 1).rank;

                        if (bestMove == null || betterMove(player, move.rank, bestMove.rank)) {
                            bestMove = move;
                            /*if (player == max && bestMove.rank > alpha) alpha = bestMove.rank;
                            else if (player == min && bestMove.rank < beta) beta = bestMove.rank;
                            if (alpha >= beta) return bestMove;*/
                        }
                        System.Console.WriteLine("Move for (" + row + ", " + col + ")'s rank: " + move.rank);
                    }
                }
            }
            System.Console.WriteLine("Best move's rank: " + bestMove.rank);
            return bestMove;
        }

        // Returns a strategic value estimate for board based on position and color of pieces. It is recommended that 
        // students implement a method with this functionality to evaluate board states at the lookahead depth cutoff.
        private int Evaluate(Board board) {
            int value = 0;
            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    int color = board.GetTile(row, col);
                    if (color != 0) {
                        int tileValue = color;
                        // Check if tile is a corner
                        bool tlCorner = row == 0 && col == 0;
                        bool trCorner = row == 0 && col == 7;
                        bool blCorner = row == 7 && col == 0;
                        bool brCorner = row == 7 && col == 7;
                        // Check if tile is not a corner but on edge
                        bool lColumn = 0 < row && row < 7 && col == 0;
                        bool rColumn = 0 < row && row < 7 && col == 7;
                        bool tRow = row == 0 && 0 < col && col < 7;
                        bool bRow = row == 7 && 0 < col && col < 7;

                        if (tlCorner || trCorner || blCorner || brCorner)
                            tileValue *= 100;
                        else if (lColumn || rColumn || tRow || bRow)
                            tileValue *= 10;
                        value += tileValue;
                    }
                }
            }
            if (board.IsTerminalState()) {
                if (value < 0) value -= 10000;
                else value += 10000;
            }
            return value;
        }

        private bool betterMove(int player, int moveRank, int bestMoveRank) {
            bool isBetterMove = false;
            if (player == max && moveRank > bestMoveRank) isBetterMove = true;
            if (player == min && moveRank < bestMoveRank) isBetterMove = true;
            return isBetterMove;
        }

        private int GetNextPlayer(int player, Board board) {
            Board boardState = new Board();
            boardState.Copy(board);
            if (boardState.HasAnyValidMove(-player)) return -player;
            else return player;
        }
    }
}
