using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    ImageFillOverTime imageFillOverTime;
    [SerializeField] Button myButton;
    void Start()
    {
        imageFillOverTime = GetComponent<ImageFillOverTime>();
    }

    public void ResetButtonColor()
    {
        StartCoroutine(ResetButtonColorCoroutine());
    }

    private IEnumerator ResetButtonColorCoroutine()
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
}
