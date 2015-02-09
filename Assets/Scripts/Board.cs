using UnityEngine;

namespace Assets.Scripts
{
    public class Board : MonoBehaviour
    {
        public const int ROWS = 3;
        public const int COLS = 3;

        [SerializeField]
        private GameObject cellPrefab;

        [SerializeField]
        private float cellOffset = 1.5f;

        private Cell[,] cells = new Cell[ROWS, COLS];

        public void Clear()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    cells[row, col].Init(row, col);
                }
            }
        }

        public bool IsDraw()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (cells[row, col].CurrentState == Cell.State.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HasWon(Cell cell)
        {
            return true;
        }

        private void CreateCells()
        {
            Transform container = (new GameObject("cells")).transform;
            
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

                    cell.Init(row, col);

                    cells[row, col] = cell;
                }
            }
        }

        private void Awake()
        {
            CreateCells();
        }
    }
}