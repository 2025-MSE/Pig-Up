using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherSystem : MonoBehaviour
{
    [Header("City for weather lookup")]
    public string city = "Seoul";

    private IEnumerator Start()
    {
        yield return ApplyWeatherBasedOnAPI();
    }

    private IEnumerator ApplyWeatherBasedOnAPI()
    {
        // 날씨 API 비동기 요청
        Task<string> weatherTask = GetWeatherDescription(city);
        while (!weatherTask.IsCompleted)
            yield return null;

        string description = weatherTask.Result;
        Debug.Log("Current Weather: " + description);

        // 날씨 description을 enum 코드로 변환
        int weatherCode = GetWeatherTypeCode(description);

        // 날씨 파티클 적용
        Weather_Controller controller = GetComponent<Weather_Controller>();
        if (controller != null)
        {
            controller.UseWeatherTypeDebug(weatherCode);
        }
        else
        {
            Debug.LogWarning("Weather_Controller component not found on this GameObject.");
        }
    }

    private int GetWeatherTypeCode(string description)
    {
        description = description.ToLower();

        if (description.Contains("clear"))
            return (int)Weather_Controller.WeatherType.SUN;
        if (description.Contains("cloud"))
            return (int)Weather_Controller.WeatherType.CLOUDY;
        if (description.Contains("rain"))
            return (int)Weather_Controller.WeatherType.RAIN;
        if (description.Contains("thunder"))
            return (int)Weather_Controller.WeatherType.THUNDERSTORM;
        if (description.Contains("snow"))
            return (int)Weather_Controller.WeatherType.SNOW;

        return (int)Weather_Controller.WeatherType.SUN; // default fallback
    }

    public async Task<string> GetWeatherDescription(string city)
    {
        string url = $"http://localhost:8080/api/weather/{city}";
        UnityWebRequest www = UnityWebRequest.Get(url);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            WeatherResponse response = JsonUtility.FromJson<WeatherResponse>(json);
            return response.description;
        }

        Debug.LogWarning("Failed to fetch weather data. Using default 'clear'.");
        return "clear";
    }

    [System.Serializable]
    public class WeatherResponse
    {
        public string description;
        public float temperature;
        public int humidity;
    }
}
