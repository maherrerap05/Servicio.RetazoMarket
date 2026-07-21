using Servicio.RetazoMarket.Business.DTOs.Auth;

namespace Servicio.RetazoMarket.Business.Interfaces
{
    public interface IAutorizacionService
    {
        void ExigirAutenticado(ActorContext actor);
        void ExigirSuperAdministrador(ActorContext actor);
        void ExigirAdministrador(ActorContext actor);
        void ExigirGestionTecnica(ActorContext actor);
        void ExigirGestionAdministrativa(ActorContext actor);
        void ExigirPropietarioOAdministrador(ActorContext actor, int idClienteRecurso);
        void ExigirClientePropietario(ActorContext actor, int idClienteRecurso);
        bool EsSuperAdministrador(ActorContext actor);
        bool EsAdministrador(ActorContext actor);
        bool EsCliente(ActorContext actor);
    }
}
