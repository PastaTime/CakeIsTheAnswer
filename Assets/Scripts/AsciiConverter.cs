using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsciiConverter : MonoBehaviour
{
    public Light light;
    public int width = 64;
    public int height = 64;
    public float pixelScale = 0.1f;
    public float fovScale = 0.02f;
    public float refreshRate = 1.0f/12.0f;

    string[] charArray = {"",",",":","!","H","$","%","@"};
    private List<List<string>> asciiLines = new List<List<string>>();

    private float refresh;
    
    // Update is called once per frame
    void Update()
    {
        if (refresh <= 0.0f)
        {
            asciiLines.Clear();
            for (int y = 0; y < height; y++)
            {
                List<string> line = new List<string>();

                for (int x = 0; x < width; x++)
                {
                    int index = 0;
                    Vector3 offset = new Vector3((x*pixelScale) - (width*0.5f*pixelScale), (y*-pixelScale) + (height*0.5f*pixelScale), 0.0f);
                    Vector3 halfSize = new Vector3(width * 0.5f, height * -0.5f, 0.0f);
                    Vector3 fovOffset = (halfSize - new Vector3(x, -y, 0.0f)) * -fovScale;
                    RaycastHit hit;
                    Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward + fovOffset) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position + offset, transform.TransformDirection(Vector3.forward + fovOffset), out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward + fovOffset) * hit.distance, Color.blue);

                        // Calculate brightness of pixel base on distance/intensity of light
                        float dist = Vector3.Distance(hit.point, light.transform.position);
                        float normDist = dist / light.range;
                        float atten = Mathf.Clamp01(1.0f / (1.0f + 25.0f * normDist*normDist) * ((1.0f - normDist) * 5.0f));
                        
                        // Shadows
                        bool isBlocked = false;
                        RaycastHit lightHit;
                        if (Physics.Linecast(hit.point, light.transform.position, out lightHit))
                        {
                            float blockDist = Vector3.Distance(hit.point, lightHit.point);
                            if (blockDist < dist * 0.9f)
                            {
                                isBlocked = true;
                            }
                        }

                        if (!isBlocked)
                        {
                            index = Math.Clamp(Mathf.RoundToInt(((float)charArray.Length) * atten), 0, charArray.Length-1);
                        }
                    }
                    line.Add(charArray[index]);
                }
                asciiLines.Add(line);
            }

            refresh = refreshRate;
        }
        else
        {
            refresh -= Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        float trueScale =  Screen.height / (float)height;
        float xo = (Screen.width * 0.5f) - (width * trueScale * 0.5f);
        float yo = (Screen.height * 0.5f) - (height * trueScale * 0.5f);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GUI.Label(new Rect((x * trueScale) + xo, (y * trueScale) + yo, trueScale*2, trueScale*2), asciiLines[y][x]);
            }
        }
    }
}
