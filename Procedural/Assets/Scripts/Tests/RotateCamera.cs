using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform cameraTarget;
    public float rotateSpeed = 50f;

    void Update()
    {
        if(Input.GetKey(KeyCode.A)) {
            transform.RotateAround(cameraTarget.position, Vector3.up, rotateSpeed * Time.deltaTime);
        } else if(Input.GetKey(KeyCode.D)) {
            transform.RotateAround(cameraTarget.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        }
    }
}
