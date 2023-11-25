using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;

public class HuggingFaceTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Query();
    }

    // Make a call to the API
    void Query()
    {
        string inputText = "I'm on my way to the forest.";
        string[] candidates = {
        "The player is going to the city",
        "The player is going to the wilderness",
        "The player is wandering aimlessly"
    };
        HuggingFaceAPI.SentenceSimilarity(inputText, OnSuccess, OnError, candidates);
    }

    // If successful, handle the result
    void OnSuccess(float[] result)
    {
        foreach (float value in result)
        {
            Debug.Log(value);
        }
    }

    // Otherwise, handle the error
    void OnError(string error)
    {
        Debug.LogError(error);
    }

}
