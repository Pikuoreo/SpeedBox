using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    private Rigidbody2D _playerRB = default;

    [Header("レベル１のステージ軍"), SerializeField] private List<GameObject> _firstLevelStages = new List<GameObject>();
    [Header("レベル２のステージ軍"), SerializeField] private List<GameObject> _secondLevelStages = new List<GameObject>();
    [Header("レベル３のステージ軍"), SerializeField] private List<GameObject> _thirdLevelStages = new List<GameObject>();
    [Header("レベル４のステージ軍"), SerializeField] private List<GameObject> _forthLevelStages = new List<GameObject>();
    [Header("レベル５のステージ軍"), SerializeField] private List<GameObject> _fifthLevelStages = new List<GameObject>();

    [SerializeField] private GameObject _player = default;
    [SerializeField] private GameObject _titleParent = default;//ステージに関するオブジェクト
    [SerializeField] private GameObject _demoStage = default;

    private GameObject _preStage = default;//ひとつ前のステージ

    [SerializeField] private TextMeshProUGUI elapsedTimeText = default;//経過時間テキスト



    [SerializeField] private Animator _loadAnimation = default;//ロードのアニメーション
    [SerializeField] private Animator _readyAnimation = default;

    private int _currentStageLevel = 0;//現在のステージレベル
    private int _currentStageNumber = 0;//現在のステージ番号

    private const int MAX_STAGE_LEVEL = 5;
    private const int MAX_STAGE_NUMBER = 9;

    private bool _isNextStageLevel = false;
    private bool _isPlayerDeathRetry = false;//trueでプレイヤーが死んでリトライした判定

    private GameProgress _thisScript = default;
    private StageCore _preStageCoreScript = default;
    private PlayerController _playerControll = default;
    private ChangeGenerateStageLevel _generateStageLevel = default;

    private AudioSource _bgmAudio = default;
    [SerializeField] private List<AudioClip> _stageBGMs = new List<AudioClip>();

    private void Start()
    {
        _thisScript = this.GetComponent<GameProgress>();

        if (_player != null)
        {
            _playerControll = _player.GetComponent<PlayerController>();
        }

        _playerRB = _player.GetComponent<Rigidbody2D>();
        _bgmAudio = this.GetComponent<AudioSource>();
    }

    private enum ChangeGenerateStageLevel
    {
        StageLevel_1 = 1,//ステージレベル１
        StageLevel_2 = 2,//ステージレベル2
        StageLevel_3 = 3,//ステージレベル3
        StageLevel_4 = 4,//ステージレベル4
        Stagelevel_5 = 5,//ステージレベル5
        StageLevel_0 = 0 //ステージレベル0
    }

    /// <summary>
    /// //生成するステージレベルを取得
    /// </summary>
    /// <param name="getStageLevel">ステージレベル</param>
    public void SetStageLevel(int getStageLevel)
    {
        //生成するステージレベルを取得
        _currentStageLevel = getStageLevel;

        //ステージレベルに合わせたenumに変更
        _generateStageLevel = (ChangeGenerateStageLevel)getStageLevel;
    }

    /// <summary>
    ///  //ステージを生成
    /// </summary>
    /// <param name="getStageNumber">各ステージの番号</param>
    public void SetStageNumber(int getStageNumber)
    {
        //タイトルに関するものを見えなくする
        _titleParent.SetActive(false);

        _loadAnimation.gameObject.transform.position = this.transform.position;
        //ロードアニメーションを流す
        _loadAnimation.SetTrigger("Load");

        //生成するステージ番号を取得
        _currentStageNumber = getStageNumber;

        //リスト用にー１をして調整
        _currentStageNumber--;
    }
    /// <summary>
    /// ステージ生成
    /// </summary>
    public void GenerateStage()
    {
        if (!_player.activeSelf)
        {
            _player.SetActive(true);
        }

        if (_isNextStageLevel)
        {
            _isNextStageLevel = false;
        }

        elapsedTimeText.text = null;
        //すでにステージ生成されていたら消す
        if (_preStage != null)
        {
            Destroy(_preStage);
        }

        //ステージレベルごとのステージ生成
        switch (_generateStageLevel)
        {
            //未実装ステージ
            case 0:
                _preStage = Instantiate(_demoStage, Vector2.zero, Quaternion.identity);
                break;
            //ステージレベル１
            case ChangeGenerateStageLevel.StageLevel_1:
                _preStage = Instantiate(_firstLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;

            //ステージレベル２
            case ChangeGenerateStageLevel.StageLevel_2:
                _preStage = Instantiate(_secondLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;

            //ステージレベル３
            case ChangeGenerateStageLevel.StageLevel_3:
                _preStage = Instantiate(_thirdLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;
            //ステージレベル4
            case ChangeGenerateStageLevel.StageLevel_4:
                _preStage = Instantiate(_forthLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;
            case ChangeGenerateStageLevel.Stagelevel_5:
                _preStage = Instantiate(_fifthLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;
        }

        if (!_isPlayerDeathRetry)
        {
            BackGroundChange();
        }
        _isPlayerDeathRetry = false;
        _currentStageNumber++;

        string setTriggerName = "isStart";
        _readyAnimation.SetTrigger(setTriggerName);
        _preStageCoreScript = _preStage.GetComponent<StageCore>();
        _loadAnimation.gameObject.transform.position = _preStageCoreScript.GivePlayerSpwanPoint();

        //ゲーム開始
        _preStageCoreScript.StageStart(_thisScript, _player, elapsedTimeText);

        int playerGravity = 5;
        _playerRB.gravityScale = playerGravity;


        //レベルごとの最終ステージだったら次のレベルへ進む
        if (_currentStageNumber > MAX_STAGE_NUMBER)
        {
            _isNextStageLevel = true;

            if (_currentStageLevel == MAX_STAGE_LEVEL)
            {
                _currentStageLevel = 0;
            }
            else
            {
                _currentStageLevel++;
            }

            SetStageLevel(_currentStageLevel);
            _currentStageNumber = 0;
        }
    }

    private void BackGroundChange()
    {
        _bgmAudio.clip = _stageBGMs[_currentStageLevel];
        _bgmAudio.volume = 0.25f;
        _bgmAudio.Play();
    }

    public void NextStage()
    {
        _loadAnimation.gameObject.transform.position = this.transform.position;

        string setTriggerName = "Load";
        //ロードアニメーションを流す
        _loadAnimation.SetTrigger(setTriggerName);
    }

    public void RetryStage(bool isPlayerDeath)
    {
        _loadAnimation.gameObject.transform.position = this.transform.position;
        _currentStageNumber--;
        _isPlayerDeathRetry = isPlayerDeath;
        if (_isNextStageLevel)
        {
            _currentStageNumber = MAX_STAGE_NUMBER;
            if (_currentStageLevel == 0)
            {
                _currentStageLevel = MAX_STAGE_LEVEL;
            }
            else
            {
                _currentStageLevel--;
            }

            SetStageLevel(_currentStageLevel);
            _isNextStageLevel = false;
        }

        string setTriggerName = "Load";
        //ロードアニメーションを流す
        _loadAnimation.SetTrigger(setTriggerName);
    }

    public void BackTitle()
    {
        _player.SetActive(false);
        _preStage.SetActive(false);
        elapsedTimeText.text = default;
        _titleParent.SetActive(true);
        _bgmAudio.clip = _stageBGMs[0];
        _bgmAudio.volume = 0.25f;
        _bgmAudio.Play();
    }

    public void EndReadyAnimation()
    {
        _playerControll.controllOn();
        _preStageCoreScript.StartStageStrategy();
    }

    public void BGM_BolumeDown()
    {
        _bgmAudio.volume = 0.15f;
    }
}
