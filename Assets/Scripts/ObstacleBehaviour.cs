using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{

    public float speed = 3.0f;
    public Rigidbody2D rb;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(-speed, 0);
        GameEvents.current.onObstacleHit += FreezeObstacle;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < ScreenBounds.bounds.x * -2) {
            Destroy(parent);
        }
    }

    private void FreezeObstacle()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnDestroy() {
        GameEvents.current.onObstacleHit -= FreezeObstacle;
    }
}
