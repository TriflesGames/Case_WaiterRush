using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Obstacle3 : ItemProperties
{
    [Header("Manuel Settings")]
    public float delaySecond;

    [Header("Custom")]
    public float second;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private CapsuleCollider capsuleCollider;

    private Sequence sequence;

    [System.Serializable]
    public class ThemeProperties
    {
        public Material mat1;
        public Material mat2;
    }

    public ThemeProperties[] themeMats;

    private void Awake()
    {
        tag = "ObstacleItem";

        GetComponent<GameState>().FillGameStart(() =>
        {
            
                Invoke("PlatformMoveUpManuel", delaySecond);
            
        });
    }

    void Start()
    {
        sequence = DOTween.Sequence();

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        capsuleCollider = skinnedMeshRenderer.GetComponent<CapsuleCollider>();

        GetThemeCount();
    }

    private void GetThemeCount()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat1;
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;

    }

    public void DeactiveCollider()
    {
        foreach (Collider item in GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }
    }

    #region Manuel
    private void PlatformMoveUpManuel()
    {
        capsuleCollider.enabled = true;

        float lastWeight = 0;
        sequence.Kill();
        sequence.Append(DOTween.To(() => lastWeight, x => lastWeight = x, 150, second).SetEase(Ease.Unset).OnUpdate(() =>
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, lastWeight);

        }).OnComplete(() =>
        {

            Invoke("PlatformMoveDownManuel", 1);
        }).SetLink(gameObject));
    }

    private void PlatformMoveDownManuel()
    {
        float lastWeight = skinnedMeshRenderer.GetBlendShapeWeight(0);
        sequence.Kill();
        sequence.Append(DOTween.To(() => lastWeight, x => lastWeight = x, 0, second * 2).SetEase(Ease.Unset).OnUpdate(() =>
        {
            if (lastWeight < 15)
            {
                capsuleCollider.enabled = false;
            }
            skinnedMeshRenderer.SetBlendShapeWeight(0, lastWeight);

        }).OnComplete(() =>
        {
            Invoke("PlatformMoveUpManuel", 1);
        }).SetLink(gameObject));
    }
    #endregion
}
