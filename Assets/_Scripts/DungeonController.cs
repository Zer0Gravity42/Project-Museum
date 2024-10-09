using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    //manager
    GameObject mainManager;
    //Audio
    public AudioSource audioSourceOnce;
    public AudioSource audioSourceLoop;
    
    public AudioClip dungeonMusicLoop;

    public AudioClip dungeonBossIntro;
    public AudioClip dungeonBossLoop;
    public AudioClip dungeonBossVoiceLine;
    public AudioClip dungeonBossDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.FindGameObjectWithTag("MainManager");
        PlaySoundLoop(dungeonMusicLoop);

        //remove any game objects already collected
        foreach(GameObject a in mainManager.GetComponent<MainManager>().artifactsInScene)
        {
            foreach(int i in mainManager.GetComponent<MainManager>().permanentArtifactIds)
            {
                if (i == a.GetComponent<Artifact>().ID)
                {
                    a.SetActive(false);
                }
            }
        }
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
    
    public void StopAllSound()
    {
        audioSourceLoop.Stop();
        audioSourceOnce.Stop();
    }
    
    public void ActivateBossAudio()
    {
        // Start the coroutine to play audio in the right order
        StartCoroutine(PlayBossAudioSequence());
    }
    
    
    // Coroutine to handle the audio sequence
    private IEnumerator PlayBossAudioSequence()
    {
        StopAllSound(); // Stop the current music
        PlaySound(dungeonBossIntro); // Plays the music intro
        PlaySound(dungeonBossVoiceLine); // Plays Paul's voice

        // Wait for the intro to finish before loading the level (so the music doesn't overlap with the boss music)
        yield return new WaitForSeconds(dungeonBossIntro.length);

        PlaySoundLoop(dungeonBossLoop); // Loops boss music
    }

    
}
