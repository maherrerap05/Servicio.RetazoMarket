using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Servicio.RetazoMarket.Api.Models.Common;
using Servicio.RetazoMarket.Business.Exceptions;

namespace Servicio.RetazoMarket.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private const string UniqueViolationSqlState = "23505";

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IOptions<JsonOptions> jsonOptions)
    {
        _next = next;
        _logger = logger;
        _jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogDebug(
                "La solicitud {Method} {Path} fue cancelada por el cliente. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            return;
        }
        catch (ValidationException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                ApiErrorResponse.Fail(ex.Message, ex.Errors));
        }
        catch (NotFoundException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.NotFound,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (UnauthorizedBusinessException ex)
        {
            var statusCode = context.User.Identity?.IsAuthenticated == true
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;

            await WriteErrorAsync(
                context,
                statusCode,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (BusinessException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: UniqueViolationSqlState } pgEx)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.Conflict,
                ApiErrorResponse.Fail(ExtraerMensajeUnicidad(pgEx.ConstraintName ?? string.Empty)));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error interno no controlado en {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            await WriteErrorAsync(
                context,
                HttpStatusCode.InternalServerError,
                ApiErrorResponse.Fail("Ocurrió un error interno inesperado."));
        }

        if (!context.Response.HasStarted)
        {
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteErrorAsync(
                    context,
                    HttpStatusCode.Unauthorized,
                    ApiErrorResponse.Fail(
                        "No autenticado. Debe enviar un token JWT válido en el encabezado Authorization."));
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await WriteErrorAsync(
                    context,
                    HttpStatusCode.Forbidden,
                    ApiErrorResponse.Fail(
                        "Acceso denegado. No tiene permisos suficientes para ejecutar esta acción."));
            }
        }
    }

    private static string ExtraerMensajeUnicidad(string constraintName)
    {
        var normalizedConstraintName = constraintName.ToLowerInvariant();

        if (normalizedConstraintName.Contains("uq_proveedores_codigo_proveedor"))
            return "Ya existe un proveedor registrado con ese código.";

        if (normalizedConstraintName.Contains("uq_descuentos_producto_cantidad"))
            return "Ya existe un descuento para el producto con la misma cantidad mínima.";

        if (normalizedConstraintName.Contains("uq_cliente_correo"))
            return "Ya existe un cliente registrado con ese correo electrónico.";

        if (normalizedConstraintName.Contains("uq_usuario_correo"))
            return "Ya existe un usuario registrado con ese correo electrónico.";

        return "Ya existe un registro con los mismos datos únicos. Verifique los campos e intente nuevamente.";
    }

    private async Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        ApiErrorResponse response)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.Clear();
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = (int)statusCode;
        response.TraceId ??= context.TraceIdentifier;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, _jsonSerializerOptions),
            context.RequestAborted);
    }
}
