using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private bool animating = false;

    ModelManipulator modelManipulator;
    ObjectManipulator manipulator;

    Transform model;
    Transform needle;
    Transform container;


    List<Action> animations;

    float timeS1 = 2f;
    float timeS2 = 2f;
    float timeS3 = 2f;
	void Start () {
        manipulator = GetComponent<ObjectManipulator>();
        modelManipulator = GetComponent<ModelManipulator>();

        model = transform.Find("ModelContainer");
        needle = transform.Find("Needle");
        container = transform;

        resizeObjects();

        animations = new List<Action>();
        populateQueue();
	}

    private void populateQueue()
    {
        animations.Add(() => stageOne());
        animations.Add(() => stageTwo());
        animations.Add(() => stageThree());
       // animations.Add(() => stage());
    }

    private void resizeObjects()
    {
        manipulator.scale(model, 0.35f);
        manipulator.scale(needle, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && !animating)
        {
           
            animationStart();
        }

    }

    public void animationStart()
    {
        animating = true;
        StartCoroutine(animationLoop());
    }

    private void stageOne() 
    {
        Debug.Log("Start Animation");
        //rotate and move the spine to a suitable location
        manipulator.rotateSmooth(model,Vector3.down, 135f, timeS1);
        manipulator.moveSmooth(model,new Vector3(-0.6f, 0, 0), timeS1);
        manipulator.zoomSmooth(model,2f, timeS1);
        //roate and move the needle
        manipulator.rotateSmooth(needle,Vector3.forward, 60f, timeS1);
        manipulator.moveSmooth(needle,new Vector3(0.5f, -0.5f, 0.18f), timeS1);
    }

    private void stageTwo()
    {
        //Rotate entire model and zoom out slightly
        manipulator.rotateSmooth(container, Vector3.up, 95f, timeS2);
        manipulator.zoomSmooth(container, 0.8f, timeS2);

    }

    private void stageThree()
    {
        manipulator.moveSmooth(needle, new Vector3(0, -0.3f, 0.18f), timeS3);
        manipulator.rotateSmooth(container, Vector3.up, 90f, timeS3);
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
                if(!manipulator.animationRunning()) //CHANGE
                {
                    stage();
                    wait = false;
                }
                yield return new WaitForSeconds(.1f);
            }
                
        }
        animating = false;
        
    }
}
