using PathCreation.Examples;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{

    protected private PlayerActor playerActor;
    protected private PathFollower pathFollower;
    protected private Animator animator;
    protected private PlayerMoveActor playerMoveActor;
    protected private PlayerGlassBottle playerGlassBottle;

    protected private CharacterController characterController;

    private void Awake()
    {
        playerActor = GetComponent<PlayerActor>();
        animator = GetComponentInChildren<Animator>();
        pathFollower = GetComponent<PathFollower>();
        characterController = GetComponent<CharacterController>();

        playerGlassBottle = GetComponentInChildren<PlayerGlassBottle>();

        playerMoveActor = GetComponentInChildren<PlayerMoveActor>();
    }
}
