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

    //인증 관리
    private FirebaseAuth auth;
    //유저
    private FirebaseUser user;

    public Action<bool> LoginState;

    public string UserId => user.UserId;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        //임시
        if(auth.CurrentUser != null)
        {
            Logout();
        }
        //계정 상태 변화 시 
        auth.StateChanged += OnChanged;
    }

    public void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;
            if(signed)
            {
                Debug.Log("로그인");
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
                Debug.Log(email + "로 로그인 되었습니다");
            }
            else
            {
                Debug.Log("로그인 실패");
            }
        });
    }

    public void Register(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            //if (!task.IsCanceled && !task.IsFaulted)
            //{
            //    Debug.Log(email + "로 회원가입");
            //}
            ////도중 취소
            ////else if (task.IsCanceled) { }
            ////실패 -> 이유 여러가지 -> 이메일 비정상 / 비밀번호 간단 / 중복 이메일 ...
            ////else if (task.IsFaulted) { }
            //else
            //{
            //    Debug.Log("회원가입 실패");
            //}
            if (task.IsCanceled)
            {
                Debug.Log("회원가입 취소");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.Log("회원가입 실패");
                Debug.Log(task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result;
            Debug.Log(email + "로 회원가입");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃됨");
    }
}
