using UnityEngine;

public class ScreenEdgeWorp : MonoBehaviour
{
    [SerializeField] private Transform _oppositionScreenEdge = default;
    [SerializeField] private bool _isHorizontalScreenEdge = false;//false�͂��Ẳ�ʒ[�g���K�[�Atrue�͉��̉�ʒ[�g���K�[
    [SerializeField] private bool _isFloorTrigger = false;
    private const float WARP_POINT_ADJUSTMENT = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(this.gameObject.name);
        const int PLAYERLAYER = 6;

        if (collision.gameObject.layer == PLAYERLAYER)
        {
            Transform _playerPositoin = collision.gameObject.transform;

            //�����̉�ʒ[�g���K�[�ɐG�ꂽ��
            if (_isHorizontalScreenEdge)
            {
                //�G�ꂽ�g���K�[�̔��΂ɂ����ʒ[��X���݈̂ړ�����
                _playerPositoin.position = new Vector3(_oppositionScreenEdge.position.x, _playerPositoin.position.y);
            }
            //�c���̉�ʒ[�g���K�[�ɐG�ꂽ��
            else
            {
                if (_isFloorTrigger)
                {
                    // �G�ꂽ�g���K�[�̔��΂ɂ����ʒ[��Y����菭�����Ɉړ�����
                    _playerPositoin.position = new Vector3(_playerPositoin.position.x, _oppositionScreenEdge.position.y - WARP_POINT_ADJUSTMENT);

                    //�������x���O�ɂ���
                    Rigidbody2D _playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                    _playerRB.velocity = new Vector2(_playerRB.velocity.x, 0);
                }
                else
                {
                    // �G�ꂽ�g���K�[�̔��΂ɂ����ʒ[��Y���݈̂ړ�����
                    _playerPositoin.position = new Vector3(_playerPositoin.position.x, _oppositionScreenEdge.position.y + WARP_POINT_ADJUSTMENT);
                }
            }
        }
    }
}
