using UnityEngine;
using PathCreation;
using Game.Actors;
using TriflesGames.Managers;
using System.Collections;
using System.Collections.Generic;

public class PlayerActor : PlayerComponent
{
    [HideInInspector]
    public bool CanMove;
    public float speed;
    [HideInInspector] public float _speed;

    [Header("BoostProperties")]
    public float boostSecond = 4;
    public float boosSpeedFactor = 1.3f;
    public GameObject boostParticlePrefab;


    [Header("Crash")]
    public GameObject txt_collisionPrefab;

    public void StartPosition(PathCreator pathCreator, float addStartDistance, float roadWith)
    {
        pathFollower.FillCreator(pathCreator, addStartDistance);
        playerMoveActor.FillRoadLenght(roadWith);
    }

    public void StartGame()
    {
        CanMove = true;
        SetSpeed(speed);
    }

    public void StopGame()
    {
        CanMove = false;
        SetSpeed(0);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        animator.SetFloat("speed", _speed);
    }

    public Transform GetCharacterModel()
    {
        return playerMoveActor.GetCharacterModel();
    }

    public void NewLiquidWave(float newWaveValue)
    {
        playerGlassBottle.NewLiquidValue(newWaveValue);
    }

    public IEnumerator Crash()
    {
        if (GameManager.Instance.IsGameOver) yield break;

        if (isBoost)
        {
            FindObjectOfType<GameLevelActor>().StopBoostEnvironment();
        }

        //Instantiate(txt_collisionPrefab, GetCharacterModel());

        SetSpeed(0);
        yield return new WaitForSeconds(0.4f);

        if (GameManager.Instance.IsGameOver) yield break;

        SetSpeed(speed);
    }

    public void Hit()
    {
        animator.SetTrigger("hit");
    }

    public void IncrementWater(float value, Color color)
    {
       // VibrationManager.Instance.TriggerLightImpact();
        playerGlassBottle.IncrementWater(value, color);
    }

    public void DecrementWater(float value)
    {
        VibrationManager.Instance.TriggerMediumImpact();
        playerGlassBottle.DecrementWater(value);
    }

    public void Empty(float emptySecond = 1.5f)
    {
        GetCharacterModel().LookAt(FindObjectOfType<BigGlassBottle>().CenterTransform, Vector3.up);
        StopGame();

        playerGlassBottle.Empty(emptySecond);
    }

    private bool isBoost;
    private GameObject boostEffectTemp;
    public void BoostActive(bool isJustSpeed = false)
    {
        if (isBoost) return;

        SetSpeed(speed * boosSpeedFactor);
        isBoost = true;

        if (!isJustSpeed)
        {
            boostEffectTemp = Instantiate(boostParticlePrefab, CameraManager.Instance.currentCamera.transform);
            CustomLevelManager.Instance.CreateBoostUI();
        }

        ((GameLevelActor)LevelManager.Instance.levelActor).StartBoostEnvironment();
    }

    public void BoostDeactive()
    {
        if (!isBoost) return;

        SetSpeed(speed);
        Destroy(boostEffectTemp);
        isBoost = false;

        CustomLevelManager.Instance.DestroyBoostUI();
    }

    public float GetTotalWater()
    {
        return playerGlassBottle.GetWater();
    }

    public Transform GetCharacterRotation()
    {
        return transform.Find("CharacterRotation");
    }

    public List<Color> GetColorList()
    {
        return playerGlassBottle.GetColorList();
    }
}
