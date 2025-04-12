using UnityEngine;

namespace MSE.Core
{
    public class ServerRef : MonoBehaviour
    {
#if UNITY_EDITOR
        public static readonly string BASE_URL = "http://localhost:8080";
#else
        public static readonly string BASE_URL = "http://localhost:8080";
#endif
    }
}
