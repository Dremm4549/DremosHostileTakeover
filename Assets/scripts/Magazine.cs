using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{

    public LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    public float groundDistance = 0.4f;

    float destoryTimer = 10f;

    bool isGrounded = false;
    public int numberOfBullets = 8;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded || transform.position.y < -2f)
        {
            destoryTimer -= Time.deltaTime;
            if(destoryTimer < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            destoryTimer = 10f;
        }
    }

}


