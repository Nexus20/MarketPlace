using MarketPlace.Application.Exceptions;

namespace MarketPlace.API.Middlewares {
    public class ExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Serilog.ILogger logger) {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                logger.Information(exception, "Validation exception is occured");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (IdentityException exception)
            {
                logger.Information(exception, "Identity exception is occured");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (NotFoundException exception)
            {
                logger.Information(exception, "Resource not found");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            catch (Exception exception) {
                HandleStatus500Exception(context, exception, logger);
            }
        }

        private void HandleStatus500Exception(HttpContext context, Exception exception, Serilog.ILogger logger) {

            logger.Error(exception, "An exception was thrown as a result of the request");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
