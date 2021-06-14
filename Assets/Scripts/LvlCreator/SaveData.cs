using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveData 
{

}


[Serializable]
public class LvlData
{
    public Color DefaultColor;
    public Color AlternativeColor;
    public List<ObstacleInfo> FriendlyObstacles;
    public List<ObstacleInfo> EnemyObstacles;
}

[Serializable]
public class ObstacleInfo
{
    public Vector3 Size;
    public Vector3 Position;
    public ObstacleShape Shape;

    
}


public enum ObstacleShape
{
    Cube,
    Cylinder,
}
