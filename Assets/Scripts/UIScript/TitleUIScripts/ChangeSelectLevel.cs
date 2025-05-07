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
        //���݂̃X�e�[�W�I��������
        _currentStageLevel.SetActive(false);

        //�ЂƂ�������
        _currentStageLevelValue--;

        //���݂̃X�e�[�W�I�����ЂƂ���
        _currentStageLevel = _selectLevelObjects[_currentStageLevelValue];

        //�X�e�[�W�I��������
        _currentStageLevel.SetActive(true);

        if (!_nextSelectLevelButton.activeSelf)
        {
            _nextSelectLevelButton.SetActive(true);
        }

        //��ԍŏ��̃X�e�[�W�I���Ȃ�ЂƂO������{�^��������
        if (_currentStageLevelValue == MIN_STAGE_LEVEL_VALUE)
        {
            _previewSelectLevelButton.SetActive(false);
        }

    }

    public void NextSelectLevels()
    {
        //���݂̃X�e�[�W�I��������
        _currentStageLevel.SetActive(false);

        //�ЂƂO������
        _currentStageLevelValue++;

        //���݂̃X�e�[�W�I�����ЂƂO��
        _currentStageLevel = _selectLevelObjects[_currentStageLevelValue];

        //�X�e�[�W�I��������
        _currentStageLevel.SetActive(true);

        if (!_previewSelectLevelButton.activeSelf)
        {
            _previewSelectLevelButton.SetActive(true);
        }

        //��ԍŏ��̃X�e�[�W�I���Ȃ�ЂƂO������{�^��������
        if (_currentStageLevelValue == MaxStageLevelValue)
        {
            _nextSelectLevelButton.SetActive(false);
        }
    }

}
