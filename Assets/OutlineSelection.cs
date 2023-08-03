
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
            ToggleOutlineFunctionality(highlight?.gameObject, false);
            highlight = null;
        }

        if (IsNotPointerOverUIElement())
        //We need to have EventSystem in the hierarchy before using EventSystem
        {
            HandleHighlightOnCheckRaycastHit(ray, tagSelectable, ColorOutlineSelected, outlineWidth);
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

            ToggleOutlineFunctionality(selection?.gameObject, false);
            selection = raycastHit.transform;
            ToggleOutlineFunctionality(selection?.gameObject, true);
            highlight = null;

            /*In this context, selection?.gameObject means that if selection is not null, 
             * it will access the gameObject property of selection. If selection is null, 
             * the whole expression will result in null, and the method ToggleOutlineFunctionality
             *  will be called with a null argument.*/
        }
        else
        {
            ToggleOutlineFunctionality(selection?.gameObject, false);
            selection = null;
        }
    }

    void HandleHighlightOnCheckRaycastHit(Ray ray, string tagSelectable, Color outlineColor, float outlineWidth)
    {
        if (Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag(tagSelectable) && highlight != selection)
            {
                ToggleOutlineFunctionality(highlight?.gameObject, true);

                if (highlight?.GetComponent<Outline>() == null)
                {
                    AddEnabledOutlineComponent(highlight?.gameObject);
                    SetColorWidthOfOutline(highlight?.gameObject, outlineColor, outlineWidth);
                }
            }
            else
            {
                highlight = null;
            }

        }
    }

    Outline AddEnabledOutlineComponent(GameObject obj)
    {
        Outline outline = obj.AddComponent<Outline>();
        outline.enabled = true;
        return outline;
    }

    void ToggleOutlineFunctionality(GameObject obj, bool enable)
    {
        var outline = obj?.GetComponent<Outline>();

        if (outline != null)
        {
            outline.enabled = enable;
        }
    }

    void SetColorWidthOfOutline(GameObject obj, Color outlineColor, float outlineWidth)
    {
        var outline = obj?.GetComponent<Outline>();

        if (outline != null)
        {
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = outlineWidth;
        }
    }

    /*
null-conditional operator (?.)  is used to safely access members (properties, methods, and fields) 
of an object when that object might be null. It prevents a NullReferenceException from being thrown 
if the object is null.

obj?.GetComponent<Outline>() means that if obj is null or does not have an Outline component attached to it,
outline will be assigned null.Otherwise, outline will hold a reference to the Outline component attached to obj.
*/



    //void EnableOutlineFunctionalityIfNeeded(GameObject obj, bool enable)
    //{
    //    obj.GetComponent<Outline>().enabled = enable;
    //}



}
