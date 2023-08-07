using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestPlayerMovement : MonoBehaviour
{
    public static bool IsMovingOnStairs { get; private set; }
    public float moveSpeed = 500f;
    NavMeshAgent agent;
    Camera mainCamera;
    Rigidbody rb;

    float moveHorizontal;
    float moveVertical;

    float delay = 5f;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {

        HandleArrowsInput();
    }

    void FixedUpdate()
    {

        MoveCharacter();
    }

    void HandleArrowsInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    }


    void MoveCharacter()
    {
        if (!PlayerStairsMovement.IsMovingOnStairs)
        {
            Vector3 movementToClick = new Vector3(moveHorizontal * moveSpeed * Time.deltaTime, rb.velocity.y, moveVertical * moveSpeed * Time.deltaTime);
            rb.velocity = movementToClick;
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            Stairs stairsComponent = other.gameObject.GetComponent<Stairs>();
            if (stairsComponent != null)
            {
                if (stairsComponent.colliderType == Stairs.ColliderType.Up)
                {
                    StartCoroutine(MoveUpstairs(stairsComponent.teleportPosition));
                }
                else if (stairsComponent.colliderType == Stairs.ColliderType.Down)
                {
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
        transform.position = pos;
        Debug.Log(" transform.position after teleport" + transform.position);
        yield return new WaitForSeconds(delay);
        IsMovingOnStairs = false;
    }

    private IEnumerator MoveUpstairs(Vector3 pos)
    {
        IsMovingOnStairs = true;
        Debug.Log("We go Upstairs");
        Debug.Log(" transform.position before teleport" + transform.position);
        transform.position = pos;
        Debug.Log(" transform.position after teleport" + transform.position);
        yield return new WaitForSeconds(delay);
        IsMovingOnStairs = false;
    }
}
