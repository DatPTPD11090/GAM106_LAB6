using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Register;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json;

public class GameRegister : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;
    public GameObject notification;
    public static int selectedRegionId;

    IEnumerator Register()
    {
        selectedRegionId = GameRegion.selectedRegionId;
        RegisterRequestData requestData = new RegisterRequestData(
            emailInput.text,
            passwordInput.text,
            nameInput.text,
            "",
            selectedRegionId
        );

        string body = JsonUtility.ToJson(requestData);

        if (emailInput.text == "" || passwordInput.text == "" || nameInput.text == "")
        {
            notification.SetActive(true);
            notification.GetComponentsInChildren<TMP_Text>()[1].text = "Vui lòng nh?p ??y ?? thông tin!";
            yield break;
        }

        using (UnityWebRequest www = new UnityWebRequest("https://localhost:7152/api/APIGame/GetAllGameLevel", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                string responseJson = www.downloadHandler.text;
                Debug.Log(responseJson);

                ResponseUserError responseErr = JsonConvert.DeserializeObject<ResponseUserError>(responseJson);
                if (responseErr != null && responseErr.data != null && responseErr.data.Count() > 0)
                {
                    notification.SetActive(true);
                    var textComponents = notification.GetComponentsInChildren<TMP_Text>();
                    string error = string.Join(", ", responseErr.data.Select(e => e.description));
                    if (textComponents.Length > 1)
                    {
                        textComponents[1].text = error;
                    }
                }
            }
            else
            {
                string json = www.downloadHandler.text;
                ResponseUserSuccess response = JsonConvert.DeserializeObject<ResponseUserSuccess>(json);
                var data = response.data;
                if (response.isSuccess)
                {
                    notification.SetActive(true);
                    notification.GetComponentsInChildren<TMP_Text>()[1].text =
                        "??ng ký thành công, vui lòng quay l?i trang và ??ng nh?p! " + data.name;
                }
            }
        }
    }

    public void OnButtonClickRegister()
    {
        StartCoroutine(Register());
    }
}
public class RegisterRequestData
{
    public string email;
    public string password;
    public string name;
    public string otherField;
    public int regionId;

    public RegisterRequestData(string email, string password, string name, string otherField, int regionId)
    {
        this.email = email;
        this.password = password;
        this.name = name;
        this.otherField = otherField;
        this.regionId = regionId;
    }
}

public class ResponseUserError
{
    public bool isSuccess;
    public ErrorData[] data;
}

public class ErrorData
{
    public string description;
}

public class ResponseUserSuccess
{
    public bool isSuccess;
    public SuccessData data;
}

public class SuccessData
{
    public string name;
}