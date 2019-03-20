using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {

    private int animationsRunning = 0;

    public ObjectManipulator() { }

    public void scale(Transform obj,float newSize)
    {

        obj.localScale *= newSize;
    }

    public void rotate(Transform obj, Vector3 rota)
    {
        obj.transform.Rotate(rota);
    }

    public void move(Transform obj, Vector3 dest)
    {
        obj.localPosition = dest;
    }


    public void moveSmooth(Transform obj, Vector3 dest, float time)
    {
        StartCoroutine(moveSmooth(obj,obj.transform.localPosition, dest, time));
    }

    public void setPosition(Transform obj, Vector3 dest)
    {
        obj.transform.localPosition = dest;
    }

    IEnumerator moveSmooth(Transform obj,Vector3 start, Vector3 end, float duration)
    {
        animationsRunning++;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            obj.transform.localPosition = Vector3.Lerp(start, end, counter / duration);
            yield return new WaitForFixedUpdate();
        }
        obj.transform.localPosition = end;
        animationsRunning--;
    }
    public void rotateSmooth(Transform obj, Vector3 direction, float angle, float time)
    {
        StartCoroutine(smoothRotate(obj, direction, angle, time));
    }
    IEnumerator smoothRotate(Transform obj,Vector3 axis, float angle, float duration)
    {
        Quaternion target = obj.transform.localRotation * Quaternion.Euler(axis * angle);
        Quaternion start = obj.transform.localRotation;
        animationsRunning++;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            obj.transform.localRotation = Quaternion.Lerp(start, target, counter / duration);
            yield return new WaitForFixedUpdate();
        }

        obj.transform.localRotation = target;
        animationsRunning--;
    }

    public void zoomSmooth(Transform obj,float scale, float time)
    {
        StartCoroutine(smoothZoom(obj,scale, time));
    }

    IEnumerator smoothZoom(Transform obj, float scale, float duration)
    {
        Vector3 target = obj.transform.localScale * scale;
        Vector3 start = obj.transform.localScale;
        float counter = 0;
        animationsRunning++;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            obj.transform.localScale = Vector3.Slerp(start, target, counter / duration);
            yield return new WaitForFixedUpdate();
        }

        obj.transform.localScale = target;
        animationsRunning--;
    }

    public bool animationRunning()
    {
        return animationsRunning != 0 ? true : false;
    }

}
