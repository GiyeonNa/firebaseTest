using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using Unity.VisualScripting;
using System;

public class AuthManager 
{
    private static AuthManager instance = null;

    public static AuthManager Instance
    {
        get
        {
            if (instance == null) instance = new AuthManager();
            return instance;
        }
    }

    //���� ����
    private FirebaseAuth auth;
    //����
    private FirebaseUser user;

    public Action<bool> LoginState;

    public string UserId => user.UserId;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        //�ӽ�
        if(auth.CurrentUser != null)
        {
            Logout();
        }
        //���� ���� ��ȭ �� 
        auth.StateChanged += OnChanged;
    }

    public void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;
            if(signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log(email + "�� �α��� �Ǿ����ϴ�");
            }
            else
            {
                Debug.Log("�α��� ����");
            }
        });
    }

    public void Register(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            //if (!task.IsCanceled && !task.IsFaulted)
            //{
            //    Debug.Log(email + "�� ȸ������");
            //}
            ////���� ���
            ////else if (task.IsCanceled) { }
            ////���� -> ���� �������� -> �̸��� ������ / ��й�ȣ ���� / �ߺ� �̸��� ...
            ////else if (task.IsFaulted) { }
            //else
            //{
            //    Debug.Log("ȸ������ ����");
            //}
            if (task.IsCanceled)
            {
                Debug.Log("ȸ������ ���");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.Log("ȸ������ ����");
                Debug.Log(task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result;
            Debug.Log(email + "�� ȸ������");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ���");
    }
}
