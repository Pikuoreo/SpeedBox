using UnityEngine;

public class ConveyorFloor : MonoBehaviour
{
    private PlayerController _playerControll = default;
    [SerializeField] private bool _conveyorDirection = false;//false�͍������Atrue�͉E����
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }

            //�R���x�A�̌������E������������
            if (_conveyorDirection)
            {
                _playerControll.AutoRightMove();
            }
            //�R���x�A�̌�������������������
            else
            {
                _playerControll.AutoLeftMove();
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�v���C���[���R���x�A���痣�ꂽ��
        if (collision.gameObject.layer == 6)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }
            //�ړ����x�����Ƃɖ߂�
            _playerControll.ExitConveyor();
        }
    }
}
