using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public static MouseCursorManager instance;


    [Header("Cursor Textures")]
    [Tooltip("The default cursor texture")]
    public Texture2D defaultCursor;

    [Tooltip("The cursor texture when the mouse pointer is over some object or UI element")]
    public Texture2D selectionForActionCursor;

    [Tooltip("The cursor texture when the mouse pointer is over some object you want to explore")]
    public Texture2D exploreCursor;

    private void Awake()
    {
        //Check if a instace already exist and destroy it, otherwise don't destroy this object on load
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
