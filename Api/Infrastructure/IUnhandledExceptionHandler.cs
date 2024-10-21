namespace Api.Infrastructure
{
    public interface IUnhandledExceptionHandler
    {
        Task OnUnhandledException(HttpContext httpContext);
    }
}