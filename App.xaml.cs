namespace ChatBy
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // 加载保存的设置
            LoadApiSettings();
            MainPage = new AppShell();
        }

        private void LoadApiSettings()
        {
            API.BaseUrl = Preferences.Get("BaseUrl", "");
            API.ApiKey = Preferences.Get("ApiKey", "");
            API.Model = Preferences.Get("Model", "");
        }
    }
}