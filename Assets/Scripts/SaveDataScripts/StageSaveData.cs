using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SaveDatas�̏W���ƍ��܂ł̎��S�񐔂��Ǘ����Ă���N���X
/// </summary>
[System.Serializable]
public class StageSaveData
{
    //�X�e�[�W���Ƃ̃Z�[�u�f�[�^�̂܂Ƃ܂�
    [SerializeField] private List<StageData> _saveDataLists = new List<StageData>();

    //���܂ł̎��S��
    [SerializeField] private int _allDeathCount = 0;

    //�X�e�[�W���Ƃ̃Z�[�u�f�[�^�̂܂Ƃ܂�
    public List<StageData> SaveDataLists
    {
        get { return _saveDataLists; }
        set { _saveDataLists = value; }
    }

    //���܂ł̎��S��
    public int AllDeathCountData
    {
        get { return _allDeathCount; }
        set { _allDeathCount = value; }
    }
}
