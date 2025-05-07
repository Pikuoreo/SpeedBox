using UnityEngine;

public class LoadAnimationScript : MonoBehaviour
{
    private GameProgress _startStageSelect = default;

    private void Start()
    {
        _startStageSelect = this.GetComponentInParent<GameProgress>();
    }
    public void StageLoad()
    {
        _startStageSelect.GenerateStage();
    }
}
