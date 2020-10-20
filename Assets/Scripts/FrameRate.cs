using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour
{

    public static GameObject instance;
    private int frameCounter = 0;
    private float timeCounter = 0.0f;
    private float refreshTime = 0.1f;
    public Text frameRateText;

    private void Start()
    {
        instance = gameObject;
        float OffsetX = Screen.width / 50;
        float OffsetY = Screen.height / 70;
        frameRateText.transform.position = new Vector3((ScreenBounds.bounds.x + frameRateText.rectTransform.rect.width + OffsetX)
                                                       , -(ScreenBounds.bounds.y + frameRateText.rectTransform.rect.height + OffsetY) + Screen.height
                                                       , 0f);
    }

    void Update()
    {
        

        if (timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
                frameCounter++;
        } else
        {
            float frameRate = frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0.0f;
            frameRateText.text = frameRate.ToString("0");
        }
    }
}
