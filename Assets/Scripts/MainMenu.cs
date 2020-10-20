using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    #endregion

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        quitButton.onClick.AddListener(QuitGame);

        backButton.onClick.AddListener(ShowMainMenu);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        fpsToggle.onValueChanged.AddListener((value) => {ToggleFPSCounter(value); });

        InitializeVolume();
        InitializeFPSCounter();
        
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
