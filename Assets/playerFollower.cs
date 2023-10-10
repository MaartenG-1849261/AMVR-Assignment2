using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFollower : MonoBehaviour
{

    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private Transform player;
    private bool active = false;
    [SerializeField]
    private GameObject shootingRange;

    void Start()
    {
    
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        { 
            if (active == false)
            {
                active = true;
                canvas.SetActive(true);

                Vector3 playerPosition = player.position;
                Quaternion playerRotation = player.rotation;
                Vector3 playerForward = player.forward;

                Vector3 newPosition = playerPosition + playerForward * 0.5f;
                canvas.GetComponent<Transform>().position = newPosition;
                canvas.GetComponent<Transform>().rotation = playerRotation;
            }
            else
            {
                active = false;
                canvas.SetActive(false);
            }
        }
    }

    public void OnResetSelect()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnStartShootingRangeSelect()
    {
        shootingRange.SetActive(true);
    }
}