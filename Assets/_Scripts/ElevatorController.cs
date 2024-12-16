using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] GameObject elevatorUI;
    [SerializeField] GameObject buttonFloor1;
    [SerializeField] GameObject buttonFloor2;
    [SerializeField] GameObject buttonFloor3;
    [SerializeField] GameObject buttonFloor4;

    [SerializeField] PlayerController playerController;
    private GameObject mainManager;

    // Start is called before the first frame update
    void Start()
    {
        CloseElevatorUI(); //For redundancy

        mainManager = GameObject.FindGameObjectWithTag("MainManager");
    }
    
    public void OpenElevatorUI()
    {
        elevatorUI.SetActive(true);
        
        //Set all buttons to default state
        buttonFloor1.SetActive(true);
        buttonFloor2.SetActive(false);
        buttonFloor3.SetActive(false);
        buttonFloor4.SetActive(false);
        
        //If floor 2 unlocked, make the button visible
        if(mainManager.GetComponent<MainManager>().CheckFloor2Unlocked())
        {
            buttonFloor2.SetActive(true);
        }
        
        //If floor 3 unlocked, make the button visible
        if(mainManager.GetComponent<MainManager>().CheckFloor3Unlocked())
        {
            buttonFloor3.SetActive(true);
        }
        
        //If floor 4 unlocked, make the button visible
        if(mainManager.GetComponent<MainManager>().CheckFloor4Unlocked())
        {
            buttonFloor4.SetActive(true);
        }
        
    }
    
    public void CloseElevatorUI()
    {
        elevatorUI.SetActive(false);
    }
    
    //Loading Floors
    public void LoadFloor1()
    {
        playerController.UpdateManagerInfo(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor1");
    }
    
    public void LoadFloor2()
    {
        playerController.UpdateManagerInfo(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor2");
    }
    
    public void LoadFloor3()
    {
        playerController.UpdateManagerInfo(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor3");
    }
    
    public void LoadFloor4()
    {
        playerController.UpdateManagerInfo(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor4");
    }
    
}
