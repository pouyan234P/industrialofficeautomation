namespace authentication.api
{
    /// <summary>
    /// Logs the exception and returns a generic message for the client.
    /// Exception details (stack traces, SQL, connection info) must never be sent to the browser.
    /// </summary>
    internal static class Errors
    {
        public const string UnexpectedMessage = "An unexpected server error occurred. Please try again or contact an administrator.";

        public static List<string> Unexpected(Microsoft.AspNetCore.Http.HttpContext context, Exception e)
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("authentication.api");
            logger.LogError(e, "Unhandled error on {Method} {Path}", context.Request.Method, context.Request.Path);
            return new List<string> { UnexpectedMessage };
        }
    }
}
