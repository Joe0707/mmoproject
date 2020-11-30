using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI;
using UnityEngine;

public class InputBox
{
    static UnityEngine.Object cacheObject = null;
    public static UIInputBox Show(string message, string title = "", string btnOK = "", string btnCancel = "", string emptyTips = "")
    {
        if (cacheObject == null)
        {
            cacheObject = Resloader.Load<UnityEngine.Object>("UI/UIInputBox");
        }
        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox inputBox = go.GetComponent<UIInputBox>();
        inputBox.Init(title, message, btnOK, btnCancel, emptyTips);
        return inputBox;
    }
}
