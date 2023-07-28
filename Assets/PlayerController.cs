using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 500f; // Adjust this value to control the player's movementToClick speed

   float moveHorizontal;
    Vector3 targetPosition;
    Vector3 mousePositionScreen;
    Vector3 movementToClick;
    Camera mainCamera;

    bool selected = false;
    Color defaultColor = Color.yellow;
    Color colorSelected = Color.green;

    Rigidbody rb;
    Renderer rend;
    void Start()
    {
        FindCamera();
        //  InitializeRigidbodyRendererComponents();
        InitializeComponent<Rigidbody>();
        InitializeComponent<Renderer>();
        SetGameObjectColor(defaultColor);
    }

    void Update()
    {
      //  HandleMouseClickInput();
        HandleArrowsInput();
    }

    void FixedUpdate()
    {
      //  MoveCharacterToMouseClick();
        MoveCharacter();
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
            else if (component is Renderer rendComponent)
            {
                rend = rendComponent;
            }
            // We can add more conditions for other components if needed
            // else if (component is YourComponentType yourComponent)
            // {
            //     // Handle your component initialization here
            // }
        }
    }

    // void InitializeRigidbodyRendererComponents()
    //{
    //    rb = GetComponent<Rigidbody>();

    //    if (rb == null)
    //    {
    //        Debug.LogError("Rigidbody component not found on the GameObject.");
    //    }

    //    rend = GetComponent<Renderer>();

    //    if (rend == null)
    //    {
    //        Debug.LogError("Renderer component not found on the GameObject.");
    //    }
    //}

    void FindCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.Log("Camera is missing");
        }
    }


    public void SetGameObjectColor(Color color)
    {
        //Renderer renderer = gameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    void HandleMouseClickInput()
    {
        // Check for mouse click
        if (Input.GetMouseButton(1) && selected)
        {
            // Get the mouse position in screen space
            mousePositionScreen = Input.mousePosition;

            // Convert the mouse position to world space
            targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //new Vector3(mousePositionScreen.x, mousePositionScreen.y, transform.position.z)
            targetPosition.z = 0;
        }

        if (Input.GetMouseButton(0) && selected)
        {
            SetGameObjectColor(defaultColor);
            selected = false;
        }

    }

    void OnMouseDown()
    {
        selected = true;
        SetGameObjectColor(colorSelected);
    }


    void MoveCharacterToMouseClick()
    {
        // Calculate the movementToClick vector towards the target position
        //   movementToClick = new Vector3((targetPosition.x - transform.position.x) * 
        //     moveSpeed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);

        // Apply the movementToClick to the Rigidbody
        //  rb.velocity = movementToClick;
        float speed = moveSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

    }




    void HandleArrowsInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
    }

    void MoveCharacter() 
    {
        Vector3 movementToClick = new Vector3(moveHorizontal * moveSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        rb.velocity = movementToClick;
    }

}