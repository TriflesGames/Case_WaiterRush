using DG.Tweening;
using TMPro;
using UnityEngine;

public class BoostText : MonoBehaviour
{
    public string[] textList;

    public int loopCount = 3;

    public float scaleFactor;
    public float second;

    int currentLoopCount;
    int selectedIndex;

    void Start()
    {
        currentLoopCount = loopCount;

        selectedIndex = Random.Range(0, textList.Length);
        Show();
    }

    void Show()
    {
        if (currentLoopCount <= 0) { Destroy(gameObject); return; }

        currentLoopCount--;

        GetComponent<TextMeshProUGUI>().text = textList[selectedIndex];
        transform.DOScale(Vector3.one * scaleFactor, second).SetEase(Ease.InExpo).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, second).SetEase(Ease.InExpo).OnComplete(() =>
            {
                selectedIndex++;
                selectedIndex =  (int)Mathf.Repeat(selectedIndex, textList.Length);
                Show();
            }).SetLink(gameObject);
        }).SetLink(gameObject); 
    }
}
