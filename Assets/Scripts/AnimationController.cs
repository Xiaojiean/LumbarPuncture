using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    ModelManipulator model;
    NeedleManipluator needle;
	// Use this for initialization
	void Start () {
        model = GetComponent<ModelManipulator>();
        needle = GetComponent<NeedleManipluator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Started Animation");
            animationStart();
        }

    }

    public void animationStart()
    {
        alignNeedle();
    }

    private void alignNeedle()
    {
       // needle.rotate(45, 0, 0);
        needle.rotateSmooth(Vector3.forward, 110f, 3f);
    }
}
