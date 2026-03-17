namespace Culinary_Guide.Services
{
    public interface ILanguageService
    {
        event EventHandler? LanguageChanged;
        AppLanguage CurrentLanguage { get; }
        void SetLanguage(AppLanguage language);
        string GetString(string key);
    }

    public enum AppLanguage
    {
        English,
        Chinese
    }
}