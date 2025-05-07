using UnityEngine;

/// <summary>
/// �X�e�[�W���̕ێ��������Z�[�u�f�[�^�Q���Ǘ�����N���X
/// </summary>
[System.Serializable]
public class StageData
{
    //�Z�[�u�������X�e�[�W�̃��x��
    [SerializeField] private int _saveStageLevel = default;

    //�Z�[�u�������X�e�[�W�̔ԍ�
    [SerializeField] private int _saveStageNumber = default;

    //�X�e�[�W�̃N���A����
    [SerializeField] private float _stageCleartime = 0;

    //�R�i�K�]���̐�
    [SerializeField] private int _stageClearstar = 0;

    //�X�e�[�W�Ɉ�x�ł��K�ꂽ���Ƃ����邩
    [SerializeField] private bool _isStageVisit = false;


    //�Z�[�u�������X�e�[�W�̃��x��
    public int SaveStageLevel
    {
        get { return _saveStageLevel; }
        set { _saveStageLevel = value; }
    }

    //�Z�[�u�������X�e�[�W�̔ԍ�
    public int SaveStageNumber
    {
        get { return _saveStageNumber; }
        set { _saveStageNumber = value; }
    }

    //�X�e�[�W�̃N���A����
    public float StageClearTime
    {
        get { return _stageCleartime; }
        set { _stageCleartime = value; }
    }

    //�R�i�K�]���̐�
    public int StageClearStar
    {
        get { return _stageClearstar; }
        set { _stageClearstar = value; }
    }

    //�X�e�[�W�Ɉ�x�ł��K�ꂽ���Ƃ����邩
    public bool IsStageVisit
    {
        get { return _isStageVisit; }
        set { _isStageVisit = value; }

    }
}
