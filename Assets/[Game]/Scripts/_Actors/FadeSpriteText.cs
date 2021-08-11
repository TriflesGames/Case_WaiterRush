using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeSpriteText : MonoBehaviour
{
    [Header("Properties")]
    public float second;
    public float yForce;
    public Ease ease_y;

    public float scaleForce;
    public Ease ease_scale;

    public Image ımg;
    public TextMeshProUGUI txt;

    void Start()
    {
        if (ımg != null)
        {
            ımg.DOFade(0, second).SetEase(ease_y);
            ımg.transform.DOLocalMoveY(yForce, second).SetRelative().SetEase(ease_y).OnComplete(() => {
                Destroy(gameObject);
            });
        }
        else if (txt != null)
        {
            txt.DOFade(0, second).SetEase(ease_y);
            txt.transform.DOLocalMoveY(yForce, second).SetRelative().SetEase(ease_y).OnComplete(()=> {
                Destroy(gameObject);
            });
        }

        if (scaleForce != 0)
        {
            if (ımg != null)
            {
                ımg.transform.DOScale(Vector3.one * scaleForce, second).SetEase(ease_scale).OnComplete(() => {
                    Destroy(gameObject);
                });
            }
            else if (txt != null)
            {
                txt.transform.DOScale(Vector3.one * scaleForce, second).SetEase(ease_scale).OnComplete(() => {
                    Destroy(gameObject);
                });
            }
        }
    }
}
