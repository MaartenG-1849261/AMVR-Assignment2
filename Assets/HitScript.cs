using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    private int hitCounter = 0;
    [SerializeField]
    private GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Entered collision nfuewhfewuhfuiewhfuiew");
        }
        
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Increase the hit counter
            hitCounter++;

            // Print the updated hit counter
            Debug.Log("Target hit! Hits: " + hitCounter);

            // You can also add any other logic you want here, like disabling the target or playing a hit animation.
        }
    }
    
    private void OnGUI()
    {
        // Create a GUI label at the top left corner of the screen
        GUI.Label(new Rect(10, 10, 200, 30), "Hits: " + hitCounter);
    }*/
}
