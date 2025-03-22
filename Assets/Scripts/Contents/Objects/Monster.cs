using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Util;

public class Monster : BaseObject
{
    [SerializeField] private float stateChangeSec = 3f;
    [SerializeField] private bool CurFilpX 
    { 
        get { return spriteRenderer.flipX; }

        set
        {
            spriteRenderer.flipX = !CurFilpX;
            // 대가리 돌리기 자동화 해야함
        }
    }

    private SpriteRenderer spriteRenderer;

    protected override void Init()
    {
        type = WorldObject_Type.Monster; 
        State = Object_State.Idle;
        spriteRenderer = FindChild<SpriteRenderer>(this.gameObject);

        StartCoroutine(StateRandom());
    }

    private IEnumerator StateRandom() // 랜덤 행동 코루틴
    {
        while(true)
        {
            List<Object_State> animList = new List<Object_State>() { Object_State.Idle, Object_State.Move, Object_State.Doing };
            animList.Remove(State);

            yield return new WaitForSeconds(stateChangeSec);

            int rand = Random.Range(0, animList.Count);
            State = animList[rand];
        }
    }

    protected override void UpdateIdle()
    {
        
    }

    protected override void UpdateMove()
    {
        
    }
}
