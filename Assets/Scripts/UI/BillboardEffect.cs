using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        //cam = Camera.main;
    }

    void LateUpdate()
    {
        /*transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(0f,transform.rotation.eulerAngles.y, 0f);*/
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation*Vector3.up);
    }
}
