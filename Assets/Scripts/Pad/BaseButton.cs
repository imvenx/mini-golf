using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    private static AudioSource UIAudioPlayer;
    private static AudioClip ButtonPressAudio;
    void Awake()
    {
        UIAudioPlayer = GameObject.Find("UIAudioPlayer").GetComponent<AudioSource>();

        ButtonPressAudio = Resources.Load<AudioClip>("Audio/drop_003");
        GetComponent<Button>().onClick.AddListener(() => { UIAudioPlayer.PlayOneShot(ButtonPressAudio); });
    }
}
