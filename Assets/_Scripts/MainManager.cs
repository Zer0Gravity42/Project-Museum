using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    //All info relating to persisting data
    public List<string> artifacts;

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
}
