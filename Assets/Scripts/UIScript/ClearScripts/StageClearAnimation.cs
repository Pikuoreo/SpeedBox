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

    private float _clearTime = 0; //今回のクリア時間
    private float _bestClearTime = default;

    private const float PITCH_INCREASE_VALUE = 0.5f;

    private Animator _clearAnimation = default;

    [SerializeField] private GameObject _clearSceneStage = default;

    [SerializeField] private TextMeshProUGUI _bestClearTimeText = default;//最速クリアタイムのテキスト
    [SerializeField] private TextMeshProUGUI _clearTimetext = default;//今回のクリア時間のテキスト
    [SerializeField] private TextMeshProUGUI _stageClearText = default;

    [SerializeField] private List<GameObject> _evaluationStars = new List<GameObject>();//評価星
    [SerializeField] private List<GameObject> _selectButtons = new List<GameObject>();//次の行動のボタン

    [Header("星1のクリア時間テキスト"), SerializeField] private TextMeshProUGUI _singleStarCleaTimetext = default;
    [Header("星2のクリア時間テキスト"), SerializeField] private TextMeshProUGUI _doubleStarCleaTimetext = default;
    [Header("星3のクリア時間テキスト"), SerializeField] private TextMeshProUGUI _tripleStarCleaTimetext = default;

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
    /// クリア時の最初のアニメーション
    /// </summary>
    /// <param name="stageClearTime">クリア時間</param>
    /// <param name="singleStarTime">星１のクリア時間</param>
    /// <param name="doubleStarTime">星２のクリア時間</param>
    /// <param name="tripleStarTime">星３のクリア時間</param>
    public void StartStageClearAnimation(float stageClearTime, float bestClearTime, float singleStarTime, float doubleStarTime, float tripleStarTime, int judgeClearEvalution)
    {

        //クリアタイムを代入
        _clearTime = stageClearTime;
        _bestClearTime = bestClearTime;

        //各評価星ごとのクリア時間を代入
        _singleStarCleaTimetext.text = singleStarTime.ToString("F3") + "↑";
        _doubleStarCleaTimetext.text = doubleStarTime.ToString("F3") + "↑";
        _tripleStarCleaTimetext.text = tripleStarTime.ToString("F3") + "↑";

        _clearEvalutionNumber = judgeClearEvalution;

        _bestClearTimeText.text = "BestClearTime:" + bestClearTime.ToString("F3");

        //int型のアニメーションパラメータを１上げる
        _animationNumber++;
        //クリアアニメーションスタート
        _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
    }

    /// <summary>
    /// クリア時の二番目のアニメーション
    /// </summary>
    public void DisplayClearStageScene()
    {
        _clearSceneStage.SetActive(true);
    }

    public void FirstTextMoveAnimation()
    {
        //int型のアニメーションパラメータを１上げる
        _animationNumber++;
        //アニメーションを流す
        _clearAnimation.SetInteger("ClearAnimation", _animationNumber);
    }

    /// <summary>
    /// クリア時間と星の表示アニメーション
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
    /// 星評価のアニメーション
    /// </summary>
    public void StartJudgeClearEvaluationAnimation()
    {
        _clearAnimation.SetInteger("EvaluationStarValue", _clearEvalutionNumber);
    }

    /// <summary>
    /// クリア時間に応じた星評価の数を返す
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
    /// ボタンの表示
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
