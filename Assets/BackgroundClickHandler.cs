using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClickHandler : MonoBehaviour
{
    ObjectSelector playerSelector;

    void Start()
    {
        playerSelector = FindObjectOfType<ObjectSelector>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // DeselectObjectSelected();
            DesselectPlayerIfClickedOntOnHisLayer();
        }
    }

     void DesselectPlayerIfClickedOntOnHisLayer()
    {
        if(playerSelector != null) 
        {
            // Perform a screen-based raycast from the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Specify the maximum distance the raycast can reach
            float maxRaycastDistance = 100f;

            // Check if the raycast hits any objects visible on the screen
            if (Physics.Raycast(ray, out hit, maxRaycastDistance))
            {
                // Check if the clicked object is the player (or on the player layer)
                if (hit.transform.gameObject.layer == playerSelector.PlayerLayer)
                {
                    // Clicked on a player, so toggle its selection state
                    Debug.Log("Clicked on a player");
                }
                else
                {
                    Debug.Log("Hit object layer: " + hit.transform.gameObject.layer);
                    playerSelector.Deselect();
                    // Clicked on an object other than the player, so deselect the current object
                    //if (playerSelector.Selected)
                    //{
                    //  //  playerSelector.Selected = false;
                    // //   playerSelector.SetGameObjectColor(playerSelector.DefaultColor);
                    //    Debug.Log("Object Deselected!");
                    //}
                }

            }

        } 
       
    }

    void DeselectObjectSelected()
    {
        // DectivateButtonWithColor the currently selected object (if any)
        //  ObjectSelector.selectedObject?.GetComponent<ObjectSelector>()?.DectivateButtonWithColor();

        // DectivateButtonWithColor the player if it was selected
        Debug.Log("Left Mouse Clicked");
        if (playerSelector != null)
        {
            Debug.Log("Player exists");

            if (playerSelector.Selected) //NOT WORKING
            {
                playerSelector.Selected = false;
                //  playerSelector.SetGameObjectColor(playerSelector.DefaultColor);

                Debug.Log("Player Deselected!");
            }
        }
    }

 
    
    

}
