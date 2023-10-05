using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HitScript : MonoBehaviour
{
    private int hitCounter = 0;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private Transform targetCenter;

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
            Vector3 colissionPoint = collision.contacts[0].point;
            float xDist = (colissionPoint.x - targetCenter.position.x);
            float yDist = (colissionPoint.y - targetCenter.position.y);
            float dist = Mathf.Sqrt(xDist * xDist + yDist * yDist);
            Debug.Log("dist is " + dist  );
            double score = Math.Ceiling(10 - ((1 + ((10 - 1) / (0.3 - 0)) * (dist - 0))));

            Debug.Log("score is " + score);
            //Debug.Log(targetCenter);
            //float distanceToCollision = Vector3.Distance(targetCenter.position, colissionPoint);
            //Debug.Log(distanceToCollision);
            //int score = CalculateScore(distanceToCollision);

            Debug.Log("Entered collision nfuewhfewuhfuiewhfuiew");


            scoreText.text = score.ToString();
        }
        
    }

    int CalculateScore(float distance)
    {
        float distancePerCircle = distance / 10;
        int score = Mathf.FloorToInt(distance / distancePerCircle);
        return score;
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
