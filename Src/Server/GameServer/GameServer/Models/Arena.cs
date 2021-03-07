﻿using Common;
using Common.Data;
using GameServer.Managers;
using GameServer.Services;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Arena
    {
        const float READY_TIME = 11f;
        const float ROUND_TIME = 60f;
        const float RESULT_TIME = 5f;
        public Map Map;
        public ArenaInfo ArenaInfo;
        public NetConnection<NetSession> Red;
        public NetConnection<NetSession> Blue;

        Map SourceMapRed;
        Map SourceMapBlue;

        int RedPoint = 9;
        int BluePoint = 10;
        private bool redReady;
        private bool blueReady;
        private ArenaStatus ArenaStatus;
        private float timer;
        private ArenaRoundStatus RoundStatus;
        private int Round;

        public bool Ready { get { return this.redReady && this.blueReady; } }

        public Arena(Map map, ArenaInfo arena, NetConnection<NetSession> red, NetConnection<NetSession> blue)
        {
            this.Map = map;
            this.ArenaInfo = arena;
            this.Red = red;
            this.Blue = blue;
            arena.ArenaId = map.InstanceID;
            this.ArenaStatus = ArenaStatus.Wait;
            this.RoundStatus = ArenaRoundStatus.None;
        }

        internal void PlayerEnter()
        {
            this.SourceMapRed = PlayerLeaveMap(this.Red);
            this.SourceMapBlue = PlayerLeaveMap(this.Blue);

            this.PlayerEnterArean();
        }

        private void PlayerEnterArean()
        {
            TeleporterDefine redPoint = DataManager.Instance.Teleporters[this.RedPoint];
            this.Red.Session.Character.Position = redPoint.Position;
            this.Red.Session.Character.Direction = redPoint.Direction;
            TeleporterDefine bluePoint = DataManager.Instance.Teleporters[this.BluePoint];
            this.Blue.Session.Character.Position = bluePoint.Position;
            this.Blue.Session.Character.Direction = bluePoint.Direction;

            this.Map.AddCharacter(this.Red, this.Red.Session.Character);
            this.Map.AddCharacter(this.Blue, this.Blue.Session.Character);
            this.Map.CharacterEnter(this.Red, this.Red.Session.Character);
            this.Map.CharacterEnter(this.Blue, this.Blue.Session.Character);

            EntityManager.Instance.AddMapEntity(this.Map.ID, this.Map.InstanceID, this.Red.Session.Character);
            EntityManager.Instance.AddMapEntity(this.Map.ID, this.Map.InstanceID, this.Blue.Session.Character);
        }

        private Map PlayerLeaveMap(NetConnection<NetSession> player)
        {
            var currentMap = MapManager.Instance[player.Session.Character.Info.mapId];
            currentMap.CharacterLeave(player.Session.Character);
            EntityManager.Instance.RemoveMapEntity(currentMap.ID, currentMap.InstanceID, player.Session.Character);
            return currentMap;
        }

        internal void EntityReady(int entityId)
        {
            if(this.Red.Session.Character.entityId == entityId)
            {
                this.redReady = true;
            }
            if(this.Blue.Session.Character.entityId == entityId)
            {
                this.blueReady = true;
            }

            if (this.Ready)
            {
                this.ArenaStatus = ArenaStatus.Game;
                this.Round = 0;
                NextRound();
            }
        }

        private void NextRound()
        {
            this.Round++;
            this.timer = READY_TIME;
            this.RoundStatus = ArenaRoundStatus.Ready;
            Log.InfoFormat("Arena Round Ready");
            ArenaService.Instance.SendArenaReady(this);
        }

        internal void Update()
        {
            if (this.ArenaStatus == ArenaStatus.Game)
            {
                UpdateRound();
            }
        }

        private void UpdateRound()
        {
            if (this.RoundStatus == ArenaRoundStatus.Ready)
            {
                this.timer -= Time.deltaTime;
                if(timer<0)
                {
                    this.RoundStatus = ArenaRoundStatus.Fight;
                    this.timer = ROUND_TIME;
                    Log.InfoFormat("Arena Round Start");
                    ArenaService.Instance.SendArenaRoundStart(this);
                }
            }else if(this.RoundStatus == ArenaRoundStatus.Fight)
            {
                this.timer -= Time.deltaTime;
                if (timer < 0)
                {
                    this.RoundStatus = ArenaRoundStatus.Result;
                    this.timer = RESULT_TIME;
                    Log.InfoFormat("Arena Round Start");
                    ArenaService.Instance.SendArenaRoundEnd(this);
                }
            }else if(this.RoundStatus == ArenaRoundStatus.Result)
            {
                this.timer -= Time.deltaTime;
                if (timer < 0)
                {
                    if (this.Round >= 3)
                    {
                        ArenaResult();
                    }
                    else
                        NextRound();
                }

            }
        }

        private void ArenaResult()
        {
            this.ArenaStatus = ArenaStatus.Result;
            //执行结算
        }
    }
}
