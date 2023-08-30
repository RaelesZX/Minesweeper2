using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Cell
    {
        public readonly int XCoordinate;
        public readonly int YCoordinate;

        public bool revealed { get; private set;}
        public State State { get; private set; }

        private List<Cell> neighbouringCells;

        private int surroundingBombs = -1;

        public Cell(int x, int y, State state = State.Blank)
        {
            this.XCoordinate = x;
            this.YCoordinate = y;
            this.State = state;
            this.neighbouringCells = new List<Cell>();
        }

        public void FlipCell()
        {
            this.revealed = true;

            if (this.State == State.Bomb) return;

            if (this.GetTotalSurroundingBombs() > 0) return;
            
            foreach (Cell cell in this.neighbouringCells)
            {
                cell.CheckCell();
            }
        }

        public void CheckCell()
        {
            if (this.revealed) return;

            if(this.State != State.Bomb || this.State != State.BombFlag)
            {
                this.revealed = true;

                if (this.GetTotalSurroundingBombs() > 0) return;

                foreach (Cell cell in this.neighbouringCells)
                {
                    cell.CheckCell();
                }
            }
        }

        public int GetTotalSurroundingBombs()
        {
            if (surroundingBombs >= 0) return surroundingBombs;

            return surroundingBombs = neighbouringCells.Where(c => c.State == State.Bomb).Count();
        }

        public void SetAsBomb()
            {
            this.State = State.Bomb;
        }

        public void Flag()
        {
            this.State = this.State == State.Bomb ? State.BombFlag : State.Flagged;
        }

        public void UnFlag()
        {
            this.State = this.State == State.BombFlag ? State.Bomb : State.Blank;
        }

        public void AddNeighbouringCell(Cell cell)
        {
            this.neighbouringCells.Add(cell);
        }

    }
}
