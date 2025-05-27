using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldData
{
    public bool[] dungeons = new bool[3];
}

[Serializable]
public abstract class BaseData
{
    //월드 클리어 현황
    public WorldData[] worlds = new WorldData[4];

    //현재 월드 클리어 번호
    public int currentWorldNumber;

    //현재 가지고 있는 아이템 리스트
    public int currentItemList;

    //획득한 룬ID
    public List<int> ownedRunes = new List<int>();

    public BaseData()
    {
        for (int i = 0; i < worlds.Length; i++)
        {
            worlds[i] = new WorldData();
        }
    }

    public int GetCurrentWorldDataNumber()
    {
        for(int i = 0; i < worlds.Length; i++)
        {
            bool isBreak = false;
            for(int j = 0; j < worlds[i].dungeons.Length; j++)
            {
                if (worlds[i].dungeons[j] == false)
                {
                    currentWorldNumber = i + 1;
                    isBreak = true;
                    break;
                }
            }
            if (isBreak) break;
        }

        return currentWorldNumber;
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

