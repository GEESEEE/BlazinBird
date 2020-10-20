using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    // Start is called before the first frame update
    private void Awake() {
        current = this;
    }

    public event Action startGame;
    public void StartGame()
    {
        if (startGame != null) 
        {
            startGame();
        }
    }

    public event Action jump;
    public void Jump()
    {
        if (jump != null)
        {
            jump();
        }
    }

    public event Action passedObstacle;
    public void PassedObstacle()
    {
        if (passedObstacle != null)
        {
            passedObstacle();
        }
    }



    // Update is called once per frame
    public event Action onObstacleHit;
    public void ObstacleHit()
    {
        if (onObstacleHit != null)
        {
            onObstacleHit();
        }
    }

}
