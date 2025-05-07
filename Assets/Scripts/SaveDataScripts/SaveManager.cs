using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// セーブデータを読み書きするクラス
/// </summary>
public class SaveManager : MonoBehaviour
{
    //セーブ先のパス
    private string _saveFilePath = null;

    //セーブデータの集合を補完しているところ
    private StageSaveData _data = null;

    //タイトルの死亡カウントのテキスト
    [SerializeField]
    private TextMeshProUGUI _allDeathCountText = default;

    //テキストにいれる文字
    private string _allDeathCountTextCharacter = "AllDeathCount:";


    const string SAVE_FILE_NAME = "saveData.json";
    private void Awake()
    {
        //セーブ先のパスを代入
        _saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        //ディレクトリの名前を取得
        string directoryPath = Path.GetDirectoryName(_saveFilePath);

        //ディレクトリが無かったら
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            //ディレクトリを作る
            Directory.CreateDirectory(directoryPath);
        }

        _data = new StageSaveData();

        //今までの死亡回数を代入
        _allDeathCountText.text = _allDeathCountTextCharacter + GetAllDeathCount().ToString();
    }

    /// <summary>
    /// 指定したステージにデータを保存
    /// </summary>
    /// <param name="stageLevel">データ検索のためのステージレベル</param>
    /// <param name="stageNumber">データ検索のためのステージ番号</param>
    /// <param name="stageClearTime">ステージのクリア時間</param>
    /// <param name="clearEvaluation">クリアした時の評価星</param>

    public void SaveStageData(int stageLevel, int stageNumber, float stageClearTime, int clearEvaluation)
    {
        // ステージがすでにリストに存在するか確認
        StageData stageData = _data.SaveDataLists.Find(stageDataList => stageDataList.SaveStageLevel == stageLevel &&
                                                                 stageDataList.SaveStageNumber == stageNumber);

        //ステージのセーブデータが無かったら
        if (stageData == null)
        {
            // ステージが見つからなければ新しく追加
            stageData = new StageData { SaveStageLevel = stageLevel, SaveStageNumber = stageNumber, StageClearTime = stageClearTime, StageClearStar = clearEvaluation };
            stageData.IsStageVisit = true;
            _data.SaveDataLists.Add(stageData);
        }
        //セーブデータが存在していたら
        else
        {
            //データを更新
            stageData.StageClearTime = stageClearTime;
            stageData.StageClearStar = clearEvaluation;
        }

        //セーブデータの内容をJson形式に変換
        string saveContent = JsonUtility.ToJson(_data, true);

        //セーブデータを書き込む
        File.WriteAllText(_saveFilePath, saveContent);
    }

    /// <summary>
    /// 今までの死亡回数を増やす・書き込む
    /// </summary>
    public void IncrementAllDeathCount()
    {
        //すでにセーブデータが保存されているならデータを読み込む
        if (IsSaveFileExists())
        {
            _data.AllDeathCountData++;

            //セーブデータの内容をJson形式に変換
            string deathCount = JsonUtility.ToJson(_data, true);

            //セーブデータを書き込む
            File.WriteAllText(_saveFilePath, deathCount);

            _allDeathCountText.text = _allDeathCountTextCharacter + _data.AllDeathCountData.ToString();
        }
    }

    /// <summary>
    /// 今までの死亡回数を読み取る
    /// </summary>
    /// <returns>今までの死亡回数を返す</returns>
    public int GetAllDeathCount()
    {
        _data = new StageSaveData();

        //すでにセーブデータが保存されているならデータを読み込む
        if (IsSaveFileExists())
        {
            string deathCountData = File.ReadAllText(_saveFilePath);

            if (!string.IsNullOrEmpty(deathCountData))
            {
                _data = JsonUtility.FromJson<StageSaveData>(deathCountData);
            }

            return _data.AllDeathCountData;
        }
        else
        {
            print("死亡回数のデータがありません。");
            return 0;

        }
    }

    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    /// <param name="stageLevel">ステージのレベル検索変数</param>
    /// <param name="stageNumber">ステージの番号検索変数</param>
    /// <returns>クリア時間、クリア時の星の数、一度プレイしたかどうかを返す</returns>
    public (float, int, bool) LoadStageData(int stageLevel, int stageNumber)
    {
        //すでにセーブデータが保存されているならデータを読み込む
        if (IsSaveFileExists())
        {
            //ファイルから内容を読み込む
            string dataContent = File.ReadAllText(_saveFilePath);

            // JSONからデータを復元
            _data = JsonUtility.FromJson<StageSaveData>(dataContent);

            StageData stageData = _data.SaveDataLists.Find(stages => stages.SaveStageNumber == stageNumber && stages.SaveStageLevel == stageLevel);

            //ステージに一度でも挑戦したことがある場合
            if (stageData != null)
            {
                return (stageData.StageClearTime,
                        stageData.StageClearStar,
                        stageData.IsStageVisit);
            }
            //ステージに一度も挑戦したことがない場合
            else
            {
                //print(stageNumber + "に来たことがありません");
                return (0, 0, false);
            }
        }
        //セーブデータがない場合
        else
        {
            return (0, 0, false);
        }
    }

    /// <summary>
    /// セーブデータの削除
    /// </summary>
    public void ClearSaveData()
    {
        if (!File.Exists(_saveFilePath))
        {
            print("aaa");
            return;
        }

        File.Delete(_saveFilePath);

        // 現在のシーン名を取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        // シーンを再読み込み
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// セーブ先にファイルがあるかどうかを見る
    /// </summary>
    /// <returns>セーブデータがあるかの真偽を返す</returns>
    private bool IsSaveFileExists()
    {

        return File.Exists(_saveFilePath);

    }

}
