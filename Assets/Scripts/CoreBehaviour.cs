using Assets.Scripts.Network;
using UnityEngine;

namespace Assets.Scripts
{
    public class CoreBehaviour : MonoBehaviour
    {
        public new GameObject gameObject { get; private set; }

        public new Transform transform { get; private set; }

        protected IGameService GameService { get; private set; }

        protected INetworkService NetworkService { get; private set; }
        
        protected virtual void Awake()
        {
            gameObject = base.gameObject;
            transform = base.transform;
        }

        protected virtual void Start()
        {
            GameService = ServiceLocator.GetService<IGameService>();
            NetworkService = ServiceLocator.GetService<INetworkService>();
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
