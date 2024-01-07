using System;


namespace EIRS.Common
{
    public class FuncResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object AdditionalData { get; set; }

        public Exception Exception { get; set; }
    }

    public class FuncResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T AdditionalData { get; set; }

        public Exception Exception { get; set; }
    }
}
