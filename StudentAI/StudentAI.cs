using GameAI.GamePlaying.Core;

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        // Constructor; creates a new Minimax AI agent behavior
        public StudentAI() {}

        // Implementation of Minimax. Determines the best move for the player specified by color on the given 
        // board, looking ahead the number of steps indicated by lookAheadDepth. This method will not be called
        // unless there is at least one possible move for the player specified by color.
        public ComputerMove Run(int color, Board board, int lookAheadDepth) {
            return Minimax(color, board, 0, lookAheadDepth);
        }

        // Recursive method that utilizes the Minimax algorithm to help the player make the best move
        private ComputerMove Minimax(int player, Board board, int depth, int maxDepth) {
            ComputerMove bestMove = null;
            Board boardState = new Board();

            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    if (board.IsValidMove(player, row, col)) {
                        ComputerMove move = new ComputerMove(row, col);
                        boardState.Copy(board);
                        boardState.MakeMove(player, row, col);

                        if (boardState.IsTerminalState() || depth == maxDepth) move.rank = Evaluate(boardState);
                        else move.rank = Minimax(GetNextPlayer(player, boardState), boardState, depth + 1, maxDepth).rank;
                        if (bestMove == null || betterMove(player, move.rank, bestMove.rank)) bestMove = move;
                    }
                }
            }
            return bestMove;
        }

        // Returns a strategic value estimate for board based on position and color of pieces. It is recommended that 
        // students implement a method with this functionality to evaluate board states at the lookahead depth cutoff.
        private int Evaluate(Board board) {
            int value = 0;
            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    int tileValue = board.GetTile(row, col);
                    if (tileValue != 0) {
                        if (row == 0 || row == 7) tileValue *= 10;
                        if (col == 0 || col == 7) tileValue *= 10;
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

        // Determines if a move is better than the best move known so far
        private bool betterMove(int player, int moveRank, int bestMoveRank) {
            bool isBetterMove = false;
            if (player == -1 && moveRank < bestMoveRank) isBetterMove = true; // Black wants negative
            if (player == 1 && moveRank > bestMoveRank) isBetterMove = true; // White wants positive
            return isBetterMove;
        }

        // Checks if the player has to forfeit a turn to their opponent
        private int GetNextPlayer(int player, Board board) {
            Board boardState = new Board();
            boardState.Copy(board);
            if (boardState.HasAnyValidMove(-player)) return -player;
            else return player;
        }
    }
}
