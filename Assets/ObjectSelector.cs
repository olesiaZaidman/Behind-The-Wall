using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public static GameObject selectedObject;

    [SerializeField] Color defaultColor = Color.yellow;
    [SerializeField] Color colorSelected = Color.green;

    Renderer rend;
    [SerializeField] bool selected = false;
     int playerLayer = 1;

    [SerializeField] bool isSwitch;

    // public const int PLAYERLAYER = 1;

    public int PlayerLayer
    {
        get { return playerLayer; }
    }

    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            SetGameObjectColor(selected ? colorSelected : defaultColor);
        }
    }

    public Color DefaultColor
    {
        get { return defaultColor; }
    }

    public Color ColorSelected
    {
        get { return colorSelected; }
    }


    void Start()
    {
        rend = GetComponent<Renderer>();
        SetGameObjectColor(defaultColor);
        AssignLayerToPlayer(playerLayer);
        isSwitch = false;
    }

     void OnMouseDown()
    {
        // Toggle the selection state
        selected = !selected;

        if (selected)
        {
            // Select the object
            SetGameObjectColor(colorSelected);
        }
        else
        {
            // Deselect the object
            SetGameObjectColor(defaultColor);
        }
    }


    public void SetGameObjectColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    public void Deselect()
    {
        Debug.Log("Should we desselect?: "+ selected);
        if (selected && !isSwitch)
        {
            isSwitch = true;
            Debug.Log("Object Deselected!");
            selected = false;
            SetGameObjectColor(defaultColor);
        }

    }

    //public void AssignLayerToGameObject(GameObject gameObject, int layer)
    //{
    //    gameObject.layer = layer;
    //}

    void AssignLayerToPlayer(int layer)
    {
        gameObject.layer = layer;
    }

    //void OnMouseDown()
    //{
    //    if (selectedObject != gameObject)
    //    {
    //        // Deselect the current object (if any)
    //        DeselectCurrentObject();

    //        // Select the clicked object
    //        selectedObject = gameObject;
    //        SetGameObjectColor(colorSelected);
    //    }
    //}

    //public void Deselect()
    //{
    //    if (selectedObject == gameObject)
    //    {
    //        // Deselect the object
    //        selectedObject = null;
    //        SetGameObjectColor(defaultColor);
    //    }
    //}

    //void DeselectCurrentObject()
    //{
    //    if (selectedObject != null)
    //    {
    //        selectedObject.GetComponent<ObjectSelector>().Deselect();
    //    }
    //}

    //void SetGameObjectColor(Color color)
    //{
    //    if (rend != null)
    //    {
    //        rend.material.color = color;
    //    }
    //}
}
