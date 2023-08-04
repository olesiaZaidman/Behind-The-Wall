
using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectSelector : MonoBehaviour
{
   // static GameObject selectedObject;
    [SerializeField] Color defaultColor = Color.yellow;
    [SerializeField] Color colorSelected = Color.green;

    Camera mainCamera;

    public Color DefaultColor => defaultColor;
    public Color ColorSelected => colorSelected;

    [SerializeField] Transform highlightOnHover; //when we just hover over object
    [SerializeField] Transform selectionAfterClick; //when we click to select
    RaycastHit raycastHit;
    Ray ray;
    MeshRenderer highlightMesh; 
    MeshRenderer selectionMesh; 

    bool leftMouseClick;
    public static bool IsSelected { get; private set; }
    string tagSelectable;

    void Start()
    {
        tagSelectable = "Selectable";
        mainCamera = Camera.main;
        highlightMesh = null;
        selectionMesh = null;
    }


    void Update()
    {
        SelectAndHighlight();
    }
    void SelectAndHighlight()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        leftMouseClick = Input.GetMouseButtonDown(0);
       
        if (highlightOnHover != null)
        {
            ResetHighlightColor(DefaultColor);
            highlightOnHover = null;
        }

        if (IsNotPointerOverUIElement())
        {
            HandleHighlightOnCheckRaycastHit(ray, tagSelectable);  // Hover Highlight

            HandleSelectionOnMouseClick();           // Click Selection
        }

    }

    bool IsNotPointerOverUIElement()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    void HandleHighlightOnCheckRaycastHit(Ray ray, string tag)
    {
        //Is Ray Hit?
        if (Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
        //     Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

            //assign the Transform component of the object that has been hit by the raycast to the highlightOnHover variable:
            highlightOnHover = raycastHit.transform;

            if (highlightOnHover.CompareTag(tag) && highlightOnHover != selectionAfterClick)
            {
              //  SaveStartColorFromMesh(GetHighlightMesh());
                SetHighlightColorIfNeeded(ColorSelected);
            }
            else
            {
                highlightOnHover = null;
            }
        }
    }

    void HandleSelectionOnMouseClick()
    {
        if (leftMouseClick)
        {
            if (highlightOnHover)
            {
                if (selectionAfterClick != null)
                {
                    ResetSelectionColor(DefaultColor);
                    selectionAfterClick = null;
                }
                //assign the Transform component of the object that has been hit by the raycast to the selectionAfterClick variable:
                selectionAfterClick = raycastHit.transform;
                IsSelected = true; // Set isSelected to true when an object is selected
                SetSelectionColorIfNeeded(ColorSelected);
                highlightOnHover = null;
            }
            else
            {
                if (selectionAfterClick)
                {
                    ResetSelectionColor(DefaultColor);
                    selectionAfterClick = null;
                    IsSelected = false; // Set isSelected to false when the selectionAfterClick is reset
                }
            }
        }

    }

    //void SaveStartColorFromMesh(MeshRenderer mesh)
    //{
    //    startColor = mesh.material.color;
    //}
    void SetSelectionColorIfNeeded(Color color)
    {
        if (GetSelectionMesh().material.color != color)
        {
            GetSelectionMesh().material.color = color;
        }
    }

    void ResetSelectionColor(Color color)
    {
        GetSelectionMesh().material.color = color;
    }

    void SetHighlightColorIfNeeded(Color color)
    {
        if (GetHighlightMesh().material.color != color)
        {
            GetHighlightMesh().material.color = color;
        }
    }

    void ResetHighlightColor(Color color)
    {
        GetHighlightMesh().sharedMaterial.color = color;
    }

    MeshRenderer GetHighlightMesh()
    {
        if (highlightMesh == null && highlightOnHover != null)
        {
            highlightMesh = highlightOnHover.GetComponent<MeshRenderer>();
        }
        return highlightMesh;
    }

    MeshRenderer GetSelectionMesh()
    {
        if (selectionMesh == null && selectionAfterClick != null)
        {
            selectionMesh = selectionAfterClick.GetComponent<MeshRenderer>();
        }
        return selectionMesh;
    }

}
