using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    //All info relating to persisting data
    public List<GameObject> tempArtifacts;

    public List<int> permanentArtifactIds;

    public int equippedSlotOneId;
    public int equippedSlotTwoId;

    public List<GameObject> keys;
    public List<GameObject> doors;

    private void Awake()
    {
        if (Instance != null) //if the mainmanager has already been copied, dont copy it again
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); //makes a special scene where all info persists
    }


    void AddAllToPermanents()
    {
        if (tempArtifacts != null) //check to make sure the temporary list is not empty
        {
            foreach (GameObject g in tempArtifacts) //loop through the temporary list and add all artifact ids to the list
            {
                permanentArtifactIds.Add(g.GetComponent<Artifact>().ID);
            }
        }
    }
}
