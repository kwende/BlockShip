using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Haptic : MonoBehaviour
{
    private DateTime _triggerTime; 
    private OVRInput.Controller _controller; 
    private OVRGrabber _grabber; 

    // Start is called before the first frame update
    void Start()
    {
        // _triggerTime = DateTime.MinValue; 
        // _controller = gameObject.name == "RightSphere" ? OVRInput.Controller.RTouch: OVRInput.Controller.LTouch; 
        // _grabber = gameObject.GetComponent<OVRGrabber>(); 
    }

    void Update()
    {
        // DateTime now = DateTime.Now; 

        // if(_triggerTime != DateTime.MinValue && 
        //     (now - _triggerTime).TotalMilliseconds >= 100)
        // {
        //     OVRInput.SetControllerVibration(0, 0, _controller);
        //     _triggerTime = DateTime.MinValue;
        // }
    }

    // void OnTriggerEnter(Collider collision)
    // {
    //     if(collision.name.StartsWith("Cube") && _grabber.grabbedObject != null)
    //     {
    //         OVRInput.SetControllerVibration(1, 1, _controller);
    //         _triggerTime = DateTime.Now; 
    //     }
    // }
}
