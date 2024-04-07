namespace Core.Common
{
    public class Enums
    {
        public enum OrderType
        {
            File = 1,
            Manual = 2
        }

        public enum OrderStatus
        {
            Initialized = 1,
            Processed = 2,
        }

        public enum ResponseStatus
        {
            success,
            error,
            fail,
            warning
        }
    }
}
