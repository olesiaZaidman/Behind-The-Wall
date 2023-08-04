
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 500f; // Adjust this value to control the player's movementToClick speed

 float moveHorizontal;
   Vector3 targetPosition;

   Vector3 movementToClick;
    Camera mainCamera;
  Vector3 newpos;
 Transform player;
     Rigidbody rb;

    RaycastHit raycastHit;
   Ray ray;

    bool rightMouseClick;
    Vector3 mousePositionScreen;
    Vector3 mousePositionWorld;
    bool isPerformingAction = false; // New property to indicate fill completion
    public bool IsPerformingAction
    {
        get { return isPerformingAction; }
        set { isPerformingAction = value; }
    }

    void Start()
    {
        FindCamera();
        InitializeComponent<Rigidbody>();
    }

    void FindCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.Log("Camera is missing");
        }
    }


    void CalculateVector()
    {
        rightMouseClick = Input.GetMouseButtonDown(1);

        if (rightMouseClick)
        {
            // Get the mouse position in screen coordinates
             mousePositionScreen = Input.mousePosition;
            // Create a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay(mousePositionScreen);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            float maxDistance = 100f;
            LayerMask layerMask = LayerMask.GetMask("Stairs", "House", "Props"); // Replace "Layer1" and "Layer2" with the names of the layers you want to interact with.
            LayerMask layerMaskGround = LayerMask.GetMask("Ground"); // Replace "Layer1" and "Layer2" with the names of the layers you want to interact with.
   

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
            {              
                // Get the world position where the ray hits an object
                 mousePositionWorld = hit.point;

              //  Vector3 distance = mousePositionWorld - transform.position;
                Debug.Log("Mouse Clicked at: " + mousePositionWorld);
             //   Debug.Log("We hit: " + hit.collider.gameObject);
             //   Debug.Log("Distance: " + distance);
               MoveObjectToPosition(mousePositionWorld, false);
            }

            if (Physics.Raycast(ray, out RaycastHit hit2, maxDistance, layerMaskGround))
            {
                // Get the world position where the ray hits an object
                mousePositionWorld = hit2.point;

              //  Vector3 distance = mousePositionWorld - transform.position;
                Debug.Log("Mouse Clicked at: " + mousePositionWorld);
                //   Debug.Log("We hit: " + hit.collider.gameObject);
                //   Debug.Log("Distance: " + distance);
                MoveObjectToPosition(mousePositionWorld, true);
            }
            else
            {
                Debug.Log("No hit within the maxDistance or no hit on the specified layers");
            }
        }
    }

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

        float speed = moveSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, newPos, speed);
    }

    void Update()
    {
        CalculateVector();
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
     //   if (ObjectSelector.IsSelected)
        //{
        //    MoveObjectToPosition(mousePositionWorld);
        //}
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
            //else if (component is Renderer rendComponent)
            //{
            //    thisGameObjectRenderer = rendComponent;
            //}
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



    //void HandleArrowsInput()
    //{
    //    moveHorizontal = Input.GetAxis("Horizontal");
    //}



}
