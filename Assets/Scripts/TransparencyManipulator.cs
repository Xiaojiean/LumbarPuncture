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

    //https://stackoverflow.com/questions/39366888/unity-mesh-renderer-wont-be-completely-transparent?rq=1 from user "Programmer"
    private void changeMat()
    {
        /*rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.EnableKeyword("_ALPHABLEND_ON");
        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = 3000;*/
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
        Debug.Log(original.r);
        Debug.Log(original.g);
        Debug.Log(original.b);

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
