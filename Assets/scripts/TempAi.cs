using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempAi : MonoBehaviour
{
    public Transform playerCamera;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.transform.LookAt(playerCamera);
        agent.SetDestination(playerCamera.position);
    }
    private void Update()
    {
       
        Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
    }
}
