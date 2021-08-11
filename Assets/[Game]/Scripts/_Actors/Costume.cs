using UnityEngine;

public class Costume : MonoBehaviour
{

    public bool isPlayer1;

    [System.Serializable]
    public class ThemeProperties
    {
        public Material Shoes;
        public Material skin;
        public Material Shorts;
        public Material Shirt;
        public Material Hair;
        public Material ShortsHeart;
    }

    public ThemeProperties[] themeMats;

    public Material[] player2Mat;

    void Start()
    {
        GetThemeCount();
    }

    private void GetThemeCount()
    {
        
        int count = CustomLevelManager.Instance.customGameData.GetThemeCount();

        if (isPlayer1)
        {
            Material[] materials = GetComponent<SkinnedMeshRenderer>().materials;

            materials[0] = themeMats[count].Shoes;
            materials[1] = themeMats[count].skin;
            materials[2] = themeMats[count].Shorts;
            materials[3] = themeMats[count].Shirt;
            materials[4] = themeMats[count].Hair;
            materials[10] = themeMats[count].ShortsHeart;

            GetComponent<SkinnedMeshRenderer>().materials = materials;
        }
        else
        {
            GetComponent<SkinnedMeshRenderer>().material = player2Mat[count];

        }

        // GetComponent<MeshRenderer>().material = themeMats[CustomLevelManager.Instance.customGameData.GetThemeCount()].mat2;
    }

}
