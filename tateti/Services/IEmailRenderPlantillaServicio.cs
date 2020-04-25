using System.Threading.Tasks;

namespace tateti.Services
{
    public interface IEmailRenderPlantillaServicio
    {
        Task<string> RenderearTemplate<T>(string nombrePlantilla, T model, string host) where T : class;
    }
}