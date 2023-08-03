using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ImageFillOverTime : MonoBehaviour
{
    [Range(0.5f, 10f)]  public float fillDuration = 2f;
    float delayBeforeDeactivate = 0.2f;
    [SerializeField] Image imageToFill;
    [SerializeField] Image imageFrame;

    float startFill = 0f;
    float targetFill = 1f;
    float timer = 0f;

    bool isFillComplete = false; // New property to indicate fill completion

    public bool IsFillComplete
    {
        get { return isFillComplete; }
    }

    bool isClicked = false; // New property to indicate fill completion

    public bool IsClicked
    {
        get { return isClicked; }
    }

    [SerializeField] Color interactionColor = new Color(1, 0.8392157f, 0.2509804f, 1);

    [SerializeField] Color defaultColor = Color.white;


    public IEnumerator FillImageOverTimeCoroutine()
    {
        isFillComplete = false;
        isClicked = true;

    //  imageToFill.color = interactionColor;
 //  imageFrame.color = interactionColor;

        while (timer < fillDuration)
        {
            // Calculate the current fill value based on the elapsed time
            startFill = Mathf.Lerp(0f, targetFill, timer / fillDuration);

            // Set the fill amount of the image
            imageToFill.fillAmount = startFill;

            // Increment the timer with the time elapsed since the last frame
            timer += Time.deltaTime;

            // Yield and wait for the next frame
            yield return null;
        }

        // Ensure the image is fully filled at the end
        imageToFill.fillAmount = targetFill;
        isFillComplete = true; // Mark the fill as complete
        isClicked = false;
        Debug.Log("Button interaction is complete");
        StartCoroutine(ResetFillImageOverTime());
    }

    public IEnumerator ResetFillImageOverTime()
   {
        yield return new WaitForSeconds(delayBeforeDeactivate);
        timer = 0f;
        startFill = 0f;
        imageToFill.fillAmount = startFill;
       
    //  imageToFill.color = defaultColor;
        //   imageFrame.color = defaultColor;    
    }
}
