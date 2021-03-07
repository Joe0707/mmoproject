using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNofity
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity,int param);
        void OnEntityEvent(EntityEvent @event);
    }
    class EntityManager:Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNofity> notifiers = new Dictionary<int, IEntityNofity>();
        public void RegisterEntityChangeNofity(int entityId,IEntityNofity notify)
        {
            this.notifiers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.Id);
            if(notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }

        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if(entity!=null)
            {
                if(data.Entity!=null)
                {
                    entity.EntityData = data.Entity;
                }
                if(notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity,data.Param);
                    notifiers[entity.entityId].OnEntityEvent(data.Event);
                }
            }
        }

        public Entity GetEntity(int entityId)
        {
            Entity entity = null;
            entities.TryGetValue(entityId, out entity);
            return entity;
        }
    }
}
