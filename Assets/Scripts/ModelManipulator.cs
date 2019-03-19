using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour {

    
    public float reducedScale;
    public float explodeTime;
    public float zoomPercent;

    private bool exploded = false;
    private bool exploding = false;
    private GameObject model;
    private List<Layer> layers;
    private int layerIndex;
    private enum LayerID {Spine, Nerves, Duramater, Arachnoid, Piamater};
    private class Layer{
        public GameObject layer;
        public bool visible;
        public Layer(GameObject go, bool vis)
        {
            layer = go;
            visible = vis;
        }
    }

    // Use this for initialization
    void Start () {
        model = transform.Find("ModelContainer").gameObject;
        modifyModelScale(reducedScale);
        layers = new List<Layer>();
        getLayers();
        layerIndex = 0;
	}
	
	private void getLayers()
    {
        foreach (Transform child in model.transform)
        {
            layers.Add(new Layer(child.gameObject, true));
        }
    }

    private void modifyModelScale(float newSize)
    {
        model.transform.localScale *= newSize;
    }

    public void zoomIn()
    {
        modifyModelScale(1 + zoomPercent);
    }

    public void zoomOut()
    {
        modifyModelScale(1 - zoomPercent);
    }

    public void rotateModel(float x,float y, float z)
    {
        model.transform.Rotate(new Vector3(x, y, z));
    }
    public void rotateModelContainer(float x, float y, float z)
    {
        this.transform.Rotate(new Vector3(x, y, z));
    }

    private void toggleLayer(int index)
    {
        layers[index].layer.SetActive(!layers[index].visible);
        layers[index].visible = !layers[index].visible;
    }

    public void addLayer()
    {
        if (layerIndex == 0) { return;}
        layerIndex--;
        toggleLayer(layerIndex);

    }

    public void removeLayer()
    {
        if (layerIndex == 4) { return;}
        toggleLayer(layerIndex);
        layerIndex++;
    }

    public void toggleIndividualLayer(string layerName)
    {
        try
        {
            LayerID id = (LayerID)System.Enum.Parse(typeof(LayerID), layerName);
            toggleLayer((int)id);
        }
        catch (ArgumentException)
        {
            Debug.Log("Invalid layer name!");
        }
    }

    public void explode()
    {
        if (exploding) { return; }
        for(int i = -2; i < 3; i++)
        {
            GameObject a = layers[i + 2].layer;
            if (exploded)
            {
                StartCoroutine(moveSmooth(a.transform, a.transform.localPosition, new Vector3(0, 0, 0), explodeTime));  //Contract
            }
            else
            {
                //a.transform.Translate(new Vector3((i * 0.8f), 0, 0));
                StartCoroutine(moveSmooth(a.transform, a.transform.localPosition, new Vector3(i * 200f, 0, 0), explodeTime)); //Explode
            }
        }
        exploded = !exploded;
    }


    IEnumerator moveSmooth(Transform objectMove, Vector3 start, Vector3 end, float duration)
    {
        exploding = true;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectMove.localPosition = Vector3.Lerp(start, end, counter/duration);
            yield return new WaitForFixedUpdate();
        }
        objectMove.localPosition = end;
        exploding = false;
    }

}
