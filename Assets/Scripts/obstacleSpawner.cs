using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject floorFirePrefab;

    public float obstacleRespawnTime = 5.0f;
    public float floorRespawnTime = 3.0f;
    public float floorYOffset;
    public float floorXOffset = 6f;

    private bool spawningObstacles;

    // Start is called before the first frame update
    void Start()
    {
        spawningObstacles = false;

        GameEvents.current.startGame += startSpawning;
        GameEvents.current.onObstacleHit += stopSpawning;

        floorYOffset = -ScreenBounds.bounds.y * 0.91f;

        spawnInitialFloor();

        StartCoroutine(obstacleWave());
        StartCoroutine(floorSpawning());
    }

    void spawnInitialFloor()
    {
        for (float x = -floorXOffset; x <= floorXOffset; x = x + 3)
        {
            spawnFloorJonko(x, floorYOffset);
        }
    }

    IEnumerator obstacleWave()
    {
        while (true) {
            if (spawningObstacles) 
            {
                spawnObstacle();
            }
            yield return new WaitForSeconds(obstacleRespawnTime);
        }
    }

    IEnumerator floorSpawning()
    {
        while (true) {
            if (!GameManager.instance.gameHasEnded) {
                spawnFloorJonko(floorXOffset, floorYOffset);
            }
            yield return new WaitForSeconds(floorRespawnTime);
        }
    }

    private void spawnObstacle()
    {
        GameObject obstacle = Instantiate(obstaclePrefab) as GameObject;
        obstacle.transform.position = new Vector2(ScreenBounds.bounds.x * 2, Random.Range(-ScreenBounds.bounds.y / 3f, ScreenBounds.bounds.y / 2.5f)); 
    }

    void spawnFloorJonko(float x, float y)
    {
        GameObject floorJonko = Instantiate(floorFirePrefab) as GameObject;
        floorJonko.transform.position = new Vector2(x, y);
    }

    private void startSpawning()
    {
        spawningObstacles = true;
    }

    private void stopSpawning() 
    {
        spawningObstacles = false;
    }


}
