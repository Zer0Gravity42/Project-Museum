using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //drag enemy prefabs into this in the scene
    public List<GameObject> Spawns = new List<GameObject>();
    //set these in the scene for each enemy, ther may be a better way to do this
    public List<Vector3> Positions = new List<Vector3>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for(int i = 0;i< Spawns.Count;i++)
            {
                //spawn the enemy at the correct position then let them see the player
                GameObject enemy = Instantiate(Spawns[i], Positions[i],Quaternion.identity);
                enemy.GetComponent<Enemy>().player = collision.GameObject();
            }
            //destroy the spawner once it is used
            Destroy(gameObject);
        }
    }
}
