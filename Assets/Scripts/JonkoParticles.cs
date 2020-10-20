using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JonkoParticles : MonoBehaviour
{

    public ParticleSystem smoke;
    public ParticleSystem fire;
    public ParticleSystem perpetualSmoke;
    public int smokeParticleCount = 5;
    public int fireParticleCount = 3;

    public Transform particleSystemTransform;
    public Transform jonkoTransform;
    public float jonkoPofDelay = 1f;
    public float OffsetX = 0.5f;
    public float OffsetY = -0.2f;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") 
        {
            GameEvents.current.jump += MakeSmoke;
            GameEvents.current.jump += MakeFire;
            GameEvents.current.onObstacleHit += PerpetualSmoke;
        }
        
        StartCoroutine(smokeBeforeGameStarts());
    }

    // Update is called once per frame
    void Update()
    {

        float angle = -jonkoTransform.eulerAngles.z * Mathf.PI / 180;
        float finalOffsetX = OffsetX * Mathf.Cos(angle) + OffsetY * Mathf.Sin(angle);
        float finalOffsetY = -OffsetX * Mathf.Sin(angle) + OffsetY * Mathf.Cos(angle);

        particleSystemTransform.position = jonkoTransform.position + new Vector3(finalOffsetX, finalOffsetY, 0f);
       
    }

    IEnumerator smokeBeforeGameStarts()
    {
        yield return new WaitForSeconds(1);
        while (!GameManager.instance.gameHasStarted)
        {
            MakeSmoke();
            MakeFire();
            yield return new WaitForSeconds(jonkoPofDelay);
        }
    }

    private void MakeSmoke()
    {
        var emitParams = new ParticleSystem.EmitParams();
        smoke.Emit(emitParams, smokeParticleCount); 
    }

    private void MakeFire()
    {
        var emitParams = new ParticleSystem.EmitParams();
        fire.Emit(emitParams, fireParticleCount);
    }

    private void PerpetualSmoke()
    {
        Destroy(fire);
        Destroy(smoke);
        perpetualSmoke.Play();
    }

    void OnDestroy()
    {
        if (SceneManager.GetActiveScene().name == "Game") 
        {
            GameEvents.current.jump -= MakeSmoke;
            GameEvents.current.jump -= MakeFire;
            GameEvents.current.onObstacleHit -= PerpetualSmoke;

        }

    }
}
