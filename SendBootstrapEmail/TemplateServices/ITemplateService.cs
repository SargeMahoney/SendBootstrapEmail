namespace SendBootstrapEmail.TemplateServices
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(string viewName, T model) where T : class, new();
    }
}
