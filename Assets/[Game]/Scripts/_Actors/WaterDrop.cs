using DG.Tweening;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(0.112f, 0.1f);
        //GetComponent<SpriteRenderer>().color = CustomLevelManager.Instance.waterColor[CustomLevelManager.Instance.customGameData.GetThemeCount()];
        transform.DOScale(new Vector3(Random.Range(0.3f, 0.4f), Random.Range(0.3f, 0.4f), Random.Range(0.3f, 0.4f)), 0.4f).SetEase(Ease.OutBack).SetLink(gameObject);
    }

}
