using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    public class TabView:MonoBehaviour
    {
        public TabButton[] tabButtons;
        public GameObject[] tabPages;
        public int index = 1;
        IEnumerator Start()
        {
            for (var i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].tabView = this;
                tabButtons[i].tabIndex = i;
            }
            yield return new WaitForEndOfFrame();
            SelectTab(0);
        }

        public void SelectTab(int index)
        {
            if(this.index!=index)
            {
                for(var i =0;i<tabButtons.Length;i++)
                {
                    tabButtons[i].Select(i == index);
                    tabPages[i].SetActive(i == index);
                }
            }
        }
    }
}
