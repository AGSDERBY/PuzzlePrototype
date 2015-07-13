using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using Puzzle.Block.Core;
using System.Collections.Generic;

public class Block : MonoBehaviour, IPointerClickHandler
{
    public string blockName;
    public bool isDestroyable = false;
    public ParticleSystem DestroyEffect;
    public float respawnDelay;
    public BlockInfo info = new BlockInfo();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Click event method
    public void OnPointerClick(PointerEventData eventData)
    {
        isDestroyable = true;
        CheckDeleteableBlock();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="blocks"></param>
    private void CheckDeleteableBlock()
    {
        // TOP
        if (info.Coordinate.Y + 1 < info.Controller.size)
        {
            CheckSampleBlock(info.Coordinate.X, info.Coordinate.Y + 1);
        }

        // BOTTOM
        if (info.Coordinate.Y - 1 >= 0)
        {
            CheckSampleBlock(info.Coordinate.X, info.Coordinate.Y - 1);
        }

        // RIGHT
        if (info.Coordinate.X + 1 < info.Controller.size)
        {
            CheckSampleBlock(info.Coordinate.X + 1, info.Coordinate.Y);
        }

        // LEFT
        if (info.Coordinate.X - 1 >= 0)
        {
            CheckSampleBlock(info.Coordinate.X - 1, info.Coordinate.Y);
        }
    }

    private void CheckSampleBlock(int xIndex, int yIndex)
    {
        try
        {
            var GameObject = info.Controller.blocksArray[xIndex, yIndex];

            if (GameObject != null)
            {
                Block block = GameObject.GetComponent<Block>();

                if (block != null && block.info.isChecked == false)
                {
                    if (block.blockName.Equals(this.blockName))
                    {
                        block.info.isChecked = true;
                        block.isDestroyable = true;
                        block.CheckDeleteableBlock();
                    }
                }
            }
        }
        catch (Exception e)
        {
            var obj = info.Controller.blocksArray[xIndex, yIndex].GetComponent<Block>();
            Debug.LogFormat("EXCEPTION {0} {1}", e.Message, obj);
        }
    }

    void OnTriggerEnter(Collider other)
    {

    }


    public void displayDeleteEffect()
    {
        ParticleSystem particleObject = Instantiate(DestroyEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.1f), this.transform.rotation) as ParticleSystem;
        Destroy(particleObject.gameObject, DestroyEffect.duration);
    }
}
