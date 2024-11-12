using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class MuseumController : MonoBehaviour
{
    private MainManager mainManager; // Reference to MainManager
    
    #region Audio Vars

    //Audio Sources
    public AudioSource audioSourceOnce;
    public AudioSource audioSourceLoop;
    
    //Audio Clips
    public AudioClip museumMusicLoop;
    
    #endregion
    #region Scoring Vars

    //Scoring
    public List<Artifact> displayedArtifacts = new List<Artifact>();
    public int museumScore = 0;
    
    public int scorePerArtifact = 10; 
    
    #endregion
    #region UI References
    
    //UI references
    public GameObject conditionIndicatorIcon;
    public TextMeshProUGUI conditionIndicatorText;
    public GameObject scoreIndicatorIcon;
    public TextMeshProUGUI scoreIndicatorText;

    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        // Fetch the MainManager instance
        mainManager = MainManager.Instance;

        UpdateMuseumScore(); // Count number of artifacts and update score
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
    
    private void UpdateMuseumScore()
    {
        
        // Get the number of permanent artifacts from MainManager
        int permanentArtifactCount = mainManager.permanentArtifactIds.Count;

        // Calculate museum score and reputation based on the number of artifacts
        museumScore = permanentArtifactCount * scorePerArtifact;
        scoreIndicatorText.text = "Museum Score:" + museumScore.ToString();
        
        Debug.Log("Museum Score Updated: " + museumScore);
        
        //Unlock new museum rooms?  or bonuses based on score thresholds
    }
}
