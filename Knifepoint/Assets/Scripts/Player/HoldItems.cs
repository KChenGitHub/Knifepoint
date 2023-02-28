using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItems : MonoBehaviour
{

    float throwForce = 200;
    Vector3 objectPos;
    float distance;

    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;


    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
        if (distance >= 3f)
        {
            isHolding = false;
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (isHolding == true)
            {
                isHolding = false;
            }


            else
            if (distance <= 3f)
            {
                isHolding = true;
                item.GetComponent<Rigidbody>().useGravity = false;
            }

        }

        if (isHolding == true)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);


            if (Input.GetMouseButtonDown(1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }


        }
        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }



    }
    
}
