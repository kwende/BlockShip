﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; 
using System.Text; 

public class RelativeBehavior : MonoBehaviour
{
    private Dictionary<string,Vector3> _blockCoordinates = new Dictionary<string,Vector3>(); 
    private bool _stateIsGood = false; 

    // Start is called before the first frame update
    void Start()
    {
        _stateIsGood = false; 
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

    void UpdateGameCubeVisuals()
    {
        bool looksGood = true; 

        Vector3 centerBlockPosition = GameObject.Find("Cube5").GetComponent<Renderer>().bounds.center; 
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
                    Vector3 otherBlockPosition = GameObject.Find(cubeName).GetComponent<Renderer>().bounds.center; 

                    Vector3 offsetVector = (otherBlockPosition - centerBlockPosition); 

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
                        int xActualOffsetSign = Mathf.Abs(offsetVector.z) > .12 ? (int)Mathf.Sign(offsetVector.z) : 0;
                        int yActualOffsetSign = Mathf.Abs(offsetVector.y) > .12 ? (int)Mathf.Sign(offsetVector.y) : 0;

                        looksGood = (xActualOffsetSign == desiredXOffsetSign) && (yActualOffsetSign == desiredYOffsetSign); 
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
            SceneManager.LoadScene("StylShip_Scene"); //Load scene called Game 
        }
        else
        {
            UpdateGameCubeVisuals();
        }
    }
}
