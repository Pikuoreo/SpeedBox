using UnityEngine;

public class WorpGate : MonoBehaviour
{
    [SerializeField] private Transform _warpDestination = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = 6;
        if (collision.gameObject.layer == playerLayer)
        {
            collision.gameObject.transform.position = _warpDestination.position;
        }
    }
}
