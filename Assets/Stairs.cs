using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public ColliderType colliderType;
    public Vector3 teleportPosition;
  //  public Transform endPosition;
    public enum ColliderType
    {
        Up = 1, //teleportPosition =  new Vector3( -7.32f,  0.78f,  -0.33f)
        Down = 2 //teleportPosition =  new Vector3(-7.32f, -0.11f, -3.8f)
    }

    //void Start()
    //{
    //    teleportPosition = endPosition.position;
    //}

}
