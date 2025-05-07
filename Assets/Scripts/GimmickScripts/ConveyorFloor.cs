using UnityEngine;

public class ConveyorFloor : MonoBehaviour
{
    private PlayerController _playerControll = default;
    [SerializeField] private bool _conveyorDirection = false;//falseは左向き、trueは右向き
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }

            //コンベアの向きが右向きだったら
            if (_conveyorDirection)
            {
                _playerControll.AutoRightMove();
            }
            //コンベアの向きが左向きだったら
            else
            {
                _playerControll.AutoLeftMove();
            }

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
            //移動速度をもとに戻す
            _playerControll.ExitConveyor();
        }
    }
}
