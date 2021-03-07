using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class ArenaManager:Singleton<ArenaManager>
    {
        public int Round;
        private ArenaInfo ArenaInfo;
        internal void SendReady()
        {
            Debug.LogFormat("ArenaManager SendReady");
            ArenaService.Instance.SendArenaReadyRequest(this.ArenaInfo.ArenaId);
        }

        internal void OnReady(int round,ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager OnReady Round", arenaInfo.ArenaId, round);
            this.Round = round;
            if (UIArena.Instance != null)
                UIArena.Instance.ShowCountDown();
        }

        internal void ExitArena(ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.ExitArena");
            this.ArenaInfo = null;
        }

        internal void EnterArena(ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.EnterArena");
            this.ArenaInfo = arenaInfo;
        }

        internal void OnRoundStart(int round, ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.OnRoundStart");
            if (UIArena.Instance != null)
                UIArena.Instance.ShowRoundStart(round, arenaInfo);
        }

        internal void OnRoundEnd(int round, ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.OnRoundEnd");
            if (UIArena.Instance != null)
                UIArena.Instance.ShowRoundResult(round, arenaInfo);
        }
    }
}
