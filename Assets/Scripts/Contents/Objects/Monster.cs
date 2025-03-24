using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Util;

public class Monster : BaseObject
{
    [SerializeField] private int Front { get { return CurFilpX ? 1 : -1; } }
    [SerializeField] private bool CurFilpX { get { return spriteRenderer.flipX; } set { spriteRenderer.flipX = !CurFilpX; } }
    [Header("몬스터 스텟")][SerializeField] private StatInfo stat;
    [Header("몬스터 행동시간 텀")][SerializeField] private float stateChangeSec = 3f;
    private SpriteRenderer spriteRenderer;

    protected override void Init()
    {
        type = WorldObject_Type.Monster; 
        State = Object_State.Idle;
        spriteRenderer = FindChild<SpriteRenderer>(this.gameObject);

        StartCoroutine(StateRandom());
        StartCoroutine(FilpRotation());
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
            StartCoroutine(FilpRotation());
        }
    }

    private IEnumerator FilpRotation()
    {
        float rand = Random.Range(1f, 2f);
        yield return new WaitForSeconds(rand);

        CurFilpX = !CurFilpX;
    }

    protected override void UpdateIdle()
    {
        
    }

    protected override void UpdateMove()
    {
        float clampX = Mathf.Clamp(transform.position.x + (Front * Time.deltaTime * 2f) , -21.5f, 23.5f);
        transform.position = new Vector3(clampX, transform.position.y, 0);
    }
}
