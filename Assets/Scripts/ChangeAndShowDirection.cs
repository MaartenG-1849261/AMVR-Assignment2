using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAndShowDirection : MonoBehaviour
{

    private float boxWidth;
    private float boxHeight;

    private LineRenderer lineRenderer;

    void Start()
    {
        boxHeight = 23;
        boxWidth = 48;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {   
        //Vector3 end = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x + gameObject.transform.position.x ,gameObject.GetComponent<Rigidbody>().velocity.y + gameObject.transform.position.y,gameObject.transform.position.z);
        if (gameObject.transform.position.x > 24 && gameObject.GetComponent<Rigidbody>().velocity.x > 0 || gameObject.transform.position.x < -24 && gameObject.GetComponent<Rigidbody>().velocity.x < 0){
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else if(gameObject.transform.position.y > 11.5 && gameObject.GetComponent<Rigidbody>().velocity.y > 0 || gameObject.transform.position.y < -11.5 && gameObject.GetComponent<Rigidbody>().velocity.y < 0){
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, -gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }

    }

    public void showDirection()
    {
        drawLine(gameObject.transform.position, findIntersection(), Color.green);
    }

    public void hideDirection()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        UnityEngine.Debug.Log("Mouse is no longer on GameObject.");
        lineRenderer.enabled = false;
    }

    public void drawLine(Vector3 start, Vector3 end, Color color){

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.enabled = true;
    }

    Vector3 findIntersection(){

        Vector3 currentPosition = gameObject.transform.position;
        Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;

        while (true){
            Vector3 nextPosition = currentPosition + velocity * Time.deltaTime;
            if (nextPosition.x < -boxWidth / 2f || nextPosition.x > boxWidth / 2f ||
                nextPosition.y < -boxHeight / 2f || nextPosition.y > boxHeight / 2f)
            {
                return nextPosition;
            }
            else{
                currentPosition = nextPosition;
            }
        }
    }
}
