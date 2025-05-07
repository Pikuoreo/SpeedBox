using UnityEngine;

public class WaterGimmic : MonoBehaviour
{
    private int _playerLayer = 6;
    private int _clearPlayerLayer = 9;

    private PlayerController _playerControll = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == _playerLayer || collision.gameObject.layer == _clearPlayerLayer)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }

            _playerControll.ChangeGrabityForWaterGimmic(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer || collision.gameObject.layer == _clearPlayerLayer)
        {
            if (_playerControll == null)
            {
                _playerControll = collision.gameObject.GetComponent<PlayerController>();
            }

            _playerControll.ChangeGrabityForWaterGimmic(false);
        }
    }
}
