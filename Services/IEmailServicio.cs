using System.Threading.Tasks;

namespace tateti.Services
{
    public interface IEmailServicio
    {
        Task EnviarEmail(string emailDestino, string asunto, string mensaje);
    }
}