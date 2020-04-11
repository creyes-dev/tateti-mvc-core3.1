using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tateti.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "NombreRequired")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "ApellidoRequired")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "EmailRequired"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequired"), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool MailEstaConfirmado { get; set; }
        public System.DateTime? FechaConfirmacionEmail { get; set; }
        public int Puntaje { get; set; }
    }
}
