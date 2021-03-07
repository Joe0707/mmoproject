using Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class StoryService : Singleton<StoryService>, IDisposable
    {
        public void Init()
        {
            StoryManager.Instance.Init();
        }

        public StoryService()
        {
            MessageDistributer.Instance.Subscribe<StoryStartResponse>(this.OnStartStory);
            MessageDistributer.Instance.Subscribe<StoryEndResponse>(this.OnStoryEnd);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StoryStartResponse>(this.OnStartStory);
            MessageDistributer.Instance.Unsubscribe<StoryEndResponse>(this.OnStoryEnd);
        }

        public void SendStartStory(int storyId)
        {
            Debug.Log("SendStartStory" + storyId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.storyStart = new StoryStartRequest();
            message.Request.storyStart.storyId = storyId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnStartStory(object sender, StoryStartResponse message)
        {
            Debug.Log("OnStartStory" + message.storyId);
            StoryManager.Instance.OnStoryStart(message.storyId);
        }

        private void SendEndStory(int storyId)
        {
            Debug.Log("SendEndStory" + storyId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.storyEnd = new StoryEndRequest();
            message.Request.storyEnd.storyId = storyId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnStoryEnd(object sender, StoryEndResponse message)
        {
            Debug.Log("StoryEndResponse" + message.storyId);
            if(message.Result == Result.Success)
            {

            }
        }



        private void OnStoryEnd(object sender, StoryEndResponse message)
        {

        }

    }
}
