using System;
using System.Collections;
using System.Collections.Generic;
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


    /* OnPointerClick uses Unity’s event system, OnMouseDown uses the physics engine. 
     * Therefore, OnMouseDown is generally better suited for gameplay controls than for UI. 
     * OnPointerClick should work best with the UI since it relies on EventSystem and 
     * should help you with clicking through objects etc. 
     * You won’t need a Collider for OnPointerClick, but you will for OnMouseDown.*/


    /*BUG ; Color change yellow-white  -is broken*/

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

    public static bool isChangingColor = false;

    void Start()
    {
        transform.localScale = defaultSize;
        playerController = FindObjectOfType<PlayerController>();
        imageFillOverTime = GetComponent<ImageFillOverTime>();

        if (imageFillOverTime == null)
        {
            imageFillOverTime = GetComponentInParent<ImageFillOverTime>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopActivateButtonAnimation();
        }
    }

    /*
     1. OnPointerEnter > if (!imageFillOverTime.IsClicked)> if(!isChangingColor)>  AnimateHoverButton  &&  isChangingColor = true
       
     2. OnPointerExit > if (!imageFillOverTime.IsClicked) > if(isChangingColor) > RestoreColorButton   
     3.  OnPointerEnter > if (!imageFillOverTime.IsClicked)> AnimateHoverButton  

     4. OnPointerClick > StartActivateButtonAnimation>:
     -  if (!imageFillOverTime.IsClicked) > AnimateClickButton 
        
     - FillImageOverTimeCoroutine >   isClicked = true;
      - DeselectButtonCoroutine(isAbort)));  >   isClicked = false;
    -  if (!imageFillOverTime.IsClicked) > RestoreButton()   


         5. OnPointerExit > RestoreColorButton   if (!imageFillOverTime.IsClicked)

   6* Anymoment. StopActivateButtonAnimation > :
    - if (imageFillOverTime.IsClicked) > ResetFillImageOverTime 
  - RestoreButton  if (!imageFillOverTime.IsClicked)
  -DeselectButtonCoroutine(isAbort)
   - isAbort = false;



RestoreButton(): ColorTransition & ScaleTransition
 RestoreSizeButton(): ScaleTransition
  RestoreColorButton(): ColorTransition

*/


    public void OnPointerEnter(PointerEventData eventData)
    {
        //  Debug.Log("Entered");
        if (!imageFillOverTime.IsClicked)
        {
            StopAllCoroutinesInList();
            coroutines.Add(StartCoroutine(AnimateHoverButton(newSize, interactionColor, transitionTime)));
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //  Debug.Log("Exited");
        if (!imageFillOverTime.IsClicked)
        {
            StopAllCoroutinesInList();

        }

        if (!imageFillOverTime.IsClicked)
        {
            coroutines.Add(StartCoroutine(RestoreColorButton(defaultColor, transitionTime)));
            /*we have balagan with colors clicks and resets :(*/
        }

        coroutines.Add(StartCoroutine(RestoreSizeButton(defaultSize, transitionTime)));

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        StartActivateButtonAnimation();
    }

    void StartActivateButtonAnimation()
    {
        StopAllCoroutinesInList();

        if (!imageFillOverTime.IsClicked) // && !isChangingColor
        {
            coroutines.Add(StartCoroutine(AnimateClickButton(superSize, interactionColor, transitionTime)));
          //  isChangingColor = true;
        }

        //if (isChangingColor)
        //{
        coroutines.Add(StartCoroutine(imageFillOverTime.FillImageOverTimeCoroutine()));     //OR coroutines.Add(StartCoroutine(StartSearchingAnimation()));
        coroutines.Add(StartCoroutine(DeselectButtonCoroutine(isAbort)));
        /*Should we unite RestoreButton & DeselectButtonCoroutine?*/

     //   if (!imageFillOverTime.IsClicked) // &&!imageFillOverTime.IsClicked && !isChangingColor
      //    {
        coroutines.Add(StartCoroutine(RestoreButton(defaultSize, defaultColor, transitionTime)));

     // }
      // Debug.Log("Clicked - works only when script is attached to the Button");}

        // }


    }


    public void StopActivateButtonAnimation()
    {
        StopAllCoroutinesInList();
        isAbort = true;

        if (imageFillOverTime.IsClicked)
        {
            coroutines.Add(StartCoroutine(imageFillOverTime.ResetFillImageOverTime()));
        }

        coroutines.Add(StartCoroutine(RestoreButton(defaultSize, defaultColor, transitionTime))); /*Should we unite RestoreButton & DeselectButtonCoroutine?*/
        coroutines.Add(StartCoroutine(DeselectButtonCoroutine(isAbort)));
        // isChangingColor = false;
        isAbort = false;
    }



    private IEnumerator DeselectButtonCoroutine(bool abort) //DeselectButtonCoroutine
    {
        //  yield return new WaitUntil(() => imageFillOverTime.IsFillComplete);
        yield return new WaitUntil(() => imageFillOverTime.IsFillComplete || abort);

        //  playerController.IsPerformingAction = false;
        //Deselect the button:
        EventSystem.current.SetSelectedGameObject(null);
    }


    public IEnumerator RestoreButton(Vector3 newSize, Color newColor, float transitionTime)
    {
        yield return new WaitForEndOfFrame();
        //   StartCoroutine(AnimationTransition(defaultSize, defaultColor, transitionTime));
        StartCoroutine(ColorTransition(newColor, transitionTime));
        StartCoroutine(ScaleTransition(newSize, transitionTime));
    }

    public IEnumerator RestoreSizeButton(Vector3 newSize, float transitionTime)
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(ScaleTransition(newSize, transitionTime));
    }
    public IEnumerator RestoreColorButton(Color newColor, float transitionTime)
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(ColorTransition(newColor, transitionTime));
    }


    public IEnumerator AnimateHoverButton(Vector3 newSize, Color newColor, float transitionTime)
    {
        //  if (!imageFillOverTime.IsClicked)
        // {
        yield return new WaitForEndOfFrame();
        //  StartCoroutine(AnimationTransition(newSize, interactionColor, transitionTime));
        StartCoroutine(ColorTransition(newColor, transitionTime));
        StartCoroutine(ScaleTransition(newSize, transitionTime));
        // }
    }

    public IEnumerator AnimateClickButton(Vector3 newSize, Color newColor, float transitionTime)
    {
        //  if (!imageFillOverTime.IsClicked)
        //   {
        yield return new WaitForEndOfFrame(); // yield return StartCoroutine(AnimationTransition(superSize, interactionColor, transitionTime));
                                              //  StartCoroutine(AnimationTransition(superSize, interactionColor, transitionTime));
        StartCoroutine(ColorTransition(newColor, transitionTime));
        StartCoroutine(ScaleTransition(newSize, transitionTime));
        //  }
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

     //   Debug.Log("Transitions complete!");

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

      //  Debug.Log("isChangedColor: " + isChangingColor);

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            yield return null;

            imageButton.color = Color.Lerp(startColorButton, newColor, timer / transitionTime);
           imageFrame.color = Color.Lerp(startColorFrame, newColor, timer / transitionTime);
          imageToFill.color = Color.Lerp(startColorFill, newColor, timer / transitionTime);
        }

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

    public void DeactivateButton()
    {
        myButton.interactable = false;
    }

    public void ActivateButton()
    {
        myButton.interactable = true;
    }

}
