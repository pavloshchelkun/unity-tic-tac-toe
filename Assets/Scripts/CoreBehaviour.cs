using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts
{
    public class CoreBehaviour : MonoBehaviour
    {
        public new GameObject gameObject { get; private set; }

        public new Transform transform { get; private set; }
        
        protected virtual void Awake()
        {
            gameObject = base.gameObject;
            transform = base.transform;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
