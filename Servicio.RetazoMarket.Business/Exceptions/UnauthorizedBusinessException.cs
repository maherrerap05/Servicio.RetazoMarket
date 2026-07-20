namespace Servicio.RetazoMarket.Business.Exceptions
{
    /// <summary>
    /// Excepción utilizada cuando un usuario no está autorizado para ejecutar una acción.
    /// </summary>
    public class UnauthorizedBusinessException : BusinessException
    {
        public UnauthorizedBusinessException(string message) : base(message)
        {
        }
    }
}
