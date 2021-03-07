using Common.Data;
using GameServer.Managers;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Story
    {
        const float READY_TIME = 11f;
        const float ROUND_TIME = 60f;
        const float RESULT_TIME = 5f;

        public Map Map;
        public int StoryId;
        public int InstanceId;
        public NetConnection<NetSession> Player;

        Map SourceMapRed;

        int startPoint = 12;

        public int Round { get; internal set; }
        private float timer = 0f;
        public Story(Map map,int storyId,int instance,NetConnection<NetSession> owner)
        {
            this.StoryId = storyId;
            this.Player = owner;
            this.Map = map;
        }

        public void PlayerEnter()
        {
            this.SourceMapRed = PlayerLeaveMap(this.Player);
            this.PlayerEnterArena();
        }

        private void PlayerEnterArena()
        {
            TeleporterDefine startPoint = DataManager.Instance.Teleporters[this.startPoint];
            this.Player.Session.Character.Position = startPoint.Position;
            this.Player.Session.Character.Direction = startPoint.Direction;
            this.Map.AddCharacter(this.Player, this.Player.Session.Character);
            this.Map.CharacterEnter(this.Player, this.Player.Session.Character);
            EntityManager.Instance.AddMapEntity(this.Map.ID, this.Map.InstanceID, this.Player.Session.Character);
        }

        private Map PlayerLeaveMap(NetConnection<NetSession> player)
        {
            var currentMap = MapManager.Instance[player.Session.Character.Info.mapId];
            currentMap.CharacterLeave(player.Session.Character);
            EntityManager.Instance.RemoveMapEntity(currentMap.ID, currentMap.InstanceID, player.Session.Character);
            return currentMap;
        }

        internal void Update()
        {

        }

        public void End()
        {

        }
    }
}
