namespace ProductManagementSystem.Client
{
    /// <summary>
    /// Сервис для управления темой приложения (светлая/темная)
    /// </summary>
    public class ThemeService
    {
        private bool _isDarkMode = false;

        public event Action? OnThemeChanged;

        /// <summary>
        /// Получить текущее состояние темы
        /// </summary>
        public bool IsDarkMode => _isDarkMode;

        /// <summary>
        /// Получить текущий класс темы для CSS
        /// </summary>
        public string ThemeClass => _isDarkMode ? "dark-theme" : "light-theme";

        /// <summary>
        /// Переключить тему
        /// </summary>
        public void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            OnThemeChanged?.Invoke();
        }

        /// <summary>
        /// Установить темную тему
        /// </summary>
        public void SetDarkTheme()
        {
            if (!_isDarkMode)
            {
                _isDarkMode = true;
                OnThemeChanged?.Invoke();
            }
        }

        /// <summary>
        /// Установить светлую тему
        /// </summary>
        public void SetLightTheme()
        {
            if (_isDarkMode)
            {
                _isDarkMode = false;
                OnThemeChanged?.Invoke();
            }
        }
    }
}
