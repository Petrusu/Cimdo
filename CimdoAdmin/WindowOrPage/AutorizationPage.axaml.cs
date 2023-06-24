using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CimdoAdmin.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CimdoAdmin;

public partial class AutorizationPage : UserControl
{
    
    public AutorizationPage()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        token = this.FindControl<TextBlock>("token"); 
        LogInBox = this.Find<TextBox>("LogInBox");
        PasswordBox = this.Find<TextBox>("PasswordBox");
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private async void LogIn_OnClick(object? sender, RoutedEventArgs e)
    {
        // Создаем экземпляр HttpClient
        using (HttpClient client = new HttpClient())
        {
            string login = LogInBox.Text;
            string password = PasswordBox.Text;

            var requestData = new
            {
                login,
                password
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Отправляем POST-запрос к локальному API
            HttpResponseMessage response =
                await client.PostAsync("http://localhost:5128/login", content);

            // Проверяем успешность запроса
            if (response.IsSuccessStatusCode)
            {
                // Читаем содержимое ответа
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                token.Text = responseObject.Token;
            }
            else
            {
                token.Text = "Ошибка: " + response.StatusCode;
            }
        }


    }
}