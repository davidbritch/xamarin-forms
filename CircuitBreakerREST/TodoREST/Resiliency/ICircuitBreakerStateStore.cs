using System;

namespace TodoREST
{
    public interface ICircuitBreakerStateStore
    {
        string Name { get; }
        CircuitBreakerState State { get; }
        Exception LastException { get; }
        DateTime? LastStateChangedDate { get; }
        bool IsClosed { get; }

        void Trip(Exception ex);
        void Reset();
        void HalfOpen();
    }
}
