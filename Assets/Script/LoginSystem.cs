using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        AuthManager.Instance.LoginState += OnChangedState;
        AuthManager.Instance.Init();
    }

    public void OnChangedState(bool sign)
    {
        outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        outputText.text += AuthManager.Instance.UserId;
    }

    public void Creat()
    {
        string e = emailInputField.text;
        string p = passwordInputField.text;
        AuthManager.Instance.Register(e, p);
    }

    public void Login()
    {
        AuthManager.Instance.Login(emailInputField.text, passwordInputField.text);
    }

    public void Logout()
    {
        AuthManager.Instance.Logout();
    }
}
