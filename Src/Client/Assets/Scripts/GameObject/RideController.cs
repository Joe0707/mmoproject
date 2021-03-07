using SkillBridge.Message;
using UnityEngine;

public class RideController:MonoBehaviour{
    public Transform mountPoint;
    public EntityController rider;
    public Vector3 offset;
    private Animator anim;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;
        this.rider.SetRidePotision(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
    }

    public void SetRider(EntityController rider)
    {
        this.rider = rider;
    }

    public void OnEntityEvent(EntityEvent entityEvent,int param)
    {
        switch(entityEvent)
        {
            case EntityEvent.Idle:
                break;
        }
    }

    public void SetRider(EntityController rider)
    {
        this.rider = rider;
    }
}