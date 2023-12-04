using System.Text.Json;

namespace Work.Tests;

public class WhatsWrongService
{
    // Settings name is unspecific enough, but it could be just me always complaining about names :)
    private readonly Settings _demoSettings;

    public WhatsWrongService(Settings demoSettings)
    {
        _demoSettings = demoSettings;
    }

    // name should be GetDemoModelsAsync
    // cancellation token parameter
    public async Task<Model[]> GetDemoModels()
    {
        // httpClient should be generated via HttpClientFactory and then Dependency Injected,
        // to prevent socket exhaustion
        // can be switched to using declaration
        using (var httpClient = new HttpClient())
        {
            // use await over .Result
            // pass cancellation token
            var response = httpClient.GetStringAsync(_demoSettings.DemoServiceUrl).Result;

            // do an String.IsNullOrWhiteSpace and then throw Exception "Result/Response was empty"
            // otherwise, wrap in try catch
            return JsonSerializer.Deserialize<Model[]>(response)
                   ?? throw new Exception("Result was empty.");
        }
    }
    
    // name should be UpdateDemoModelAsync
    // cancellation token parameter
    public async void UpdateDemoModel(string json)
    {
        // as before, HttpClientFactory and DI
        // can be switched to using declaration
        using (var httpClient = new HttpClient())
        {
            var stringContent = new StringContent(json);
            // missing await and CancellationToken
            httpClient.PutAsync(_demoSettings.DemoServiceUrl, stringContent);
        }
    }
}


public class Settings
{
    public String DemoServiceUrl { get; set; }
}

public class Model
{
    
}