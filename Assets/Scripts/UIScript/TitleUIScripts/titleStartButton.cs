using UnityEngine;

public class titleStartButton : MonoBehaviour
{
    [SerializeField] private GameObject _titleObject = default;
    [SerializeField] private GameObject _stageSelectObject = default;

    private AudioSource _seAudio = default;

    private void Start()
    {
        _seAudio = this.GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        _titleObject.SetActive(false);
        _stageSelectObject.SetActive(true);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
