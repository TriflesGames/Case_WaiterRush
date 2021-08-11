using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Obstacle2 : MonoBehaviour
{
    [Header("Manuel Settings")]
    public float delaySecond;
    public bool isInverse;

    [Header("Custom")]
    public float hitSpeed;
    public float loopSecond;

    public Ease ease;

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
            Invoke("LoopManuel", delaySecond);         
        });
    }

    private void Start()
    {
        GetThemeCount();
    }

    private void LoopManuel()
    {
        if (isInverse)
        {
            transform.DOMove(transform.position - transform.right, hitSpeed).SetEase(ease).OnComplete(() =>
            {
                transform.DOMove(transform.position + transform.right, hitSpeed).OnComplete(() =>
                {
                    Invoke("LoopManuel", delaySecond);
                }).SetLink(gameObject);
            }).SetLink(gameObject);
        }
        else
        {
            transform.DOMove(transform.position + transform.right, hitSpeed).SetEase(ease).OnComplete(() =>
            {
                transform.DOMove(transform.position - transform.right, hitSpeed).OnComplete(() =>
                {
                    Invoke("LoopManuel", delaySecond);
                }).SetLink(gameObject);
            }).SetLink(gameObject);
        }
    }

    private void GetThemeCount()
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[0] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat1;
        materials[1] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;

        GetComponent<MeshRenderer>().materials = materials;
    }

    public void DeactiveCollider()
    {
        foreach (Collider item in GetComponents<Collider>())
        {
            item.enabled = false;
        }
    }
}
