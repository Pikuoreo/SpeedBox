using UnityEngine;

public class ReadyTextControll : MonoBehaviour
{
    [SerializeField] private GameProgress _gameProgress = default;
    public void EndAnimation()
    {
        _gameProgress.EndReadyAnimation();
    }
}
