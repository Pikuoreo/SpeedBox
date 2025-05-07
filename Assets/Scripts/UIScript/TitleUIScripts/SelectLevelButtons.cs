using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButtons : MonoBehaviour
{

    private bool _isVisit = false;

    [SerializeField] private GameProgress _progressScript = default;
    [SerializeField] private SaveManager _saveManager = default;

    [SerializeField] private int _stageLevel = default;
    [SerializeField] private int _stageNumber = default;

    [SerializeField] private Sprite _stageSprite = default;
    [SerializeField] private Image _evaluationStar = default;
    [SerializeField] private List<Sprite> _evaluationStarImages = new List<Sprite>();
    [SerializeField] private TextMeshProUGUI _stageNumberText = default;

    // Start is called before the first frame update
    void Awake()
    {

        _stageNumberText.text = "Stage" + _stageLevel + "_" + _stageNumber;
    }

    private void OnEnable()
    {
        if (_evaluationStar != null)
        {
            _evaluationStar.sprite = _evaluationStarImages[_saveManager.LoadStageData(_stageLevel, _stageNumber).Item2];
        }

        if (_isVisit)
        {

            return;
        }
        else
        {
            _isVisit = _saveManager.LoadStageData(_stageLevel, _stageNumber).Item3;
            if (_isVisit)
            {
                this.GetComponent<Image>().sprite = _stageSprite;
            }
        }
    }

    public void OnClick()
    {
        _progressScript.SetStageLevel(_stageLevel);
        _progressScript.SetStageNumber(_stageNumber);
    }

}
