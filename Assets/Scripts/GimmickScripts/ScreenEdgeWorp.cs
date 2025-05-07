using UnityEngine;

public class ScreenEdgeWorp : MonoBehaviour
{
    [SerializeField] private Transform _oppositionScreenEdge = default;
    [SerializeField] private bool _isHorizontalScreenEdge = false;//falseはたての画面端トリガー、trueは横の画面端トリガー
    [SerializeField] private bool _isFloorTrigger = false;
    private const float WARP_POINT_ADJUSTMENT = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(this.gameObject.name);
        const int PLAYERLAYER = 6;

        if (collision.gameObject.layer == PLAYERLAYER)
        {
            Transform _playerPositoin = collision.gameObject.transform;

            //横軸の画面端トリガーに触れたら
            if (_isHorizontalScreenEdge)
            {
                //触れたトリガーの反対にある画面端にX軸のみ移動する
                _playerPositoin.position = new Vector3(_oppositionScreenEdge.position.x, _playerPositoin.position.y);
            }
            //縦軸の画面端トリガーに触れたら
            else
            {
                if (_isFloorTrigger)
                {
                    // 触れたトリガーの反対にある画面端にY軸より少し下に移動する
                    _playerPositoin.position = new Vector3(_playerPositoin.position.x, _oppositionScreenEdge.position.y - WARP_POINT_ADJUSTMENT);

                    //落下速度を０にする
                    Rigidbody2D _playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                    _playerRB.velocity = new Vector2(_playerRB.velocity.x, 0);
                }
                else
                {
                    // 触れたトリガーの反対にある画面端にY軸のみ移動する
                    _playerPositoin.position = new Vector3(_playerPositoin.position.x, _oppositionScreenEdge.position.y + WARP_POINT_ADJUSTMENT);
                }
            }
        }
    }
}
