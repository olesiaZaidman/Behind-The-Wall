
using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectSelector : MonoBehaviour
{
    [SerializeField] static GameObject selectedObject;
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


        //  Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
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

        // Selection
        if (leftMouseClick && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlight)
            {
                if (selection != null)
                {
                    ResetSelectionColor(DefaultColor);
                }

                selection = raycastHit.transform;

                SetSelectionColorIfNeeded(ColorSelected);
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    ResetSelectionColor(DefaultColor);

                    selection = null;
                }
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
