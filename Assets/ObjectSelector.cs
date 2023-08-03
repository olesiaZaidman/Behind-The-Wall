
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
   
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    MeshRenderer highlightMesh; 
    MeshRenderer selectionMesh; 

    bool leftMouseClick;
 //   static bool isSelected;
    public static bool IsSelected { get; private set; }
    //public static bool IsSelected
    //{
    //    get { return isSelected; }
    //    private set { isSelected = value; }
    //}


    void Start()
    {
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
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        leftMouseClick = Input.GetMouseButtonDown(0);

        // Highlight
        if (highlight != null)
        {
            ResetHighlightColor(DefaultColor);
            highlight = null;
        }
       
        CheckAndHandleRaycastHit(ray);

        // Selection
        if (leftMouseClick && IsNotPointerOverGameObject())
        {
            HandleSelection();       
        }
    }

    private bool IsNotPointerOverGameObject()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    void CheckAndHandleRaycastHit(Ray ray)
    {
        if (IsNotPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            //assign the Transform component of the object that has been hit by the raycast to the highlight variable:
            //  Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            highlight = raycastHit.transform;

            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                SetHighlightColorIfNeeded(ColorSelected);
            }
            else
            {
                highlight = null;
            }
        }
    }

    void HandleSelection()
    {
        if (highlight)
        {
            if (selection != null)
            {
                ResetSelectionColor(DefaultColor);
            }
            //assign the Transform component of the object that has been hit by the raycast to the selection variable:
            selection = raycastHit.transform;

            IsSelected = true; // Set isSelected to true when an object is selected
            SetSelectionColorIfNeeded(ColorSelected);
            highlight = null;
        }
        else
        {
            if (selection)
            {
                ResetSelectionColor(DefaultColor);
                selection = null;
                IsSelected = false; // Set isSelected to false when the selection is reset
            }
        }
    }


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
        if (highlightMesh == null && highlight != null)
        {
            highlightMesh = highlight.GetComponent<MeshRenderer>();
        }
        return highlightMesh;
    }

    MeshRenderer GetSelectionMesh()
    {
        if (selectionMesh == null && selection != null)
        {
            selectionMesh = selection.GetComponent<MeshRenderer>();
        }
        return selectionMesh;
    }

}
