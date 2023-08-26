using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class HandDataGathering : MonoBehaviour
{
    
    public LeapServiceProvider leapServiceProvider;
    private float time;

    public List<float[]>[][] handData = new List<float[]>[5][];

    private void OnEnable()
    {
        for (int i = 0; i < handData.Length; i++)
        {
            handData[i] = new List<float[]>[4];
            for (int j = 0; j < handData[i].Length; j++)
            {
                handData[i][j] = new List<float[]>();
            }
        }

        leapServiceProvider.OnUpdateFrame += OnUpdateFrame;
        time = 0.0f;
    }

    private void OnDisable()
    {
        leapServiceProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        //Get a list of all the hands in the frame and loop through
        //to find the first hand that matches the Chirality
        
        time += Time.deltaTime;
        //Use a helpful utility function to get the first hand that matches the Chirality
        Hand _rightHand = frame.GetHand(Chirality.Right);

        // Getting all the fingers
        for (int i=0; i<5; i++) // only 5 fingers
        {
            // Get the ith finger
            Finger _currentFinger = _rightHand.Fingers[i];

            // Get the bone data for this finger
            for (int j=0; j<4; j++) // only 4 bones per finger
            {
                float[] staticList = new float[4]; 
                staticList[0] = time;
                staticList[1] = _currentFinger.bones[j].PrevJoint.x; 
                staticList[2] = _currentFinger.bones[j].PrevJoint.y; 
                staticList[3] = _currentFinger.bones[j].PrevJoint.z;

                handData[i][j].Add(staticList);
                
                /*
                if (updateCount != 0) {
                    Debug.Log($"Finger {i} bone {j}: ");
                    string arrayAsString = "[" + string.Join(", ", handData[i][j][updateCount-1]) + "]"; // can try handData[i][j] after
                    Debug.Log(arrayAsString);
                    arrayAsString = "[" + string.Join(", ", handData[i][j][updateCount]) + "]"; // can try handData[i][j] after
                    Debug.Log(arrayAsString);
                }
                */
            }
            /* DEBUGGING
            Debug.Log($"Current finger number is {i}");
            PrintListToConsole(staticList);

            
            if (i == 0)
            {
                PrintListToConsole(staticList);
            }
            */
        }

        // Below is required if we want to print out the data every time its saved to handData
        // updateCount += 1;

        // Want to reconstruct the vector for this hand elswhere in view.
        // To do this, need to use Debug.DrawLine(origin, endPoint, Color.red);
    }

    

    private void LateUpdate()
    {
        /*
        for (int i=0; i<5; i++) // only 5 fingers
        {
            start = new Vector3(1,1,1);
            // Metacarpal bone
            start = new Vector3(handData[i][0][drawCount][1], handData[i][0][drawCount][2]*2.0f, handData[i][0][drawCount][3]);
            endvc = new Vector3(handData[i][1][drawCount][1], handData[i][1][drawCount][2], handData[i][1][drawCount][3]);
            Debug.DrawLine(start = start, endvc = endvc, Color.red);

            // Proximal bone
            start += new Vector3(handData[i][1][drawCount][1], handData[i][1][drawCount][2]*2.0f, handData[i][1][drawCount][3]);
            endvc = new Vector3(handData[i][2][drawCount][1], handData[i][2][drawCount][2], handData[i][2][drawCount][3]);
            Debug.DrawLine(start = start, endvc = endvc, Color.red);

            // Intr bone
            start += new Vector3(handData[i][2][drawCount][1], handData[i][2][drawCount][2]*2.0f, handData[i][2][drawCount][3]);
            endvc = new Vector3(handData[i][3][drawCount][1], handData[i][3][drawCount][2], handData[i][3][drawCount][3]);
            Debug.DrawLine(start = start, endvc = endvc, Color.red);
        }
        drawCount += 1;
        */
    }
    

    private void OnDestroy()
    {
        // Trying to just print out last few rows of one type to see if its the variable or if its the function
        /*
        PrintListToConsole(handData[0][0]);
        PrintListToConsole(handData[0][1]);
        PrintListToConsole(handData[0][2]);
        PrintListToConsole(handData[0][3]);
        PrintListToConsole(handData[0][4]);
        PrintListToConsole(handData[0][5]);
        

        SaveDataToCSV(handData[0],"thumb");
        SaveDataToCSV(handData[1],"index");
        SaveDataToCSV(handData[2],"middle");
        SaveDataToCSV(handData[3],"ring");
        SaveDataToCSV(handData[4],"pinky");
        

        PrintToConsole(handData, 0);
        PrintToConsole(handData, 1);
        PrintToConsole(handData, 2);
        PrintToConsole(handData, 3);
        PrintToConsole(handData, 4);
        */
        SaveData(handData);
    }

    /* 
    void PrintListToConsole(float[] list)
    {
        string result = "[";

        for (int i = 0; i < list.Count; i++)
        {
            result += list[i].ToString();
            
            if (i < list.Count - 1)
            {
                result += ", ";
            }
            else {
                result += "]";
            }
        }

        Debug.Log(result);
    }
    
    
    void SaveDataToCSV(List<List<float>> data, string fileName)
    {
        StringBuilder csv = new StringBuilder();
        
        csv.AppendLine(string.Join(",", new List<string> {"time", "x", "y", "z"}));
        
        foreach (List<float> row in data)
        {
            csv.AppendLine(string.Join(",", row));
        }

        string fullFileName = GenerateFileNameWithDate(fileName, "csv");

        string path = "C:/Users/xavie/OneDrive/Documents/University/Mechatronics Units/FYP/Unity Data/" + fullFileName;
        File.WriteAllText(path, csv.ToString());

        Debug.Log("Data saved to: " + path);
    }
    

    public static string GenerateFileNameWithDate(string baseFileName, string extension)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"{timestamp}_{baseFileName}.{extension}";
    }
    */
    
    void PrintToConsole(List<float[]>[][] handData, int fingerNumber)
    {
        for (int i=0; i<4; i++)
        {
            /* For printing every data point to console - not too useful.
            for (int j=0;j < handData[fingerNumber][i].Count; j++)
            {
                string arrayAsString = "[" + string.Join(", ", handData[fingerNumber][i][j]) + "]"; // can try handData[i][j] after
                Debug.Log(arrayAsString);
            }
            string arrayAsString = "[" + string.Join(", ", handData[fingerNumber][i][updateCount]) + "]"; // can try handData[i][j] after
            Debug.Log(arrayAsString);
            */
            Debug.Log($"Finger {fingerNumber} bone {i}: ");
            string arrayAsString = "[" + string.Join(", ", handData[fingerNumber][i][10]) + "]"; // can try handData[i][j] after
            Debug.Log(arrayAsString);
        }
    }

    void SaveData(List<float[]>[][] data)
    {
        string currentDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "handDataRaw_" + currentDateTime + ".json";

        string relativePath = Path.Combine("..", "Data", "Raw");
        string fullPath = Path.Combine(Application.dataPath, relativePath);
        
        // Ensure the "data" directory exists
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        string serializedData = JsonConvert.SerializeObject(data); 
        string path = Path.Combine(fullPath, fileName);
        File.WriteAllText(path, serializedData);
    }
}