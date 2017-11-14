using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace TodoREST
{
    public class CircuitBreakerService : ICircuitBreakerService
    {
        readonly ICircuitBreakerStateStore _stateStore;
        readonly object _halfOpenSyncObject = new object();

        string _resourceName;
        TimeSpan _openToHalfOpenWaitTime;

        public string ResourceName { get { return _resourceName; } }

        public bool IsClosed { get { return _stateStore.IsClosed; } }

        public bool IsOpen { get { return !IsClosed; } }

        public CircuitBreakerService(string resource, int openToHalfOpenWaitTime)
        {
            _stateStore = CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore(resource);
            _resourceName = resource;
            _openToHalfOpenWaitTime = new TimeSpan(0, 0, 0, 0, openToHalfOpenWaitTime);
        }

        public async Task<HttpResponseMessage> InvokeAsync(Func<Task<HttpResponseMessage>> operation)
        {
            HttpResponseMessage response = null;

            if (IsOpen)
            {
                // Circuit breaker is open
                System.Diagnostics.Debug.WriteLine("Circuit is open.");
                return await WhenCircuitIsOpenAsync(operation);
            }
            else
            {
                // Circuit breaker is closed - execute the operation
                try
                {
                    System.Diagnostics.Debug.WriteLine("Circuit is closed. Executing operation.");
                    response = await operation();
                }
                catch (Exception ex)
                {
                    // Retrip the breaker immediately and throw the exception so that
                    // the caller can tell the type of exception that was thrown
                    System.Diagnostics.Debug.WriteLine("Tripping the circuit breaker.");
                    TrackException(ex);
                    throw;
                }
            }
            return response;
        }

        public async Task<HttpResponseMessage> InvokeAsync(Func<Task<HttpResponseMessage>> operation, Action<CircuitBreakerOpenException> circuitBreakerOpenAction, Action<Exception> anyOtherExceptionAction)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await InvokeAsync(operation);
            }
            catch (CircuitBreakerOpenException ex)
            {
                // Perform a different operation when the circuit breaker is open
                circuitBreakerOpenAction(ex);
            }
            catch (Exception ex)
            {
                anyOtherExceptionAction(ex);
            }
            return response;
        }

        async Task<HttpResponseMessage> WhenCircuitIsOpenAsync(Func<Task<HttpResponseMessage>> operation)
        {
            HttpResponseMessage response = null;

            // The circuit breaker is Open. Check if the Open timeout has expired.
            // If it has, set the state to HalfOpen. Another approach might be to
            // check for the HalfOpen state that had be set by some other operation.
            if (_stateStore.LastStateChangedDate + _openToHalfOpenWaitTime < DateTime.UtcNow)
            {
                // The Open timeout has expired. Allow one operation to execute. Note that, in
                // this example, the circuit breaker is set to HalfOpen after being
                // in the Open state for some period of time. An alternative would be to set
                // this using some other approach such as a timer, test method, manually, and
                // so on, and check the state here to determine how to handle execution
                // of the action.
                // Limit the number of threads to be executed when the breaker is HalfOpen.
                // An alternative would be to use a more complex approach to determine which
                // threads or how many are allowed to execute, or to execute a simple test
                // method instead.
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_halfOpenSyncObject, ref lockTaken);
                    if (lockTaken)
                    {
                        // Set the circuit breaker to half open
                        _stateStore.HalfOpen();

                        // Attempt the operation
                        response = await operation();

                        // If this action succeeds, reset the state and allow other operations.
                        // In reality, instead of immediately returning to the Closed state, a counter
                        // here would record the number of successful operations and return the
                        // circuit breaker to the Closed state only after a specified number succeed.
                        _stateStore.Reset();
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    // Trip the breaker and throw the exception so that
                    // the caller can tell the type of exception that was thrown
                    _stateStore.Trip(ex);
                    throw;
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(_halfOpenSyncObject);
                    }
                }
            }

            // The Open timeout hasn't yet expired. Throw a CircuitBreakerOpen exception to
            // inform the caller that the call was not actually attempted,
            // and return the most recent exception received.
            throw new CircuitBreakerOpenException(
                "The circuit is open. Refer to the inner exception for more details.",
                _stateStore.LastException);
        }

        void TrackException(Exception ex)
        {
            // For simplicity in this example, open the circuit breaker on the first exception.
            // In reality this would be more complex. A certain type of exception, such as one
            // that indicates a service is offline, might trip the circuit breaker immediately. 
            // Alternatively it may count exceptions locally or across multiple instances and
            // use this value over time, or the exception/success ratio based on the exception
            // types, to open the circuit breaker.
            _stateStore.Trip(ex);
        }
    }
}
