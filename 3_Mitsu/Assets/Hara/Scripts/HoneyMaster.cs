using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class HoneyMaster : MonoBehaviour
{
    [SerializeField, Tooltip("蜂の巣Prefab")] private HoneyComb honeyCombPrefab = null;
    [SerializeField, Tooltip("蜂Prefab")] private BeeControl beePrefab = null;

    [SerializeField, Header("蜂の巣を生成する予定地")] private Vector3[] createPos = null;

    public Vector3[] CreatePos { set { createPos = value; } }

    [SerializeField, Header("蜂の巣の生成上限数"), Range(1, 15)] private int maxHoneyComb = 5;
    [SerializeField, Header("蜂の最大生成数"), Range(1, 15)] private int maxBee = 5;
    [SerializeField, Header("蜂の移動速度"), Range(1.0f, 5.0f)] private float speed = 1.0f;
    [SerializeField, Header("蜂の巡回行動範囲"), Range(5.0f, 20.0f)] private float maxMoveArea = 10.0f;
    [SerializeField, Header("PlayStatusオブジェクト")] private PlayerStatus status = null;
    [SerializeField, Header("拠点の中心座標")] private Vector3 hubPos = Vector3.zero;
    [SerializeField, Header("拠点の判定範囲"), Range(1.0f, 5.0f)] private float hubArea = 1.0f;

    // 管理用の蜂の巣配列を用意
    private HoneyComb[] honeyComb = null;

    // 管理用の蜂の配列を用意
    private BeeControl[] beeControl = null;

    private List<int> locationIDList = new List<int>();
    private List<int> activeBeeID = new List<int>();

    private float[] randomMoveTime = null;
    private float[] spawnDelayTime = null;
    private bool[] fixPosition = null;
    private bool playerInHub = false;  // プレイヤーが拠点にいる場合のフラグ
    private bool gamePlay = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateHoneyComb();
        CreateBee();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            gamePlay = GameStatus.Instance.gameMode == GameStatus.GameMode.Play;
        }
        catch
        {
            gamePlay = true;
        }

        if (gamePlay)
        {
            PlayerGPS();
            CheckState();
        }
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
        honeyComb = new HoneyComb[maxHoneyComb];
        spawnDelayTime = new float[honeyComb.Length];
        for (int i = 0; i < honeyComb.Length; i++)
        {
            honeyComb[i] = Instantiate(honeyCombPrefab);
        }

        // ロケーションIDリストを用意
        for (int i = 0; i < createPos.Length; i++)
        {
            locationIDList.Add(i);
        }

        // 巣を配置
        foreach (var comb in honeyComb)
        {
            int index = Random.Range(0, locationIDList.Count);
            comb.transform.position = createPos[locationIDList[index]];
            comb.LocationID = locationIDList[index];
            comb.SetHoney();
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
        fixPosition = new bool[beeControl.Length];
        for(int i = 0; i < beeControl.Length; i++)
        {
            beeControl[i] = Instantiate(beePrefab);
            beeControl[i].Speed = speed;
            beeControl[i].Init();
            beeControl[i].gameObject.SetActive(false);
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
            // プレイヤーと最も距離が離れている蜂をリスポーン対象にする
            if(status != null)
            {
                float saveDistance = 0;
                int saveIndex = 0;
                for(int i = 0; i < list.Count; i++)
                {
                    Vector2 player = new Vector2(status.PlayerTransform.position.x, status.PlayerTransform.position.y);
                    Vector2 bee = new Vector2(beeControl[list[i]].transform.position.x, beeControl[list[i]].transform.position.y);
                    float dis = Vector2.Distance(player, bee);
                    if(saveDistance < dis)
                    {
                        saveDistance = dis;
                        saveIndex = i;
                    }
                    index = saveIndex;
                }
            }
            else
            {
                index = Random.Range(0, list.Count);
            }
            index = list[index];
        }

        // 蜂を指定個所に配置
        var target = beeControl[index];
        target.transform.position = createPos[locationID];
        target.gameObject.SetActive(true);
        target.SpawnAnimation();
    }

    /// <summary>
    /// 蜂関連のステータスをチェック
    /// </summary>
    private void CheckState()
    {
        if(honeyComb == null || createPos.Length <= 0) { return; }

        // 蜂の巣
        int index = 0;
        foreach(var comb in honeyComb)
        {
            int id = comb.LocationID;

            // 蜂の出撃許可が出ていた場合
            if (comb.SpawnFlag)
            {
                if (activeBeeID.Contains(id) == false)
                {
                    activeBeeID.Add(id);
                    SpawnBee(id);
                }

                // 10秒後に蜂の巣を再生成
                spawnDelayTime[index] += Time.deltaTime;
                if (spawnDelayTime[index] > 10.0f)
                {
                    spawnDelayTime[index] = 0;
                    locationIDList.Add(id);
                    int rand = Random.Range(0, locationIDList.Count);
                    comb.transform.position = createPos[locationIDList[rand]];
                    comb.LocationID = locationIDList[rand];
                    comb.SetHoney();
                    locationIDList.RemoveAt(rand);

                    int num = activeBeeID.IndexOf(comb.LocationID);
                    if(num >= 0)
                    {
                        activeBeeID.RemoveAt(num);
                    }
                }
            }
            index++;
        }

        // 蜂
        if(beeControl == null) { return; }

        foreach(var bee in beeControl)
        {
            if (bee.HitFlag && playerInHub == false)
            {
                HitToPlayer();
                break;
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
        if(beeControl != null && createPos.Length > 0)
        {
            for(int i = 0; i < beeControl.Length; i++)
            {
                if (beeControl[i].gameObject.activeSelf)
                {
                    if (beeControl[i].Chase && playerInHub == false)
                    {
                        Vector3 movePos;
                        // 追跡状態のときはプレイヤーの座標に向かってくる
                        if (status != null)
                        {
                            movePos = status.PlayerTransform.position;
                        }
                        else
                        {
                            movePos = MouseToWorld();
                        }
                        beeControl[i].MasterControl = false;
                        fixPosition[i] = false;
                        beeControl[i].MovePos = movePos;
                    }
                    else
                    {
                        // 非追跡状態のときは蜂の巣の生成座標をランダムに取得して向かってくる
                        randomMoveTime[i] += Time.deltaTime;
                        if (randomMoveTime[i] > 5.0f)
                        {
                            beeControl[i].MovePos = new Vector3(Random.Range(-maxMoveArea, maxMoveArea), Random.Range(-maxMoveArea, maxMoveArea), 0);

                            fixPosition[i] = false;

                            randomMoveTime[i] = 0;
                        }

                        // 蜂が拠点エリア内に侵入しないように座標を修正する
                        Vector2 bee = new Vector2(beeControl[i].transform.position.x, beeControl[i].transform.position.y);
                        Vector2 hub = new Vector2(hubPos.x, hubPos.y);
                        if (bee.x > hub.x - hubArea && bee.x < hub.x + hubArea && bee.y > hub.y - hubArea && bee.y < hub.y + hubArea && fixPosition[i] == false)
                        {
                            fixPosition[i] = true;

                            float x = hub.x + hubArea <= maxMoveArea ? Random.Range(hub.x + hubArea, maxMoveArea) : maxMoveArea;
                            float y = hub.y + hubArea <= maxMoveArea ? Random.Range(hub.y + hubArea, maxMoveArea) : maxMoveArea;
                            Vector3 target;

                            if(bee.x > hub.x && bee.y > hub.y)
                            {
                                target = new Vector3(x, y, 0);
                            }
                            else if(bee.x > hub.x && bee.y <= hub.y)
                            {
                                target = new Vector3(x, y * -1, 0);
                            }
                            else if(bee.x <= hub.x && bee.y > hub.y)
                            {
                                target = new Vector3(x * -1, y, 0);
                            }
                            else
                            {
                                target = new Vector3(x * -1, y * -1, 0);
                            }

                            beeControl[i].MovePos = target;
                        }

                        beeControl[i].MasterControl = true;
                    }
                }
            }
        }

        // プレイヤーが拠点付近にいるかチェック
        if(status != null)
        {
            Vector2 playerPos = new Vector2(status.PlayerTransform.position.x, status.PlayerTransform.position.y);
            Vector2 hubCenter = new Vector2(hubPos.x, hubPos.y);
            float distance = Vector2.Distance(playerPos, hubCenter);
            playerInHub = distance < hubArea;
        }
        else
        {
            playerInHub = false;
        }
    }

    /// <summary>
    /// 蜂がプレイヤーに接触した時に呼び出す処理
    /// </summary>
    private void HitToPlayer()
    {
        Debug.Log("接触しました");
    }
}
