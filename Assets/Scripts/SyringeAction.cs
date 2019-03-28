using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeAction : MonoBehaviour, IManipulationHandler, INavigationHandler
{
    private Vector3 manipulationOriginalPosition = Vector3.zero;

    private bool navigationEnabled = true;
    private bool movable = true;
    private bool modelAxisRotation = true;
    private float stage1Sensitivity = 0.5f;
    private float stage0Sensitivity = 2f;

    private int mode = 0;

    ObjectManipulator needle;


    Vector3 tmp;

    // Use this for initialization
    void Start()
    {
        needle = transform.Find("Needle").GetComponent<ObjectManipulator>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            StartCoroutine(A(other));
        }
        
    }

    IEnumerator A(Collider c)
    {
        tmp = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z);
        yield return new WaitForSeconds(0.3f);
        changeMode();
    }

    public void changeMode()
    {
        mode++;
        Debug.Log("On mode: " + mode);
        switch (mode)
        {
            case 0:
                modelAxisRotation = true;
                break;
            case 1:
                InputManager.Instance.PopModalInputHandler();
                transform.localRotation = Quaternion.Euler(new Vector3(1, 0, 0) * 270);
                navigationEnabled = false;
                break;
            case 2:
                
                break;
            case 3:
                InputManager.Instance.PopModalInputHandler();
                transform.position = tmp;
                navigationEnabled = true;
                modelAxisRotation = false;
                break;
            case 4:
               
                movable = false;
                InputManager.Instance.PopModalInputHandler();
                transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 334, 0));
                needle.moveSmooth(new Vector3(needle.getXPos(),0,needle.getZPos()), 5);
                break;
          
        }
    }

    public int getMode()
    {
        return mode;
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
        if (!navigationEnabled && movable)
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
        if (navigationEnabled && movable)
        {
            if (modelAxisRotation)
            {
                rotationFactor = eventData.NormalizedOffset.x * stage0Sensitivity;
                transform.localRotation = transform.localRotation * Quaternion.Euler(new Vector3(1, 0, 0) * -1 * rotationFactor);
            }
            else
            {
                rotationFactor = eventData.NormalizedOffset.x * stage1Sensitivity;
                transform.localRotation = transform.localRotation * Quaternion.Euler(new Vector3(0, 0, 1) * -1 * rotationFactor);
            }
               
        }

    }


    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
      
    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

}
