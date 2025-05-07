using System.Collections.Generic;
using UnityEngine;

public class TimePlusItem : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _objectSprits = new List<SpriteRenderer>();
    [SerializeField] private float _timePlusValue = default;
    private CircleCollider2D _itemCollider = default;
    private bool _getItem = false;

    private void Start()
    {
        _itemCollider = this.gameObject.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !_getItem)
        {
            _getItem = true;
            GetComponentInParent<StageCore>().GetTimePlusItem(_timePlusValue);
            _itemCollider.enabled = false;
            foreach (SpriteRenderer objectSprite in _objectSprits)
            {
                objectSprite.enabled = false;
            }
        }
    }


}
