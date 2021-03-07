using Common;
using GameServer.Core;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EntityManager:Singleton<EntityManager>
    {
        private int index = 0;
        public Dictionary<int,Entity> AllEntities = new Dictionary<int, Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public int GetMapIndex(int mapId,int instanceId)
        {
            return mapId * 1000 + instanceId;
        }

        public void AddEntity(int mapId,int instanceId,Entity entity)
        {
            AllEntities.Add(entity.EntityData.Id, entity);
            //加入管理器生成唯一ID
            entity.EntityData.Id = ++this.index;
            AddMapEntity(mapId, instanceId, entity);
        }

        public void AddMapEntity(int mapId, int instanceId, Entity entity)
        {
            List<Entity> entities = null;
            int index = this.GetMapIndex(mapId, instanceId);
            if (!MapEntities.TryGetValue(index, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }
            entities.Add(entity);
        }

        public void RemoveEntity(int mapId,int instanceID,Entity entity)
        {
            this.AllEntities.Remove(entity.entityId);
            this.RemoveMapEntity(mapId, instanceID, entity);
        }

        public Entity GetEntity(int entityId)
        {
            Entity result = null;
            this.AllEntities.TryGetValue(entityId, out result);
            return result;
        }

        internal void RemoveMapEntity(int mapId, int instanceID, Entity entity)
        {
            this.MapEntities[this.GetMapIndex(mapId,instanceID)].Remove(entity);
        }

        internal object GetCreature(int entityId)
        {
            return GetEntity(entityId) as Creature;
        }

        public List<T> GetMapEntities<T>(int mapId,Predicate<Entity> match)where T :Creature
        {
            List<T> result = new List<T>();
            foreach(var entity in this.MapEntities[mapId])
            {
                if (entity is T && match.Invoke(entity))
                    result.Add((T)entity);
            }
            return result;
        }

        public List<T> GetMapEntitiesInRange<T> (int mapId,Vector3Int pos,int range) where T : Creature
        {
            return this.GetMapEntities<T>(mapId, (entity) =>
            {
                T creature = entity as T;
                return creature.Distance(pos) < range;
            });
        }
    }
}
