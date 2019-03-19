using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleManipluator : MonoBehaviour {

    private GameObject needle;
    private int animationNum = 0;


    private void Start()
    {
        needle = transform.Find("Needle").gameObject;
    }

    public void modifyScale(float newSize)
    {
        needle.transform.localScale *= newSize;
    }

    public void rotate(float x, float y, float z)
    {
        needle.transform.Rotate(new Vector3(x, y, z));
    }


    public void move(Vector3 dest, float time)
    {
        StartCoroutine(moveSmooth(transform.localPosition, dest, time));
    }

    public void setPosition(Vector3 dest)
    {
        needle.transform.localPosition = dest;
    }

    IEnumerator moveSmooth(Vector3 start, Vector3 end, float duration)
    {
        animationNum++;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            needle.transform.localPosition = Vector3.Lerp(start, end, counter / duration);
            yield return new WaitForFixedUpdate();    
        }
        needle.transform.localPosition = end;
        animationNum--;
    }
    public void rotateSmooth(Vector3 direction, float angle, float time)
    {
        StartCoroutine(smoothRotate(direction, angle, time));
    }
    IEnumerator smoothRotate(Vector3 axis, float angle, float duration)
    {
        Quaternion target = needle.transform.localRotation * Quaternion.Euler(axis * angle);
        Quaternion start = needle.transform.localRotation;
        animationNum++;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            needle.transform.localRotation = Quaternion.Lerp(start, target, counter / duration);
            yield return new WaitForFixedUpdate();
        }

        needle.transform.localRotation = target;
        animationNum--;
    }

    public void zoomSmooth(float scale,float time)
    {
        StartCoroutine(smoothZoom(scale, time));
    }

    IEnumerator smoothZoom(float scale, float duration)
    {
        Vector3 target = needle.transform.localScale * scale;
        Vector3 start = needle.transform.localScale;
        float counter = 0;
        animationNum++;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            needle.transform.localScale = Vector3.Slerp(start, target, counter/duration);
            yield return new WaitForFixedUpdate();
        }

        needle.transform.localScale = target;
        animationNum--;
    }

    public bool animationsRunning()
    {
        return animationNum != 0 ? true : false;
    }

}
