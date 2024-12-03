using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Login;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginGame : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public GameObject notification;
    private string baseUrl = "https://localhost:7152";

    IEnumerator Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        RequestLoginData requestData = new RequestLoginData(email, password);
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(baseUrl + "/api/auth/login", jsonData))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ResponseLogin response = JsonUtility.FromJson<ResponseLogin>(request.downloadHandler.text);
                if (response.isSuccess)
                {
                    PlayerPrefs.SetString("token", response.data.token);
                    SceneManager.LoadScene(1);
                }
                else
                {
                    notification.GetComponent<TextMeshProUGUI>().text = response.notification;
                }
            }
            else
            {
                notification.GetComponent<TextMeshProUGUI>().text = "Login failed: " + request.error;
            }
        }
    }

    public void OnLoginButtonClicked()
    {
        StartCoroutine(Login());
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("token"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
