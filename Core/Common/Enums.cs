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
            Initializata = 1,
            Procesata = 2,
            Facturata = 3,
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
