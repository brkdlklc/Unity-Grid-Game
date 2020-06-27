using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip directionClick, colorClick;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void playButtonClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(directionClick);
    }
    public void playColorChangeSound()
    {
        GetComponent<AudioSource>().PlayOneShot(colorClick);
    }
}
