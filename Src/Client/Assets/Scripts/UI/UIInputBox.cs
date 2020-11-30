using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UIInputBox:MonoBehaviour
    {
        public Text title;
        public Text message;
        public Text tips;
        public Button buttonYes;
        public Button buttonNo;
        public InputField input;

        public Text buttonYesTitle;
        public Text buttonNoTitle;

        public delegate bool SubmitHandler(string inputText, out string tips);
        public event SubmitHandler OnSubmit;
        public UnityAction OnCancle;

        public string emptyTips;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Init(string title,string message,string btnOK="",string btnCancel="",string emptyTips = "")
        {
            this.message.text = message;
            this.tips.text = null;
            this.OnSubmit = null
                ;
            this.emptyTips = emptyTips;
            if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = title;
            if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = title;
            this.buttonYes.onClick.AddListener(OnClickYes);
            this.buttonNo.onClick.AddListener(OnClickNo);
        }

        void OnClickYes()
        {
            this.tips.text = "";
            if(string.IsNullOrEmpty(input.text))
            {
                this.tips.text = this.emptyTips;
                return;
            }
            if(OnSubmit!=null)
            {
                string tips;
                if(!OnSubmit(this.input.text,out tips))
                {
                    this.tips.text = tips;
                    return;
                }
            }
            Destroy(this.gameObject);
        }

        void OnClickNo()
        {
            Destroy(this.gameObject);
            if (this.OnCancle != null)
                this.OnCancle();
        }

    }
}
