using System.Collections.Generic;
using UnityEngine;

public class ColorWall : MonoBehaviour
{
    [Header("赤色の壁"), SerializeField] private List<GameObject> _redWallObject = new List<GameObject>();
    [Header("緑色の壁"), SerializeField] private List<GameObject> _greenWallObject = new List<GameObject>();
    [Header("青色の壁"), SerializeField] private List<GameObject> _blueWallObject = new List<GameObject>();
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
    /// 左クリックをしたときの色ギミック変更
    /// </summary>
    private void ColorChange()
    {
        switch (_currentColor)
        {
            //何も色がついていない時
            case GimicColor.None:
                //ギミックを赤判定にする
                RedWallActiveChange(false);
                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);

                _currentColor = GimicColor.Red;

                break;

            //現在の色が赤の時
            case GimicColor.Red:
                //ギミックを緑判定にする
                RedWallActiveChange(true);
                GreenWallActiveChange(false);

                _backGroundSprite.color = new Color(0, MAX_COLOR_VALUE, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Green;
                break;

            //現在の色が緑の時
            case GimicColor.Green:

                //ギミックを青判定にする
                GreenWallActiveChange(true);
                BlueWallActiveChange(false);

                _backGroundSprite.color = new Color(0, 0, MAX_COLOR_VALUE, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Blue;
                break;

            //現在の色が青の時
            case GimicColor.Blue:

                //ギミックを赤判定にする
                RedWallActiveChange(false);
                BlueWallActiveChange(true);

                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Red;
                break;
        }
    }

    /// <summary>
    /// 右クリックをした時の色ギミック変更
    /// </summary>
    private void ReverseColorChange()
    {
        switch (_currentColor)
        {
            //現在の色が赤の時
            case GimicColor.Red:

                //ギミックを青判定にする
                RedWallActiveChange(true);
                BlueWallActiveChange(false);

                _backGroundSprite.color = new Color(0, 0, MAX_COLOR_VALUE, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Blue;
                break;

            //現在の色が緑の時
            case GimicColor.Green:

                //ギミックを赤判定にする
                RedWallActiveChange(false);
                GreenWallActiveChange(true);

                _backGroundSprite.color = new Color(MAX_COLOR_VALUE, 0, 0, _BACKGROUND_COLOR_A);
                _currentColor = GimicColor.Red
                    ;
                break;

            //現在の色が青の時
            case GimicColor.Blue:

                //ギミックを緑判定にする
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
