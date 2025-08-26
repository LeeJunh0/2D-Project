using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Friend : BaseObject
{
    [Header("친구 스텟")]
    [SerializeField] private string monsterName;
    [SerializeField] private FriendStat stat;

    [Header("친구 행동시간 텀")]
    [SerializeField] private float stateChangeSec = 3f;

    [SerializeField] private int Front { get { return CurFilpX ? 1 : -1; } }
    [SerializeField] private bool CurFilpX { get { return spriteRenderer.flipX; } set { spriteRenderer.flipX = !CurFilpX; } }

    private SpriteRenderer spriteRenderer;
    private RevenueObject revenue;

    public FriendStat Stat { get => stat; set => stat = value; }

    protected override void Init()
    {
        spriteRenderer = gameObject.FindChild<SpriteRenderer>();
        revenue = gameObject.FindChild<RevenueObject>();

        type = EWorldObject_Type.Friend;
        State = EObject_State.Idle;
        RarityOutLine();

        coroutineList = new List<Coroutine>
        {
            StartCoroutine(FilpRotation()),
            StartCoroutine(StateRandom()),
            StartCoroutine(GoldCreate())
        };
    }

    public void RarityOutLine()
    {
        switch (stat.Rarity)
        {
            case EFriend_Rarity.Normal:
                spriteRenderer.material = MainManager.Addressable.Load<Material>("NormalOutLine");
                break;
            case EFriend_Rarity.Rare:
                spriteRenderer.material = MainManager.Addressable.Load<Material>("RareOutLine");
                break;
            case EFriend_Rarity.Named:
                spriteRenderer.material = MainManager.Addressable.Load<Material>("NamedOutLine");
                break;
            case EFriend_Rarity.Boss:
                spriteRenderer.material = MainManager.Addressable.Load<Material>("BossOutLine");
                break;
        }
    }

    private IEnumerator StateRandom() // 랜덤 행동 코루틴
    {
        while (true)
        {
            if (PlayerDataManager.Instance.IsLoadCompleted == false)
            {
                yield return null;
                continue;
            }

            List<EObject_State> animList = new List<EObject_State>() { EObject_State.Idle, EObject_State.Move, EObject_State.Doing };
            animList.Remove(State);

            yield return new WaitForSeconds(stateChangeSec);

            int rand = Random.Range(0, animList.Count);
            State = animList[rand];
        }
    }

    private IEnumerator GoldCreate()
    {
        while (true)
        {
            if (PlayerDataManager.Instance.IsLoadCompleted == false)
            {
                yield return null;
                continue;
            }

            stat.curCoinTime += Time.deltaTime;
            yield return null;

            // 수익
            if (stat.curCoinTime >= stat.info.coinPerSec)
            {
                double gold = Stat.info.Coin;
                PlayerDataManager.Instance.Gold += gold;
                PlayerDataManager.Instance.GoldUpdate();

                // UI 초기화 및 생성
                revenue.CreateRevenue(gold);
                stat.curCoinTime = 0f;
            }
        }
    }

    private IEnumerator FilpRotation()
    {
        while (true)
        {
            float rand = Random.Range(1f, 2f);
            yield return new WaitForSeconds(rand);

            CurFilpX = !CurFilpX;
        }
    }

    protected override void UpdateIdle()
    {

    }

    protected override void UpdateMove()
    {
        float clampX = Mathf.Clamp(transform.position.x + (Front * Time.deltaTime * 2f), -21.5f, 23.5f);
        transform.position = new Vector3(clampX, transform.position.y, 0);
    }

    private void OnDisable()
    {
        if (coroutineList.Count <= 0)
            return;

        foreach(var co in coroutineList)
            StopCoroutine(co);
    }
}
