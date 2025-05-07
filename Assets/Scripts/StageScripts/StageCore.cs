using TMPro;
using UnityEngine;

public class StageCore : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnpoint = default;//�v���C���[�̏o���ʒu

    [SerializeField] private BoxCollider2D _goalPointCollider = default;//�S�[���|�C���g�̓����蔻��

    [SerializeField] private StageClearAnimation _stageClearAnimationScript = default;
    [SerializeField] private GameOverAnimation _gameOverAnimationScript = default;

    private PlayerController _playerController = default;
    private GameProgress _gameProgress = default;

    [Header("��1�̃N���A����"), SerializeField] private float _singleStarCleartime = default;
    [Header("��2�̃N���A����"), SerializeField] private float _doubleStarCleartime = default;
    [Header("��3�̃N���A����"), SerializeField] private float _tripleStarCleartime = default;

    private TextMeshProUGUI elapsedTimeText = default;//�������Ԃ̃e�L�X�g

    private GameObject _player = default;

    private float _timeLimit = 10f;//��������

    private bool _isGoal = false;//true�ŃS�[������
    private bool _isGameOver = false;//true�ŃQ�[���I�[�o�[����
    private bool _isGameStart = false;//true�ŃQ�[���J�n����]

    [SerializeField] private int _stageLevel = default;
    [SerializeField] private int _stageNumber = default;

    private SaveManager _saveManager = default;

    private void Start()
    {

        //�Z�[�u�f�[�^��ۊǂ��Ă���I�u�W�F�N�g��T��
        _saveManager = GameObject.FindGameObjectWithTag("SaveData").GetComponent<SaveManager>();

        //�X�e�[�W�ɒ��킵�����Ƃ��Ȃ�������
        if (_saveManager.LoadStageData(_stageLevel, _stageNumber).Item1 == 0)
        {
            //�X�e�[�W�̃Z�[�u�f�[�^�����
            _saveManager.SaveStageData(_stageLevel, _stageNumber, 0, 0);
        }
    }

    private void Update()
    {
        //�������Ԃ����炵�Ă���
        if (_isGameStart)
        {
            TimeMeasurement();
        }
    }

    /// <summary>
    /// �N���A���̕]�����̐���n��
    /// </summary>
    /// <returns></returns>
    private int JudgeClearEvaluation()
    {
        //�N���A���Ԃ��R�����Ԃ�葁��������
        if (_timeLimit >= _tripleStarCleartime)
        {
            //3���]����Ԃ�
            int tripleStarEvaluation = 3;

            return tripleStarEvaluation;
        }
        //�N���A���Ԃ��Q�����Ԃ�葁��������
        else if (_timeLimit >= _doubleStarCleartime)
        {
            //�����Ԃ�
            int doubleStarEvaluation = 2;

            return doubleStarEvaluation;
        }
        else
        {
            //�����Ԃ�
            int singleStarEvaluation = 1;

            return singleStarEvaluation;
        }
    }

    private void TimeMeasurement()
    {
        //���Ԃ��O���傫��������
        if (_timeLimit > 0 && !_isGameOver)
        {
            //���Ԃ����炵�Ă���
            _timeLimit -= Time.deltaTime;
            elapsedTimeText.text = _timeLimit.ToString("F3");

            //�����I��
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _playerController.ControllOff();
                //�X�e�[�W�̋����I��
                _gameProgress.BackTitle();
            }

            //�������g���C
            if (Input.GetKeyDown(KeyCode.R))
            {
                _isGameOver = true;
                _playerController.ControllOff();
                RestartStage();
            }

        }
        else if (!_isGameOver)
        {
            //�Q�[���I�[�o�[
            elapsedTimeText.text = "";
            GameOver();
        }



    }
    public void StageStart(GameProgress gameProgressScript, GameObject playerObject, TextMeshProUGUI timeText)
    {
        //�X�N���v�g�̎擾
        _gameProgress = gameProgressScript;

        //�e�L�X�g�̑��
        elapsedTimeText = timeText;

        //�v���C���[�̎擾
        _player = playerObject;
        _player.transform.position = _playerSpawnpoint.position;
        _playerController = _player.GetComponent<PlayerController>();
        _player.SetActive(true);

        //�S�[���|�C���g�̓����蔻����o��
        _goalPointCollider.enabled = true;
    }

    public void StageClear()
    {
        if (!_isGoal)
        {
            elapsedTimeText.text = "";
            //�N���A�������A�w�i��BGM���ʂ�������������
            _gameProgress.BGM_BolumeDown();

            _isGameStart = false;
            _isGoal = true;

            float _bestStageClearTime = _saveManager.LoadStageData(_stageLevel, _stageNumber).Item1;

            int clearEvalutionValue = JudgeClearEvaluation();

            //�N���A���Ԃ��x�X�g�N���A���Ԃ�������������N���A���Ԃ�n��
            if (_bestStageClearTime <= _timeLimit)
            {
                //�ŐV�̃N���A���Ԃ��L�^����
                _saveManager.SaveStageData(_stageLevel, _stageNumber, _timeLimit, clearEvalutionValue);
                //�Q�[���N���A�̃A�j���[�V�����𗬂�
                _stageClearAnimationScript.StartStageClearAnimation(_timeLimit, _timeLimit, _singleStarCleartime, _doubleStarCleartime, _tripleStarCleartime, clearEvalutionValue);
            }
            //�N���A���Ԃ��x�X�g�N���A���Ԃ����x��������x�X�g�N���A���Ԃ�n��
            else
            {
                //�Q�[���N���A�̃A�j���[�V�����𗬂�
                _stageClearAnimationScript.StartStageClearAnimation(_timeLimit, _bestStageClearTime, _singleStarCleartime, _doubleStarCleartime, _tripleStarCleartime, clearEvalutionValue);
            }
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        //�v���C���[�𓮂��Ȃ�����
        _playerController.ControllOff();

        //�S�[���̓����蔻�������
        _goalPointCollider.enabled = false;

        //�Q�[���I�[�o�[�̃A�j���[�V�����𗬂�
        _gameOverAnimationScript.StartGameOverAnimation();

        _saveManager.IncrementAllDeathCount();

    }
    public void RestartStage()
    {
        //�X�e�[�W�Ē���
        _gameProgress.RetryStage(true);
    }

    public void StartStageStrategy()
    {
        //�X�e�[�W�J�n
        _isGameStart = true;
    }

    public Vector2 GivePlayerSpwanPoint()
    {
        //�v���C���[�̏o���ʒu��n��
        return _playerSpawnpoint.position;
    }

    public (int, int) GiveStageName()
    {
        //�X�e�[�W�̃��x���A�ԍ���n��
        return (_stageLevel, _stageNumber);
    }

    public void GetTimePlusItem(float plusValue)
    {
        _timeLimit += plusValue;
    }
}
