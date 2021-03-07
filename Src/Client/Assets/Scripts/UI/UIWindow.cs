using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;
    public virtual Type Type
    {
        get { return this.GetType(); }
    }
    public GameObject Root;
    public enum WindowResult
    {
        None = 0,
        Yes,
        No
    }
    public void Close(WindowResult result = WindowResult.None)
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
        UIManager.Instance.Close(this.Type);
        if(this.OnClose!=null)
        {
            this.OnClose(this, result);
        }
        this.OnClose = null;
    }

    public virtual void OnCloseClick()
    {
        this.Close();
    }
    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }

    private void OnMouseDown()
    {
        Debug.LogFormat(this.name + "Clicked");
    }
}
