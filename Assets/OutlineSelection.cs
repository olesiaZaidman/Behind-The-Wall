
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;


    [SerializeField] Color colorOutlineSelected = Color.white;
    public Color ColorOutlineSelected => colorOutlineSelected;
    float outlineWidth = 6f;
    bool leftMouseClick;
    string tagSelectable;
    Camera mainCamera;
    Ray ray;

    void Start()
    {
        tagSelectable = "Selectable";
        mainCamera = Camera.main;
    }

    bool IsNotPointerOverUIElement()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        leftMouseClick = Input.GetMouseButtonDown(0);

        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
      
        if (IsNotPointerOverUIElement()) 
            //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            HandleHighlightOnCheckRaycastHit(ray,tagSelectable, ColorOutlineSelected, outlineWidth);         
        }

        // Selection
        if (leftMouseClick)
        {
            HandleSelectionOnMouseClick();
        }
    }

    void HandleSelectionOnMouseClick()
    {
        if (highlight)
        {
            if (selection != null)
            {
                selection.gameObject.GetComponent<Outline>().enabled = false;
            }
            selection = raycastHit.transform;
            selection.gameObject.GetComponent<Outline>().enabled = true;
            highlight = null;
        }
        else
        {
            if (selection)
            {
                selection.gameObject.GetComponent<Outline>().enabled = false;
                selection = null;
            }
        }
    }

    void HandleHighlightOnCheckRaycastHit(Ray ray, string tagSelectable, Color outlineColor, float outlineWidth)
    {
        if (Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag(tagSelectable) && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = outlineColor;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = outlineWidth;
                }
            }
            else
            {
                highlight = null;
            }

        }
    }

    void EnableOutlineIfNeeded(GameObject obj)
    {



        //if (obj.GetComponent<Outline>() != null)
        //{
        //    obj.GetComponent<Outline>().enabled = true;
        //}
        //else
        //{
        //    Outline outline = obj.AddComponent<Outline>();
        //    outline.enabled = true;
        //    outline.OutlineColor = ColorOutlineSelected;
        //    outline.OutlineWidth = outlineWidth;
        //}
    }
}
