using System.Threading.Tasks;

namespace tateti.ViewEngines
{
    public interface IEmailViewEngine
    {
        Task<string> RenderEmailToString<TModel>(string viewName, TModel model);
    }
}