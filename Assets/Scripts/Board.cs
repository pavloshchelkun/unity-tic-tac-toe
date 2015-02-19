using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Board : CoreBehaviour
    {
        private const int ROWS = 3;
        private const int COLS = 3;

        public GameObject cellPrefab;
        public float cellOffset = 1.5f;

        private Cell[,] cells = new Cell[ROWS, COLS];

        private Cell lastChangedCell;

        private Seed currentPlayer;

        private event Action<Seed, int, int> onBoardChange;

        public void Init(Action<Seed, int, int> onBoardChangeAction)
        {
            CreateCells();
            SetPlayer(Seed.Empty);
            onBoardChange = onBoardChangeAction;
        }

        public void SetPlayer(Seed seed)
        {
            currentPlayer = seed;
        }

        public void SetCell(Seed seed, int row, int col)
        {
            SetPlayer(seed);
            OnCellClick(cells[row, col]);
        }

        public void Clear()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    cells[row, col].Clear();
                }
            }

            lastChangedCell = null;
        }

        public bool IsDraw()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (cells[row, col].Content == Seed.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HasWon(Seed seed)
        {
            if (lastChangedCell == null)
            {
                return false;
            }

            return HasWonInTheRow(seed)
                || HasWonInTheColumn(seed)
                || HasWonInTheDiagonal(seed)
                || HasWonInTheOppositeDiagonal(seed);
        }

        private bool HasWonInTheRow(Seed seed)
        {
            int row = lastChangedCell.Row;

            return cells[row, 0].HasSeed(seed) 
                && cells[row, 1].HasSeed(seed) 
                && cells[row, 2].HasSeed(seed);
        }

        private bool HasWonInTheColumn(Seed seed)
        {
            int col = lastChangedCell.Col;

            return cells[0, col].HasSeed(seed)
                && cells[1, col].HasSeed(seed)
                && cells[2, col].HasSeed(seed);
        }

        private bool HasWonInTheDiagonal(Seed seed)
        {
            return cells[0, 0].HasSeed(seed)
                && cells[1, 1].HasSeed(seed)
                && cells[2, 2].HasSeed(seed);
        }

        private bool HasWonInTheOppositeDiagonal(Seed seed)
        {
            return cells[0, 2].HasSeed(seed)
                && cells[1, 1].HasSeed(seed)
                && cells[2, 0].HasSeed(seed);
        }

        private void OnCellClick(Cell cell)
        {
            if (currentPlayer != Seed.Empty && cell.IsEmpty)
            {
                cell.Set(currentPlayer);
                lastChangedCell = cell;

                if (onBoardChange != null)
                {
                    onBoardChange(currentPlayer, cell.Row, cell.Col);
                }
            }
        }

        private void CreateCells()
        {
            Transform container = (new GameObject("Cells")).transform;
            
            container.transform.parent = transform;
            container.transform.localPosition = new Vector3(-cellOffset, -cellOffset, 0f);

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    GameObject go = (GameObject)Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                    Cell cell = go.GetComponent<Cell>();

                    cell.transform.parent = container;
                    cell.transform.localPosition = new Vector3(col * cellOffset, row * cellOffset, 0f);

                    cell.Init(row, col, OnCellClick);

                    cells[row, col] = cell;
                }
            }
        }
    }
}
