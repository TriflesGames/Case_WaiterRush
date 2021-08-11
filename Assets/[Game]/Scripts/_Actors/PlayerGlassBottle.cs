using DG.Tweening;
using Game.Actors;
using System.Collections.Generic;
using TriflesGames.Managers;
using UnityEngine;

public class PlayerGlassBottle : MonoBehaviour
{
    public LayerMask targetMask;

    public WaterDrop waterDrop;
    public ParticleSystem dropParticleSystem;
    public ParticleSystem emptyParticle;
    public ParticleSystem takeParticlePrefab;

    private float waveSpeed = 100;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private bool isAvailableForDecrement;
    private RaycastHit hit;

    private LiquidMeshActor liquidMeshActor;

    private bool isStopWave;

    [Space(20)]
    public List<SkinnedMeshRenderer> waterPieces = new List<SkinnedMeshRenderer>();
    public int currentWaterPieceIndex;

    private void Awake()
    {
        gameObject.tag = "WaterItem";
    }

    private GameLevelActor gameLevelActor;
    Sequence sequence;

    void Start()
    {
        gameLevelActor = ((GameLevelActor)LevelManager.Instance.levelActor);
        isAvailableForDecrement = true;
        sequence = DOTween.Sequence();
    }

    

    public void IncrementWater(float unit, Color color)
    {
        if (currentWaterPieceIndex >= 10) return;

        #region particle1
        ParticleSystem particleSystemTemp = Instantiate(takeParticlePrefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);
        ParticleSystem.MainModule particleSystemMainTemp = particleSystemTemp.main;
        particleSystemMainTemp.startColor = color;
        #endregion

        sequence.Kill(true);
        SkinnedMeshRenderer skinnedMeshRendererTemp = waterPieces[currentWaterPieceIndex];

        skinnedMeshRendererTemp.material.color = color;
        skinnedMeshRendererTemp.gameObject.SetActive(true);

        float currentWaterUnit = skinnedMeshRendererTemp.GetBlendShapeWeight(0);
        float nextWaterUnit = 100;

        skinnedMeshRendererTemp.GetComponent<LiquidMeshActor>().isWaveActive = true;
        sequence.Append(DOTween.To(() => currentWaterUnit, x => currentWaterUnit = x, nextWaterUnit, 0.4f).SetEase(Ease.Linear).OnUpdate(() =>
        {   
            skinnedMeshRendererTemp.SetBlendShapeWeight(0, currentWaterUnit);

        }).OnComplete(()=> { 
        

        
        }).SetLink(gameObject));
        
        currentWaterPieceIndex++;
    }

    public void DecrementWater(float unit)
    {
        if (GameManager.Instance.IsGameOver) return;
        if (!isAvailableForDecrement) return;

        isAvailableForDecrement = false;
        Invoke("AvailableDecrement", 0.4f);

        FindObjectOfType<PlayerActor>().Hit();

        if (currentWaterPieceIndex <= 0)
        {
            gameLevelActor.FinishLevel(false);
            return;
            
        }

        sequence.Kill(true);
        currentWaterPieceIndex--;
        SkinnedMeshRenderer skinnedMeshRendererTemp = waterPieces[currentWaterPieceIndex];

        #region particle1
        ParticleSystem particleSystemTemp = Instantiate(dropParticleSystem, transform.position, Quaternion.Euler(-80, Random.Range(0, 360), 0), transform);
        ParticleSystem.MainModule particleSystemMainTemp = particleSystemTemp.main;
        particleSystemMainTemp.startColor = skinnedMeshRendererTemp.material.color;
        #endregion

        float currentWaterUnit = skinnedMeshRendererTemp.GetBlendShapeWeight(0);
        float nextWaterUnit = 0;
        sequence.Append(DOTween.To(() => currentWaterUnit, x => currentWaterUnit = x, nextWaterUnit, 0.4f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            skinnedMeshRendererTemp.SetBlendShapeWeight(0, currentWaterUnit);
        }).OnComplete(() =>
        {
            skinnedMeshRendererTemp.SetBlendShapeWeight(0, 0);
            skinnedMeshRendererTemp.GetComponent<LiquidMeshActor>().isWaveActive = false;
            skinnedMeshRendererTemp.gameObject.SetActive(false);
        }).SetLink(gameObject));
    }

    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.MainModule mainModule;
    public void Empty(float emptySecond)
    {
        StopWave();

        if (GetColorList().Count == 0) { return; }

        emissionModule = emptyParticle.emission;
        emissionModule.rateOverTime = 50;

        mainModule = emptyParticle.main;

        NextWaterPiece(currentWaterPieceIndex - 1);
    }

    void NextWaterPiece(int index)
    {
        float currentWaterUnit = waterPieces[index].GetBlendShapeWeight(0);

        mainModule.startColor = waterPieces[index].material.color;
        DOTween.To(() => currentWaterUnit, x => currentWaterUnit = x, 0, 0.2f).OnUpdate(() =>
        {
            waterPieces[index].SetBlendShapeWeight(0, currentWaterUnit);
        }).OnComplete(() =>
        {
            waterPieces[index].gameObject.SetActive(false);
            index--;

            if (index >= 0)
            {
                NextWaterPiece(index);
            }
            else
            {
                emissionModule.rateOverTime = 0;
            }

        }).SetLink(gameObject);
    }

    void AvailableDecrement()
    {
        isAvailableForDecrement = true;
    }

    public float GetWater()
    {
        return 100;//Mathf.Abs(110 - skinnedMeshRenderer.GetBlendShapeWeight(0));
    }

    public List<Color> GetColorList()
    {
        List<Color> newList = new List<Color>();
        for (int i = 0; i < currentWaterPieceIndex; i++)
        {
            newList.Add(waterPieces[i].material.color);
        }

        return newList;
    }

    #region Wave

    public void StopWave()
    {
        isStopWave = true;
    }

    public void NewLiquidValue(float newWaveValue)
    {
        liquidMeshActor.waveHeight = newWaveValue;
    }

    private void PlatformMoveUp()
    {
        if (isStopWave) return;

        float lastWeight = 0;
        DOTween.To(() => lastWeight, x => lastWeight = x, 100, waveSpeed).SetSpeedBased().OnUpdate(() =>
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, lastWeight);

        }).OnComplete(() =>
        {
            Invoke("PlatformMoveDown", 0);
        }).SetLink(gameObject);
    }

    private void PlatformMoveDown()
    {
        if (isStopWave) return;

        float lastWeight = skinnedMeshRenderer.GetBlendShapeWeight(1);
        DOTween.To(() => lastWeight, x => lastWeight = x, 0, waveSpeed * 2).SetSpeedBased().OnUpdate(() =>
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, lastWeight);

        }).OnComplete(() =>
        {

            Invoke("PlatformMoveUp", 0);
        }).SetLink(gameObject);
    }

    #endregion
}
