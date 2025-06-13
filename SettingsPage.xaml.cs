using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatBy
{
    public partial class SettingsPage : ContentPage, INotifyPropertyChanged
    {
        private string _baseUrl;
        private string _apiKey;
        private string _model;

        public string BaseUrl
        {
            get => _baseUrl;
            set
            {
                if (_baseUrl != value)
                {
                    _baseUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (_apiKey != value)
                {
                    _apiKey = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
            BindingContext = this;
        }

        private void LoadSettings()
        {
            // 从应用程序设置加载保存的值
            BaseUrl = Preferences.Get("BaseUrl", API.BaseUrl);
            ApiKey = Preferences.Get("ApiKey", API.ApiKey);
            Model = Preferences.Get("Model", API.Model);
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // 保存到静态API类
            API.BaseUrl = BaseUrl;
            API.ApiKey = ApiKey;
            API.Model = Model;

            // 保存到应用程序设置
            Preferences.Set("BaseUrl", BaseUrl);
            Preferences.Set("ApiKey", ApiKey);
            Preferences.Set("Model", Model);

            StatusLabel.Text = "设置已保存！";
            StatusLabel.TextColor = Colors.Green;
        }

        private async void OnTestConnectionClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "测试中...";
            StatusLabel.TextColor = Colors.Orange;

            API api = new API();
            string result = api.getRespone(BaseUrl, ApiKey, Model);

            if (result.StartsWith("Error:"))
            {
                StatusLabel.Text = "连接失败: " + result;
                StatusLabel.TextColor = Colors.Red;
            }
            else
            {
                StatusLabel.Text = "连接成功!";
                StatusLabel.TextColor = Colors.Green;
            }
        }
    }
}