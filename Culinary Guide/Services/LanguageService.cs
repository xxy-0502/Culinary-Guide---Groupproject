using System.Globalization;
using Culinary_Guide.Resources;

namespace Culinary_Guide.Services
{
    public class LanguageService : ILanguageService
    {
        private AppLanguage _currentLanguage;
        
        public event EventHandler? LanguageChanged;
        
        public AppLanguage CurrentLanguage => _currentLanguage;
        
        public LanguageService()
        {
            var savedLanguage = Preferences.Default.Get("app_language", "en-US");
            _currentLanguage = savedLanguage == "zh-CN" ? AppLanguage.Chinese : AppLanguage.English;
            ApplyLanguage(_currentLanguage);
        }
        
        public void SetLanguage(AppLanguage language)
        {
            if (_currentLanguage == language) return;
            
            _currentLanguage = language;
            var cultureCode = language == AppLanguage.Chinese ? "zh-CN" : "en-US";
            Preferences.Default.Set("app_language", cultureCode);
            ApplyLanguage(language);
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public string GetString(string key)
        {
            return AppResources.ResourceManager.GetString(key, AppResources.Culture) ?? key;
        }
        
        private void ApplyLanguage(AppLanguage language)
        {
            var culture = language == AppLanguage.Chinese 
                ? new CultureInfo("zh-CN") 
                : new CultureInfo("en-US");
            
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            AppResources.Culture = culture;
        }
    }
}