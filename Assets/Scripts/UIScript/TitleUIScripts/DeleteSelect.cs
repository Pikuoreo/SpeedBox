using UnityEngine;

public class DeleteSelect : MonoBehaviour
{
    [SerializeField] private GameObject _titleButtons = default;
    [SerializeField] private GameObject _confirmationButtons = default;

    public void Onclick()
    {
        _titleButtons.SetActive(false);
        _confirmationButtons.SetActive(true);
    }

    public void Canseled()
    {
        _titleButtons.SetActive(true);
        _confirmationButtons.SetActive(false);
    }
}
