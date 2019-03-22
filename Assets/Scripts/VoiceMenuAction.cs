using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class VoiceMenuAction : MonoBehaviour, ISpeechHandler
{

    private bool mode = false;
    public GameObject animator;
    public GameObject manipulator;
	// Use this for initialization
	void Start ()
    {
        animator.SetActive(false);
    }

    void Update()
    {

    }

    void changeMode()
    {
        mode = !mode;
        if (mode)
        {
            animator.SetActive(true);
            manipulator.SetActive(false);
        }
        else
        {
            animator.SetActive(false);
            manipulator.SetActive(true);
        }
    }
	

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        
        if (eventData.RecognizedText.Equals("Change Mode"))
        {
            
            changeMode();
        }
        else if (eventData.RecognizedText.Equals("Begin Show"))
        {
            if (mode)
            {
                Debug.Log("start animation");
                animator.GetComponent<AnimationController>().animationStart(); 
            }
        }
       
        else
        {
            return;
        }

        eventData.Use();
    }
}
