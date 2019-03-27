using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class TextToSpeechController : MonoBehaviour {

   
    private TextToSpeech tts;

    private string piamater = "This is the Piamater. The Vascular innermost layer of the meninges. Extends into the anterior median fissure and reflects onto the anterior and posterior roots as they cross over the sub-arachnoid space. ";
    private string duramater = "This is the Duramater. The Outermost layer of the meninges with extradural space separating it from the bones and vertebral canal. Tubular coverings of dura mater surround the spinal nerves and their roots as they pass laterally. These coverings merge to become part of the outer covering of the nerves, known as the epineurium.";
    private string arachnoid = "This is the Arachnoid. A Thin, delicate, avascular layer forming the middle layer of the meninges. It lies against the deep surface of the dura mater with the subarachnoid space separating it from the pia mater. And inside is the Subarachnoid Space, Found between the pia and arachnoid mater containing cerebrospinal fluid. Superiorly the subarachnoid space around the spinal cord becomes continuous with the foramen magnum to surround the brain. Inferiorly, it terminates at the level of the lower border of vertebrae at S2.";

    void Start () {
        tts = GetComponent<TextToSpeech>();
	}

    public void startSpeech(int layer)
    {
        tts.StopSpeaking();
        string msg;
        switch (layer)
        {
            case 1:
                msg = string.Format(piamater, tts.Voice.ToString());
                break;
            case 2:
                msg = string.Format(duramater, tts.Voice.ToString());
                break;
            case 3:
                msg = string.Format(arachnoid, tts.Voice.ToString());
                break;
            default:
                return;
        }
        
        tts.StartSpeaking(msg);
    }
	
	
}
