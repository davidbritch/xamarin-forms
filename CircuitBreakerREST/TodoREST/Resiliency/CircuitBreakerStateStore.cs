using System;
using System.Collections.Concurrent;

namespace TodoREST
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        ConcurrentStack<Exception> _exceptionsSinceLastStateChange;
        CircuitBreakerState _state;
        DateTime? _lastStateChangedDate;

        public string Name { get; private set; }

        public CircuitBreakerState State
        {
            get
            {
                if (_state.Equals(CircuitBreakerState.None))
                {
                    _state = CircuitBreakerState.Closed;
                }
                return _state;
            }
            private set { _state = value; }
        }

        public Exception LastException
        {
            get
            {
                Exception lastException = null;
                _exceptionsSinceLastStateChange.TryPeek(out lastException);
                return lastException;
            }
        }

        public DateTime? LastStateChangedDate
        {
            get { return _lastStateChangedDate; }
            private set { _lastStateChangedDate = value; }
        }

        public bool IsClosed
        {
            get { return State.Equals(CircuitBreakerState.Closed); }
        }

        public CircuitBreakerStateStore(string key)
        {
            _exceptionsSinceLastStateChange = new ConcurrentStack<Exception>();
            Name = key;
        }

        public void Trip(Exception ex)
        {
            ChangeState(CircuitBreakerState.Open);
            _exceptionsSinceLastStateChange.Push(ex);
        }

        public void Reset()
        {
            ChangeState(CircuitBreakerState.Closed);
            _exceptionsSinceLastStateChange.Clear();
        }

        public void HalfOpen()
        {
            ChangeState(CircuitBreakerState.HalfOpen);
        }

        void ChangeState(CircuitBreakerState state)
        {
            State = state;
            LastStateChangedDate = DateTime.UtcNow;
        }
    }
}
