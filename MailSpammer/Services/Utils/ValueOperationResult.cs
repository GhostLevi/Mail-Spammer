using System;

namespace Services.Utils
{
    public class ValueOperationResult<TValue>
    {
        private ValueOperationResult()
        {
        }

        public class Success : ValueOperationResult<TValue>
        {
            private Success() { }

            public Success(TValue value)
            {
                Value = value;
            }

            public TValue Value { get; private set; }
        }

        public class Failure : ValueOperationResult<TValue>
        {
            private Failure() { }

            public Failure(Exception innerException)
            {
                InnerException = innerException;
            }

            public Exception InnerException { get; private set; }
        }
    }
}