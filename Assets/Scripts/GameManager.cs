using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public bool gameHasStarted = false;
    public bool gameHasEnded = false;

    public int score;



    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("MainMenu");
        TransitionManager.instance.FadeIn();
    }

    public void PlayGame()
    {
        StartCoroutine( Load("Game"));
    }
    public void StartGame () 
    {
        if (!gameHasStarted) 
        {
            gameHasStarted = true;
            score = 0;
            GameEvents.current.StartGame();
        }
    }

    public void PassedObstacle() 
    {
        if (!gameHasEnded) 
        {
            score += 1;
            GameEvents.current.PassedObstacle();
        }
    }

    public void EndGame()
    {
        if (!gameHasEnded) 
        {
            gameHasEnded = true;
            GameEvents.current.ObstacleHit();
        }
        
    }

    public void GoToMainMenu()
    {
        StartCoroutine( Load("MainMenu"));
    }

    IEnumerator Load(string sceneName)
    {
        TransitionManager.instance.FadeOut() ;

        yield return new WaitForSeconds(TransitionManager.transitionTime);

        SceneManager.LoadScene(sceneName);

        gameHasEnded = false;
        gameHasStarted = false;

        TransitionManager.instance.FadeIn() ;
    }


}
