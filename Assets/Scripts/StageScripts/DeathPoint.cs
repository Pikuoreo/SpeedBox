using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    private StageCore _stageCore = default;
    private void Start()
    {
        _stageCore = this.GetComponentInParent<StageCore>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int playerLayer = 6;
        if (collision.gameObject.layer == playerLayer)
        {
            _stageCore.GameOver();
            collision.gameObject.GetComponent<PlayerController>().Death();
        }
    }
}
