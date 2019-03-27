using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private bool animating = false;

    ModelManipulator modelManipulator;

    ObjectManipulator model;
    ObjectManipulator syringe;
    ObjectManipulator needle;
    ObjectManipulator container;


    List<Action> animations;

    float timeS1 = 0.3f; //should be 5
    float timeS2 = 0.2f;
    float timeS3 = 2f;
    float timeS4 = 1f;
    float timeS5 = 2f;
    float timeS6 = 1.7f; 
    float timeS7 = 2f;
    float timeS8 = 2f;
    float timeS9 = 3f;
    float timeS10 = 5f;

    void Start () {
        container = GetComponent<ObjectManipulator>();
        model = transform.Find("ModelContainer").GetComponent<ObjectManipulator>();
        syringe = transform.Find("Syringe").GetComponent<ObjectManipulator>();
        needle = transform.Find("Syringe").Find("Needle").GetComponent<ObjectManipulator>();
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
        animations.Add(() => stageFour());
        animations.Add(() => stageFive());
        animations.Add(() => stageSix());
        animations.Add(() => stageSeven());
        animations.Add(() => stageEight());
        animations.Add(() => stageNine());
        animations.Add(() => stageTen());
    }

    private void resizeObjects()
    {
        model.scale(0.35f);
        syringe.scale(1);
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
        
        //rotate and move the spine to a suitable location
        model.rotateSmooth(Vector3.down, 135f, timeS1);
        model.moveSmooth(new Vector3(-0.6f, 0, 0), timeS1);
        model.zoomSmooth(2f, timeS1);
        //roate and move the needle
        syringe.rotateSmooth(Vector3.forward, 240f, timeS1);
        syringe.moveSmooth(new Vector3(0.5f, -0.5f, 0.15f), timeS1);
    }

    private void stageTwo()
    {
        //Rotate entire model and zoom out slightly
        container.rotateSmooth(Vector3.up, 95f, timeS2);
        container.zoomSmooth(0.8f, timeS2);
    }

    private void stageThree()
    {
        //Pan around and zoom in to show where needle enters
        syringe.moveSmooth(new Vector3(0, -0.3f, 0.15f), timeS3);
        container.rotateSmooth(Vector3.up, 90f, timeS3);
        container.zoomSmooth(2f, timeS3);
    }

    private void stageFour()
    {
        //start moving needle towards spine
        syringe.moveSmooth(new Vector3(-0.4f,-0.1f,0.15f), timeS4);
    }

    private void stageFive()
    {
        //start to remove spine layer and center model
        
        container.moveSmooth(new Vector3(container.getXPos() -1.2f, container.getYPos(), container.getZPos()), timeS5);
        container.zoomSmooth(1.5f, timeS5);
    }

    private void stageSix()
    {
        //show needle move towards spine
        modelManipulator.fadeIndividualLayer("Spine", 0f, timeS6 * 0.8f);
        needle.moveSmooth(new Vector3(needle.getXPos(), 2.2f, needle.getZPos()), timeS6);
    }

    private void stageSeven() //move needle in more
    {
        modelManipulator.toggleIndividualLayer("Spine");
        needle.moveSmooth(new Vector3(needle.getXPos(), needle.getYPos() - 0.25f, needle.getZPos()), timeS7);
        modelManipulator.fadeIndividualLayer("Duramater", 0f, timeS7 * 0.8f);
    }

    private void stageEight() //moves needle slightly more in
    {
        modelManipulator.toggleIndividualLayer("Duramater");
        modelManipulator.fadeIndividualLayer("Arachnoid", 0f, timeS8 * 0.8f);
        needle.moveSmooth(new Vector3(needle.getXPos(), needle.getYPos() - 0.2f, needle.getZPos()), timeS8);
        container.moveSmooth(new Vector3(container.getXPos() - 0.7f, container.getYPos(), container.getZPos()), timeS8);
        container.zoomSmooth(1.5f, timeS8);
    }

    private void stageNine() //moves needle slightly more in and shows how far it can go in the layer
    {
        modelManipulator.toggleIndividualLayer("Arachnoid");
        modelManipulator.fadeIndividualLayer("Piamater", 0.3f, timeS9 / 2);
        needle.moveSmooth(new Vector3(needle.getXPos(), needle.getYPos() - 0.8f, needle.getZPos()), timeS9);
        container.moveSmooth(new Vector3(container.getXPos() - 0.7f, container.getYPos(), container.getZPos()), timeS9);
        container.zoomSmooth(1.5f, timeS9);
    }

    private void stageTen()
    {
        //modelManipulator.toggleIndividualLayer("Piamater");
        container.rotateSmooth(Vector3.up, -90f, timeS10);
        container.moveSmooth(new Vector3(container.getXPos() + 1.2f, container.getYPos() - 0.3f, container.getZPos()), timeS10);
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
                if(!syringe.animationRunning() && !needle.animationRunning() && !model.animationRunning() && !container.animationRunning()) 
                {
                    stage(); //Run relevant stage
                    wait = false;
                }
                yield return new WaitForSeconds(.05f);
            }    
        }
        animating = false;
    }
}
