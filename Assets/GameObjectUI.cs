using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameObjectUI : MonoBehaviour
{
    public Image imageToFill;
    [Range(0.5f, 10f)]  public float fillDuration = 2f;

    float startFill = 0f;
    float targetFill = 1f;
    float timer = 0f;
    private bool isFillComplete = false; // New property to indicate fill completion

    public bool IsFillComplete
    {
        get { return isFillComplete; }
    }

    public void FillImageOverTime()
    {
        // Start the image fill coroutine
        StartCoroutine(FillImageOverTimeCoroutine());
    }

    private IEnumerator FillImageOverTimeCoroutine()
    {
        isFillComplete = false;

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
        StartCoroutine(ResetFillImageOverTime());
    }

    IEnumerator ResetFillImageOverTime()
   {
        yield return new WaitForSeconds(0.5f);
        timer = 0f;
        startFill = 0f;
        imageToFill.fillAmount = startFill;
        imageToFill.color = Color.white;

    }


}
