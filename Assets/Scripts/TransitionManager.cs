using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public static TransitionManager instance;
    public Animator crossfade;
    public static float transitionTime = .5f;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }

    public void FadeOut()
    {
        crossfade.SetTrigger("StartScene");
    }

    public void FadeIn()
    {
        crossfade.SetTrigger("EndScene");
    }


}
