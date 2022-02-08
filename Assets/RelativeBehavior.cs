using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; 
using System.Text; 

public class RelativeBehavior : MonoBehaviour
{
    private Dictionary<string,Vector3> _blockCoordinates = new Dictionary<string,Vector3>(); 
    private int _counter; 
    private bool _stateIsGood = false; 
    // Start is called before the first frame update
    void Start()
    {
        _counter  = 0; 
        _stateIsGood = false; 

        UpdateBlockCoordinateCache(); 
    }

    private void UpdateBlocks(bool stateIsGood)
    {
        string materialPrefix = stateIsGood ? "good" : "bad"; 
        for(int i=1;i<=9;i++)
        {
            string cubeName = $"Cube{i}";
            Material material = Resources.Load<Material>($"{materialPrefix}{i}");
            GameObject.Find($"Cube{i}").GetComponent<MeshRenderer>().material = material; 
        }
    }

    private void UpdateBlockCoordinateCache()
    {
        _blockCoordinates.Clear(); 
        for(int i=0;i<9;i++)
        {
            string name = $"Cube{i}"; 
            GameObject obj = GameObject.Find(name); 
            _blockCoordinates.Add(name, obj.transform.position); 
        }
    }

    private void SetToBlockCoordinateCache()
    {
        List<GameObject> objects = new List<GameObject>(); 

        // enumerate, and get objects out of the way
        for(int i=1;i<=9;i++)
        {
            string cubeName = $"Cube{i}";
            GameObject obj = GameObject.Find($"Cube{i}"); 
            obj.GetComponent<Rigidbody>().useGravity = false; 
            obj.transform.position = obj.transform.position * 1000; 
            objects.Add(obj); 
        }

        objects = objects.OrderBy(n=>n.transform.position.y).ToList(); 

        StringBuilder sb = new StringBuilder(); 
        foreach(GameObject obj in objects)
        {
            sb.AppendLine($"Sending {obj.name} to {_blockCoordinates[obj.name]}"); 
            obj.transform.position = _blockCoordinates[obj.name]; 
            obj.GetComponent<Rigidbody>().useGravity = true; 
        }

        Debug.Log(sb.ToString());
    }

    void UpdateGameCubeVisuals()
    {
        bool looksGood = true; 

        Vector3 centerBlockPosition = GameObject.Find("Cube5").transform.position; 
        const int CenterIndex = 4; 
        const int CenterX = 1, CenterY = 1; 

        for(int y=0;y<3 && looksGood;y++)
        {
            for(int x=0;x<3 && looksGood;x++)
            {
                int index = y* 3 + x; 
                if(index != CenterIndex)
                {
                    string cubeName = $"Cube{index+1}"; 
                    GameObject otherBlock = GameObject.Find(cubeName); 

                    Vector3 offsetVector = (otherBlock.transform.position - centerBlockPosition); 

                    // this was just decided experimentally
                    if(offsetVector.magnitude > .6)
                    {
                        looksGood = false; 
                    }
                    else
                    {
                        // get direction sign
                        int desiredXOffsetSign = x - CenterX; 
                        int desiredYOffsetSign = y - CenterY; 

                        // +z is up, +y is right: I'm thinking x/y for standard cartesian and it
                        // makes more sense in my head
                        int xActualOffsetSign = (int)Mathf.Sign(offsetVector.z);
                        int yActualOffsetSign = (int)Mathf.Sign(offsetVector.y); 

                        if((desiredXOffsetSign != 0 && xActualOffsetSign != desiredXOffsetSign))
                        {
                            looksGood = false; 
                        }
                        else if(desiredYOffsetSign != 0 && yActualOffsetSign != desiredYOffsetSign)
                        {
                            looksGood = false; 
                        }
                        else if(desiredYOffsetSign == 0 && Mathf.Abs(offsetVector.y) > .04)
                        {
                            looksGood = false; 
                        }
                        else if(desiredXOffsetSign == 0 && Mathf.Abs(offsetVector.z) > .04)
                        {
                            looksGood = false; 
                        }
                    }
                }
            }
        }


        // only update block textures once when a state change has been 
        // detected
        if(looksGood && !_stateIsGood)
        {
            Debug.Log("Looks good...setting to numbers.");
            _stateIsGood = true; 
            UpdateBlocks(true); 
        }
        else if(!looksGood && _stateIsGood)
        {
            Debug.Log("Looks bad...setting to faces.");
            _stateIsGood = false; 
            UpdateBlocks(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One))
        {
            SetToBlockCoordinateCache(); 
        }
        else
        {
            UpdateGameCubeVisuals();
        }
    }
}
