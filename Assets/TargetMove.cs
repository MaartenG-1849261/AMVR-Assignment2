using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed at which the target moves
    private int moveDirection; // 1 for right, -1 for left
    private float lifeTimer = 100;
    private bool isOriginalTarget = true;

    private void Start()
    {
        // Randomly choose the initial movement direction
        moveDirection = (Random.Range(0, 2) == 0) ? 1 : -1;
    }

    private void Update()
    {

        // Move the target horizontally
        transform.Translate(Vector3.right * moveSpeed * moveDirection * Time.deltaTime);
        if (transform.position.x >= 4.8 || transform.position.x <= -4.8)
        {
            moveDirection *= -1;
        }
        // Update the life timer
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f && !isOriginalTarget)
        {
            // Disable the GameObject instead of destroying it
            gameObject.SetActive(false);
        }

    }

    void OnCollisionEnter(Collision collision) 
    {
        Debug.Log("qqqqqqqqq");
        gameObject.SetActive(false);
    }

    public void SetAsOriginalTarget()
    {
        isOriginalTarget = true;
    }

    public void SetAsNonOriginalTarget()
    {
        isOriginalTarget = false;
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        // Change direction when hitting the wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection *= -1;
        }
    }*/
}