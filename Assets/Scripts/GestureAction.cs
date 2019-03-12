using HoloToolkit.Unity.InputModule;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour, INavigationHandler, IManipulationHandler, ISpeechHandler
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    [SerializeField]
    private float RotationSensitivity = 2f;

 

    private bool isNavigationEnabled = true;
    public bool IsNavigationEnabled
    {
        get { return isNavigationEnabled; }
        set { isNavigationEnabled = value; }
    }

    private Vector3 manipulationOriginalPosition = Vector3.zero;

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (isNavigationEnabled)
        {
                       
            //  float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity
            float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
            
            // 2.c: transform.Rotate around the Y axis using rotationFactor.
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (!isNavigationEnabled)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
            manipulationOriginalPosition = transform.position;
        }
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!isNavigationEnabled)
        {
            // transform's position be the manipulationOriginalPosition + eventData.CumulativeDelta
            transform.position = manipulationOriginalPosition + eventData.CumulativeDelta;
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        
        if (eventData.RecognizedText.Equals("Move Model"))
        {
            isNavigationEnabled = false;
        }
        else if (eventData.RecognizedText.Equals("Rotate Model"))
        {
            isNavigationEnabled = true;
        }
        else if (eventData.RecognizedText.Equals("Zoom In"))
        {
            
            manipulateSize(true);
        }
        else if (eventData.RecognizedText.Equals("Zoom Out"))
        {
            
            manipulateSize(false);
        }
        else if (eventData.RecognizedText.Equals("Flip Model"))
        {
            manipulateRotation(true);
        }
        else
        {
            return;
        }

        eventData.Use();
    }

    void manipulateSize(bool increase)
    {
        float modifier;
        if (increase)
        {
            modifier = 1.25f;
        }
        else
        {
            modifier = 0.75f;
        }

        transform.localScale *= modifier;
    }

    void manipulateRotation(bool bird) 
    {
        float xRotation = 0; 
        if (bird)
        {
            xRotation = 90;
        }
        else
        {
            xRotation = 90;
        }
        
        Vector3 vec = new Vector3(xRotation, 0, 0);
        transform.Rotate(vec);
    }
}