using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public GameObjectUI gameObjectUI;
    public Button myButton;
    private ColorBlock normalColorBlock;

    void Start()
    {
        normalColorBlock = myButton.colors; 
        // Add an event listener to the button to handle the click

        //  myButton.onClick.AddListener(OnButtonClick);

    }

    //void OnButtonClick()
    //{
    //    // Call the FillImageOverTime method in ImageFillOverTime script
    //    gameObjectUI.FillImageOverTime();
    //}

  

    public void ResetButtonColor()
    {
        StartCoroutine(ResetButtonColorCoroutine());
    }

    private IEnumerator ResetButtonColorCoroutine()
    {
        // Wait until the image fill is complete
        yield return new WaitUntil(() => gameObjectUI.IsFillComplete);

        //  myButton.image.color = Color.white;
        // myButton.colors = normalColorBlock;
        //Color.normalColor; ColorBlock.defaultColorBlock;
        //  ColorBlock updatedColorBlock = myButton.colors;
        //   updatedColorBlock.selectedColor = normalColorBlock.normalColor;
        //  myButton.colors = updatedColorBlock;



    }

    public void Deactivate()
    {
        myButton.interactable = false;
    }

    // Method to select the button
    public void Activate()
    {
        myButton.interactable = true;
    }
}
