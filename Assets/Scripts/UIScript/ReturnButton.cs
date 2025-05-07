using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private GameObject _titleObject = default;
    [SerializeField] private GameObject _stageSelectObject = default;

    public void OnClick()
    {
        _titleObject.SetActive(true);
        _stageSelectObject.SetActive(false);
    }
}
