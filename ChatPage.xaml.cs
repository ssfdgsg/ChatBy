namespace ChatBy
{
    public partial class ChatPage : ContentPage
    {
        private readonly API _api = new API();
        private bool _isBusy = false;

        public ChatPage()
        {
            InitializeComponent();
            CheckApiSettings();
        }

        private void CheckApiSettings()
        {
            // 从偏好设置加载API设置
            API.BaseUrl = Preferences.Get("BaseUrl", "");
            API.ApiKey = Preferences.Get("ApiKey", "");
            API.Model = Preferences.Get("Model", "");

            if (string.IsNullOrEmpty(API.BaseUrl) || string.IsNullOrEmpty(API.ApiKey) || string.IsNullOrEmpty(API.Model))
            {
                AddMessageBubble("请先在设置页面配置API参数", false);
            }
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            string message = MessageEntry.Text?.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            // 清空输入框
            string userMessage = MessageEntry.Text;
            MessageEntry.Text = string.Empty;

            // 添加用户消息到聊天界面
            AddMessageBubble(userMessage, true);

            _isBusy = true;

            // 添加AI思考中的提示
            Label thinkingLabel = new Label
            {
                Text = "正在思考...",
                TextColor = Colors.Gray,
                FontSize = 14,
                Margin = new Thickness(15, 0, 15, 0)
            };
            MessageContainer.Children.Add(thinkingLabel);

            // 滚动到底部
            await Task.Delay(100);
            await ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, true);

            // 发送请求到API
            string response = await _api.SendMessageAsync(userMessage);

            // 移除思考中的提示
            MessageContainer.Children.Remove(thinkingLabel);

            // 添加AI回复到聊天界面
            AddMessageBubble(response, false);
            _isBusy = false;

            // 滚动到底部
            await Task.Delay(100);
            await ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, true);
        }

        private void AddMessageBubble(string message, bool isUser)
        {
            Frame bubble = new Frame
            {
                CornerRadius = 10,
                Padding = new Thickness(10),
                BackgroundColor = isUser ? Color.FromArgb("#DCF8C6") : Color.FromArgb("#FFFFFF"),
                BorderColor = isUser ? Color.FromArgb("#DCF8C6") : Color.FromArgb("#E5E5E5"),
                HasShadow = false,
                Margin = isUser ? new Thickness(50, 5, 10, 5) : new Thickness(10, 5, 50, 5),
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start
            };

            bubble.Content = new Label
            {
                Text = message,
                TextColor = Colors.Black,
                FontSize = 16,
            };

            MessageContainer.Children.Add(bubble);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckApiSettings();
        }
    }
}