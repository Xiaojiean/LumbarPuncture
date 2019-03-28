using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeController : MonoBehaviour {

    
    public GameObject syringeObj;
    private SyringeAction syringeAction;
    private TextToSpeech tts;
    private GameObject goalSyringe;
    private ModelManipulator model;

    private string stage0 = "The patient is asked to position themselves in the foetal position on their side. The needle will be inserted between two vertebrae in the lumbar region. Please rotate the so that it is perpendicular with the spin.";
    private string stage1 = "Well done! Now we need to align the needle so that we are correctly piercing between two of the vertebrae. Select the syringe and move it to the indicated zone.";
    private string stage2 = "Awesome! Almost there: We just need some final adjustment in order make this perfect. Please rotate it slightly to the right.";
    private string stage3 = "Now that the syringe is in place, we can start to insert the needle. The needle will pierce through the skin, the subcutaneous tissue and dura mater, into the spinal canal. The spinal fluid pressure can be meansured, or a sample of the spinal fluid can be collected for further analysis. Congratulations you have just performed a Lumbar Puncture! ";

    void Start () {

        tts = GetComponent<TextToSpeech>();
        syringeObj = GameObject.Instantiate(syringeObj);
        model = GetComponent<ModelManipulator>();
        syringeAction = syringeObj.GetComponent<SyringeAction>();
        goalSyringe = transform.Find("goal").gameObject;
        goalSyringe.SetActive(false);
        speek(stage0);
    }

    void Update()
    {

        if(syringeAction.getMode() == 0 && syringeObj.transform.localRotation.eulerAngles.x >= 265f && syringeObj.transform.localRotation.eulerAngles.x <= 275f)
        {
            
            syringeAction.changeMode();
            goalSyringe.SetActive(true);
            speek(stage1);
        }
        else if(syringeAction.getMode() == 2)
        {
            goalSyringe.SetActive(false);
            syringeAction.changeMode();
            speek(stage2);
        }
        else if(syringeAction.getMode() == 3 && syringeObj.transform.localRotation.eulerAngles.y <= 337f && syringeObj.transform.localRotation.eulerAngles.y >= 332f)
        {
           syringeAction.changeMode();
           speek(stage3);
            fadeLayers();
        }

    }

    void speek(string toTalk)
    {
        tts.StopSpeaking();
        string msg = string.Format(toTalk, tts.Voice.ToString());
        tts.StartSpeaking(msg);
    }

    public void OnDestroy()
    {
        Destroy(syringeObj);
    }

    private void fadeLayers()
    {
        model.fadeIndividualLayer("Spine", 0, 2f);
        model.fadeIndividualLayer("Duramater", 0, 2f);
        model.fadeIndividualLayer("Arachnoid", 0, 2f);
        model.fadeIndividualLayer("Piamater", 0.5f, 2f);
    }
}
