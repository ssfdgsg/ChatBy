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
            // ��Ӧ�ó������ü��ر����ֵ
            BaseUrl = Preferences.Get("BaseUrl", API.BaseUrl);
            ApiKey = Preferences.Get("ApiKey", API.ApiKey);
            Model = Preferences.Get("Model", API.Model);
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // ���浽��̬API��
            API.BaseUrl = BaseUrl;
            API.ApiKey = ApiKey;
            API.Model = Model;

            // ���浽Ӧ�ó�������
            Preferences.Set("BaseUrl", BaseUrl);
            Preferences.Set("ApiKey", ApiKey);
            Preferences.Set("Model", Model);

            StatusLabel.Text = "�����ѱ��棡";
            StatusLabel.TextColor = Colors.Green;
        }

        private async void OnTestConnectionClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "������...";
            StatusLabel.TextColor = Colors.Orange;

            API api = new API();
            string result = api.getRespone(BaseUrl, ApiKey, Model);

            if (result.StartsWith("Error:"))
            {
                StatusLabel.Text = "����ʧ��: " + result;
                StatusLabel.TextColor = Colors.Red;
            }
            else
            {
                StatusLabel.Text = "���ӳɹ�!";
                StatusLabel.TextColor = Colors.Green;
            }
        }
    }
}