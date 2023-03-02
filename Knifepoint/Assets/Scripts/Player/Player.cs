using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Knife Throwing")]
    [SerializeField] public bool canThrowKnife;
    [SerializeField] private float throwForce;

    [Header("Objects")]
    [SerializeField] private FPSController FPSController;
    [SerializeField] private GameObject handKnife; //The knife attached to the hand model
    [SerializeField] private GameObject throwKnife; //Knife prefab for throwing
    [SerializeField] private GameObject knifeThrowPoint;

    

    // Start is called before the first frame update
    void Start()
    {
        canThrowKnife = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            ThrowKnife();
        }
    }

    private void ThrowKnife()
    {
        if (canThrowKnife)
        {
            GameObject newKnife = Instantiate(throwKnife, knifeThrowPoint.transform.position, knifeThrowPoint.transform.rotation);
            newKnife.GetComponent<Knife>().player = this;
            newKnife.GetComponent<Rigidbody>().AddForce(knifeThrowPoint.transform.forward * throwForce);
            handKnife.SetActive(false);
            canThrowKnife = false;
        }
        
    }

    /// <summary>
    /// Called from the knife script to allow the player to throw the knife again
    /// </summary>
    public void ThrowKnifeReset()
    {
        canThrowKnife = true;
        handKnife.SetActive(true);
    }
}
