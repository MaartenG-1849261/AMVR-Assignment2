using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeDirection : MonoBehaviour
{

    private Vector3 boundingBoxSize;
    // Start is called before the first frame update
    void Start()
    {
        boundingBoxSize = gameObject.GetComponent<Renderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > 24 && gameObject.GetComponent<Rigidbody>().velocity.x > 0 || gameObject.transform.position.x < -24 && gameObject.GetComponent<Rigidbody>().velocity.x < 0){
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else if(gameObject.transform.position.y > 11.5 && gameObject.GetComponent<Rigidbody>().velocity.y > 0 || gameObject.transform.position.y < -11.5 && gameObject.GetComponent<Rigidbody>().velocity.y < 0){
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, -gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
    }
}
