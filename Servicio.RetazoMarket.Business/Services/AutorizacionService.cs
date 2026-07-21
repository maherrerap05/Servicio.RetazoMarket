using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.Business.Exceptions;
using Servicio.RetazoMarket.Business.Interfaces;

namespace Servicio.RetazoMarket.Business.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private const string SuperAdministrador = "SUPERADMINISTRADOR";
        private const string Administrador = "ADMINISTRADOR";
        private const string Cliente = "CLIENTE";

        public void ExigirAutenticado(ActorContext actor)
        {
            if (actor is null || !actor.autenticado || actor.id_usuario <= 0)
                throw new UnauthorizedBusinessException("Se requiere un usuario autenticado.");
        }

        public void ExigirSuperAdministrador(ActorContext actor)
        {
            ExigirAutenticado(actor);
            if (!EsSuperAdministrador(actor))
                throw new UnauthorizedBusinessException("La operación requiere el rol SUPERADMINISTRADOR.");
        }

        public void ExigirAdministrador(ActorContext actor)
        {
            ExigirAutenticado(actor);
            if (!EsAdministrador(actor))
                throw new UnauthorizedBusinessException("La operación requiere un rol administrativo.");
        }

        public void ExigirGestionTecnica(ActorContext actor) =>
            ExigirSuperAdministrador(actor);

        public void ExigirGestionAdministrativa(ActorContext actor) =>
            ExigirAdministrador(actor);

        public void ExigirPropietarioOAdministrador(ActorContext actor, int idClienteRecurso)
        {
            ExigirAutenticado(actor);
            if (EsAdministrador(actor))
                return;

            if (!EsCliente(actor) || actor.id_cliente != idClienteRecurso)
                throw new UnauthorizedBusinessException("No está autorizado para operar sobre este recurso.");
        }

        public void ExigirClientePropietario(ActorContext actor, int idClienteRecurso)
        {
            ExigirAutenticado(actor);
            if (!EsCliente(actor) || actor.id_cliente != idClienteRecurso)
                throw new UnauthorizedBusinessException("El cliente solo puede operar sobre sus propios recursos.");
        }

        public bool EsSuperAdministrador(ActorContext actor) =>
            TieneRol(actor, SuperAdministrador);

        public bool EsAdministrador(ActorContext actor) =>
            EsSuperAdministrador(actor) || TieneRol(actor, Administrador);

        public bool EsCliente(ActorContext actor) =>
            TieneRol(actor, Cliente);

        private static bool TieneRol(ActorContext? actor, string rol) =>
            actor?.roles.Any(x => string.Equals(x?.Trim(), rol, StringComparison.OrdinalIgnoreCase)) == true;
    }
}
