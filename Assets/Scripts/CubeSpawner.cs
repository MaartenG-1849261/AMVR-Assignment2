using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CubeSpawner : MonoBehaviour
{
    public Transform pointerPoseControllerR;
    public Transform pointerPoseControllerL;

    public GameObject cubePrefab;
    public int numberOfCubes;
    public float minSize;
    public float maxSize;
    public float minVeloc;
    public float maxVeloc;
    private Vector3 boundingBoxSize;
    private Vector3 boundingBoxPos;
    private List<GameObject> cubesNearClick = new List<GameObject>();
    private string targetName;
    private GameObject targetCopy;
    private GameObject targetCube;
    private int score;
    private bool started = false;

    [SerializeField]
    private bool rightHanded = true;

    [SerializeField]
    private GameObject leftController;

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private AudioSource audioSource;

    private List<GameObject> cubes = new List<GameObject>();

    private string logFilePath = "Assets/Logs/log.txt";
    private Stopwatch timer = new Stopwatch();

    private int aantalJuistePreselectie = 0;
    private int aantalFoutePreselectie = 0;
    private int aantalFouteSelecties = 0;
    private int aantalGemisteSelecties = 0;
    private int aantalJuisteSelecties = 0;

    void Start()
    {
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }

        boundingBoxSize = gameObject.GetComponent<Collider>().bounds.size;
        boundingBoxPos = gameObject.GetComponent<Collider>().bounds.center;
        UnityEngine.Debug.Log(boundingBoxSize);
        UnityEngine.Debug.Log(boundingBoxPos);
        score = 0;

        for (int i = 0; i < numberOfCubes; i++){

            (float xCoord, float yCoord, int zCoord, float xDirec, float yDirec, float size, float speed) randomValues = getRandomParameters();

            Vector3 spawnPosition = new Vector3(randomValues.xCoord, randomValues.yCoord, randomValues.zCoord);
            GameObject cube = Instantiate(cubePrefab, spawnPosition, transform.rotation);
            cube.transform.localScale = new Vector3(randomValues.size, randomValues.size, (float) 0.1);
            cube.GetComponent<Rigidbody>().velocity = new Vector3(randomValues.xDirec, randomValues.yDirec,0);
            cube.name = i.ToString();
            cubes.Add(cube);
        }

        initializeTarget();

        if (rightHanded)
        {
            leftController.SetActive(false);
        }
        else
        {
            rightController.SetActive(false);
        }
    }

    void Update()
    {
        if (((OVRInput.GetDown(OVRInput.Button.One) && rightHanded) || (OVRInput.GetDown(OVRInput.Button.Three) && !rightHanded)) && !started)
        {
            timer.Start();
        }

        Ray ray;
        if (rightHanded)
        {
            ray = new Ray(pointerPoseControllerR.position, pointerPoseControllerR.forward);
        }
        else
        {
            ray = new Ray(pointerPoseControllerL.position, pointerPoseControllerL.forward);
        }
        
        RaycastHit raycastHit;
        LineRenderer LineDrawer = GetComponent<LineRenderer>();
        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform != null)
            {
                if (raycastHit.transform.name == "playingField")
                {
                    Vector3 center = raycastHit.point;
                    LineDrawer.startWidth = 0.2f;
                    LineDrawer.endWidth = 0.2f;
                    LineDrawer.startColor = Color.black;
                    LineDrawer.endColor = Color.black;
                    LineDrawer.enabled = true;

                    float Theta = 0f;
                    float radius = 5f;

                    int Size = (int)((1f / 0.01f) + 1f);
                    LineDrawer.SetVertexCount(Size);
                    for (int i = 0; i < Size; i++)
                    {
                        Theta += (2.0f * Mathf.PI * 0.01f);
                        float x = radius * Mathf.Cos(Theta);
                        float y = radius * Mathf.Sin(Theta);
                        LineDrawer.SetPosition(i, new Vector3(center.x + x, center.y + y, 19));
                    }
                }
            }
        }
        else
        {
            LineDrawer.enabled = false;
        }


        if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && rightHanded) || (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && !rightHanded))
        {
            //Debug.Log ("mouseDown = " + Input.mousePosition.x + " " + Input.mousePosition.y);
            //Debug.Log ("position in plane = " + gameObject.transform.position);


            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    Vib();

                    UnityEngine.Debug.Log(targetName);
                    if (raycastHit.transform.name == targetName)
                    {
                        UnityEngine.Debug.Log("succes");
                        score++;
                        aantalJuisteSelecties++;
                        audioSource.Play();
                        if (score < 20)
                        {
                            initializeTarget();
                            scoreText.text = score.ToString() + "/20";

                        }
                        else
                        {
                            timer.Stop();
                            LogToFile("Time: " + timer.ElapsedMilliseconds);
                            LogToFile("Aantal juiste preselecties: " + aantalJuistePreselectie);
                            LogToFile("Aantal foute preselecties: " + aantalFoutePreselectie);
                            LogToFile("Aantal juiste selecties: " + aantalJuisteSelecties);
                            LogToFile("Aantal foute selecties: " + aantalFouteSelecties);
                            LogToFile("Aantal gemiste selecties: " + aantalGemisteSelecties);

                            scoreText.text = "Done!";
                            Scene scene = SceneManager.GetActiveScene();
                            SceneManager.LoadScene(scene.name);
                        }
                        clearRaster();
                    }
                    else if (raycastHit.transform.name == "playingField")
                    {
                        clearRaster();
                        cubesNearClick = calculateCubesNearClick(raycastHit.point);
                        showInRaster(cubesNearClick);
                    }
                    else
                    {
                        clearRaster();
                        aantalFouteSelecties++;
                    }
                }
            }
            else
            {
                aantalGemisteSelecties++;
            }
        }
    }

    (float xCoord, float yCoord, int zCoord, float xDirec, float yDirec, float size, float speed) getRandomParameters(){
        float size = Random.Range(minSize, maxSize);
        float xCoord = Random.Range(((float) -(boundingBoxSize.x/2) + size), ((float) (boundingBoxSize.x/2) - size));
        float yCoord = Random.Range(((float) -(boundingBoxSize.y/2) + size), ((float) (boundingBoxSize.y/2) - size));
        int zCoord = (int) Random.Range((boundingBoxPos.z - boundingBoxSize.z/2), (boundingBoxPos.z + boundingBoxSize.z/2));
        float speed = Random.Range(minVeloc, maxVeloc);
        float xDirec = Random.Range(minVeloc, maxVeloc);
        float yDirec = Random.Range(minVeloc ,maxVeloc);

        return (xCoord, yCoord, zCoord, xDirec, yDirec, size, speed);
    }

    List<GameObject> calculateCubesNearClick(Vector3 clickCoordinate){

        List<GameObject> cubesList = new List<GameObject>();

        bool targetFound = false;

        foreach (var cube in cubes){
            Vector3 positionOnPlane = new Vector3(cube.transform.position.x,cube.transform.position.y,(boundingBoxPos.z - boundingBoxSize.z/2));
            float distance = Vector3.Distance(positionOnPlane, clickCoordinate);
            if (distance <= 5.0f){
                // Cube is within the radius, do something with it
                //Debug.Log("Cube is near the click!");

                if (cube.name == targetCube.name)
                {
                    aantalJuistePreselectie++;
                    targetFound = true;
                }

                GameObject newCube = Instantiate(cubePrefab, new Vector3(25f, cube.transform.position.y, 0f), Quaternion.identity);

                newCube.transform.localScale = cube.transform.localScale;
                newCube.GetComponent<MeshRenderer>().material.color = cube.GetComponent<MeshRenderer>().material.color;
                newCube.name = cube.name;
                cubesList.Add(newCube);
            }
        }

        if (targetFound == false)
        {
            aantalFoutePreselectie++;
        }

        return cubesList;
    }

    void showInRaster(List<GameObject> cubes){
        float rasterSpacingX = 2.5f;  
        float rasterSpacingY = 2.5f;  
        int cubesPerRow = 5;          


        for (int i = 0; i < cubes.Count; i++)
        {
            var cube = cubes[i];

            // Calculate the position in the raster
            float xPos = -6.25f + (i % cubesPerRow) * rasterSpacingX;
            float yPos = -10f - (i / cubesPerRow) * rasterSpacingY;

            // Set the position of the cube in the raster
            cube.transform.position = new Vector3(xPos, yPos, -10);
            BoxCollider col = cube.AddComponent<BoxCollider> ();
        }
    }

    void initializeTarget(){

        if (targetCopy != null){
            Destroy(targetCopy);
        }

        if (targetCube != null)
        {
            targetCube.GetComponent<MeshRenderer>().material.color = Color.white;
        }

        targetCube = cubes[Random.Range(0, numberOfCubes)];
        targetCube.GetComponent<MeshRenderer>().material.color = Color.green;
        targetCopy = Instantiate(cubePrefab, new Vector3(25f, targetCube.transform.position.y, 0f), Quaternion.identity);
        targetCopy.transform.localScale = targetCube.transform.localScale;
        targetCopy.GetComponent<MeshRenderer>().material.color = targetCube.GetComponent<MeshRenderer>().material.color;
        targetCopy.transform.position = new Vector3(-40, 0, targetCube.transform.position.z);
        targetCopy.GetComponent<MeshRenderer>().material.color = Color.green;
        targetName = targetCube.name;
    }

    public void Vib()
    {
        Invoke("startVib", .1f);
        Invoke("stopVib", .4f);
    }

    public void startVib()
    {
        if (rightHanded)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }
        else
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        }
    }

    public void stopVib()
    {
        if (rightHanded)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
    }

    void LogToFile(string logMessage)
    {
        // Append the log message to the file
        using (StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine($"{System.DateTime.Now} - {logMessage}");
        }
    }

    void clearRaster()
    {
        if (cubesNearClick.Count != 0)
        {
            foreach (var cube in cubesNearClick)
            {
                Destroy(cube);
            }
            cubesNearClick.Clear();
        }
    }
}
