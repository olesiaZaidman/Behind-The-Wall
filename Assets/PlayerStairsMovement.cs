using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStairsMovement : MonoBehaviour
{
  //  Vector3 stairsDirection;
    bool isMovingOnStairs;

   // public bool IsMovingOnStairs => isMovingOnStairs;
  public static bool IsMovingOnStairs { get; private set; }
    public float moveSpeed = 500f;
   // NavMeshAgent agent;


    //void Start()
    //{        
    //    agent = GetComponent<NavMeshAgent>();   
    //}
    void OnTriggerEnter(Collider other)
    {
       // if (other.gameObject.layer == LayerMask.NameToLayer("Stairs"))

       if(other.gameObject.CompareTag("Stairs"))
        {
            Stairs stairsComponent = other.gameObject.GetComponent<Stairs>();

            if (stairsComponent != null)
            {
                // Check the enum value
                if (stairsComponent.colliderType == Stairs.ColliderType.Up)
                {
                    // Call the function for Up
                    StartCoroutine(MoveUpstairs(stairsComponent.teleportPosition));
                   
                }
                else if (stairsComponent.colliderType == Stairs.ColliderType.Down)
                {
                    // Call the function for Down
                    StartCoroutine(MoveDownstairs(stairsComponent.teleportPosition));

                }
            }

             
        }
    }

    private IEnumerator MoveDownstairs(Vector3 pos)
    {
        IsMovingOnStairs = true;
        Debug.Log("We go Downstairs");
        Debug.Log(" transform.position before teleport" + transform.position);
        //Teleport to pos
        //  transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed);
        transform.position = pos;
        Debug.Log(" transform.position after teleport" + transform.position);
        yield return new WaitForSeconds(0.5f);
        IsMovingOnStairs = false;
    }

    private IEnumerator MoveUpstairs(Vector3 pos)
    {
        IsMovingOnStairs = true;
        Debug.Log("We go Upstairs");
        Debug.Log(" transform.position before teleport" + transform.position);
        transform.position = pos;
        // transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed);
        Debug.Log(" transform.position after teleport" + transform.position);
        yield return new WaitForSeconds(0.5f);
        IsMovingOnStairs = false;
    }

    //private void FixedUpdate()
    //{
    //    if (IsMovingOnStairs)
    //    {
    //        // Apply the stairs direction to the player's movement
           
    //        Vector3 movement = stairsDirection * moveSpeed * Time.fixedDeltaTime;
    //        agent.SetDestination(movement);
    //       // transform.position += movement;
    //    }
    //}

    //private Vector3 GetStairsDirection(Bounds stairsBounds)
    //{
    //    // Calculate the direction based on the stairs' collider size
    //    // You can modify this based on your stairs' orientation and collider setup

    //    // For example, if the stairs are oriented along the Y-axis:
    //    return new Vector3(0f, 1f, 0f);
    //}
}
