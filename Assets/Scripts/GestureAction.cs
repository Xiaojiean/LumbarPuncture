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
    private bool modelAxisRotation = true; //true for y, false for x;

    private ModelManipulator model;

    void Start()
    {
        model = GetComponent<ModelManipulator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("="))
        {
            model.zoomIn();
        }
        else if (Input.GetKeyDown("-"))
        {
            model.zoomOut();
        }
        else if (Input.GetKeyDown("0")){
            isNavigationEnabled = !isNavigationEnabled;
        }
        else if (Input.GetKeyDown("9")){
            modelAxisRotation = !modelAxisRotation;
        }
        else if (Input.GetKeyDown("]"))
        {
            model.addLayer();
        }
        else if (Input.GetKeyDown("["))
        {
            model.removeLayer();
        }

    }



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
            if(modelAxisRotation)
            {
                float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
                model.rotateModelContainer(0, -1 * rotationFactor, 0);
            }
            else
            {
                float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
                model.rotateModel(1 * rotationFactor, 0, 0);
            }      
              
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
            model.zoomIn();
        }
        else if (eventData.RecognizedText.Equals("Zoom Out"))
        {
            model.zoomOut();
        }
        else
        {
            return;
        }

        eventData.Use();
    }

 
}