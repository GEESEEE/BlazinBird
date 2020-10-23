using UnityEngine;
using UnityEngine.UI;

public class ScreenBounds
{

    public static Vector2 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    public static Vector2 safeBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.safeArea.width, Screen.safeArea.height, Camera.main.transform.position.z));


}

