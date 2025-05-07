using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    private Animator _gameOverAnimation = default;
    private int _animationNumber = 0;
    private StageCore _stageCore = default;
    private void Start()
    {
        _stageCore = this.gameObject.GetComponentInParent<StageCore>();
        _gameOverAnimation = this.GetComponent<Animator>();
    }
    public void StartGameOverAnimation()
    {
        _animationNumber++;
        _gameOverAnimation.SetInteger("AnimationValue", _animationNumber);
    }

    public void RestartStage()
    {
        _stageCore.RestartStage();
    }
}
