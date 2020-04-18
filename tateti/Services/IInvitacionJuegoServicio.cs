using System;
using System.Threading.Tasks;
using tateti.Models;

namespace tateti.Services
{
    public interface IInvitacionJuegoServicio
    {
        Task ActualizarInvitacion(InvitacionJuegoModel invitacion);
        Task<InvitacionJuegoModel> AgregarInvitacion(InvitacionJuegoModel invitacion);
        Task<InvitacionJuegoModel> ObtenerInvitacion(Guid id);
    }
}