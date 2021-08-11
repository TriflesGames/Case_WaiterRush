using Game.Actors;
using TriflesGames.Managers;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    private byte totalNumberReceived;

    PlayerActor playerActor;
    void Start()
    {
        playerActor = GetComponentInParent<PlayerActor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.IsGameOver) return;


        if (other.CompareTag("ObstacleItem"))
        {
            if (other.GetComponentInParent<Obstacle4>() != null)
            {
                other.GetComponentInParent<Obstacle4>().DeactiveCollider();
            }
            else if (other.GetComponentInParent<Obstacle1>() != null)
            {
                other.GetComponentInParent<Obstacle1>().DeactiveCollider();
            }
            else if (other.GetComponentInParent<Obstacle2>() != null)
            {
                other.GetComponentInParent<Obstacle2>().DeactiveCollider();
            }
            else if (other.GetComponentInParent<Obstacle3>() != null)
            {
                other.GetComponentInParent<Obstacle3>().DeactiveCollider();
            }

            playerActor.GetComponent<PlayerMoveActor>().AddForce(-Vector3.forward);

            if (lastCollider != other.gameObject)
            {
                lastCollider = other.gameObject;
                totalNumberReceived = 0;

                playerActor.DecrementWater(20);

                StartCoroutine(playerActor.Crash());

                CameraManager.Instance.Shake();
            }
        }
        else
            if (other.CompareTag("WaterItem"))
        {
            IncrementPerTakedWaterCount();

            other.GetComponent<UnitGlassBottle>().CustomDestroy();
            playerActor.IncrementWater(10, other.GetComponent<UnitGlassBottle>().GetColor());
        }
    }

    private GameObject lastCollider = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.IsGameOver) return;

        
        if (collision.gameObject.CompareTag("Finish"))
        {
            FindObjectOfType<GameLevelActor>().FinishPlatform();
        }
        else if (collision.gameObject.CompareTag("Faster"))
        {
            playerActor.BoostActive(true);
        }
        /* else if (collision.gameObject.CompareTag("WaterItem"))
         {
             IncrementPerTakedWaterCount();

             collision.gameObject.GetComponent<UnitGlassBottle>().CustomDestroy();
             playerActor.IncrementWater(10);
         }*/
    }


    private void IncrementPerTakedWaterCount()
    {
        totalNumberReceived++;

        if (totalNumberReceived == 7)
        {
            playerActor.BoostActive();
            totalNumberReceived = 0;
        }
    }
}
