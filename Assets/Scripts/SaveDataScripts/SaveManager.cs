using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Z�[�u�f�[�^��ǂݏ�������N���X
/// </summary>
public class SaveManager : MonoBehaviour
{
    //�Z�[�u��̃p�X
    private string _saveFilePath = null;

    //�Z�[�u�f�[�^�̏W����⊮���Ă���Ƃ���
    private StageSaveData _data = null;

    //�^�C�g���̎��S�J�E���g�̃e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _allDeathCountText = default;

    //�e�L�X�g�ɂ���镶��
    private string _allDeathCountTextCharacter = "AllDeathCount:";


    const string SAVE_FILE_NAME = "saveData.json";
    private void Awake()
    {
        //�Z�[�u��̃p�X����
        _saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        //�f�B���N�g���̖��O���擾
        string directoryPath = Path.GetDirectoryName(_saveFilePath);

        //�f�B���N�g��������������
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            //�f�B���N�g�������
            Directory.CreateDirectory(directoryPath);
        }

        _data = new StageSaveData();

        //���܂ł̎��S�񐔂���
        _allDeathCountText.text = _allDeathCountTextCharacter + GetAllDeathCount().ToString();
    }

    /// <summary>
    /// �w�肵���X�e�[�W�Ƀf�[�^��ۑ�
    /// </summary>
    /// <param name="stageLevel">�f�[�^�����̂��߂̃X�e�[�W���x��</param>
    /// <param name="stageNumber">�f�[�^�����̂��߂̃X�e�[�W�ԍ�</param>
    /// <param name="stageClearTime">�X�e�[�W�̃N���A����</param>
    /// <param name="clearEvaluation">�N���A�������̕]����</param>

    public void SaveStageData(int stageLevel, int stageNumber, float stageClearTime, int clearEvaluation)
    {
        // �X�e�[�W�����łɃ��X�g�ɑ��݂��邩�m�F
        StageData stageData = _data.SaveDataLists.Find(stageDataList => stageDataList.SaveStageLevel == stageLevel &&
                                                                 stageDataList.SaveStageNumber == stageNumber);

        //�X�e�[�W�̃Z�[�u�f�[�^������������
        if (stageData == null)
        {
            // �X�e�[�W��������Ȃ���ΐV�����ǉ�
            stageData = new StageData { SaveStageLevel = stageLevel, SaveStageNumber = stageNumber, StageClearTime = stageClearTime, StageClearStar = clearEvaluation };
            stageData.IsStageVisit = true;
            _data.SaveDataLists.Add(stageData);
        }
        //�Z�[�u�f�[�^�����݂��Ă�����
        else
        {
            //�f�[�^���X�V
            stageData.StageClearTime = stageClearTime;
            stageData.StageClearStar = clearEvaluation;
        }

        //�Z�[�u�f�[�^�̓��e��Json�`���ɕϊ�
        string saveContent = JsonUtility.ToJson(_data, true);

        //�Z�[�u�f�[�^����������
        File.WriteAllText(_saveFilePath, saveContent);
    }

    /// <summary>
    /// ���܂ł̎��S�񐔂𑝂₷�E��������
    /// </summary>
    public void IncrementAllDeathCount()
    {
        //���łɃZ�[�u�f�[�^���ۑ�����Ă���Ȃ�f�[�^��ǂݍ���
        if (IsSaveFileExists())
        {
            _data.AllDeathCountData++;

            //�Z�[�u�f�[�^�̓��e��Json�`���ɕϊ�
            string deathCount = JsonUtility.ToJson(_data, true);

            //�Z�[�u�f�[�^����������
            File.WriteAllText(_saveFilePath, deathCount);

            _allDeathCountText.text = _allDeathCountTextCharacter + _data.AllDeathCountData.ToString();
        }
    }

    /// <summary>
    /// ���܂ł̎��S�񐔂�ǂݎ��
    /// </summary>
    /// <returns>���܂ł̎��S�񐔂�Ԃ�</returns>
    public int GetAllDeathCount()
    {
        _data = new StageSaveData();

        //���łɃZ�[�u�f�[�^���ۑ�����Ă���Ȃ�f�[�^��ǂݍ���
        if (IsSaveFileExists())
        {
            string deathCountData = File.ReadAllText(_saveFilePath);

            if (!string.IsNullOrEmpty(deathCountData))
            {
                _data = JsonUtility.FromJson<StageSaveData>(deathCountData);
            }

            return _data.AllDeathCountData;
        }
        else
        {
            print("���S�񐔂̃f�[�^������܂���B");
            return 0;

        }
    }

    /// <summary>
    /// �X�e�[�W�f�[�^�̓ǂݍ���
    /// </summary>
    /// <param name="stageLevel">�X�e�[�W�̃��x�������ϐ�</param>
    /// <param name="stageNumber">�X�e�[�W�̔ԍ������ϐ�</param>
    /// <returns>�N���A���ԁA�N���A���̐��̐��A��x�v���C�������ǂ�����Ԃ�</returns>
    public (float, int, bool) LoadStageData(int stageLevel, int stageNumber)
    {
        //���łɃZ�[�u�f�[�^���ۑ�����Ă���Ȃ�f�[�^��ǂݍ���
        if (IsSaveFileExists())
        {
            //�t�@�C��������e��ǂݍ���
            string dataContent = File.ReadAllText(_saveFilePath);

            // JSON����f�[�^�𕜌�
            _data = JsonUtility.FromJson<StageSaveData>(dataContent);

            StageData stageData = _data.SaveDataLists.Find(stages => stages.SaveStageNumber == stageNumber && stages.SaveStageLevel == stageLevel);

            //�X�e�[�W�Ɉ�x�ł����킵�����Ƃ�����ꍇ
            if (stageData != null)
            {
                return (stageData.StageClearTime,
                        stageData.StageClearStar,
                        stageData.IsStageVisit);
            }
            //�X�e�[�W�Ɉ�x�����킵�����Ƃ��Ȃ��ꍇ
            else
            {
                //print(stageNumber + "�ɗ������Ƃ�����܂���");
                return (0, 0, false);
            }
        }
        //�Z�[�u�f�[�^���Ȃ��ꍇ
        else
        {
            return (0, 0, false);
        }
    }

    /// <summary>
    /// �Z�[�u�f�[�^�̍폜
    /// </summary>
    public void ClearSaveData()
    {
        if (!File.Exists(_saveFilePath))
        {
            print("aaa");
            return;
        }

        File.Delete(_saveFilePath);

        // ���݂̃V�[�������擾
        string currentSceneName = SceneManager.GetActiveScene().name;

        // �V�[�����ēǂݍ���
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// �Z�[�u��Ƀt�@�C�������邩�ǂ���������
    /// </summary>
    /// <returns>�Z�[�u�f�[�^�����邩�̐^�U��Ԃ�</returns>
    private bool IsSaveFileExists()
    {

        return File.Exists(_saveFilePath);

    }

}
