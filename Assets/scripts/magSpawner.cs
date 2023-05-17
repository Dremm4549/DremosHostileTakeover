using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magSpawner : MonoBehaviour
{
    [SerializeField] GameObject magPrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] float spawnSpeed;
    [SerializeField] bool outsideZone = false;

    private void Start()
    {
        Instantiate(magPrefab, spawnPos.position, Quaternion.Euler(0, 0, 0));
    }

    private void Update()
    {
        spawnSpeed -= Time.deltaTime;
        if (outsideZone && spawnSpeed <= 0)
        {
            Instantiate(magPrefab, spawnPos.position, Quaternion.Euler(0, 0, 0));
            spawnSpeed = 1.0f;
            outsideZone = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MagTag")
        {
            outsideZone = true;
        }
        Debug.Log(other.tag);
        
    }
}
