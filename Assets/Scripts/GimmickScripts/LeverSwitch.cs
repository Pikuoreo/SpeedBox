using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [SerializeField] private Sprite _AlreadyStartedImage = default;
    [SerializeField] private List<Animator> _connectionOpenObjects = new List<Animator>();
    [SerializeField] private List<Animator> _connectionCloseObjects = new List<Animator>();

    private SpriteRenderer _leverSwitchSprite = default;
    private BoxCollider2D _leverSwitchCollider = default;

    private void Start()
    {
        _leverSwitchSprite = this.GetComponent<SpriteRenderer>();
        _leverSwitchCollider = this.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = 6;
        if (collision.gameObject.layer == playerLayer)
        {
            if (_connectionOpenObjects != null)
            {
                foreach (Animator connectionOpenObjectAnimation in _connectionOpenObjects)
                {
                    connectionOpenObjectAnimation.SetBool("isOpen", true);
                }
            }

            if (_connectionCloseObjects != null)
            {
                foreach (Animator connectionCloseObjectAnimation in _connectionCloseObjects)
                {
                    connectionCloseObjectAnimation.SetBool("isClose", true);
                }
            }



            _leverSwitchSprite.sprite = _AlreadyStartedImage;
            _leverSwitchCollider.enabled = false;
        }
    }
}
