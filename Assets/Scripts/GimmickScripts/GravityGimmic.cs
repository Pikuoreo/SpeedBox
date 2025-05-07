using System.Collections;
using UnityEngine;

public class GravityGimmic : MonoBehaviour
{
    [SerializeField] private bool _isGravityDerection = true;//trueだったら下向きの重力、falseだったら上向きの重力
    private bool _isUsed = false;

    private Animator _boxMoveAnimator = default;

    private void Start()
    {
        _boxMoveAnimator = this.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = 6;
        int _clearPlayerLayer = 9;
        if (!_isUsed && collision.gameObject.layer == playerLayer || collision.gameObject.layer == _clearPlayerLayer)
        {
            _isUsed = true;
            collision.gameObject.GetComponent<PlayerController>().ChangeGravityScale(_isGravityDerection);

            //重力の向きを下にするブロックなら
            if (_isGravityDerection)
            {
                //下にはねるアニメーション再生
                _boxMoveAnimator.SetTrigger("DownGravityMove");
            }
            //重力の向きを上にするブロックなら
            else
            {
                //上にはねるアニメーションを流す
                _boxMoveAnimator.SetTrigger("UpGravityMove");
            }
        }
        StartCoroutine(ReAvailable());
    }

    private IEnumerator ReAvailable()
    {
        float waitTime = 3f;
        yield return new WaitForSeconds(waitTime);
        _isUsed = false;
        yield break;
    }
}
