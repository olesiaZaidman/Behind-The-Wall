using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerClickHandler   //IPointerUpHandler? //using UnityEngine.EventSystems;
{
    ImageFillOverTime imageFillOverTime;
    [SerializeField] Button myButton;

    public class CustomUIEvent : UnityEvent { } //using UnityEngine.Events;
    public CustomUIEvent OnUiEvent;
    void Start()
    {
        imageFillOverTime = GetComponent<ImageFillOverTime>();

        if (imageFillOverTime == null)
        {
            imageFillOverTime = GetComponentInParent<ImageFillOverTime>();  
        }

        myButton.onClick.AddListener(OnButtonClick); 
    }

    void OnButtonClick()
    {
        imageFillOverTime.StartImageFillOverTime();
        DeselectButtonOnFillComplete();
    }


    public void DeselectButtonOnFillComplete() //DeselectButtonOnFillComplete
    {
        StartCoroutine(DeselectButtonCoroutine());
    }

    private IEnumerator DeselectButtonCoroutine() //DeselectButtonCoroutine
    {
        // Wait until the image fill is complete
        yield return new WaitUntil(() => imageFillOverTime.IsFillComplete);
        //Deselect the button:

        EventSystem.current.SetSelectedGameObject(null);
    }


    public void DeactivateButton()
    {
        myButton.interactable = false;
    }

    public void ActivateButton()
    {
        myButton.interactable = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exited");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked - works only when script is attached to the Button");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down - works only when script is attached to the Button");
    }

  
}
