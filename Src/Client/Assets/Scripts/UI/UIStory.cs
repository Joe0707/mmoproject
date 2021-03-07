using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class UIStory:UIWindow
{
    public Text title;
    public Text descript;

    StoryDefine story;

    public void SetStory(StoryDefine story)
    {
        this.story = story;
        this.title.text = story.Name;
        this.descript.text = story.Description;
    }

    public void OnClickStart()
    {
        if (!StoryManager.Instance.StartStory(this.story.ID))
        {

        }
    }
}
