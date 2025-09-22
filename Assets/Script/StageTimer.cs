using UnityEngine;
using UnityEngine.UI;

public class StageTimer : MonoBehaviour
{
    public float elapsedTime { get; private set; } = 0f;
    public bool isRunning { get; private set; } = false;
    [SerializeField] private Text timerText;

    void Update()
    {
        if (!isRunning) return;
        elapsedTime += Time.deltaTime;
        if (timerText != null)
            timerText.text = elapsedTime.ToString("F2") + "•b";
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
