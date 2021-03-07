using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class ArenaService:Singleton<ArenaService>,IDisposable
    {
        public void Init()
        {

        }
        public ArenaService()
        {
            MessageDistributer.Instance.Subscribe<ArenaChallengeRequest>(this.OnArenaChallengeRequest);
            MessageDistributer.Instance.Subscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
            MessageDistributer.Instance.Subscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Subscribe<ArenaEndResponse>(this.OnArenaEnd);
            MessageDistributer.Instance.Subscribe<ArenaReadyResponse>(this.OnArenaReady);
            MessageDistributer.Instance.Subscribe<ArenaRoundStartResponse>(this.OnArenaRoundStart);
            MessageDistributer.Instance.Subscribe<ArenaRoundEndResponse>(this.OnArenaRoundEnd);


        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ArenaChallengeRequest>(this.OnArenaChallengeRequest);
            MessageDistributer.Instance.Unsubscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
            MessageDistributer.Instance.Unsubscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Unsubscribe<ArenaEndResponse>(this.OnArenaEnd);
            MessageDistributer.Instance.Unsubscribe<ArenaReadyResponse>(this.OnArenaReady);
            MessageDistributer.Instance.Unsubscribe<ArenaRoundStartResponse>(this.OnArenaRoundStart);
            MessageDistributer.Instance.Unsubscribe<ArenaRoundEndResponse>(this.OnArenaRoundEnd);
        }


        public void SendArenaChallengeRequest(int targetId,string targetName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.arenaChallengeReq = new ArenaChallengeRequest();
            message.Request.arenaChallengeReq.ArenaInfo = new ArenaInfo();
            message.Request.arenaChallengeReq.ArenaInfo.Red = new ArenaPlayer()
            {
                EntityId = User.Instance.CurrentCharacterInfo.Id,
                Name = User.Instance.CurrentCharacterInfo.Name
            };
            message.Request.arenaChallengeReq.ArenaInfo.Blue = new ArenaPlayer()
            {
                EntityId = targetId,
                Name = targetName
            };
            NetClient.Instance.SendMessage(message);
        }

        public void SendArenaChallengeResponse(bool accept,ArenaChallengeRequest request)
        {
            Debug.Log("SendArenaChallengeResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.arenaChallengeRes = new ArenaChallengeResponse();
            message.Request.arenaChallengeRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.arenaChallengeRes.Errormsg = accept ? "" : "对方拒绝了挑战请求";
            message.Request.arenaChallengeRes.ArenaInfo = request.ArenaInfo;
            NetClient.Instance.SendMessage(message);
        }

        private void OnArenaChallengeRequest(object sender,ArenaChallengeRequest request)
        {
            Debug.Log("OnArenaChallengeRequest");
            var confirm = MessageBox.Show(string.Format("邀请你竞技场对战",request.ArenaInfo.Red.Name),"竞技场对战",MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                this.SendArenaChallengeResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendArenaChallengeResponse(false, request);
            };
        }


        private void OnArenaChallengeResponse(object sender,ArenaChallengeResponse message)
        {
            Debug.Log("OnArenaChallengeResponse");
            if (message.Result != Result.Success)
                MessageBox.Show(message.Errormsg, "对方拒绝挑战");
        }

        private void OnArenaEnd(object sender, ArenaEndResponse message)
        {
            Debug.Log("OnArenaEnd");
            ArenaManager.Instance.ExitArena(message.ArenaInfo);
        }

        private void OnArenaBegin(object sender, ArenaBeginResponse message)
        {
            Debug.Log("OnArenaBegin");
            ArenaManager.Instance.EnterArena(message.ArenaInfo);
        }

        internal void SendArenaReadyRequest(int arenaId)
        {
            Debug.Log("SendArenaChallengeResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.arenaReady = new ArenaReadyRequest();
            message.Request.arenaReady.entityId = User.Instance.CurrentCharacter.entityId;
            message.Request.arenaReady.arenaId = arenaId;
            NetClient.Instance.SendMessage(message);
        }


        private void OnArenaReady(object sender, ArenaReadyResponse message)
        {
            ArenaManager.Instance.OnReady(message.Round, message.ArenaInfo);
        }

        private void OnArenaRoundEnd(object sender, ArenaRoundEndResponse message)
        {
            ArenaManager.Instance.OnRoundStart(message.Round, message.ArenaInfo);

        }

        private void OnArenaRoundStart(object sender, ArenaRoundStartResponse message)
        {
            ArenaManager.Instance.OnRoundEnd(message.Round, message.ArenaInfo);

        }

    }
}
