using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : BaseObject
{
    [Header("몬스터 스텟")]
    [SerializeField] private string monsterName;
    [SerializeField] private MonsterStat stat;
    [SerializeField] private float createTime;

    [Header("몬스터 행동시간 텀")]
    [SerializeField] private float stateChangeSec = 3f;

    [SerializeField] private int Front { get { return CurFilpX ? 1 : -1; } }
    [SerializeField] private bool CurFilpX { get { return spriteRenderer.flipX; } set { spriteRenderer.flipX = !CurFilpX; } }
    
    private SpriteRenderer spriteRenderer;
    private RevenueObject revenue;

    protected override void Init()
    {
        type = EWorldObject_Type.Monster; 
        State = EObject_State.Idle; 

        spriteRenderer = gameObject.FindChild<SpriteRenderer>();
        revenue = gameObject.FindChild<RevenueObject>();
 
        StartCoroutine(StateRandom());
        StartCoroutine(GoldCreate());
        StartCoroutine(FilpRotation());
    }

    private IEnumerator StateRandom() // 랜덤 행동 코루틴
    {
        while(true)
        {
            List<EObject_State> animList = new List<EObject_State>() { EObject_State.Idle, EObject_State.Move, EObject_State.Doing };
            animList.Remove(State);

            yield return new WaitForSeconds(stateChangeSec);

            int rand = Random.Range(0, animList.Count);
            State = animList[rand];
            StartCoroutine(FilpRotation());
        }
    }

    private IEnumerator GoldCreate()
    {
        while(true)
        {
            yield return new WaitForSeconds(createTime);

            // 몬스터 수익
            double gold = stat.CoinDefault * stat.CoinCoefficient;
            MainManager.PlayerData.Gold += gold;
            MainManager.PlayerData.TextUpdate();

            // UI 초기화 및 생성
            revenue.CreateRevenue(gold);
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
