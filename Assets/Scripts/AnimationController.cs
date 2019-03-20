using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private bool animating = false;

    ModelManipulator modelManipulator;

    ObjectManipulator model;
    ObjectManipulator needle;
    ObjectManipulator container;


    List<Action> animations;

    float timeS1 = 2f;
    float timeS2 = 2f;
    float timeS3 = 2f;
	void Start () {
        container = GetComponent<ObjectManipulator>();
        model = transform.Find("ModelContainer").GetComponent<ObjectManipulator>();
        needle = transform.Find("Needle").GetComponent<ObjectManipulator>();

        modelManipulator = GetComponent<ModelManipulator>();

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
        model.scale(0.35f);
        needle.scale(1);
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
        model.rotateSmooth(Vector3.down, 135f, timeS1);
        model.moveSmooth(new Vector3(-0.6f, 0, 0), timeS1);
        model.zoomSmooth(2f, timeS1);
        //roate and move the needle
        needle.rotateSmooth(Vector3.forward, 240f, timeS1);
        needle.moveSmooth(new Vector3(0.5f, -0.5f, 0.18f), timeS1);
    }

    private void stageTwo()
    {
        //Rotate entire model and zoom out slightly
        container.rotateSmooth(Vector3.up, 95f, timeS2);
        container.zoomSmooth(0.8f, timeS2);

    }

    private void stageThree()
    {
        needle.moveSmooth(new Vector3(0, -0.3f, 0.18f), timeS3);
        container.rotateSmooth(Vector3.up, 90f, timeS3);
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
                if(!needle.animationRunning() && !model.animationRunning() && !container.animationRunning()) 
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
