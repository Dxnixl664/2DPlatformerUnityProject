using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraYPosition;

    void Update()
    {
        if(playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, cameraYPosition, transform.position.z);
        }
    }
}

