using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour {

    public float reducedScale = 0.005f;

    private GameObject model;

	// Use this for initialization
	void Start () {
        model = transform.Find("SpineMesh").gameObject;
        modifyModelScale(reducedScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void modifyModelScale(float newSize)
    {
        model.transform.localScale *= newSize;
    }

    public void zoomIn()
    {
        this.transform.localScale *= 1.20f;
    }

    public void zoomOut()
    {
        this.transform.localScale *= 0.80f;
    }

    public void rotateModel(float x,float y, float z)
    {
        model.transform.Rotate(new Vector3(x, y, z));
    }
    public void rotateModelContainer(float x, float y, float z)
    {
        this.transform.Rotate(new Vector3(x, y, z));
    }
}
