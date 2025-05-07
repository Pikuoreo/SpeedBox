using UnityEngine;

/// <summary>
/// ステージ毎の保持したいセーブデータ群を管理するクラス
/// </summary>
[System.Serializable]
public class StageData
{
    //セーブしたいステージのレベル
    [SerializeField] private int _saveStageLevel = default;

    //セーブしたいステージの番号
    [SerializeField] private int _saveStageNumber = default;

    //ステージのクリア時間
    [SerializeField] private float _stageCleartime = 0;

    //３段階評価の数
    [SerializeField] private int _stageClearstar = 0;

    //ステージに一度でも訪れたことがあるか
    [SerializeField] private bool _isStageVisit = false;


    //セーブしたいステージのレベル
    public int SaveStageLevel
    {
        get { return _saveStageLevel; }
        set { _saveStageLevel = value; }
    }

    //セーブしたいステージの番号
    public int SaveStageNumber
    {
        get { return _saveStageNumber; }
        set { _saveStageNumber = value; }
    }

    //ステージのクリア時間
    public float StageClearTime
    {
        get { return _stageCleartime; }
        set { _stageCleartime = value; }
    }

    //３段階評価の数
    public int StageClearStar
    {
        get { return _stageClearstar; }
        set { _stageClearstar = value; }
    }

    //ステージに一度でも訪れたことがあるか
    public bool IsStageVisit
    {
        get { return _isStageVisit; }
        set { _isStageVisit = value; }

    }
}
