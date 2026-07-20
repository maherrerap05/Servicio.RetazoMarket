namespace Servicio.RetazoMarket.Business.Exceptions
{
    /// <summary>
    /// Excepción utilizada cuando un recurso no es encontrado en el sistema.
    /// </summary>
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
