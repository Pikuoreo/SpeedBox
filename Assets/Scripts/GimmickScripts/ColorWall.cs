using System.Collections.Generic;
using UnityEngine;

public class ColorWall : MonoBehaviour
{
    [Header("�ԐF�̕�"), SerializeField] private List<GameObject> _redWallObject = new List<GameObject>();
    [Header("�ΐF�̕�"), SerializeField] private List<GameObject> _greenWallObject = new List<GameObject>();
    [Header("�F�̕�"), SerializeField] private List<GameObject> _blueWallObject = new List<GameObject>();
    [SerializeField] private SpriteRenderer _backGroundSprite = default;

    private GimicColor _currentColor = GimicColor.None;
    private bool _isStartGimmic = false;

    private const int MAX_COLOR_VALUE = 1;
    private const float _BACKGROUND_COLOR_A = 0.1f;

    private void Update()
    {
        if (_isStartGimmic && Input.GetMouseButtonDown(0))
        {
            ColorChange();
        }
        else if (_isStartGimmic && Input.GetMouseButtonDown(1))
        {
            ReverseColorChange();
        }
    }
    enum GimicColor
    {
        Red,
        Green,
        Blue,
        None
    }

    /// <summary>
    /// ���N���b�N�������Ƃ��̐F�M�~�b�N�ύX
    /// </summary>
    private void ColorChange()
    {
        switch (_currentColor)
        {
            //�����F�����Ă��Ȃ���
            case GimicColor.None:
                //�M�~�b�N��Ԕ���ɂ���
                RedWallActiveChange(false);
                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);

                _currentColor = GimicColor.Red;

                break;

            //���݂̐F���Ԃ̎�
            case GimicColor.Red:
                //�M�~�b�N��Δ���ɂ���
                RedWallActiveChange(true);
                GreenWallActiveChange(false);

                _backGroundSprite.color = new Color(0, MAX_COLOR_VALUE, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Green;
                break;

            //���݂̐F���΂̎�
            case GimicColor.Green:

                //�M�~�b�N�����ɂ���
                GreenWallActiveChange(true);
                BlueWallActiveChange(false);

                _backGroundSprite.color = new Color(0, 0, MAX_COLOR_VALUE, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Blue;
                break;

            //���݂̐F���̎�
            case GimicColor.Blue:

                //�M�~�b�N��Ԕ���ɂ���
                RedWallActiveChange(false);
                BlueWallActiveChange(true);

                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Red;
                break;
        }
    }

    /// <summary>
    /// �E�N���b�N���������̐F�M�~�b�N�ύX
    /// </summary>
    private void ReverseColorChange()
    {
        switch (_currentColor)
        {
            //���݂̐F���Ԃ̎�
            case GimicColor.Red:

                //�M�~�b�N�����ɂ���
                RedWallActiveChange(true);
                BlueWallActiveChange(false);

                _backGroundSprite.color = new Color(0, 0, MAX_COLOR_VALUE, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Blue;
                break;

            //���݂̐F���΂̎�
            case GimicColor.Green:

                //�M�~�b�N��Ԕ���ɂ���
                RedWallActiveChange(false);
                GreenWallActiveChange(true);

                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Red
                    ;
                break;

            //���݂̐F���̎�
            case GimicColor.Blue:

                //�M�~�b�N��Δ���ɂ���
                GreenWallActiveChange(false);
                BlueWallActiveChange(true);

                _backGroundSprite.color = new Color(0, MAX_COLOR_VALUE, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Green;
                break;
        }
    }

    private void RedWallActiveChange(bool isActive)
    {
        foreach (GameObject redWalls in _redWallObject)
        {
            redWalls.SetActive(isActive);
        }
    }

    private void GreenWallActiveChange(bool isActive)
    {
        foreach (GameObject greenWalls in _greenWallObject)
        {
            greenWalls.SetActive(isActive);
        }
    }

    private void BlueWallActiveChange(bool isActive)
    {
        foreach (GameObject blueWalls in _blueWallObject)
        {
            blueWalls.SetActive(isActive);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        const int PLAYER_LAYER = 6;

        if (collision.gameObject.layer == PLAYER_LAYER && !_isStartGimmic)
        {
            _isStartGimmic = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            ColorChange();
        }
    }
}
