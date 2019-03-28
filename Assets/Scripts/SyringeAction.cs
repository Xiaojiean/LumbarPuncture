using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeAction : MonoBehaviour, IManipulationHandler, INavigationHandler
{
    private Vector3 manipulationOriginalPosition = Vector3.zero;

    private bool navigationEnabled = true;
    private bool modelAxisRotation = false;
    private float RotationSensitivity = 2f;

    private int mode = 0;

    ObjectManipulator needle;

    // Use this for initialization
    void Start()
    {
        needle = GetComponent<ObjectManipulator>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void changeMode()
    {
        mode++;

        switch (mode)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }



    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        if (!navigationEnabled)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
            manipulationOriginalPosition = transform.position;
        }
        
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!navigationEnabled)
        {
            transform.position = manipulationOriginalPosition + eventData.CumulativeDelta;
        }
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        float rotationFactor;
        if (navigationEnabled)
        {

                rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
                transform.localRotation = transform.localRotation * Quaternion.Euler(new Vector3(1,0,0) * -1 *  rotationFactor);
        }

    }


    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        
    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
       
    }


    public Transform getTransform() {
        return transform;
    }
}
