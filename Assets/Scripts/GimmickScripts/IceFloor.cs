using UnityEngine;

public class IceFloor : MonoBehaviour
{
    private PlayerController _playerControll = default;
    private void OnCollisionStay2D(Collision2D collision)
    {
        int playerLayer = 6;
        if (collision.gameObject.layer == playerLayer)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }

            _playerControll.JudgeOnIceFloor(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //プレイヤーがコンベアから離れたら
        if (collision.gameObject.layer == 6)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }
            _playerControll.JudgeOnIceFloor(false);
        }
    }
}
