using TMPro;
using UnityEngine;

public class StageCore : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnpoint = default;//プレイヤーの出現位置

    [SerializeField] private BoxCollider2D _goalPointCollider = default;//ゴールポイントの当たり判定

    [SerializeField] private StageClearAnimation _stageClearAnimationScript = default;
    [SerializeField] private GameOverAnimation _gameOverAnimationScript = default;

    private PlayerController _playerController = default;
    private GameProgress _gameProgress = default;

    [Header("星1のクリア時間"), SerializeField] private float _singleStarCleartime = default;
    [Header("星2のクリア時間"), SerializeField] private float _doubleStarCleartime = default;
    [Header("星3のクリア時間"), SerializeField] private float _tripleStarCleartime = default;

    private TextMeshProUGUI elapsedTimeText = default;//制限時間のテキスト

    private GameObject _player = default;

    private float _timeLimit = 10f;//制限時間

    private bool _isGoal = false;//trueでゴール判定
    private bool _isGameOver = false;//trueでゲームオーバー判定
    private bool _isGameStart = false;//trueでゲーム開始判定]

    [SerializeField] private int _stageLevel = default;
    [SerializeField] private int _stageNumber = default;

    private SaveManager _saveManager = default;

    private void Start()
    {

        //セーブデータを保管しているオブジェクトを探す
        _saveManager = GameObject.FindGameObjectWithTag("SaveData").GetComponent<SaveManager>();

        //ステージに挑戦したことがなかったら
        if (_saveManager.LoadStageData(_stageLevel, _stageNumber).Item1 == 0)
        {
            //ステージのセーブデータを作る
            _saveManager.SaveStageData(_stageLevel, _stageNumber, 0, 0);
        }
    }

    private void Update()
    {
        //制限時間を減らしていく
        if (_isGameStart)
        {
            TimeMeasurement();
        }
    }

    /// <summary>
    /// クリア時の評価星の数を渡す
    /// </summary>
    /// <returns></returns>
    private int JudgeClearEvaluation()
    {
        //クリア時間が３つ星時間より早かったら
        if (_timeLimit >= _tripleStarCleartime)
        {
            //3つ星評価を返す
            int tripleStarEvaluation = 3;

            return tripleStarEvaluation;
        }
        //クリア時間が２つ星時間より早かったら
        else if (_timeLimit >= _doubleStarCleartime)
        {
            //二つ星を返す
            int doubleStarEvaluation = 2;

            return doubleStarEvaluation;
        }
        else
        {
            //一つ星を返す
            int singleStarEvaluation = 1;

            return singleStarEvaluation;
        }
    }

    private void TimeMeasurement()
    {
        //時間が０より大きかったら
        if (_timeLimit > 0 && !_isGameOver)
        {
            //時間を減らしていく
            _timeLimit -= Time.deltaTime;
            elapsedTimeText.text = _timeLimit.ToString("F3");

            //強制終了
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _playerController.ControllOff();
                //ステージの強制終了
                _gameProgress.BackTitle();
            }

            //強制リトライ
            if (Input.GetKeyDown(KeyCode.R))
            {
                _isGameOver = true;
                _playerController.ControllOff();
                RestartStage();
            }

        }
        else if (!_isGameOver)
        {
            //ゲームオーバー
            elapsedTimeText.text = "";
            GameOver();
        }



    }
    public void StageStart(GameProgress gameProgressScript, GameObject playerObject, TextMeshProUGUI timeText)
    {
        //スクリプトの取得
        _gameProgress = gameProgressScript;

        //テキストの代入
        elapsedTimeText = timeText;

        //プレイヤーの取得
        _player = playerObject;
        _player.transform.position = _playerSpawnpoint.position;
        _playerController = _player.GetComponent<PlayerController>();
        _player.SetActive(true);

        //ゴールポイントの当たり判定を出す
        _goalPointCollider.enabled = true;
    }

    public void StageClear()
    {
        if (!_isGoal)
        {
            elapsedTimeText.text = "";
            //クリアした時、背景のBGM音量を少しずつ下げる
            _gameProgress.BGM_BolumeDown();

            _isGameStart = false;
            _isGoal = true;

            float _bestStageClearTime = _saveManager.LoadStageData(_stageLevel, _stageNumber).Item1;

            int clearEvalutionValue = JudgeClearEvaluation();

            //クリア時間がベストクリア時間よりも早かったらクリア時間を渡す
            if (_bestStageClearTime <= _timeLimit)
            {
                //最新のクリア時間を記録する
                _saveManager.SaveStageData(_stageLevel, _stageNumber, _timeLimit, clearEvalutionValue);
                //ゲームクリアのアニメーションを流す
                _stageClearAnimationScript.StartStageClearAnimation(_timeLimit, _timeLimit, _singleStarCleartime, _doubleStarCleartime, _tripleStarCleartime, clearEvalutionValue);
            }
            //クリア時間がベストクリア時間よりも遅かったらベストクリア時間を渡す
            else
            {
                //ゲームクリアのアニメーションを流す
                _stageClearAnimationScript.StartStageClearAnimation(_timeLimit, _bestStageClearTime, _singleStarCleartime, _doubleStarCleartime, _tripleStarCleartime, clearEvalutionValue);
            }
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        //プレイヤーを動けなくする
        _playerController.ControllOff();

        //ゴールの当たり判定を消す
        _goalPointCollider.enabled = false;

        //ゲームオーバーのアニメーションを流す
        _gameOverAnimationScript.StartGameOverAnimation();

        _saveManager.IncrementAllDeathCount();

    }
    public void RestartStage()
    {
        //ステージ再挑戦
        _gameProgress.RetryStage(true);
    }

    public void StartStageStrategy()
    {
        //ステージ開始
        _isGameStart = true;
    }

    public Vector2 GivePlayerSpwanPoint()
    {
        //プレイヤーの出現位置を渡す
        return _playerSpawnpoint.position;
    }

    public (int, int) GiveStageName()
    {
        //ステージのレベル、番号を渡す
        return (_stageLevel, _stageNumber);
    }

    public void GetTimePlusItem(float plusValue)
    {
        _timeLimit += plusValue;
    }
}
