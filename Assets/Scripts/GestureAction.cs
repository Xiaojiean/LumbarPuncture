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
    public float reducedScale;
    private bool isNavigationEnabled = true;
    private bool modelAxisRotation = true; //true for y, false for x;
    

    private ModelManipulator modelManipulator;

    private ObjectManipulator container;
    private ObjectManipulator model;


    private Vector3 manipulationOriginalPosition = Vector3.zero;

    void Start()
    {
        modelManipulator = GetComponent<ModelManipulator>();

        container = GetComponent<ObjectManipulator>();
        model = transform.Find("ModelContainer").GetComponent<ObjectManipulator>();

        model.scale(reducedScale);
    }

    void Update()
    {
        if (Input.GetKeyDown("="))
        {
            model.zoomSmooth(1.2f,0.5f);
        }
        else if (Input.GetKeyDown("-"))
        {
            model.zoomSmooth(0.8f, 0.5f);
        }
        else if (Input.GetKeyDown("0")){
            isNavigationEnabled = !isNavigationEnabled;
        }
        else if (Input.GetKeyDown("9")){
            modelAxisRotation = !modelAxisRotation;
        }
        else if (Input.GetKeyDown("]"))
        {
            modelManipulator.addLayer();
        }
        else if (Input.GetKeyDown("["))
        {
            modelManipulator.removeLayer();
        }
        else if (Input.GetKeyDown("8"))
        {
            modelManipulator.explode();
        }
    }



    public bool IsNavigationEnabled
    {
        get { return isNavigationEnabled; }
        set { isNavigationEnabled = value; }
    }

    

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        float rotationFactor;
        if (isNavigationEnabled)
        {
            
            if (modelAxisRotation)
            {
                rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
                container.rotate(new Vector3(0, -1 * rotationFactor, 0));
            }
            else
            {
                rotationFactor = -eventData.NormalizedOffset.x * RotationSensitivity;
                model.rotate(new Vector3(1 * rotationFactor, 0, 0));
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
            model.zoomSmooth(1.2f, 0.5f);
        }
        else if (eventData.RecognizedText.Equals("Zoom Out"))
        {
            model.zoomSmooth(0.8f, 0.5f);
        }
        else
        {
            return;
        }

        eventData.Use();
    }

 
}