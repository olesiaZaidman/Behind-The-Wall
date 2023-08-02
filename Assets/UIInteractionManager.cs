using UnityEngine;
using UnityEngine.UI;

public class UIInteractionManager : MonoBehaviour
{

    [SerializeField] Image imageFrame;

    [SerializeField] Image imageToFill;

    [SerializeField] Image imageButton;


    [SerializeField] Color interactionColor = new Color(1, 0.8392157f, 0.2509804f, 1);

    [SerializeField] Color defaultColor = Color.white;


    void SetImageColor(Image _i, Color _color)
    {
        _i.color = _color;
    }

    public void DectivateButtonAndImageFillWithDeafultColor() ////or DectivateButtonDeafultWithColor?
    {
        SetImageColor(imageFrame, defaultColor);
        SetImageColor(imageToFill, defaultColor);
        SetImageColor(imageButton, defaultColor);
    }

    public void DectivateButtonAndImageFillWithDeafultColor(Color _colorDefault) ////or DectivateButtonDeafultWithColor? or DectivateButtonDeafultWithColor
    {
        SetImageColor(imageFrame, _colorDefault);
        SetImageColor(imageToFill, _colorDefault);
        SetImageColor(imageButton, _colorDefault);
    }

    public void ActivateButtonAndImageFillWithInteractionColor() ////or DectivateButtonDeafultWithColor?
    {
        SetImageColor(imageFrame, interactionColor);
        SetImageColor(imageToFill, interactionColor);
        SetImageColor(imageButton, interactionColor);
    }

    public void ActivateButtonAndImageFillWithInteractionColor(Color _colorInteraction) ////or DectivateButtonDeafultWithColor? or DectivateButtonDeafultWithColor
    {
        SetImageColor(imageFrame, _colorInteraction);
        SetImageColor(imageToFill, _colorInteraction);
        SetImageColor(imageButton, _colorInteraction);
    }



    public Color GetInteractionColor()
    {
        return interactionColor;
    }

    public void SetInteractionColor(Color _color)
    {
        interactionColor = _color;
    }

    public void ActivateButtonWithInteractionColor() //or ActivateButtonWithInteractionColor?
    {
        SetImageColor(imageButton, interactionColor);
    }


    public void ActivateButtonWithInteractionColor(Color _colorInteraction) //or ActivateButtonWithInteractionColor? or ActivateButtonWithInteractionColor
    {
        SetImageColor(imageButton, _colorInteraction);
    }

    public void ActivateImageFillWithInteractionColor(Color _colorInteraction) //or ActivateButtonWithInteractionColor? or ActivateButtonWithInteractionColor
    {
        SetImageColor(imageFrame, _colorInteraction);
        SetImageColor(imageToFill, _colorInteraction);
    }

    public void ActivateImageFillWithInteractionColor() //or ActivateButtonWithInteractionColor? or ActivateButtonWithInteractionColor
    {
        SetImageColor(imageFrame, interactionColor);
        SetImageColor(imageToFill, interactionColor);
    }

    public void DectivateButtonWithDeafultColor(Color _colorDefault) ////or DectivateButtonDeafultWithColor? or DectivateButtonDeafultWithColor
    {
        SetImageColor(imageButton, _colorDefault);
    }

    public void DectivateButtonDeafultWithColor() ////or DectivateButtonDeafultWithColor?
    {
        SetImageColor(imageButton, defaultColor);
    }

    public void DectivateImageFillWithDeafultColor(Color _colorDefault) ////or DectivateButtonDeafultWithColor? or DectivateButtonDeafultWithColor
    {
        SetImageColor(imageFrame, _colorDefault);
        SetImageColor(imageToFill, _colorDefault);
    }

    public void DectivateImageFillWithDeafultColor() ////or DectivateButtonDeafultWithColor?
    {
        SetImageColor(imageFrame, defaultColor);
        SetImageColor(imageToFill, defaultColor);
    }


    /*EXTRA
     
   
*/

}


//public void SetUIInteractionManager(UIInteractionManager manager)
//{
//    uiManager = manager;
//}

/* public UIInteractionManager uiManager;
public ImageFillOverTime imageFillOverTime;

private void Start()
{
    // Set the reference to the UIInteractionManager in ImageFillOverTime
    imageFillOverTime.SetUIInteractionManager(uiManager);
}

public void StartFillingImageOverTime()
{
    imageFillOverTime.StartImageFillOverTime();
}*/
