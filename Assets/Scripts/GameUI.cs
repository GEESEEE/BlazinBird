using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public Text scoreText;

    public GameObject deathScreen;
    public Animator deathScreenAnimator;
    public Text finalScoreText;
    public Text highScoreText;

    public Button restartButton;
    public Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        deathScreen.SetActive(false);
        GameEvents.current.startGame += UpdateScoreText;
        GameEvents.current.passedObstacle += UpdateScoreText;
        GameEvents.current.onObstacleHit += showDeathScreen;
    }



    public void UpdateScoreText()
    {
        scoreText.text = GameManager.instance.score.ToString();
    }

    public void showDeathScreen()
    {
        deathScreen.SetActive(true);
        scoreText.text = "";
        finalScoreText.text = GameManager.instance.score.ToString();

        if (GameManager.instance.score > PlayerPrefs.GetInt("Highscore", 0))
        {
            highScoreText.text = GameManager.instance.score.ToString();
            PlayerPrefs.SetInt("Highscore", GameManager.instance.score);
        }
        else
        {
            highScoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        }
        deathScreenAnimator.SetTrigger("GameHasEnded");

        restartButton.onClick.AddListener(() => { AudioManager.instance.Play("ClickButton"); 
                                                fadeDeathScreen();
                                                GameManager.instance.PlayGame();
                                                DisableButtons(); });

        mainMenuButton.onClick.AddListener(() => { AudioManager.instance.Play("ClickButton"); 
                                                fadeDeathScreen();
                                                GameManager.instance.GoToMainMenu();
                                                DisableButtons(); });
    }

    public void fadeDeathScreen()
    {
        deathScreenAnimator.SetBool("FadeDeathscreen", true);
    }

    public void DisableButtons()
    {
        restartButton.interactable = false;
        mainMenuButton.interactable = false;
    }


    private void OnDestroy()
    {
        GameEvents.current.startGame -= UpdateScoreText;
        GameEvents.current.passedObstacle -= UpdateScoreText;
        GameEvents.current.onObstacleHit -= showDeathScreen;

    }
}
