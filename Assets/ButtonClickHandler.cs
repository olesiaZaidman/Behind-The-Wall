using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
   IPointerClickHandler  //using UnityEngine.EventSystems; IPointerUpHandler,   IPointerDownHandler,
{
    [System.Serializable]
    public class CustomUIEvent : UnityEvent { } //using UnityEngine.Events;
    public CustomUIEvent OnUiEvent;
    PlayerController playerController;
    ImageFillOverTime imageFillOverTime;

    [SerializeField] Button myButton;

    [SerializeField] Image imageButton;
    [SerializeField] Image imageFrame;
    [SerializeField] Image imageToFill;

    Vector3 defaultSize = new Vector3(0.008f, 0.008f, 0.008f);
    Vector3 newSize = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 superSize = new Vector3(0.015f, 0.015f, 0.015f);

    float transitionTime = 0.2f;

    [SerializeField] Color interactionColor = new Color(1, 0.8392157f, 0.2509804f, 1);
    [SerializeField] Color defaultColor = Color.white;

    /*we need to have a reference to current courutine and stop it we is started another one - 
     * otherwise we have bug on enter exi*/

    private List<Coroutine> coroutines = new List<Coroutine>();

    bool isAbort = false;

    void Start()
    {
        transform.localScale = defaultSize;
        playerController = FindObjectOfType<PlayerController>();
        imageFillOverTime = GetComponent<ImageFillOverTime>();

        if (imageFillOverTime == null)
        {
            imageFillOverTime = GetComponentInParent<ImageFillOverTime>();
        }

        // myButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopActivateButtonAnimation();
        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    playerController.IsPerformingAction = true;
        //}
    }

    public void StopAllCoroutinesInList()
    {
        for (int i = 0; i < coroutines.Count; i++)
        {
            if (coroutines[i] != null)
            {
                StopCoroutine(coroutines[i]);
                coroutines[i] = null;
            }
        }
        coroutines.Clear();
        /*   arr.forEach(Couroutine c =>
        {
            StopCoroutine(c);
        });*/
    }


    //void OnButtonClick() //same as OnPointerClick?
    //{

    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //  Debug.Log("Entered");
        if (!imageFillOverTime.IsInInteraction)
        {
            StopAllCoroutinesInList();
        }
        coroutines.Add(StartCoroutine(AnimateHoverButton()));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //  Debug.Log("Exited");
        if (!imageFillOverTime.IsInInteraction)
        {
            StopAllCoroutinesInList();
        }
        coroutines.Add(StartCoroutine(RestoreButton()));
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        StartActivateButtonAnimation();
    }

    public void StopActivateButtonAnimation()
    {
        StopAllCoroutinesInList();
        isAbort = true;

        if (imageFillOverTime.IsInInteraction)
        {
            coroutines.Add(StartCoroutine(imageFillOverTime.ResetFillImageOverTime()));
        }

        coroutines.Add(StartCoroutine(RestoreButton())); /*Should we unite RestoreButton & DeselectButtonCoroutine?*/
        coroutines.Add(StartCoroutine(DeselectButtonCoroutine(isAbort)));

        isAbort = false;

    }

    private IEnumerator DeselectButtonCoroutine(bool abort) //DeselectButtonCoroutine
    {
        //  yield return new WaitUntil(() => imageFillOverTime.IsFillComplete);
        yield return new WaitUntil(() => imageFillOverTime.IsFillComplete || abort);

        playerController.IsPerformingAction = false;
        //Deselect the button:
        EventSystem.current.SetSelectedGameObject(null);
    }


    void StartActivateButtonAnimation()
    {
        StopAllCoroutinesInList();
        coroutines.Add(StartCoroutine(AnimateClickButton()));

        coroutines.Add(StartCoroutine(imageFillOverTime.FillImageOverTimeCoroutine()));     //OR coroutines.Add(StartCoroutine(StartSearchingAnimation()));
        coroutines.Add(StartCoroutine(DeselectButtonCoroutine(isAbort)));
        /*Should we unite RestoreButton & DeselectButtonCoroutine?*/
        if (!imageFillOverTime.IsInInteraction)
        {
            coroutines.Add(StartCoroutine(RestoreButton()));
        }
        // Debug.Log("Clicked - works only when script is attached to the Button");}

   
    }

    //private IEnumerator StartSearchingAnimation()  //bool isAction
    //{
    //    yield return new WaitUntil(() => playerController.IsPerformingAction);

    //    coroutines.Add(StartCoroutine(imageFillOverTime.FillImageOverTimeCoroutine()));
    //    coroutines.Add(StartCoroutine(DeselectButtonCoroutine(isAbort)));
    //    /*Should we unite RestoreButton & DeselectButtonCoroutine?*/
    //    if (!imageFillOverTime.IsInInteraction)
    //    {
    //        coroutines.Add(StartCoroutine(RestoreButton()));
    //    }
    //    // Debug.Log("Clicked - works only when script is attached to the Button");}

    //}




    public IEnumerator RestoreButton()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(AnimationTransition(defaultSize, defaultColor, transitionTime));
    }

    public IEnumerator AnimateHoverButton()
    {
        if (!imageFillOverTime.IsInInteraction)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(AnimationTransition(newSize, interactionColor, transitionTime));
        }
    }

    public IEnumerator AnimateClickButton()
    {
        if (!imageFillOverTime.IsInInteraction)
        {
            yield return new WaitForEndOfFrame(); // yield return StartCoroutine(AnimationTransition(superSize, interactionColor, transitionTime));
            StartCoroutine(AnimationTransition(superSize, interactionColor, transitionTime));
        }
    }


    //public IEnumerator AnimationTransition(Vector3 newSize, Color newColor, float transitionTime)
    //{
    //    float timer = 0;

    //    Vector3 startSize = transform.localScale;
    //    Color startColor = imageButton.color;

    //    while (timer < transitionTime)
    //    {
    //        // Increment the timer with the time elapsed since the last frame
    //        timer += Time.deltaTime;

    //        // Yield and wait for the next frame
    //        yield return null;

    //        // Calculate the current scale value based on the elapsed time
    //        transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
    //        imageFrame.transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
    //        imageToFill.transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);

    //        // Calculate the current Color value based on the elapsed time
    //        imageButton.color = Color.Lerp(startColor, newColor, timer / transitionTime);
    //        imageFrame.color = Color.Lerp(startColor, newColor, timer / transitionTime);
    //        imageToFill.color = Color.Lerp(startColor, newColor, timer / transitionTime);

    //    }
    //}

    public IEnumerator AnimationTransition(Vector3 newSize, Color newColor, float transitionTime)
    {
        // Start the scale transition
        yield return StartCoroutine(ScaleTransition(newSize, transitionTime));

        // Start the color transition
        yield return StartCoroutine(ColorTransition(newColor, transitionTime));

        Debug.Log("Transitions complete!");

    }


    public IEnumerator ScaleTransition(Vector3 newSize, float transitionTime)
    {
        float timer = 0;
        Vector3 startSize = transform.localScale;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            yield return null;

            transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
            imageFrame.transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
            imageToFill.transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
        }
    }

    public IEnumerator ColorTransition(Color newColor, float transitionTime)
    {
        float timer = 0;
        Color startColorButton = imageButton.color;
        Color startColorFrame = imageFrame.color;
        Color startColorFill = imageToFill.color;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            yield return null;

            imageButton.color = Color.Lerp(startColorButton, newColor, timer / transitionTime);
            imageFrame.color = Color.Lerp(startColorFrame, newColor, timer / transitionTime);
            imageToFill.color = Color.Lerp(startColorFill, newColor, timer / transitionTime);
        }
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
