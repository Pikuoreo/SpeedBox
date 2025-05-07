using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItem : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _objectSprits = new List<SpriteRenderer>();
    private CircleCollider2D _itemCollider = default;

    private void Start()
    {
        _itemCollider = this.gameObject.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<PlayerController>().PossibleDoubleJump();
            _itemCollider.enabled = false;
            foreach (SpriteRenderer objectSprite in _objectSprits)
            {
                objectSprite.enabled = false;
            }
            StartCoroutine(ReAppearance());
        }
    }

    private IEnumerator ReAppearance()
    {
        float waitTime = 1.5f;

        yield return new WaitForSeconds(waitTime);
        _itemCollider.enabled = true;
        foreach (SpriteRenderer objectSprite in _objectSprits)
        {
            objectSprite.enabled = true;
        }
    }
}
