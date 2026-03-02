using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class BaseObject : MonoBehaviour
{
    [Header("현재 상태")]
    [SerializeField] private EFriend_State curState;

    [Header("오브젝트 타입")]
    [SerializeField] protected EWorldObject_Type type = EWorldObject_Type.None;

    private Animator anim;
    protected List<Coroutine> coroutineList;

    public virtual EFriend_State State
    {
        get { return curState; }
        set
        {
     
            curState = value;

            switch (curState)
            {
                case EFriend_State.Idle:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                case EFriend_State.Move:
                    anim.CrossFade("Move", 0.1f);
                    break;
                case EFriend_State.Doing:
                    {
                        int id = Animator.StringToHash("Base Layer.Idle");
                        if (anim.HasState(0, id) == true)
                            anim.CrossFade("Doing", 0.1f);
                        else
                            State = EFriend_State.Idle;
                    }
                    break;
            }
        }
    }
    protected abstract void Init();

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
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
            case EFriend_State.Idle:
                UpdateIdle();
                break;
            case EFriend_State.Move:
                UpdateMove();
                break;
            case EFriend_State.Doing:
                UpdateDoing();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateDoing() { }
}
