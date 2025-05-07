using UnityEngine;

public class Play_UI_Select_SE : MonoBehaviour
{
    private AudioSource _seAudio = default;
    private void Start()
    {
        _seAudio = GetComponent<AudioSource>();
    }
    public void PlaySE(AudioClip playSEs)
    {
        _seAudio.PlayOneShot(playSEs);
    }
}
