using System;

namespace Services.Utils
{
    public class OperationResult
    {
        private OperationResult()
        {
        }

        public class Success : OperationResult
        {
        }

        public class Failure : OperationResult
        {
        }
    }
}