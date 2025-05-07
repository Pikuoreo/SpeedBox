using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SaveDatasの集合と今までの死亡回数を管理しているクラス
/// </summary>
[System.Serializable]
public class StageSaveData
{
    //ステージごとのセーブデータのまとまり
    [SerializeField] private List<StageData> _saveDataLists = new List<StageData>();

    //今までの死亡回数
    [SerializeField] private int _allDeathCount = 0;

    //ステージごとのセーブデータのまとまり
    public List<StageData> SaveDataLists
    {
        get { return _saveDataLists; }
        set { _saveDataLists = value; }
    }

    //今までの死亡回数
    public int AllDeathCountData
    {
        get { return _allDeathCount; }
        set { _allDeathCount = value; }
    }
}
