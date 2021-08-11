using System.Collections.Generic;
using UnityEngine;

public class PlatformUnit : MonoBehaviour
{

    [System.Serializable]
    public class ThemeProperties
    {
        public Material mat1;
        public Material mat2;
    }

    public ThemeProperties[] themeMats;

    private List<Transform> points = new List<Transform>();

    private void Start()
    {
        for (byte i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i));
        }

        GetThemeCount();
    }

    private void GetThemeCount()
    {

        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[0] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat1;
        materials[1] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;

        GetComponent<MeshRenderer>().materials = materials;
    }
}
