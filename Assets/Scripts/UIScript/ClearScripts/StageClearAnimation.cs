using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageClearAnimation : MonoBehaviour
{
    private int _animationNumber = default;
    private int _clearEvalutionNumber = default;

    private const int DEFAULT_PITCH = 1;
    private const int DEFAULT_CLEARTIME_ANIMATION = 3;
    private const int BEST_CLEARTIME_ANIMATION = 4;

    private float _clearTime = 0; //����̃N���A����
    private float _bestClearTime = default;

    private const float PITCH_INCREASE_VALUE = 0.5f;

    private Animator _clearAnimation = default;

    [SerializeField] private GameObject _clearSceneStage = default;

    [SerializeField] private TextMeshProUGUI _bestClearTimeText = default;//�ő��N���A�^�C���̃e�L�X�g
    [SerializeField] private TextMeshProUGUI _clearTimetext = default;//����̃N���A���Ԃ̃e�L�X�g
    [SerializeField] private TextMeshProUGUI _stageClearText = default;

    [SerializeField] private List<GameObject> _evaluationStars = new List<GameObject>();//�]����
    [SerializeField] private List<GameObject> _selectButtons = new List<GameObject>();//���̍s���̃{�^��

    [Header("��1�̃N���A���ԃe�L�X�g"), SerializeField] private TextMeshProUGUI _singleStarCleaTimetext = default;
    [Header("��2�̃N���A���ԃe�L�X�g"), SerializeField] private TextMeshProUGUI _doubleStarCleaTimetext = default;
    [Header("��3�̃N���A���ԃe�L�X�g"), SerializeField] private TextMeshProUGUI _tripleStarCleaTimetext = default;

    private AudioSource _seAudio = default;
    [SerializeField] private AudioClip _starSE = default;
    [SerializeField] private AudioClip _blessingSE = default;

    // Start is called before the first frame update
    void Start()
    {
        _clearAnimation = this.GetComponent<Animator>();
        _seAudio = this.GetComponent<AudioSource>();
        ClearTextChange();
    }

    /// <summary>
    /// �N���A���̍ŏ��̃A�j���[�V����
    /// </summary>
    /// <param name="stageClearTime">�N���A����</param>
    /// <param name="singleStarTime">���P�̃N���A����</param>
    /// <param name="doubleStarTime">���Q�̃N���A����</param>
    /// <param name="tripleStarTime">���R�̃N���A����</param>
    public void StartStageClearAnimation(float stageClearTime, float bestClearTime, float singleStarTime, float doubleStarTime, float tripleStarTime, int judgeClearEvalution)
    {

        //�N���A�^�C������
        _clearTime = stageClearTime;
        _bestClearTime = bestClearTime;

        //�e�]�������Ƃ̃N���A���Ԃ���
        _singleStarCleaTimetext.text = singleStarTime.ToString("F3") + "��";
        _doubleStarCleaTimetext.text = doubleStarTime.ToString("F3") + "��";
        _tripleStarCleaTimetext.text = tripleStarTime.ToString("F3") + "��";

        _clearEvalutionNumber = judgeClearEvalution;

        _bestClearTimeText.text = "BestClearTime:" + bestClearTime.ToString("F3");

        //int�^�̃A�j���[�V�����p�����[�^���P�グ��
        _animationNumber++;
        //�N���A�A�j���[�V�����X�^�[�g
        _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
    }

    /// <summary>
    /// �N���A���̓�Ԗڂ̃A�j���[�V����
    /// </summary>
    public void DisplayClearStageScene()
    {
        _clearSceneStage.SetActive(true);
    }

    public void FirstTextMoveAnimation()
    {
        //int�^�̃A�j���[�V�����p�����[�^���P�グ��
        _animationNumber++;
        //�A�j���[�V�����𗬂�
        _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
    }

    /// <summary>
    /// �N���A���ԂƐ��̕\���A�j���[�V����
    /// </summary>
    public IEnumerator StartClearEvaluationAnimation(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        foreach (GameObject _clearEvaluationStars in _evaluationStars)
        {
            _clearEvaluationStars.SetActive(true);
        }

        _clearTimetext.enabled = true;
        _clearTimetext.text = "ClearTime:" + _clearTime.ToString("F3");

        StartJudgeClearEvaluationAnimation();
        yield break;
    }
    /// <summary>
    /// ���]���̃A�j���[�V����
    /// </summary>
    public void StartJudgeClearEvaluationAnimation()
    {
        _clearAnimation.SetInteger("EvaluationStarValue", _clearEvalutionNumber);
    }

    /// <summary>
    /// �N���A���Ԃɉ��������]���̐���Ԃ�
    /// </summary>
    /// <returns></returns>

    public void BestClearTimeEvaluation()
    {

        if (_clearTime >= _bestClearTime)
        {
            _animationNumber = BEST_CLEARTIME_ANIMATION;
            _bestClearTimeText.color = Color.yellow;
            _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
        }
        else
        {
            _animationNumber = DEFAULT_CLEARTIME_ANIMATION;
            _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
        }

    }
    /// <summary>
    /// �{�^���̕\��
    /// </summary>
    public void SelectButtonDisplay()
    {
        foreach (GameObject buttons in _selectButtons)
        {
            buttons.SetActive(true);
        }
    }
    public void PlayStarSE()
    {
        _seAudio.PlayOneShot(_starSE);
        _seAudio.pitch += PITCH_INCREASE_VALUE;
    }

    public void PlayBlessingSE()
    {
        _seAudio.pitch = DEFAULT_PITCH;
        _seAudio.PlayOneShot(_blessingSE);
    }
    private void ClearTextChange()
    {
        StageCore stageCoreScript = this.GetComponentInParent<StageCore>();

        int stageLevel = stageCoreScript.GiveStageName().Item1;
        int stageNumber = stageCoreScript.GiveStageName().Item2;
        _stageClearText.text = "Stage" + stageLevel + "_" + stageNumber + " " + "Clear!!";
    }
}
