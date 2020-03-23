using GameAI.GamePlaying.Core;

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
            return Minimax(color, board, 0);
        }

        private ComputerMove Minimax(int player, Board board, int depth) {
            ComputerMove bestMove = null;
            Board boardState = new Board();

            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    if (board.IsValidMove(player, row, col)) {
                        ComputerMove move = new ComputerMove(row, col);
                        boardState.Copy(board);
                        boardState.MakeMove(player, row, col);

                        if (boardState.IsTerminalState() || depth == maxDepth) move.rank = Evaluate(boardState);
                        else move.rank = Minimax(GetNextPlayer(player, boardState), boardState, depth + 1).rank;

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
                    int color = board.GetTile(row, col);
                    if (color != 0) {
                        int tileValue = color;
                        if (row == 0 || row == 7)
                            tileValue *= 10;
                        if (col == 0 || col == 7)
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
            if (player == max && moveRank < bestMoveRank) isBetterMove = true; // Black wants negative
            if (player == min && moveRank > bestMoveRank) isBetterMove = true; // White wants positive
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
