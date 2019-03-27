using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour {

    public float explodeTime;

    private bool exploded = false;
    private bool exploding = false;

    private GameObject model;

    private TextToSpeechController ttsc;

    private List<Layer> layers;
    private int layerIndex;

    private enum LayerID {Spine, Nerves, Duramater, Arachnoid, Piamater};
    private class Layer{
        public GameObject layer;
        public List<TransparencyManipulator> transparencyManipulators;
        public bool visible;
        public Layer(GameObject go,List<TransparencyManipulator> a, bool vis)
        {
            layer = go;
            visible = vis;
            transparencyManipulators = a;
        }
    }

    // Use this for initialization
    void Start () {
        model = transform.Find("ModelContainer").gameObject;
        ttsc = GetComponent<TextToSpeechController>();
        layers = new List<Layer>();
        getLayers();
        layerIndex = 0;
	}
	
	private void getLayers()
    {
        foreach (Transform child in model.transform)
        {
            List<TransparencyManipulator> tmp = new List<TransparencyManipulator>();
            foreach(Transform a in child.transform)
            {
                TransparencyManipulator tm = a.gameObject.AddComponent<TransparencyManipulator>();
                tmp.Add(tm);
            }
            layers.Add(new Layer(child.gameObject, tmp ,true));
        }
    }


    private void toggleLayer(int index)
    {
        if(exploded || exploding) { return; }
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
        ttsc.startSpeech(layerIndex);
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
            ObjectManipulator om = a.GetComponent<ObjectManipulator>();
            if (exploded)
            {
               om.moveSmooth(new Vector3(0, 0, 0), explodeTime);
            }
            else
            {
               om.moveSmooth(new Vector3(i * 200f, 0, 0), explodeTime);
            }
        }
        exploded = !exploded;
    }

    public void fadeIndividualLayer(string layerName, float fadeAmount, float time)
    {
        try
        {
            LayerID id = (LayerID)System.Enum.Parse(typeof(LayerID), layerName);
            fade((int)id, fadeAmount, time);
        }
        catch (ArgumentException)
        {
            Debug.Log("Invalid layer name!");
        }
    }


    private void fade(int index, float alpha, float time)
    {
        List<TransparencyManipulator> transpManList = layers[index].transparencyManipulators;

        foreach(TransparencyManipulator t in transpManList)
        {
            t.fade(alpha, time);
        }
    }
}
