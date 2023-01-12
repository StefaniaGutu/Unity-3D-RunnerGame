using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundButton;
    Button currentButton;
    [SerializeField] Image SoundOnIcon;
    [SerializeField] Image SoundOffIcon;
    private bool muted = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateButtonIcon();
        AudioListener.pause = muted;
        currentButton = soundButton.GetComponent<Button>();
    }

    public void OnButtonPress()
    {
        muted = !muted;
        AudioListener.pause = muted;
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (muted)
        {
            SoundOffIcon.enabled = true;
            SoundOnIcon.enabled = false;
        }
        else
        {
            SoundOffIcon.enabled = false;
            SoundOnIcon.enabled = true;
        }
    }
}
