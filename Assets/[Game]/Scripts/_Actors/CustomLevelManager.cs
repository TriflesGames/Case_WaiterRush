using Game.GlobalVariables;
using TriflesGames.ManagerFramework;
using UnityEngine;

public class CustomLevelManager : Manager<CustomLevelManager>
{
    public int themeCount = 3;

    [Header("LevelProperties")]
    public Color[] waterColor;

    //public sbyte themeIndex;

    [HideInInspector]public bool isTutorialLevel;

    [HideInInspector]public CustomGameData customGameData;

    protected override void MB_Awake()
    {
        base.MB_Awake();

        customGameData = CustomGameData.Get();
        if (customGameData == null)
        {
            customGameData = new CustomGameData();
            var isSuccess = customGameData.Register();
            if (!isSuccess) Debug.LogError("GameData Entity register error!");
        }

        // Load game data
        customGameData.Load();
        //customGameData.themeCount = themeIndex;
    }

    public void CreateBoostUI()
    {
        Publish(CustomManagerEvents.CreateBoostUI);
    }

    public void DestroyBoostUI()
    {
        Publish(CustomManagerEvents.DestroyBoostUI);
    }

    protected override void MB_Update()
    {
        base.MB_Update();

        if (isTutorialLevel)
        {
            if (Input.GetMouseButtonDown(0))
            {
               Publish(CustomManagerEvents.TutorialCompleted);
                isTutorialLevel = false;
            }
        }
    }

}
