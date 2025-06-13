using System.Text;
using System.Text.Json;

namespace ChatBy
{
    internal class API
    {
        public static string BaseUrl = "";
        public static string ApiKey = "";
        public static string Model = ""; // Default model, can be changed by the user

        public String getRespone(String BaseUrl, String ApiKey, String Model)
        {
            String responseContent = "Waiting...";
            if (String.IsNullOrEmpty(BaseUrl) || String.IsNullOrEmpty(ApiKey) || String.IsNullOrEmpty(Model))
            {
                return "Please set BaseUrl, ApiKey, and Model first.";
            }

            var client = new HttpClient();
            var requestUrl = $"{BaseUrl.TrimEnd('/')}/v1";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {ApiKey}");

            try
            {
                var response = client.Send(request);
                responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                responseContent = $"Error: {ex.Message}";
            }

            // Return the response content as a string
            return responseContent;
        }

        public async Task<string> SendMessageAsync(string prompt)
        {
            try
            {
                if (string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(Model))
                {
                    return "Please set BaseUrl, ApiKey, and Model first.";
                }
                var client = new HttpClient();
                var requestUrl = $"{BaseUrl.TrimEnd('/')}/v1/chat/completions";
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("Authorization", $"Bearer {ApiKey}");
                request.Content = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        model = Model,
                        messages = new[]
                        {
                            new { role = "user", content = prompt }
                        }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    if (jsonResponse.TryGetProperty("choices", out var choices) && choices[0].TryGetProperty("message", out var message))
                    {
                        return message.GetProperty("content").GetString() ?? "No content returned.";
                    }
                    return "No valid response from API.";
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
