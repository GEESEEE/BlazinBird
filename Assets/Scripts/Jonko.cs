using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jonko : MonoBehaviour
{

    public Transform playerBodyTransform;
    public Transform jonkoTransform;
    public float initialOffsetX = 1.04f;
    public float initialOffsetY = -0.29f;

    public GameObject jonko;
    public float jonkoRotationOffset = 305;
    public PolygonCollider2D jonkoCollider;
    public Rigidbody2D playerRB;

    void Awake()
    {
        SetPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") 
        {
            GameEvents.current.onObstacleHit += InitializeCollider;
        }
        
        SetPosition();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameHasEnded) {
            SetPosition();
        } 
    }

    void SetPosition() 
    {
        float angle = -playerBodyTransform.eulerAngles.z * Mathf.PI / 180;
        float finalOffsetX = initialOffsetX * Mathf.Cos(angle) + initialOffsetY * Mathf.Sin(angle); 
        float finalOffsetY = -initialOffsetX * Mathf.Sin(angle) + initialOffsetY * Mathf.Cos(angle);

        jonkoTransform.position = playerBodyTransform.position + new Vector3(finalOffsetX, finalOffsetY, 0f) ;
        jonkoTransform.eulerAngles = playerBodyTransform.eulerAngles + new Vector3(0f, 0f, jonkoRotationOffset);
    }

    void InitializeCollider()
    {
        Rigidbody2D jonkoRB = jonko.AddComponent<Rigidbody2D>();
        jonkoRB.mass = 0.1f;
        jonkoRB.gravityScale = 1.5f;
        jonkoRB.velocity = playerRB.velocity;
        jonkoCollider.isTrigger = false;
    }

    void OnDestroy() {
        if (SceneManager.GetActiveScene().name == "Game") 
        {
            GameEvents.current.onObstacleHit -= InitializeCollider;
        }

    }

}
