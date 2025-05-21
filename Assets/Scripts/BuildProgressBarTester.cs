using UnityEngine;

public class BuildProgressBarTester : MonoBehaviour
{
    [SerializeField] private BuildProgressBar progressBar;
    private float progress;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            progress += Time.deltaTime * 0.2f;
            progressBar.SetProgress(progress);
        }
    }
}
