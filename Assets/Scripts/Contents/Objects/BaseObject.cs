using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Util;

public abstract class BaseObject : MonoBehaviour
{
    [SerializeField] private Object_State curState;
    [SerializeField] protected WorldObject_Type type = WorldObject_Type.None;

    private Animator anim;
    
    public virtual Object_State State
    {
        get { return curState; }
        set
        {
            anim = FindChild<Animator>(this.gameObject);
            curState = value;

            switch (curState)
            {
                case Object_State.Idle:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                case Object_State.Move:
                    anim.CrossFade("Move", 0.1f);
                    break;
                case Object_State.Doing:
                    anim.CrossFade("Doing", 0.1f);
                    break;
            }
        }
    }
    protected abstract void Init();

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (State)
        {
            case Object_State.Idle:
                UpdateIdle();
                break;
            case Object_State.Move:
                UpdateMove();
                break;
            case Object_State.Doing:
                UpdateDoing();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateDoing() { }
}
