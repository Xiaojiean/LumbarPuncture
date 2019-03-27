using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyManipulator : MonoBehaviour {

    private MeshRenderer rend;
    private bool fading = false;

    Material transparentMat;
    Material normalMat;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        normalMat = rend.material;
        string name = (normalMat.name.Split(' ')[0]);
        
        if (name == "spinemesh")
        {
            transparentMat = Resources.Load<Material>("spinemesh_transp");
        }
        else
        {
            transparentMat = Resources.Load<Material>("yellow_transp"); 

        }
    }

    public void fade(float alphaValue, float duration)
    {
        if (fading) { return; }
        fading = true;
        changeMat();
        StartCoroutine(smoothFade(alphaValue, duration));
    }


    private void changeMat()
    {
      
        rend.material = transparentMat;

    }

    IEnumerator smoothFade(float alphaDesired, float duration)
    {
        float counter = 0;
        
        if (!rend.enabled)
        {
            rend.enabled = true;
        }
        Color original = rend.material.color;
        
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(original.a, alphaDesired, counter / duration);
            rend.material.color = new Color(original.r, original.g, original.b, alpha);
            
            yield return new WaitForFixedUpdate();
        }
        rend.material.color = new Color(original.r, original.g, original.b, alphaDesired);
       

        fading = false;
    }
}
