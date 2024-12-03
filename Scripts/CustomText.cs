using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomText : MonoBehaviour
{
    public TMP_InputField inputTitle;
    public TMP_InputField inputDescription;
    public Button buttonPost;

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator PostRequest(string uri)
    {
        var request = new UnityWebRequest(uri, "POST");
        var jsonData = "{\"title\":\"" + inputTitle.text + "\",\"description\":\"" + inputDescription.text + "\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Form upload complete! Response: " + request.downloadHandler.text);
        }
    }

    IEnumerator PutRequest(string uri, string jsonData)
    {
        var request = new UnityWebRequest(uri, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Update complete! Response: " + request.downloadHandler.text);
        }
    }

    IEnumerator DeleteRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Delete complete! Response: " + webRequest.downloadHandler.text);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(GetRequest("https://api.example.com/data"));
    }
}

