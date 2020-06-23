using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMaster : MonoBehaviour
{
    [SerializeField, Tooltip("蜂の巣Prefab")] private HoneyComb honeyCombPrefab = null;
    [SerializeField, Tooltip("蜂Prefab")] private BeeControl beePrefab = null;

    [SerializeField, Header("蜂の巣を生成する予定地")] private Vector3[] createPos = null;

    public Vector3[] CreatePos { set { createPos = value; } }

    [SerializeField, Header("蜂の巣の生成上限数"), Range(1, 15)] private int maxHoneyComb = 5;
    [SerializeField, Header("蜂の最大生成数"), Range(1, 15)] private int maxBee = 5;

    // 管理用の蜂の巣配列を用意
    private HoneyComb[] honeyComb = null;

    // 管理用の蜂の配列を用意
    private BeeControl[] beeControl = null;

    private List<int> locationIDList = new List<int>();

    private float[] randomMoveTime = null;
    private int[] lastPosID = null;

    [SerializeField, Tooltip("テスト用プレイヤー")] private GameObject playerTest = null;

    // Start is called before the first frame update
    void Start()
    {
        CreateHoneyComb();
        CreateBee();
    }

    // Update is called once per frame
    void Update()
    {
        playerTest.transform.position = MouseToWorld();

        PlayerGPS();
        CheckHoneyComb();
    }

    /// <summary>
    /// 蜂の巣を生成する処理
    /// </summary>
    private void CreateHoneyComb()
    {
        // 最大生成数分の座標が用意されていなかった場合
        if(createPos.Length < maxHoneyComb)
        {
            Debug.LogError("最大生成数分の座標を用意してください");
            return;
        }

        // 蜂の巣のインスタンスを生成（初回のみ）
        if(honeyComb == null)
        {
            honeyComb = new HoneyComb[maxHoneyComb];
            for(int i = 0; i < honeyComb.Length; i++)
            {
                honeyComb[i] = Instantiate(honeyCombPrefab);
            }
        }

        // ロケーションIDリストを用意
        for(int i = 0; i < createPos.Length; i++)
        {
            locationIDList.Add(i);
        }

        // 巣を配置
        foreach(var comb in honeyComb)
        {
            int index = Random.Range(0, locationIDList.Count);
            comb.transform.position = createPos[locationIDList[index]];
            comb.LocationID = locationIDList[index];
            locationIDList.RemoveAt(index);
        }
    }

    /// <summary>
    /// 蜂を生成
    /// </summary>
    private void CreateBee()
    {
        beeControl = new BeeControl[maxBee];
        randomMoveTime = new float[beeControl.Length];
        lastPosID = new int[beeControl.Length];
        for(int i = 0; i < beeControl.Length; i++)
        {
            beeControl[i] = Instantiate(beePrefab);
            beeControl[i].Init();
            beeControl[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < lastPosID.Length; i++)
        {
            lastPosID[i] = createPos.Length;
        }
    }

    /// <summary>
    /// 蜂を呼び出す
    /// </summary>
    private void SpawnBee(int locationID)
    {
        if(beeControl == null) { return; }

        List<int> list = new List<int>();
        int index = 0;

        // アクティブ状態でない蜂を検索
        foreach(var bee in beeControl)
        {
            if(bee.gameObject.activeSelf == false)
            {
                list.Add(index);
            }
            index++;
        }

        if(list.Count == 0)
        {
            // 追跡状態でない蜂を検索
            index = 0;
            foreach (var bee in beeControl)
            {
                if (bee.Chase == false)
                {
                    list.Add(index);
                }
                index++;
            }
        }

        if(list.Count == 0)
        {
            index = Random.Range(0, beeControl.Length);
        }
        else
        {
            index = Random.Range(0, list.Count);
            index = list[index];
        }

        // 蜂を指定個所に配置
        var target = beeControl[index];
        target.transform.position = createPos[locationID];
        target.gameObject.SetActive(true);
        target.SpawnAnimation();
    }

    /// <summary>
    /// 蜂の出撃許可をチェック
    /// </summary>
    private void CheckHoneyComb()
    {
        if(honeyComb == null) { return; }

        foreach(var comb in honeyComb)
        {
            if (comb.SpawnFlag)
            {
                comb.SpawnFlag = false;
                SpawnBee(comb.LocationID);
            }
        }
    }

    /// <summary>
    /// テスト用の関数
    /// </summary>
    /// <returns></returns>
    private Vector3 MouseToWorld()
    {
        Vector3 mouse = Input.mousePosition;
        Camera main = Camera.main;
        mouse.z = 10;
        Vector3 pos = main.ScreenToWorldPoint(mouse);
        return pos;
    }

    /// <summary>
    /// プレイヤーの座標情報を渡す
    /// </summary>
    private void PlayerGPS()
    {
        if(beeControl != null)
        {
            for(int i = 0; i < beeControl.Length; i++)
            {
                if (beeControl[i].Chase)
                {
                    // 追跡状態のときはプレイヤーの座標に向かってくる
                    beeControl[i].MovePos = MouseToWorld();
                }
                else
                {
                    // 非追跡状態のときは蜂の巣の生成座標をランダムに取得して向かってくる
                    randomMoveTime[i] += Time.deltaTime;
                    if(randomMoveTime[i] > 2.0f)
                    {
                        int index = Random.Range(0, createPos.Length);

                        while(index == lastPosID[i])
                        {
                            index = Random.Range(0, createPos.Length);
                        }

                        lastPosID[i] = index;

                        beeControl[i].MovePos = createPos[index];

                        randomMoveTime[i] = 0;
                    }   
                }
            }
        }
    }
}
