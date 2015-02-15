using System;

namespace Assets.Scripts.Signals
{
    public class Signal
    {
        private event Action Listener = delegate { };
        private event Action OnceListener = delegate { };

        public void AddListener(Action callback)
        {
            Listener += callback;
        }

        public void AddOnce(Action callback)
        {
            OnceListener += callback;
        }

        public void RemoveListener(Action callback)
        {
            Listener -= callback;
        }

        public void Dispatch()
        {
            Listener();
            OnceListener();
            OnceListener = delegate { };
        }
    }

    public class Signal<T>
    {
        private event Action<T> Listener = delegate { };
        private event Action<T> OnceListener = delegate { };

        public void AddListener(Action<T> callback)
        {
            Listener += callback;
        }

        public void AddOnce(Action<T> callback)
        {
            OnceListener += callback;
        }

        public void RemoveListener(Action<T> callback)
        {
            Listener -= callback;
        }

        public void Dispatch(T type1)
        {
            Listener(type1);
            OnceListener(type1);
            OnceListener = delegate { };
        }
    }

    public class Signal<T, U>
    {
        private event Action<T, U> Listener = delegate { };
        private event Action<T, U> OnceListener = delegate { };

        public void AddListener(Action<T, U> callback)
        {
            Listener += callback;
        }

        public void AddOnce(Action<T, U> callback)
        {
            OnceListener += callback;
        }

        public void RemoveListener(Action<T, U> callback)
        {
            Listener -= callback;
        }

        public void Dispatch(T type1, U type2)
        {
            Listener(type1, type2);
            OnceListener(type1, type2);
            OnceListener = delegate { };
        }
    }

    public class Signal<T, U, V>
    {
        private event Action<T, U, V> Listener = delegate { };
        private event Action<T, U, V> OnceListener = delegate { };

        public void AddListener(Action<T, U, V> callback)
        {
            Listener += callback;
        }

        public void AddOnce(Action<T, U, V> callback)
        {
            OnceListener += callback;
        }

        public void RemoveListener(Action<T, U, V> callback)
        {
            Listener -= callback;
        }

        public void Dispatch(T type1, U type2, V type3)
        {
            Listener(type1, type2, type3);
            OnceListener(type1, type2, type3);
            OnceListener = delegate { };
        }
    }

    public class Signal<T, U, V, W>
    {
        private event Action<T, U, V, W> Listener = delegate { };
        private event Action<T, U, V, W> OnceListener = delegate { };

        public void AddListener(Action<T, U, V, W> callback)
        {
            Listener += callback;
        }

        public void AddOnce(Action<T, U, V, W> callback)
        {
            OnceListener += callback;
        }

        public void RemoveListener(Action<T, U, V, W> callback)
        {
            Listener -= callback;
        }

        public void Dispatch(T type1, U type2, V type3, W type4)
        {
            Listener(type1, type2, type3, type4);
            OnceListener(type1, type2, type3, type4);
            OnceListener = delegate { };
        }
    }
}
