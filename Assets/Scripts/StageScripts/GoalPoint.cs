using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private StageCore _stageCore = default;
    private void Start()
    {
        _stageCore = this.GetComponentInParent<StageCore>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = 6;
        if (collision.gameObject.layer == playerLayer)
        {

            //�v���C���[�𑀍�s�\�ɂ���
            collision.gameObject.GetComponent<PlayerController>().ControllOff();

            //�N���A�����n��
            _stageCore.StageClear();
        }
    }
}
