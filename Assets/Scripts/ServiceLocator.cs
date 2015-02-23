using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class ServiceLocator
    {
        private static readonly IDictionary<object, object> services;

        static ServiceLocator()
        {
            services = new Dictionary<object, object>();
        }

        public static void AddService<T>(T service) where T: class
        {
            services[typeof (T)] = service;
        }

        public static void RemoveService<T>() where T : class
        {
            services.Remove(typeof (T));
        }

        public static T GetService<T>() where T : class
        {
            object service;

            if (services.TryGetValue(typeof (T), out service))
            {
                return (T) service;
            }

            return default(T);
        }
    }
}
