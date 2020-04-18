using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using tateti.Models;

namespace tateti.Services
{
    public class InvitacionJuegoServicio : IInvitacionJuegoServicio
    {
        public static ConcurrentBag<InvitacionJuegoModel> _invitaciones;

        public InvitacionJuegoServicio()
        {
            _invitaciones = new ConcurrentBag<InvitacionJuegoModel>();
        }

        public Task<InvitacionJuegoModel> AgregarInvitacion(InvitacionJuegoModel invitacion)
        {
            invitacion.Id = Guid.NewGuid();
            _invitaciones.Add(invitacion);
            return Task.FromResult(invitacion);
        }

        public Task ActualizarInvitacion(InvitacionJuegoModel invitacion)
        {
            _invitaciones = new ConcurrentBag<InvitacionJuegoModel>(
                _invitaciones.Where(i => i.Id != invitacion.Id))
            { invitacion };
            return Task.CompletedTask;
        }

        public Task<InvitacionJuegoModel> ObtenerInvitacion(Guid id)
        {
            return Task.FromResult(_invitaciones.FirstOrDefault(i => i.Id == id));
        }

    }
}
