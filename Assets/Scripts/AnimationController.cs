using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    ModelManipulator model;
    NeedleManipluator needle;

    List<Action> animations;

    float timeS1 = 2f;
    float timeS2 = 2f;
	// Use this for initialization
	void Start () {
        model = GetComponent<ModelManipulator>();
        needle = GetComponent<NeedleManipluator>();
        animations = new List<Action>();
        populateQueue();
	}

    private void populateQueue()
    {
        animations.Add(() => stageOne());
        animations.Add(() => stageTwo());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animationStart();
        }

    }

    public void animationStart()
    {
        StartCoroutine(animationLoop());
    }

    private void stageOne() //Here we rotate and move the spine to a suitable location
    {
        model.rotateSmooth(Vector3.down, 135f, timeS1);
        model.move(new Vector3(-0.6f, 0, 0), timeS1);
        model.zoomSmooth(2f, timeS1);
    }

    private void stageTwo()
    {
        needle.rotateSmooth(Vector3.forward, 60f, timeS2);
        needle.move(new Vector3(0.5f, -0.5f, 0.2f), timeS2);
    }

    IEnumerator animationLoop()
    {
        bool wait;
        foreach(Action stage in animations)
        {
            //Wait until previous animation has finished before moving onto the next one
            wait = true;
            while (wait)
            {
                if(!model.animationsRunning() && !needle.animationsRunning())
                {
                    stage();
                    wait = false;
                }
                yield return new WaitForSeconds(.1f);
            }
                
        }
        
    }
}
