using System;

namespace Minesweeper
{
    public class GameBoard
    {
        public readonly int boardWidth;
        public readonly int boardHeight;
        public readonly int bombs;

        private Cell[,] _board;

        public Cell[,] Board
        {
            get { return this._board;}
        }

        public GameBoard(int boardWidth, int boardHeight, int bombs)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            this.bombs = bombs;

            this.GenerateBoard();
            this.PlaceBombs();
            this.AssignCellNeighbours();
        }

        private void GenerateBoard()
        {
            _board = new Cell[boardHeight, boardWidth];
            for (var y = 0; y < boardHeight; y++)
            {
                for (var x = 0; x < boardWidth; x++)
                {
                    _board[y, x] = new Cell(x, y);
                }
            }
        }

        private void PlaceBombs()
        {
            var random = new Random();

            var bombsPlaced = 0;

            while (bombsPlaced != this.bombs)
            {
                var x = random.Next(0, boardWidth);
                var y = random.Next(0, boardHeight);

                if (_board[y, x].State == State.Bomb) continue;

                _board[y, x].SetAsBomb();
                bombsPlaced++;
            }
        }

        private void AssignCellNeighbours()
        {
            for (var y = 0; y < boardHeight; y++)
            {
                for (var x = 0; x < boardWidth; x++)
                {
                    var cell = _board[y, x];
                    this.GetNeighbour(cell, x - 1, y);
                    this.GetNeighbour(cell, x + 1, y);
                    this.GetNeighbour(cell, x, y + 1);
                    this.GetNeighbour(cell, x, y - 1);

                    this.GetNeighbour(cell, x - 1, y + 1);
                    this.GetNeighbour(cell, x + 1, y + 1);

                    this.GetNeighbour(cell, x - 1, y - 1);
                    this.GetNeighbour(cell, x + 1, y - 1);
                }
            }
        }

        private void GetNeighbour(Cell cell, int neighbourX, int neighbourY)
        {
            if (neighbourY < 0 || neighbourX < 0) return;

            if (neighbourX >= boardWidth) return;

            if (neighbourY >= boardHeight) return;

            cell.AddNeighbouringCell(_board[neighbourY, neighbourX]);
        }
    }
}