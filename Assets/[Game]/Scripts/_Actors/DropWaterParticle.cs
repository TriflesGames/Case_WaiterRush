using Game.Actors;
using System.Collections.Generic;
using UnityEngine;

public class DropWaterParticle : MonoBehaviour
{
    public SpriteRenderer splashParticlePrefab;
    bool isCompleted = false;

    /*public List<ParticleCollisionEvent> collisionEvents;

    private void OnParticleCollision(GameObject other)
    {
        if (isCompleted) { return; }

        if (other.CompareTag("Platform"))
        {
            isCompleted = true;
            GetComponent<ParticleSystem>().GetCollisionEvents(other)
        }
    }*/

    private ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;

    public List<ParticleCollisionEvent> collisionEvents;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        if (isCompleted) { return; }

        if (other.CompareTag("Platform"))
        {
            isCompleted = true;
            Vector3 pos = collisionEvents[0].intersection;

            SpriteRenderer spriteRendererTemp= Instantiate(splashParticlePrefab, pos, Quaternion.Euler(-90, 0, 0), FindObjectOfType<GameLevelActor>().transform);
            spriteRendererTemp.color = mainModule.startColor.color;
        }

       /* while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }*/
    }
}
