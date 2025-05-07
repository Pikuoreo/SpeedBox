using System.Collections.Generic;
using UnityEngine;

public class ChangeSelectLevel : MonoBehaviour
{
    [SerializeField] List<GameObject> _selectLevelObjects = new List<GameObject>();

    [SerializeField] private GameObject _previewSelectLevelButton = default;
    [SerializeField] private GameObject _nextSelectLevelButton = default;

    private GameObject _currentStageLevel = default;

    private int _currentStageLevelValue = 0;

    private int MaxStageLevelValue = 0;
    private const int MIN_STAGE_LEVEL_VALUE = 0;


    private void Start()
    {
        _currentStageLevel = _selectLevelObjects[0];
        MaxStageLevelValue = _selectLevelObjects.Count - 1;
    }

    public void PreviewSelectLevels()
    {
        //現在のステージ選択を消す
        _currentStageLevel.SetActive(false);

        //ひとつ後ろを見る
        _currentStageLevelValue--;

        //現在のステージ選択をひとつ後ろへ
        _currentStageLevel = _selectLevelObjects[_currentStageLevelValue];

        //ステージ選択を可視化
        _currentStageLevel.SetActive(true);

        if (!_nextSelectLevelButton.activeSelf)
        {
            _nextSelectLevelButton.SetActive(true);
        }

        //一番最初のステージ選択ならひとつ前を見るボタンを消す
        if (_currentStageLevelValue == MIN_STAGE_LEVEL_VALUE)
        {
            _previewSelectLevelButton.SetActive(false);
        }

    }

    public void NextSelectLevels()
    {
        //現在のステージ選択を消す
        _currentStageLevel.SetActive(false);

        //ひとつ前を見る
        _currentStageLevelValue++;

        //現在のステージ選択をひとつ前へ
        _currentStageLevel = _selectLevelObjects[_currentStageLevelValue];

        //ステージ選択を可視化
        _currentStageLevel.SetActive(true);

        if (!_previewSelectLevelButton.activeSelf)
        {
            _previewSelectLevelButton.SetActive(true);
        }

        //一番最初のステージ選択ならひとつ前を見るボタンを消す
        if (_currentStageLevelValue == MaxStageLevelValue)
        {
            _nextSelectLevelButton.SetActive(false);
        }
    }

}
