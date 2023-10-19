using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    public Text[] text;
    public Vector2 vec2DAxis_L = Vector2.zero;
    public Vector2 vec2DAxis_R = Vector2.zero;
    public bool isGrip_L = false;
    private bool DownTrigger_L;
    private bool DownTrigger_R;
    public bool isTrigger_L = false;
    public bool isGrip_R = false;
    public bool isTrigger_R = false;
private int out_trash=2;
    public GameObject Hand_L;
    public GameObject Hand_R;
    public GameObject drop_areas;
    public GameObject trash;
    public int I;
    public float time = 10;
    public float time2 = 300;
    public CharacterController character;
    public GameObject Light;
    public GameObject Light2;
    private bool door = true;
    public bool dooring;
    public GameObject Door;
    public GameObject Door2;
    public GameObject Door3;
    public GameObject Book, Book1;
    public GameObject mm;
    public GameObject canvas;
    public int[] ints = new int[4];
    public int[] ints2 = new int[4];
    public int i;
    public int t = 10;
    public AudioSource source;
    public AudioSource source2;
    public AudioSource BGM;
    private float time3;
    public Light L;
    OffsetGrab offsetGrabScript;

    // Start is called before the first frame update
    void Start()
    {
        Light.SetActive(false);
        trash.GetComponent<Trashcan>().Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the left XR controller
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out vec2DAxis_L);
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out isGrip_L);
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out isTrigger_L);

        // Perform a raycast from the left hand
        Ray(Hand_L);

        // Handle game logic
        if (time == 0)
        {
            BGM.enabled = true;
            time2 -= Time.deltaTime;
            text[1].text = (((int)time2) / 60).ToString() + ":" + (((int)time2) % 60).ToString();

            // Change light color based on time
            if (time2 > 180)
            {
                L.color = Color.white;
            }
            else if (time2 <= 180 && time2 > 60)
            {
                L.color = Color.yellow;
            }
            else
            {
                L.color = Color.red;
            }

            if (time2 < 0)
            {
                I = 99;
                transform.Find("Camera").localEulerAngles = new Vector3(0, transform.Find("Main Camera").localEulerAngles.y * 1.5f, 0);
                transform.Find("Camera").position = new Vector3(transform.Find("Main Camera").position.x, 0, transform.Find("Main Camera").position.z);

                time2 = 0;
                BGM.enabled = false;
            }
        }

        if (dooring)
        {
            if (door)
            {
                if (Door.transform.eulerAngles.y > 90)
                {
                    Door.transform.eulerAngles = new Vector3(0, Door.transform.eulerAngles.y - Time.deltaTime * 60, 0);
                }
                else
                {
                    Door.transform.eulerAngles = new Vector3(0, 90, 0);
                    door = false;
                    dooring = false;
                }
            }
            else
            {
                if (Door.transform.eulerAngles.y < 180)
                {
                    Door.transform.eulerAngles = new Vector3(0, Door.transform.eulerAngles.y + Time.deltaTime * 30, 0);
                }
                else
                {
                    Door.transform.eulerAngles = new Vector3(0, 180, 0);
                    door = true;
                    dooring = false;
                }
            }
        }

        if (I != 0 && i != 99)
        {
            Move();
        }

        // Handle different game states
        switch (I)
        {
            case 0:
                if (time > 0)
                {
                    text[0].gameObject.SetActive(true);
                    trash.GetComponent<Trashcan>().Disable();
                    time -= Time.deltaTime;
                    text[0].text = ((int)time + 1).ToString();
                }
                else
                {
                    text[0].gameObject.SetActive(false);
                    I = 1;
                    time = 0;
                    text[2].gameObject.SetActive(true);
                }
                break;
            case 1:
                if (OnTrigger("Light"))
                {
                  //  trash.GetComponent<Trashcan>().Disable();
                    Light.SetActive(true);
                    text[3].gameObject.SetActive(true);
                    I = 2;

                    /*
                    offsetGrabScript = Book.GetComponent<OffsetGrab>();
                    // Check if the trigger button was released over the "Shell" object
                    if (!isTrigger_L && DownTrigger_L)
                    {
                    offsetGrabScript = Book.GetComponent<OffsetGrab>();
                        if (Ray(Hand_L) != null && Ray(Hand_L).name == "Shell")
                        {
                            text[3].gameObject.SetActive(true);
                            
                        }
                    }

                    //Cleaning task basically means to only enable the trash
                    //trash.GetComponent<Trashcan>().Enable();
                    
                    //ThrowToTrash();

                    //disabling possibility to throw stuff in the trash
                    break;*/


                }
                break;
            case 2:
                if (OnTrigger("FlatTV"))
                {
            trash.GetComponent<Trashcan>().Disable(); //disabling possibility to throw stuff in the trash
                    text[4].gameObject.SetActive(true);
                    I = 3;
                }
                break;
            case 3:
                if (OnTrigger("Microwave"))
                {
                    text[5].gameObject.SetActive(true);
                    dooring = true;
                    source.Play();
                    I = 4;
                }
                break;
            case 4:
                if (OnTrigger("Light2"))
                {
                    text[6].gameObject.SetActive(true);
                    Light2.gameObject.SetActive(true);
                    I = 5;
                }
                break;
            case 5:
                if (OnTrigger("Door1"))
                {
                    dooring = true;

			
                    source.Play();
                }

                if (!dooring && door)
                {
                    I = 6;
                    source.Play();
                }
                break;
            case 6:
                if (Door2.transform.eulerAngles.y > 1)
                {
                    Door2.transform.eulerAngles = new Vector3(0, Door2.transform.eulerAngles.y - Time.deltaTime * 30, 0);
                }
                else
                {
                    Door2.transform.eulerAngles = new Vector3(0, 0, 0);
                    I = 7;
                    mm.SetActive(true);
                }
                break;
            case 7:
                if (ints[0] == ints2[0] && ints[1] == ints2[1] && ints[2] == ints2[2] && ints[3] == ints2[3])
                {
                    I = 8;
                    transform.Find("Camera").localEulerAngles = new Vector3(0, transform.Find("Main Camera").localEulerAngles.y * 1.5f, 0);
                    transform.Find("Camera").position = new Vector3(transform.Find("Main Camera").position.x, 0, transform.Find("Main Camera").position.z);
                    source.Play();
                }
                break;
            case 8:
                if (Door3.transform.eulerAngles.y < 180)
                {
                    Door3.transform.eulerAngles = new Vector3(0, Door3.transform.eulerAngles.y + Time.deltaTime * 30, 0);
                }
                canvas.SetActive(true);
                text[8].text = "You won!";

                // Handle quitting the application
                if (OnTrigger("Button"))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                break;
            case 99:
                canvas.SetActive(true);
                text[8].text = "You lost...";

                // Handle quitting the application
                if (OnTrigger("Button"))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                break;
            default:
                break;
        }
        MM();
    }

    // Perform a raycast from the specified hand
    public GameObject Ray(GameObject hand)
    {
        Ray ray = new Ray(hand.transform.position, hand.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            return hitInfo.collider.gameObject;
        }
        return null;
    }

    // Check if the trigger button was pressed over an object with the specified name
    public bool OnTrigger(string name)
    {
        if (Ray(Hand_L) != null)
        {
            if (isTrigger_L && Ray(Hand_L).name == name)
            {
                return true;
            }
        }

        DownTrigger_L = isTrigger_L;
        return false;
    }

    // Handle selecting and entering digits for a code
    public void MM()
    {
        if (i > 3)
        {
            i = 3;
        }
        else if (i < -1)
        {
            i = -1;
        }

        if (time3 >= 0)
        {
            time3 -= Time.deltaTime;
        }
        else
        {
            for (int x = 0; x < 10; x++)
            {
                if (OnTrigger(x.ToString()))
                {
                    i++;
                    ints[i] = x;
                    time3 = 0.3f;
                    break;
                }
            }
        }

        if (OnTrigger("ɾ") || Input.GetKeyDown("a"))
        {
            ints[i] = 0;
            i--;
        }

        switch (i)
        {
            case -1:
                text[7].text = null;
                break;
            case 0:
                text[7].text = ints[0].ToString();
                break;
            case 1:
                text[7].text = ints[0].ToString() + ints[1].ToString();
                break;
            case 2:
                text[7].text = ints[0].ToString() + ints[1].ToString() + ints[2].ToString();
                break;
            case 3:
                text[7].text = ints[0].ToString() + ints[1].ToString() + ints[2].ToString() + ints[3].ToString();
                break;
            default:
                break;
        }
    }

    public void ThrowToTrash(){
        //if user has thrown an object to the trash, then decrement the amount of objects to throw
        out_trash--;

        if(out_trash==0){ //if all objects thrown then end this task and check for others
            gameObject.GetComponent<AudioSource>().Play();
	I=2;
        }
    }

    // Move the character based on input
    public void Move()
    {
        character.Move(transform.forward * vec2DAxis_L.y * Time.deltaTime + transform.right * vec2DAxis_L.x * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + vec2DAxis_R.x, 0);
    }
}
