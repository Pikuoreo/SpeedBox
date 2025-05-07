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

            //プレイヤーを操作不能にする
            collision.gameObject.GetComponent<PlayerController>().ControllOff();

            //クリア判定を渡す
            _stageCore.StageClear();
        }
    }
}
