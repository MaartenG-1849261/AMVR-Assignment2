using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class secondCubeSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject leftController;

    [SerializeField]
    private GameObject rightController;

    private GameObject selectedCube = null;

    public Transform pointerPoseControllerR;
    public Transform pointerPoseControllerL;
    private string targetName;
    private int score;
    [SerializeField]
    private TMP_Text scoreText;

    public GameObject cubePrefab;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private bool rightHanded;
    private bool started = false;

    public int numberOfCubes;
    public float minSize;
    public float maxSize;
    public float minVeloc;
    public float maxVeloc;
    private Vector3 boundingBoxSize;
    private Vector3 boundingBoxPos;
    private GameObject targetCopy;
    private GameObject targetCube;


    private List<GameObject> cubes = new List<GameObject>();

    private string logFilePath = "Assets/Logs/log2.txt";
    private Stopwatch timer = new Stopwatch();
    private int aantalJuistePreselectie = 0;
    private int aantalFoutePreselectie = 0;
    private int aantalFouteSelecties = 0;
    private int aantalGemisteSelecties = 0;
    private int aantalJuisteSelecties = 0;

    private bool firstCubeSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        boundingBoxSize = new Vector3(48, 23, 2);
        boundingBoxPos = new Vector3(0, 0, -100);
        UnityEngine.Debug.Log(boundingBoxSize);
        UnityEngine.Debug.Log(boundingBoxPos);

        for (int i = 0; i < numberOfCubes; i++){

            (float xCoord, float yCoord, int zCoord, float xDirec, float yDirec, float size, float speed) randomValues = getRandomParameters();

            Vector3 spawnPosition = new Vector3(randomValues.xCoord, randomValues.yCoord, (float)(-99+(boundingBoxSize.z/numberOfCubes)*i));
            GameObject cube = Instantiate(cubePrefab, spawnPosition, transform.rotation);
            cube.transform.localScale = new Vector3(randomValues.size, randomValues.size, (float) 0.01);
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

    // Update is called once per frame
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
        //UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
        
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform != null)
            {
                if (selectedCube == null && firstCubeSelected == false)
                {
                    UnityEngine.Debug.Log("qqqqq");
                    selectedCube = raycastHit.transform.gameObject;
                    selectedCube.GetComponent<ChangeAndShowDirection>().showDirection();
                    firstCubeSelected = true;
                }
                else if (selectedCube != null && selectedCube != raycastHit.transform.gameObject)
                {
                    UnityEngine.Debug.Log(selectedCube);
                    selectedCube.GetComponent<ChangeAndShowDirection>().hideDirection();
                    selectedCube = raycastHit.transform.gameObject;
                    selectedCube.gameObject.GetComponent<ChangeAndShowDirection>().showDirection();
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("niks");
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
                    }
                    else
                    {
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
        float speed = Random.Range(1,10);
        float xDirec = Random.Range(minVeloc,maxVeloc);
        float yDirec = Random.Range(minVeloc,maxVeloc);

        return (xCoord, yCoord, zCoord, xDirec, yDirec, size, speed);
    }

    void initializeTarget()
    {

        if (targetCopy != null)
        {
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
}
