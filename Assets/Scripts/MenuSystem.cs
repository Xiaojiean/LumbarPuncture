using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class MenuSystem : MonoBehaviour, ISpeechHandler { 

    public GameObject manipulation;
    public GameObject animationOb;
    public GameObject practice;
    private GameObject current;
    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if(eventData.RecognizedText.Equals("Manipulation mode"))
        {
            changeMode(0);
        }
        else if (eventData.RecognizedText.Equals("Animation mode"))
        {
            changeMode(1);
        }
        else if (eventData.RecognizedText.Equals("Practice mode"))
        {
            changeMode(2);
        }
    }

    // Use this for initialization
    void Start () {
		current = GameObject.Instantiate(manipulation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void changeMode(int mode)
    {
        Destroy(current);

        switch (mode)
        {
            case 0:
                current = GameObject.Instantiate(manipulation);
                break;
            case 1:
                current =  GameObject.Instantiate(animationOb);
                break;
            case 2:
                current =  GameObject.Instantiate(practice);
                break;
        }

    }
}
