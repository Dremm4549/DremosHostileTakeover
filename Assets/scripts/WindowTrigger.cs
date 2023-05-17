using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindowTrigger : MonoBehaviour
{
    public Transform newPos;
    public Enemy enemy;
    public Transform player;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("WOAH Dude time to maf-h.. teleport");
        
        enemy = other.GetComponent<Enemy>();
        if (player != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.SetDestination(player.position);
        }
        //enemy.agent.enabled = false;
        //enemy.agent.Warp(newPos.position);  
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("WOAH Dude time to maf-h.. teleport");

    //    enemy = FindObjectOfType<Enemy>();
    //    enemy.agent.enabled = false;
    //    enemy.agent.Warp(newPos.position);
    //}

    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.SetDestination(player.position);
        }
    }
}
