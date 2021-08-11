using DG.Tweening;
using Game.Actors;
using System.Collections.Generic;
using TMPro;
using TriflesGames.Managers;
using UnityEngine;

public class BigGlassBottle : ItemProperties
{
    public Transform CenterTransform;

    public List<SkinnedMeshRenderer> waterPieces = new List<SkinnedMeshRenderer>();
    public int currentWaterPieceIndex;

    [System.Serializable]
    public class ThemeProperties
    {
        public Material mat1;
        public Material mat2;
    }

    public ThemeProperties[] themeMats;

    public GameObject[] themeObjs;

    private float waveSpeed = 100;
    [HideInInspector] public SkinnedMeshRenderer skinnedMeshRenderer;

    public TextMeshProUGUI txt_percentage;

    private Transform mainCamera;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        mainCamera = Camera.main.transform;
        //SetWater(CustomLevelManager.Instance.customGameData.totalWater);
        GetThemeCount();

        GetWaterList(CustomLevelManager.Instance.customGameData.colors);


        txt_percentage.enabled = false;
    }

    public void SetWater(float newValue)
    {
        txt_percentage.text = "%" + ((int)(100 - newValue)).ToString();
        skinnedMeshRenderer.SetBlendShapeWeight(0, newValue);
    }

    private void Update()
    {
        txt_percentage.transform.parent.LookAt(mainCamera);
    }

    public void IncrementWater(List<Color> colors)
    {
        //txt_percentage.transform.parent.SetParent(transform.parent);

        CameraManager.Instance.LookAround();

        #region süs 
        float currentWaterPiece = currentWaterPieceIndex * 10;
        float newWaterPiece = Mathf.Clamp((currentWaterPieceIndex + colors.Count) * 10, 0, 100);

        float totalSecond = colors.Count * 0.25f;

        txt_percentage.enabled = true;
      
        DOTween.To(() => currentWaterPiece, x => currentWaterPiece = x, newWaterPiece, totalSecond).SetDelay(0.2f).OnUpdate(() =>
        {
            txt_percentage.text = "%" + ((int)currentWaterPiece).ToString();
            if (Time.frameCount % 20 == 0)
            {
                VibrationManager.Instance.TriggerLightImpact();
            }

            if (Time.frameCount % 10 == 0)
            {
                //txt_percentage.transform.DOKill();
                txt_percentage.transform.DOScale(new Vector3(-1.9f, 1.9f, 1.9f), 0.1f).SetLoops(2, LoopType.Yoyo);
            }
        });
        #endregion

        colors.Reverse();
        NextWaterPiece(0, colors.Count, colors);    
    }

    void NextWaterPiece(int index, int intMaxIndex, List<Color> colors)
    {
        if(intMaxIndex==0)
        {

            ((GameLevelActor)LevelManager.Instance.levelActor).FinishLevel(false);
            return;
        }

        float currentWaterUnit = waterPieces[currentWaterPieceIndex + index].GetBlendShapeWeight(0);

        waterPieces[currentWaterPieceIndex + index].gameObject.SetActive(true);
        waterPieces[currentWaterPieceIndex + index].GetComponent<LiquidMeshActor>().isWaveActive = true;

        waterPieces[currentWaterPieceIndex + index].material.color = colors[index];

        DOTween.To(() => currentWaterUnit, x => currentWaterUnit = x, 100, 0.25f).OnUpdate(() =>
        {
            waterPieces[currentWaterPieceIndex + index].SetBlendShapeWeight(0, currentWaterUnit);
        }).OnComplete(() =>
        {
            index++;

            if (currentWaterPieceIndex + index >= 10)
            {
                CustomLevelManager.Instance.customGameData.IncrementTheme();
                ((GameLevelActor)LevelManager.Instance.levelActor).FinishLevel(true);
                return;
            }

            if (index < intMaxIndex)
            {
                NextWaterPiece(index, intMaxIndex, colors);
            }
            else
            {
                CustomLevelManager.Instance.customGameData.SetColors(GetColorList());
                ((GameLevelActor)LevelManager.Instance.levelActor).FinishLevel(true);
            }
        }).SetLink(gameObject);
    }

    public void DecrementWater()
    {

    }

    private void GetWaterList(List<Color> colors)
    {
        for (int i = 0; i < waterPieces.Count; i++)
        {
            if (i < colors.Count)
            {
                waterPieces[i].material.color = colors[i];
                waterPieces[i].SetBlendShapeWeight(0, 100);
                waterPieces[i].gameObject.SetActive(true);
                waterPieces[i].GetComponent<LiquidMeshActor>().isWaveActive = true;
            }
            else
            {
                waterPieces[i].SetBlendShapeWeight(0, 0);
                waterPieces[i].gameObject.SetActive(false);
                waterPieces[i].GetComponent<LiquidMeshActor>().isWaveActive = false;
            }
        }

        if (currentWaterPieceIndex != 0)
        {
            currentWaterPieceIndex = colors.Count + 1;
        }
        else
        {
            currentWaterPieceIndex = colors.Count;
        }
    }

    public List<Color> GetColorList()
    {
        List<Color> newList = new List<Color>();
        for (int i = 0; i < waterPieces.Count; i++)
        {
            if (waterPieces[i].gameObject.activeInHierarchy)
            {
                newList.Add(waterPieces[i].material.color);
            }
        }

        return newList;
    }

    private void GetThemeCount()
    {
        sbyte themeCount = CustomLevelManager.Instance.customGameData.GetThemeCount();


        themeObjs[themeCount].SetActive(true);

        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[0] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat1;
        materials[1] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;

        GetComponent<MeshRenderer>().materials = materials;

        //skinnedMeshRenderer.material.color = CustomLevelManager.Instance.waterColor[themeCount];
    }

    #region Wave

    private void WaveMoveUp()
    {

        float lastWeight = 0;
        DOTween.To(() => lastWeight, x => lastWeight = x, 100, waveSpeed).SetSpeedBased().OnUpdate(() =>
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, lastWeight);

        }).OnComplete(() =>
        {
            Invoke("WaveMoveDown", 0);
        }).SetLink(gameObject);
    }

    private void WaveMoveDown()
    {
        float lastWeight = skinnedMeshRenderer.GetBlendShapeWeight(1);
        DOTween.To(() => lastWeight, x => lastWeight = x, 0, waveSpeed * 2).SetSpeedBased().OnUpdate(() =>
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, lastWeight);

        }).OnComplete(() =>
        {

            Invoke("WaveMoveUp", 0);
        }).SetLink(gameObject);
    }

    #endregion
}
