using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    //Audio
    public AudioSource audioSourceOnce;
    public AudioSource audioSourceLoop;
    public AudioClip menuButton;
    public AudioClip menuMusicLoop;
    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        PlaySoundLoop(menuMusicLoop);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    //onclick
    public void MoveMenu()
    {
        PlaySound(menuButton);
        anim.Play("moveAnimation");
    }

    //onclick
    public void StartGame()
    {
        //Play sound
        PlaySound(menuButton);
        
        //Start the coroutine to wait for the sound to finish before loading the level
        StartCoroutine(LoadAfterSound(menuButton.length));
    }
    
    public void QuitGame() 
    {
        //Play sound
        PlaySound(menuButton);
        
        Application.Quit();
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
    
    //Stop looping sound - as needed
    public void StopLoopSound()
    {
        audioSourceLoop.Stop();
    }
    
    //Wait for sound to finish before proceeding to main game
    IEnumerator LoadAfterSound(float time)
    {
        yield return new WaitForSeconds(time);
        
        // Load the level after the sound finishes
        Application.LoadLevel("NewPlayerIntro");
    }
    
}
