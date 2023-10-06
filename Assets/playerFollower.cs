using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFollower : MonoBehaviour
{

    [SerializeField]
    private GameObject canvas;
    private bool active = false;

    void Start()
    {
    
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            Debug.Log("werkt");
            if (active == false)
            {
                active = true;
                canvas.SetActive(true);
            }
            else
            {
                active = false;
                canvas.SetActive(false);
            }
        }
    }
}