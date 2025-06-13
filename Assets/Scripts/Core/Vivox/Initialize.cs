// using UnityEngine;
// using System;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Vivox;

// async void InitializeAsync()
// {
//     try
//     {
//         await UnityServices.InitializeAsync();
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();
//         await VivoxService.Instance.InitializeAsync();
//         Debug.Log("Vivox initialized successfully.");
//     }
//     catch (Exception e)
//     {
//         Debug.LogError($"Failed to initialize Vivox: {e.Message}");
//     }
// }
// public class Initialize : MonoBehaviour
// {
//     void Start()
//     {
//         InitializeAsync();
//     }
// }
