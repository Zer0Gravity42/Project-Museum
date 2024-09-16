using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumController : MonoBehaviour
{
    //Audio
    public AudioSource audioSourceOnce;
    public AudioSource audioSourceLoop;

    public AudioClip museumMusicLoop;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlaySoundLoop(museumMusicLoop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(clip);
    }
    
    public void PlaySoundLoop(AudioClip clip)
    {
        audioSourceLoop.loop = true;
        audioSourceLoop.clip = clip;
        audioSourceLoop.Play();
    }
}
