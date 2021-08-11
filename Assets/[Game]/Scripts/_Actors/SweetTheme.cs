using UnityEngine;

public class SweetTheme : MonoBehaviour
{

    [System.Serializable]
    public class ThemeProperties
    {
        public Material[] mats;
    }

    public ThemeProperties[] themeMats;

    void Start()
    {
        GetThemeCount();
    }

    private void GetThemeCount()
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mats[i];
        }

        GetComponent<MeshRenderer>().materials = materials;
    }

}
