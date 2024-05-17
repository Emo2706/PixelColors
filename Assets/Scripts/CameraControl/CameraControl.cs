using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] int InitialZoom;
    Camera _cam;
    [SerializeField] float zoomAmount;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;  
    }

    // Update is called once per frame
    void Update()
    {
        _cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomAmount;
    }
}
