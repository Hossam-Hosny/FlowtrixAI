using Microsoft.JSInterop;

namespace FlowtrixAi.Client
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isDarkMode;

        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkMode => _isDarkMode;

        public async Task ToggleThemeAsync()
        {
            _isDarkMode = !_isDarkMode;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _isDarkMode ? "dark" : "light");
            await ApplyThemeAsync();
            OnThemeChanged?.Invoke();
        }

        public async Task InitializeAsync()
        {
            var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            _isDarkMode = theme == "dark";
            await ApplyThemeAsync();
            OnThemeChanged?.Invoke();
        }

        private async Task ApplyThemeAsync()
        {
            if (_isDarkMode)
            {
                await _jsRuntime.InvokeVoidAsync("document.body.classList.add", "dark-mode");
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("document.body.classList.remove", "dark-mode");
            }
        }
    }
}
