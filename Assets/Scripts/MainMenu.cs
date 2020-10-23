using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class MainMenu : MonoBehaviour
{
    #region Variables;
    [Header("Main Menu")]
    public GameObject mainMenu;
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
   
    [Space]
    [Header("Options Menu")]
    public GameObject optionsMenu;
    public Button backButton;

    public Slider volumeSlider;
    public float startVolume = 0f;

    public Toggle fpsToggle;
    public ParticleSystem FPSToggleParticles;
    public GameObject fpsCanvas;
    public GameObject fpsCanvasPrefab;

    public TMP_Dropdown fpsDropdown;
    private int[] fpsOptions;

    public Button fpsWarning;
    public TextMeshProUGUI fpsWarningText;
    public Animator fpsWarningAnimator;

    

    #endregion

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        fpsWarning.gameObject.SetActive(false);

        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        quitButton.onClick.AddListener(QuitGame);

        InitializeVolume();
        InitializeFPSCounter();
        InitializeFPSDropdown();

        backButton.onClick.AddListener(ShowMainMenu);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        fpsToggle.onValueChanged.AddListener((value) => {ToggleFPSCounter(value); });

        fpsDropdown.onValueChanged.AddListener((value) => { SetFrameRate(value);
                                                            showFPSWarning(); } );

    }

    public void InitializeVolume()
    {
        if (PlayerPrefs.GetFloat("MasterVolume", 420) == 420)
        {
            SetVolume(startVolume);
            volumeSlider.value = startVolume;
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
    }

    public void InitializeFPSCounter()
    {
    
        if (PlayerPrefs2.GetBool("FPSCounter", true))
        {
            if (FrameRate.instance == null) 
            {
                fpsCanvas = Instantiate(fpsCanvasPrefab) as GameObject;
                fpsCanvas.transform.SetParent(FindObjectOfType<Canvas>().transform);
            } else 
            {
                fpsCanvas = FrameRate.instance;
            }

        } else if (fpsCanvas != null) 
        {
            Destroy(fpsCanvas);
        }
    }

    public void InitializeFPSDropdown()
    {
        fpsDropdown.ClearOptions();

        int refreshRate = Screen.currentResolution.refreshRate;
        if (refreshRate == 0)
        {
            fpsOptions = new int[4]{30, 60, 90, 120};
            GameManager.instance.standardFPS = 30;
        } else 
        {
            GameManager.instance.standardFPS = -1;
            fpsOptions = new int[(int) refreshRate / 30];
            for (int i = 0; i * 30 + 30 <= refreshRate; i++)
            {
                fpsOptions[i] = i * 30 + 30;
            }
        }
       

        List<string> fpsStrings = new List<string>();
        for (int i = 0; i < fpsOptions.Length; i++)
        {
            fpsStrings.Add(fpsOptions[i].ToString());
        }

        fpsDropdown.AddOptions(fpsStrings);

        int fps = PlayerPrefs.GetInt("FrameRate", GameManager.instance.standardFPS);
        fpsDropdown.value = Array.IndexOf(fpsOptions, fps);
        GameManager.instance.SetFrameRate(fps);
    }

    #endregion

    #region Main Menu
    public void PlayGame()
    {
        AudioManager.instance.Play("ClickButton");
        DisableButtons();
        GameManager.instance.PlayGame();
    }

    public void ShowOptionsMenu()
    {
        AudioManager.instance.Play("ClickButton");
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        if (PlayerPrefs2.GetBool("FPSCounter", true)) 
        {
            fpsToggle.isOn = true;
            FPSToggleParticles.Play();

        } else {
            fpsToggle.isOn = false;
        }

    }

    public void SetFrameRate(int frameRate)
    {
        frameRate = fpsOptions[frameRate];
        if (frameRate == 30)
        {
            frameRate = GameManager.instance.standardFPS;
        }

        PlayerPrefs.SetInt("FrameRate", frameRate);
        GameManager.instance.SetFrameRate(frameRate);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game!");
        AudioManager.instance.Play("ClickButton");
        Application.Quit();
    }

    #endregion

    #region Options Menu

    public void SetVolume(float volume)
    {
        AudioManager.instance.mainMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void ToggleFPSCounter(bool isToggled)
    {
        AudioManager.instance.Play("ClickButton");
        if (isToggled) 
        {
            FPSToggleParticles.Play();

            PlayerPrefs2.SetBool("FPSCounter", true);
            if (fpsCanvas == null)
            {
                fpsCanvas = Instantiate(fpsCanvasPrefab) as GameObject;
                fpsCanvas.transform.SetParent(FindObjectOfType<Canvas>().transform);
            }

        } else {

            FPSToggleParticles.Stop();

            PlayerPrefs2.SetBool("FPSCounter", false);
            if (fpsCanvas != null) {
                Destroy(fpsCanvas);
            }
        }
    }

    public void showFPSWarning()
    {
        StartCoroutine(ShowFPSWarning());
    }

    IEnumerator ShowFPSWarning()
    {
        int refreshRate = Screen.currentResolution.refreshRate;
        if (refreshRate < fpsOptions[fpsDropdown.value]) 
        {
            fpsWarning.gameObject.SetActive(true);
            
            fpsWarningText.SetText("Frame rate should not be higher than Screen refresh rate " + 
                                    Environment.NewLine + Environment.NewLine + 
                                    " Refresh Rate: " + refreshRate);

            fpsWarningAnimator.SetTrigger("StartFPSWarning");
            yield return new WaitForSeconds(TransitionManager.transitionTime);
            fpsWarning.onClick.AddListener(removeFPSWarning);
        }
        
    }

    public void removeFPSWarning()
    {
        StartCoroutine( RemoveFPSWarning() );
    }

    IEnumerator RemoveFPSWarning()
    {
        fpsWarning.onClick.RemoveListener(removeFPSWarning);
        fpsWarningAnimator.SetTrigger("EndFPSWarning");
        yield return new WaitForSeconds(TransitionManager.transitionTime);
        fpsWarning.gameObject.SetActive(false);
    }


    public void ShowMainMenu()
    {
        AudioManager.instance.Play("ClickButton");
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    #endregion

    public void DisableButtons()
    {
        playButton.interactable = false;
        optionsButton.interactable = false;
        quitButton.interactable = false;
        backButton.interactable = false;
    }


}
