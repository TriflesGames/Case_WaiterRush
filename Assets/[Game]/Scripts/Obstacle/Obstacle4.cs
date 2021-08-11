using DG.Tweening;
using UnityEngine;


[SelectionBase]
public class Obstacle4 : ItemProperties
{
    [Header("Manuel Settings")]
    public float delaySecond;

    public AnimationCurve customEaseUp, customEaseDown;

    [Header("Custom")]
    public float second;

    private Transform obje;

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
            Invoke("MoveDown", delaySecond);
        });
    }

    void Start()
    {
        obje = transform.GetChild(0);
        GetThemeCount();
    }
 
    private void MoveDown()
    {
        obje.transform.DOLocalMoveY(0.1f, second).SetDelay(second).SetEase(customEaseDown).OnComplete(()=> {
            MoveUp();
        }).SetLink(gameObject);
    }

    private void MoveUp()
    {
        obje.transform.DOLocalMoveY(2.38f, second).SetEase(customEaseUp).OnComplete(() => {
            Invoke("MoveDown", second);
        }).SetLink(gameObject);
    }

    private void GetThemeCount()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat1;
        transform.GetChild(1).GetComponent<MeshRenderer>().material = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;
    }

    public void DeactiveCollider()
    {
        foreach (Collider item in GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }
    }
}
