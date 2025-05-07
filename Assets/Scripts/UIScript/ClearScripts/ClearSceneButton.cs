using UnityEngine;

public class ClearSceneButton : MonoBehaviour
{
    private GameProgress _gameProgressScript = default;

    private Play_UI_Select_SE _playSelectSE = default;
    [SerializeField] private AudioClip _returnButtonSE = default;
    [SerializeField] private AudioClip _defaultSelectSE = default;
    [SerializeField] private GameObject _clearSceneCanvas = default;
    private void Start()
    {
        _playSelectSE = Camera.main.GetComponent<Play_UI_Select_SE>();
        const string FIND_TAG = "GameProgress";
        _gameProgressScript = GameObject.FindGameObjectWithTag(FIND_TAG).GetComponent<GameProgress>();
    }
    public void ReturnTitleOnButton()
    {
        _gameProgressScript.BackTitle();
        _playSelectSE.PlaySE(_returnButtonSE);
        _clearSceneCanvas.SetActive(false);

    }
    public void RetryStageOnButton()
    {
        _gameProgressScript.RetryStage(false);
        _playSelectSE.PlaySE(_defaultSelectSE);
        _clearSceneCanvas.SetActive(false);

    }
    public void NextStageOnButton()
    {
        _gameProgressScript.NextStage();
        _playSelectSE.PlaySE(_defaultSelectSE);
        _clearSceneCanvas.SetActive(false);

    }
}
