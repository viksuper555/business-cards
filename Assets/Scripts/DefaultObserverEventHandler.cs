using UnityEngine;
using Vuforia;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

enum CardType
{
    Basic,
    Advanced
}

public class DefaultObserverEventHandler : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;

    private static CardType ParseType(string tag)
    {
        switch (tag)
        {
            case "Basic Card":
                return CardType.Basic;
            case "Advanced Card":
                return CardType.Advanced;
        }
        throw new NotImplementedException();
    }
    // URL to fetch the data
    private readonly Dictionary<CardType, string> dataUrls = new() { 
        { CardType.Basic, "https://raw.githubusercontent.com/viksuper555/business-cards-links/master/data/basic-cards.json"},
        { CardType.Advanced, "https://raw.githubusercontent.com/viksuper555/business-cards-links/master/data/advanced-cards.json"},

    };
    void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED)
        {
            Debug.Log("Target is tracked");
            // You can add more logic here when the target is tracked
            string key = mObserverBehaviour.TargetName;  // Use the ObserverBehaviour's TargetName as the key
            if (!string.IsNullOrEmpty(key))
            {
                StartCoroutine(FetchDataFromWeb(key));
            }
        }
        else
        {
            Debug.Log("Target is lost");
            // You can add more logic here when the target is lost
        }
    }

    private IEnumerator FetchDataFromWeb(string key)
    {
        var type = ParseType(gameObject.tag);
        var dataUrl = dataUrls[type];
        Debug.LogError($"Data url: {dataUrl}");
        UnityWebRequest request = UnityWebRequest.Get(dataUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string jsonData = request.downloadHandler.text;
            ParseAndSaveData(jsonData, key, type);
        }
    }

    private void ParseAndSaveData(string jsonData, string key, CardType type)
    {
        JObject json = JObject.Parse(jsonData);
        if (json[key] != null)
        {
            JArray data = (JArray)json[key];

            GlobalMetadata.Name = data[0].ToString();
            GlobalMetadata.Number = data[1].ToString();
            Debug.Log($"Name: {GlobalMetadata.Name}");
            Debug.Log($"Number: {GlobalMetadata.Number}");

            if (type == CardType.Basic)
            {
                GlobalMetadata.LinkedIn = data[2].ToString();

            }
            else if(type == CardType.Advanced)
            {
                GlobalMetadata.Email = data[2].ToString();
                GlobalMetadata.Website = data[3].ToString();
                GlobalMetadata.LinkedIn = data[4].ToString();

                Debug.Log($"Email: {GlobalMetadata.Email}");
                Debug.Log($"Website: {GlobalMetadata.Website}");
            }
            Debug.Log($"LinkedIn: {GlobalMetadata.LinkedIn}");
        }
        else
        {
            Debug.LogError($"Key '{key}' not found in JSON data.");
        }
    }
}
