using DG.Tweening;
using Game.Actors;
using TriflesGames.Managers;
using UnityEngine;

[SelectionBase]
public class UnitGlassBottle : ItemProperties
{
    public ParticleSystem takeWaterParticle;

    private float waveSpeed = 100;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        gameObject.tag = "WaterItem";
    }

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        //PlatformMoveUp();

        ParticleSystem.MainModule main = takeWaterParticle.main;
        main.startColor = skinnedMeshRenderer.material.color;

        transform.DOMoveY(0.17f, 0);

        GetThemeCount();
    }

    private bool isTook;

    private void Update()
    {
        if (isTook)
        {
            transform.position = Vector3.MoveTowards(transform.position, FindObjectOfType<PlayerGlassBottle>().transform.position, 0.3f);
            //transform.DOLocalMove(FindObjectOfType<PlayerGlassBottle>().transform.position, 0.4f).SetEase(Ease.Unset);
        }
    }

    public void CustomDestroy()
    {
        isTook = true;
        //SpawnParticle();

        transform.DOScale(Vector3.one * 0.5f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Destroy(gameObject);

        }).SetLink(gameObject);

    }

    void SpawnParticle()
    {
        Instantiate(takeWaterParticle, transform.position, Quaternion.Euler(-80, 0, 0), transform.parent);
    }


    private void GetThemeCount()
    {
        // GetComponentInChildren<TrailRenderer>().startColor = CustomLevelManager.Instance.waterColor[CustomLevelManager.Instance.customGameData.GetThemeCount()];

        //skinnedMeshRenderer.material.color = CustomLevelManager.Instance.waterColor[CustomLevelManager.Instance.customGameData.GetThemeCount()];

        int rnd = Random.Range(0, CustomLevelManager.Instance.waterColor.Length);

        GetComponentInChildren<TrailRenderer>().startColor = CustomLevelManager.Instance.waterColor[rnd];
        skinnedMeshRenderer.material.color = CustomLevelManager.Instance.waterColor[rnd];
    }

    public Color GetColor()
    {
        return skinnedMeshRenderer.material.color;
    }

    #region Wave

    private void PlatformMoveUp()
    {

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
