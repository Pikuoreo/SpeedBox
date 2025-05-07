using System.Collections;
using UnityEngine;

public class GravityGimmic : MonoBehaviour
{
    [SerializeField] private bool _isGravityDerection = true;//true�������牺�����̏d�́Afalse�������������̏d��
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

            //�d�͂̌��������ɂ���u���b�N�Ȃ�
            if (_isGravityDerection)
            {
                //���ɂ͂˂�A�j���[�V�����Đ�
                _boxMoveAnimator.SetTrigger("DownGravityMove");
            }
            //�d�͂̌�������ɂ���u���b�N�Ȃ�
            else
            {
                //��ɂ͂˂�A�j���[�V�����𗬂�
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
