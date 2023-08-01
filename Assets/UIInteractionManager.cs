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

    public void DectivateButtonAndImageFillWithColor() ////or DectivateButtonWithColor?
    {
        SetImageColor(imageFrame, defaultColor);
        SetImageColor(imageToFill, defaultColor);
        SetImageColor(imageButton, defaultColor);
    }

    public void DectivateButtonAndImageFillWithColor(Color _colorDefault) ////or DectivateButtonWithColor? or DectivateButtonWithColor
    {
        SetImageColor(imageFrame, _colorDefault);
        SetImageColor(imageToFill, _colorDefault);
        SetImageColor(imageButton, _colorDefault);
    }

    public void ActivateButtonAndImageFillWithColor() ////or DectivateButtonWithColor?
    {
        SetImageColor(imageFrame, interactionColor);
        SetImageColor(imageToFill, interactionColor);
        SetImageColor(imageButton, interactionColor);
    }

    public void ActivateButtonAndImageFillWithColor(Color _colorInteraction) ////or DectivateButtonWithColor? or DectivateButtonWithColor
    {
        SetImageColor(imageFrame, _colorInteraction);
        SetImageColor(imageToFill, _colorInteraction);
        SetImageColor(imageButton, _colorInteraction);
    }



    public Color GetColor(int i)
    {
        return interactionColor;
    }

    public void SetColor(Color _color)
    {
        interactionColor = _color;
    }




    /*EXTRA
     
    public void ActivateButtonWithColor() //or ActivateButtonWithColor?
    {
        SetImageColor(imageButton, interactionColor);
    }


    public void ActivateButtonWithColor(Color _colorInteraction) //or ActivateButtonWithColor? or ActivateButtonWithColor
    {
        SetImageColor(imageButton, _colorInteraction);
    }

    public void ActivateImageFillWithColor(Color _colorInteraction) //or ActivateButtonWithColor? or ActivateButtonWithColor
    {
        SetImageColor(imageFrame, _colorInteraction);
        SetImageColor(imageToFill, _colorInteraction);
    }

    public void ActivateImageFillWithColor() //or ActivateButtonWithColor? or ActivateButtonWithColor
    {
        SetImageColor(imageFrame, interactionColor);
        SetImageColor(imageToFill, interactionColor);
    }

    public void DectivateButtonWithColor(Color _colorDefault) ////or DectivateButtonWithColor? or DectivateButtonWithColor
    {
        SetImageColor(imageButton, _colorDefault);
    }

    public void DectivateButtonWithColor() ////or DectivateButtonWithColor?
    {
        SetImageColor(imageButton, defaultColor);
    }

    public void DectivateImageFillWithColor(Color _colorDefault) ////or DectivateButtonWithColor? or DectivateButtonWithColor
    {
        SetImageColor(imageFrame, _colorDefault);
        SetImageColor(imageToFill, _colorDefault);
    }

    public void DectivateImageFillWithColor() ////or DectivateButtonWithColor?
    {
        SetImageColor(imageFrame, defaultColor);
        SetImageColor(imageToFill, defaultColor);
    }
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
