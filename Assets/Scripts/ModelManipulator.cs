using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour {

    
    public float reducedScale;

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
        catch (ArgumentException e)
        {
            Debug.Log("Invalid layer name!");
        }
    }

}
