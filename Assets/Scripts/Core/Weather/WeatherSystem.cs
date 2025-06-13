using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeatherSystem : MonoBehaviour
{
    [Header("City for weather lookup")]
    public string city = "Seoul";  // ← Inspector에서 입력된 값 사용

    public Weather_Controller weatherController;

    void Start()
    {
        StartCoroutine(GetWeatherAndApply());
    }

    IEnumerator GetWeatherAndApply()
    {
        string encodedCity = UnityWebRequest.EscapeURL(city);
        string weatherApiUrl = $"http://localhost:8080/api/weather/current?city={encodedCity}";

        UnityWebRequest www = UnityWebRequest.Get(weatherApiUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            WeatherResponse response = JsonUtility.FromJson<WeatherResponse>(www.downloadHandler.text);
            Debug.Log("Current Weather: " + response.main);
            ApplyWeather(response.main);
        }
        else
        {
            Debug.LogError("Failed to get weather: " + www.error);
        }
    }

    void ApplyWeather(string main)
    {
        if (main.Contains("Rain"))
            weatherController.UseWeatherTypeDebug((int)Weather_Controller.WeatherType.RAIN);
        else if (main.Contains("Snow"))
            weatherController.UseWeatherTypeDebug((int)Weather_Controller.WeatherType.SNOW);
        else if (main.Contains("Cloud"))
            weatherController.UseWeatherTypeDebug((int)Weather_Controller.WeatherType.CLOUDY);
        else
            weatherController.UseWeatherTypeDebug((int)Weather_Controller.WeatherType.SUN);
    }

    [System.Serializable]
    public class WeatherResponse
    {
        public string main;
        public string description;
        public float temp;
        public float feelsLike;
        public int humidity;
    }
}
