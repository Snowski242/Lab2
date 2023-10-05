using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    public PathController controller;
    private void OnTriggerEnter(Collider other)
    {
        if(other == null)
        {
            controller.MoveSpeed = 3;
        }
        else
        {
            controller.MoveSpeed = 1;
        }
    }

    
}
