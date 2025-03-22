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
            // �밡�� ������ �ڵ�ȭ �ؾ���
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

    private IEnumerator StateRandom() // ���� �ൿ �ڷ�ƾ
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
