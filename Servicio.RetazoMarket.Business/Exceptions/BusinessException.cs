namespace Servicio.RetazoMarket.Business.Exceptions
{
    /// <summary>
    /// Excepción base para la capa de negocio.
    /// Todas las excepciones personalizadas del dominio deben heredar de esta clase.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
