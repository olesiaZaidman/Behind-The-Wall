
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 500f; // Adjust this value to control the player's movementToClick speed
    NavMeshAgent agent;
   // PlayerStairsMovement stairsMovement;
    Camera mainCamera;
    Rigidbody rb;

    bool rightMouseClick;
    Vector3 mousePositionScreen;
    Vector3 mousePositionWorld;
    bool isPerformingAction = false; // New property to indicate fill completion
    float moveHorizontal;

    public bool IsPerformingAction
    {
        get { return isPerformingAction; }
        set { isPerformingAction = value; }
    }

    void Start()
    {
        FindCamera();
       // stairsMovement = GetComponent<PlayerStairsMovement>();  
        InitializeComponent<Rigidbody>();
        InitializeComponent<NavMeshAgent>();
    }

    void FindCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.Log("Camera is missing");
        }
    }


    void CalculateVectorOnMouseClick()
    {
        rightMouseClick = Input.GetMouseButtonDown(1);

        if (rightMouseClick)
        {
            // Get the mouse position in screen coordinates
             mousePositionScreen = Input.mousePosition;
            // Create a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay(mousePositionScreen);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
          //  float maxDistance = 100f;
            LayerMask layerMask = LayerMask.GetMask("House", "Props"); // Replace "Layer1" and "Layer2" with the names of the layers you want to interact with.
            LayerMask layerMaskGround = LayerMask.GetMask("Ground"); // Replace "Layer1" and "Layer2" with the names of the layers you want to interact with.
          //  LayerMask layerMaskStairs = LayerMask.GetMask("Stairs");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {              
                // Get the world position where the ray hits an object
                 mousePositionWorld = hit.point;

              //  Vector3 distance = mousePositionWorld - transform.position;
                Debug.Log("Mouse Clicked at: " + mousePositionWorld);
             //   Debug.Log("We hit: " + hit.collider.gameObject);
             //   Debug.Log("Distance: " + distance);
               MoveObjectToPosition(mousePositionWorld, false);
            }

            if (Physics.Raycast(ray, out RaycastHit hit2, Mathf.Infinity, layerMaskGround))
            {
                // Get the world position where the ray hits an object
                mousePositionWorld = hit2.point;
                Debug.Log("Mouse Clicked at: " + mousePositionWorld);
                //   Debug.Log("We hit: " + hit.collider.gameObject);
               
               MoveObjectToPosition(mousePositionWorld, true);
               
            }

            //if (Physics.Raycast(ray, out RaycastHit hit3, Mathf.Infinity, layerMaskStairs))
            //{
            //    mousePositionWorld = hit3.point;
            //    Debug.Log("Mouse Clicked at: " + mousePositionWorld);
            //    //   Debug.Log("We hit: " + hit.collider.gameObject);
            //    PlayerStairsMovement.IsMovingOnStairs = true;
            //    // UseStairs(mousePositionWorld, true);

            //}

            else
            {
                Debug.Log("No hit within the maxDistance or no hit on the specified layers");
            }
        }
    }
    //void UseStairs(Vector3 targetPosition, bool isStairs)
    //{
    //    Vector3 newPos;
    // //  float zPosDefault = -1.052f;

    //    if (isStairs)
    //    {
    //        newPos = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
    //        agent.SetDestination(newPos);
    //    }
    //    return;
    //}
    void MoveObjectToPosition(Vector3 targetPosition, bool isGround)
    {
        Vector3 newPos;
        float zPosDefault = -1.052f;

        if (isGround)
        {
             newPos = new Vector3(targetPosition.x, CheckYPosMousClick(targetPosition.y), targetPosition.z);
        }
        else         
        {
             newPos = new Vector3(targetPosition.x, CheckYPosMousClick(targetPosition.y), zPosDefault);
        }

        agent.SetDestination(newPos);


      //  float speed = moveSpeed * Time.fixedDeltaTime;
      //  transform.position = Vector3.MoveTowards(transform.position, newPos, speed);
    }

    void Update()
    {
        if (PlayerStairsMovement.IsMovingOnStairs)
        { return; }
        CalculateVectorOnMouseClick();
        HandleArrowsInput();
    }

    float CheckYPosMousClick(float yPosClick)
    {
        foreach (var floorThreshold in FloorThresholdManager.floorThresholds)
        {
            if (yPosClick > floorThreshold.MinY && yPosClick < floorThreshold.MaxY)
            {
                return floorThreshold.YPosition;
            }
        }
        // If the yPosClick does not fall within any floor thresholds, return a default value
        return FloorThresholdManager.floorThresholds[0].YPosition; // default value 
    }

   
    void FixedUpdate()
    {
        
        //if (rightMouseClick)
        if (ObjectSelector.IsSelected)
        {
            MoveCharacter();
        }
    }



    void InitializeComponent<T>() where T : Component
    {
        T component = GetComponent<T>();

        if (component == null)
        {
            Debug.LogError($"Component of type {typeof(T)} not found on the GameObject.");
        }
        else
        {
            if (component is Rigidbody rbComponent)
            {
                rb = rbComponent;
            }
            else if (component is NavMeshAgent agentComponent)
            {
                agent = agentComponent;
            }
            // We can add more conditions for other components if needed
            // else if (component is YourComponentType yourComponent)
            // {
            //     // Handle your component initialization here
            // }
        }
    }




    //void HandleMouseClickInput()
    //{

    //}


    //void MoveCharacterToMouseClick()
    //{

    //}

    void MoveCharacter()
    {
        Vector3 movementToClick = new Vector3(moveHorizontal * moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        rb.velocity = movementToClick;
    }


    void HandleArrowsInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
    }



}
