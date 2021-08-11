using System;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;


public enum Side
{
    random,left,center,right
}

public enum UnitItem
{
    empty, UnitGlassBottle, Obstacle1, Obstacle2, Obstacle3, Obstacle4
}

public class PlatformCreator : MonoBehaviour
{
    [Serializable]
    public class CreatorClass
    {
        public UnitItem unitItem1;
        public Side side1;

        public UnitItem unitItem2;
        public Side side2;

        public UnitItem unitItem3;
        public Side side3;
    }

    public List<CreatorClass> creatorClasses = new List<CreatorClass>();

    private float roadWidth;
    private float sizeZ;

    private void Start()
    {

    }

    private void Awake()
    {/*
        roadWidth = GetComponent<RoadMeshCreator>().roadWidth;
        GameObject platformPrefab = (GameObject)Resources.Load("AutoLevelObjects/Platform");
        float totalSize = 0;
        GameObject temp;

        for (sbyte i = -1; i < creatorClasses.Count; i++)
        {
            if (i == -1)
            {
                temp = Instantiate((GameObject)Resources.Load("AutoLevelObjects/Platform_Start"), transform);
                temp.transform.position = new Vector3(0, 0, totalSize);               
            }
            else
            {
                temp = Instantiate(platformPrefab, transform);
                temp.transform.position = new Vector3(0, 0, totalSize);
                temp.GetComponent<PlatformUnit>().SetItem(creatorClasses[i].unitItem1, roadWidth, 0, creatorClasses[i].side1);
                temp.GetComponent<PlatformUnit>().SetItem(creatorClasses[i].unitItem2, roadWidth, 1, creatorClasses[i].side2);
                temp.GetComponent<PlatformUnit>().SetItem(creatorClasses[i].unitItem3, roadWidth, 2, creatorClasses[i].side3);
            }

            sizeZ = temp.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * temp.transform.localScale.z - 0.1f;
            totalSize += sizeZ;

            if (i == creatorClasses.Count-1)
            {
                temp = Instantiate((GameObject)Resources.Load("AutoLevelObjects/Platform_End"), transform);
                temp.transform.position = new Vector3(0, 0, totalSize);
            }
        }

       */


        #region manuel calculate
        float totalSize = 0;

        Transform platformParentTemp = transform.parent.Find("PlatformParent");
        for (int i = 0; i < platformParentTemp.childCount; i++)
        {
            if(platformParentTemp.GetChild(i).CompareTag("Platform"))
            {
                totalSize += platformParentTemp.GetChild(i).GetComponent<MeshFilter>().sharedMesh.bounds.size.z * platformParentTemp.GetChild(i).transform.localScale.z - 0.1f;
            }
        }
        #endregion


        PathCreator pathCreator = GetComponent<PathCreator>();

        pathCreator.bezierPath.SetPoint(0, new Vector3(0, 0, 0), false);
        pathCreator.bezierPath.SetPoint(1, new Vector3(0, 0, 1), false);
        pathCreator.bezierPath.SetPoint(2, new Vector3(0, 0, totalSize - 1), false);
        pathCreator.bezierPath.SetPoint(3, new Vector3(0, 0, totalSize+1.3f), false);
        pathCreator.bezierPath.SetPoint(4, new Vector3(0, 0, totalSize+1.4f), false);


        pathCreator.bezierPath.SetPoint(5, new Vector3(0, 2.8f, totalSize+8.5f), false);
        pathCreator.bezierPath.SetPoint(6, new Vector3(0, 3.1f, totalSize+9.8f), false);
        pathCreator.bezierPath.SetPoint(7, new Vector3(0, 3.1f, totalSize +9.9f), false);



        pathCreator.bezierPath.SetPoint(8, new Vector3(0, 3.15f, totalSize + 10f), false);
        pathCreator.bezierPath.SetPoint(9, new Vector3(0, 3.15f, totalSize + 10.5f), true);
    }
}
