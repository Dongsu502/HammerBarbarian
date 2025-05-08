using JetBrains.Annotations;
using System;

[Serializable]
public class WorldData
{
    public bool[] dungeons = new bool[3];
}

[Serializable]
public abstract class BaseData
{
    public WorldData[] worlds = new WorldData[7];

    public BaseData()
    {
        for (int i = 0; i < worlds.Length; i++)
        {
            worlds[i] = new WorldData();
        }
    }
}

[Serializable]
public class Data1 : BaseData
{

}

[Serializable]
public class Data2 : BaseData
{

}

[Serializable]
public class Data3 : BaseData
{

}

