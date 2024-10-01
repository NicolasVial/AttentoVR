using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParametersReader : MonoBehaviour
{

    [SerializeField] private TextAsset file;

    private List<List<string>> parameters = new List<List<string>>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Parameters:
     * 0: subj NOT USED
     * 1: mod NOT USED
     * 2: co USED
     * 3: standard NOT USED
     * 4: stim NOT USED
     * 5: block NOT USED
     * 6: mod2 NOT USED
     * 7: stim_letter NOT USED
     * 8: standard_letter NOT USED
     * 9: ord.st USED
     * 10: trial USED
     * 11: stim1 USED  
     * 12: stim2 USED
     * 13: stim1_let NOT USED
     * 14: stim2_let NOT USED
     * 15: incongruency USED
     * 16: visual_real_standard NOT USED
     * 17: blur USED
     * 18: resp USED
     * */

    public List<List<string>> ReadParameters()
    {
        parameters.Clear();
        StreamReader reader = new StreamReader(new MemoryStream(file.bytes));
        string line;
        int counter = 0;
        while ((line = reader.ReadLine()) != null)
        {
            if(counter > 0)
            {
                List<string> lineParameters = new List<string>();
                string[] values = line.Replace(" ", "").Split(',');
                foreach (string value in values)
                {
                    lineParameters.Add(value);
                }
                parameters.Add(lineParameters);
            }
            counter++;
        }
        reader.Close();
        return parameters;
    }

    public string GetFirstLine()
    {
        StreamReader reader = new StreamReader(new MemoryStream(file.bytes));
        string line = reader.ReadLine();
        reader.Close();
        return line;
    }




}
