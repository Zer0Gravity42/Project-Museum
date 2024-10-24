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
    
    // Start is called before the first frame update
    void Start()
    {
        CloseElevatorUI(); //For redundancy
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenElevatorUI()
    {
        elevatorUI.SetActive(true);
    }
    
    public void CloseElevatorUI()
    {
        elevatorUI.SetActive(false);
    }
    
    //Loading Floors
    public void LoadFloor1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor1");
    }
    
    public void LoadFloor2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor2");
    }
    
    public void LoadFloor3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor3");
    }
    
    public void LoadFloor4()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor4");
    }
    
}
