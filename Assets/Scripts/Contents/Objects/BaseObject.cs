using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class BaseObject : MonoBehaviour
{
    [Header("현재 상태")]
    [SerializeField] private EObject_State curState;

    [Header("오브젝트 타입")]
    [SerializeField] protected EWorldObject_Type type = EWorldObject_Type.None;

    private Animator anim;
    
    public virtual EObject_State State
    {
        get { return curState; }
        set
        {
            anim = gameObject.FindChild<Animator>();
            curState = value;

            switch (curState)
            {
                case EObject_State.Idle:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                case EObject_State.Move:
                    anim.CrossFade("Move", 0.1f);
                    break;
                case EObject_State.Doing:
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
            case EObject_State.Idle:
                UpdateIdle();
                break;
            case EObject_State.Move:
                UpdateMove();
                break;
            case EObject_State.Doing:
                UpdateDoing();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateDoing() { }
}
