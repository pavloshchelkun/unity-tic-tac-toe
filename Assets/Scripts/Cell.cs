using UnityEngine;

namespace Assets.Scripts
{
    public class Cell : MonoBehaviour
    {
        public enum State
        {
            Empty,
            Cross,
            Nought
        }

        [SerializeField]
        private GameObject spriteCross;

        [SerializeField]
        private GameObject spriteNought;

        public int Row { get; private set; }

        public int Col { get; private set; }

        public State CurrentState { get; private set; }

        public void Init(int row, int col)
        {
            Row = row;
            Col = col;

            name = string.Format("cell [{0}, {1}]", row, col);

            Clear();
        }

        public void Set(State state)
        {
            CurrentState = state;

            switch (state)
            {
                case State.Empty:
                    spriteCross.SetActive(false);
                    spriteNought.SetActive(false);
                    break;
                case State.Cross:
                    spriteCross.SetActive(true);
                    spriteNought.SetActive(false);
                    break;
                case State.Nought:
                    spriteCross.SetActive(false);
                    spriteNought.SetActive(true);
                    break;
            }
        }

        public void Clear()
        {
            Set(State.Empty);
        }

        public void OnClick()
        {
            Set(State.Cross);
        }
    }
}
