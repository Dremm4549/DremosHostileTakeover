using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrigger : MonoBehaviour
{

    public Enemy enemy;
    public Transform player;
    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.SetDestination(player.position);
        }
    }
}
