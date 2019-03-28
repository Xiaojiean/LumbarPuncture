using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeController : MonoBehaviour {

    public float scale;

    ObjectManipulator container;
    ObjectManipulator model;
    public GameObject syringeObj;
    private SyringeAction syringeAction;

    void Start () {
        container = GetComponent<ObjectManipulator>();
        model = transform.Find("ModelContainer").GetComponent<ObjectManipulator>();
        syringeObj = GameObject.Instantiate(syringeObj);
        syringeAction = syringeObj.GetComponent<SyringeAction>();
        initialisePositions();
    }

    void Update()
    {
         
        
        if(syringeObj.transform.localRotation.eulerAngles.x >= 260f && syringeObj.transform.localRotation.eulerAngles.x <= 280f)
        {
            syringeAction.changeMode();
        }
       
    }


    void initialisePositions()
    {
        container.rotate(new Vector3(0,-100,35));
        model.rotate(new Vector3(90,0,0));
        
        
        container.scale(scale);
    }

    public void OnDestroy()
    {
        Destroy(syringeObj);
    }


}
