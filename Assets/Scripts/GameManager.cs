using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UITimer uiTimer;
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        uiTimer.SetTime(elapsedTime);
    }
}
