using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleManipluator : MonoBehaviour {

    private GameObject needle;

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

    public void rotateSmooth(Vector3 direction, float angle, float time)
    {
        StartCoroutine(smoothRotate(direction,angle,time));
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
        
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            needle.transform.localPosition = Vector3.Lerp(start, end, counter / duration);
            yield return new WaitForFixedUpdate();    
        }
        needle.transform.localPosition = end;
    }

    IEnumerator smoothRotate(Vector3 axis, float angle, float duration)
    {
        Quaternion target = needle.transform.localRotation * Quaternion.Euler(axis * angle);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            needle.transform.localRotation = Quaternion.Slerp(transform.localRotation, target, counter / duration);
            yield return new WaitForFixedUpdate();
        }

        needle.transform.localRotation = target;
    }

}
