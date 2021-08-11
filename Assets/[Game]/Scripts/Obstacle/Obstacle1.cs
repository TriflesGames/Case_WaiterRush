using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class Obstacle1 : MonoBehaviour
{
    [System.Serializable]
    public class ThemeProperties
    {
        public Material mat1;
        public Material mat2;
    }

    public ThemeProperties[] themeMats;

    [Header("Custom")]
    public bool isInverse;
    [Range(60, 150)]
    public float rotateSpeed;

    private void Awake()
    {
        tag = "ObstacleItem";

        GetComponent<GameState>().FillGameStart(() =>
        {
            Vector3 newRot = new Vector3(0, 1, 0);

            if (isInverse)
            {
                newRot = new Vector3(0, -1, 0);
            }

            transform.DORotate(newRot, rotateSpeed, RotateMode.Fast).SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
        });
    }

    void Start()
    {
        GetThemeCount();
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
