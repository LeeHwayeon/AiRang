using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SearchPw : MonoBehaviour
{
    public TMP_InputField emailInput;

    public GameObject alertPanel;
    public TextMeshProUGUI alertMsg;
    public GameObject loginCanvas;
    private const string URL = "http://localhost:8081/api/";

    public void SendPwBtn()
    {
        alertMsg.text = "메일을 확인하는 중 입니다.";
        alertPanel.gameObject.SetActive(true);
        StartCoroutine(SendPwBtnCo());
    }

    private void ChangeLoginCanvas()
	{
        gameObject.SetActive(false);
        loginCanvas.SetActive(true);
    }

    private void ChangeText()
	{
        alertMsg.text = "잠시 뒤에 로그인페이지로 이동합니다.";
    }

    IEnumerator SendPwBtnCo()
    {
        User.UserController user = new User.UserController { email = emailInput.text };
        string jsonData = JsonUtility.ToJson(user);
        using (UnityWebRequest request = UnityWebRequest.Post(URL + "user/mail", jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);
            if(!request.isHttpError)
            {
                alertMsg.text = "임시 비밀번호가 \n 발급되었습니다! \n 메일을 확인해주세요.";
                Invoke("ChangeText", 2f);
                Invoke("ChangeLoginCanvas", 4f);

            }
            else
            {
                alertMsg.text = "가입되지 않은 이메일입니다 \n 회원가입을 해주세요.";
            }
        }
    }

}