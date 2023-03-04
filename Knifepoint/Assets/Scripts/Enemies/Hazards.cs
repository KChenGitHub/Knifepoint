using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{

    public int hazardDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Player took Damage.");
            FPSController player = collision.gameObject.GetComponent<FPSController>();

            player.TakeDamage(hazardDamage);
        }
    }
}
