namespace webapi
{
    public class SD
    {
        public static string identityApiBase { get; set; }
        public static string gatewayApiBase { get; set; }
        /*public static string ProductApiBase { get; set; }
        public static string ShoppingApiBase { get; set; }
        public static string Identity { get; set; }*/
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
