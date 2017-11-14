using System.Collections.Concurrent;

namespace TodoREST
{
    public class CircuitBreakerStateStoreFactory
    {
        static ConcurrentDictionary<string, ICircuitBreakerStateStore> StateStores = new ConcurrentDictionary<string, ICircuitBreakerStateStore>();

        internal static ICircuitBreakerStateStore GetCircuitBreakerStateStore(string key)
        {
            if (!StateStores.ContainsKey(key))
            {
                StateStores.TryAdd(key, new CircuitBreakerStateStore(key));
            }
            return StateStores[key];
        }
    }
}
