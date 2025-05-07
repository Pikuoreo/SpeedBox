using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    private Rigidbody2D _playerRB = default;

    [Header("���x���P�̃X�e�[�W�R"), SerializeField] private List<GameObject> _firstLevelStages = new List<GameObject>();
    [Header("���x���Q�̃X�e�[�W�R"), SerializeField] private List<GameObject> _secondLevelStages = new List<GameObject>();
    [Header("���x���R�̃X�e�[�W�R"), SerializeField] private List<GameObject> _thirdLevelStages = new List<GameObject>();
    [Header("���x���S�̃X�e�[�W�R"), SerializeField] private List<GameObject> _forthLevelStages = new List<GameObject>();
    [Header("���x���T�̃X�e�[�W�R"), SerializeField] private List<GameObject> _fifthLevelStages = new List<GameObject>();

    [SerializeField] private GameObject _player = default;
    [SerializeField] private GameObject _titleParent = default;//�X�e�[�W�Ɋւ���I�u�W�F�N�g
    [SerializeField] private GameObject _demoStage = default;

    private GameObject _preStage = default;//�ЂƂO�̃X�e�[�W

    [SerializeField] private TextMeshProUGUI elapsedTimeText = default;//�o�ߎ��ԃe�L�X�g



    [SerializeField] private Animator _loadAnimation = default;//���[�h�̃A�j���[�V����
    [SerializeField] private Animator _readyAnimation = default;

    private int _currentStageLevel = 0;//���݂̃X�e�[�W���x��
    private int _currentStageNumber = 0;//���݂̃X�e�[�W�ԍ�

    private const int MAX_STAGE_LEVEL = 5;
    private const int MAX_STAGE_NUMBER = 9;

    private bool _isNextStageLevel = false;
    private bool _isPlayerDeathRetry = false;//true�Ńv���C���[������Ń��g���C��������

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
        StageLevel_1 = 1,//�X�e�[�W���x���P
        StageLevel_2 = 2,//�X�e�[�W���x��2
        StageLevel_3 = 3,//�X�e�[�W���x��3
        StageLevel_4 = 4,//�X�e�[�W���x��4
        Stagelevel_5 = 5,//�X�e�[�W���x��5
        StageLevel_0 = 0 //�X�e�[�W���x��0
    }

    /// <summary>
    /// //��������X�e�[�W���x�����擾
    /// </summary>
    /// <param name="getStageLevel">�X�e�[�W���x��</param>
    public void SetStageLevel(int getStageLevel)
    {
        //��������X�e�[�W���x�����擾
        _currentStageLevel = getStageLevel;

        //�X�e�[�W���x���ɍ��킹��enum�ɕύX
        _generateStageLevel = (ChangeGenerateStageLevel)getStageLevel;
    }

    /// <summary>
    ///  //�X�e�[�W�𐶐�
    /// </summary>
    /// <param name="getStageNumber">�e�X�e�[�W�̔ԍ�</param>
    public void SetStageNumber(int getStageNumber)
    {
        //�^�C�g���Ɋւ�����̂������Ȃ�����
        _titleParent.SetActive(false);

        _loadAnimation.gameObject.transform.position = this.transform.position;
        //���[�h�A�j���[�V�����𗬂�
        _loadAnimation.SetTrigger("Load");

        //��������X�e�[�W�ԍ����擾
        _currentStageNumber = getStageNumber;

        //���X�g�p�Ɂ[�P�����Ē���
        _currentStageNumber--;
    }
    /// <summary>
    /// �X�e�[�W����
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
        //���łɃX�e�[�W��������Ă��������
        if (_preStage != null)
        {
            Destroy(_preStage);
        }

        //�X�e�[�W���x�����Ƃ̃X�e�[�W����
        switch (_generateStageLevel)
        {
            //�������X�e�[�W
            case 0:
                _preStage = Instantiate(_demoStage, Vector2.zero, Quaternion.identity);
                break;
            //�X�e�[�W���x���P
            case ChangeGenerateStageLevel.StageLevel_1:
                _preStage = Instantiate(_firstLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;

            //�X�e�[�W���x���Q
            case ChangeGenerateStageLevel.StageLevel_2:
                _preStage = Instantiate(_secondLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;

            //�X�e�[�W���x���R
            case ChangeGenerateStageLevel.StageLevel_3:
                _preStage = Instantiate(_thirdLevelStages[_currentStageNumber], Vector2.zero, Quaternion.identity);
                break;
            //�X�e�[�W���x��4
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

        //�Q�[���J�n
        _preStageCoreScript.StageStart(_thisScript, _player, elapsedTimeText);

        int playerGravity = 5;
        _playerRB.gravityScale = playerGravity;


        //���x�����Ƃ̍ŏI�X�e�[�W�������玟�̃��x���֐i��
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
        //���[�h�A�j���[�V�����𗬂�
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
        //���[�h�A�j���[�V�����𗬂�
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
