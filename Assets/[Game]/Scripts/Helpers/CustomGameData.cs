
using System.Collections.Generic;
using TriflesGames.ManagerFramework;
using UnityEngine;

public class CustomGameData : Entity<CustomGameData>
{
    #region Variables

    public float totalWater = 100;
    public sbyte themeCount = 0;

    public List<Color> colors = new List<Color>();

    #endregion

    protected override bool Init()
    {
        return true;
    }

    public void SetColors(List<Color> colors)
    {
        this.colors = colors;
        Save();
    }

    public void UpdateWater(float newTotalWater)
    {
        totalWater = newTotalWater;
        Save();
    }

    public void IncrementTheme()
    {
        this.colors.Clear();
        totalWater = 100;
        themeCount++;
        Save();
    }

    public sbyte GetThemeCount()
    {
        sbyte newThemeCount = 0;
        newThemeCount = (sbyte)Mathf.Repeat(themeCount, CustomLevelManager.Instance.themeCount);
        return newThemeCount;
    }
}
