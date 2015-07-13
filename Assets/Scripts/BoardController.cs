using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using log4net;
using Puzzle.Block.Core;
using System;

public class BoardController : MonoBehaviour
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public List<GameObject> blocks;
    public GameObject[,] blocksArray;
    private Dictionary<int, Sprite> sprites;
    private List<BlockInfo> deletedBlockPosition = new List<BlockInfo>();
    public int size = 4;
    private float spacing = 1.05f;

    void Awake()
    {
        Initialize();
    }

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// ボード初期化
    /// </summary>
    private void Initialize()
    {
        blocks = new List<GameObject>();
        blocksArray = new GameObject[size,size];
        sprites = new Dictionary<int, Sprite>();
        var test = Resources.LoadAll("Sprites", typeof(Sprite));

        for (int i = 0; i < test.Length; i++)
        {
            sprites.Add(i, (Sprite)test[i]);
        }

        int blockSize = size * size;

        for (int index = 0; index < blockSize; index++)
        {
            float xOffset = index / size * spacing;
            float yOffset = index % size * spacing;

            int xIndex = index / size;
            int yIndex = index % size;

            BlockInfo info = new BlockInfo();
            info.Position = new Vector3(xOffset, yOffset);
            info.Coordinate = new Coordinate(xIndex, yIndex);
            info.Controller = this;
            CreateBlock(info);
        }
    }

    /// <summary>
    /// ブロック生成
    /// </summary>
    /// <param name="xPosition"></param>
    /// <param name="yPosition"></param>
    private GameObject CreateBlock(BlockInfo info)
    {
        GameObject blockPrefab = Resources.Load("Prefabs/Core/Block_new") as GameObject;
        GameObject blockInstance = (GameObject)Instantiate(blockPrefab, info.Position, transform.rotation);

        // Sprite Setting
        Block block = blockInstance.GetComponent<Block>();

        //Sprite spriteResource = sprites[Random.Range(0, sprites.Keys.Count)];
        Sprite spriteResource = sprites[Random.Range(0, 3)];
        blockInstance.GetComponent<SpriteRenderer>().sprite = spriteResource;
        block.blockName = spriteResource.name;

        // Effect Setting
        GameObject explosionEffect = Resources.Load("Prefabs/Fx/MagicEffects/prefab/skillAttack2") as GameObject;
        block.DestroyEffect = explosionEffect.GetComponent<ParticleSystem>();

        info.reSpawnDelay = 1.5f;
        info.isChecked = false;

        block.info = info;
        blocks.Add(blockInstance);
        blocksArray[info.Coordinate.X, info.Coordinate.Y] = blockInstance;

        return blockInstance;
    }

    // Update is called once per frame
    void Update()
    {
        // 削除可能なブロックを削除
        DeleteBlocks();
        // 生成可能であれば新しいブロックを作成する。
        CreateNewBlock();
    }

    /// <summary>
    /// 新しいブロックを生成
    /// </summary>
    private void CreateNewBlock()
    {
        for (int index = 0; index < deletedBlockPosition.Count; index++)
        {
            var info = deletedBlockPosition[index];

            if (info.IsReSpawnable(Time.time))
            {
                CreateBlock(info);
                deletedBlockPosition.RemoveAt(index);
            }

        }
    }

    /// <summary>
    /// 削除可能なブロックをチェックし削除
    /// その後、削除した場所を記録。
    /// </summary>
    private void DeleteBlocks()
    {
        foreach (GameObject block in blocks)
        {
            if (block != null)
            {
                var target = block.GetComponent<Block>();

                if (target.isDestroyable)
                {
                    target.displayDeleteEffect();

                    var info = target.info;
                    info.SetReSpawnTime(Time.time);
                    deletedBlockPosition.Add(info);
                    blocksArray[info.Coordinate.X, info.Coordinate.Y] = null;
                    Destroy(block.gameObject);
                }
            }
        }
    }
}
