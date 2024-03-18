using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZXS.Game;
using ZXS.Utils;
using Random = UnityEngine.Random;

public class GameLunch : UnitySingleton<GameLunch>
{
    public bool isFalling;
    private GameObject gameOver;
    public static GameLunch THIS;
    private GameObject ItemPre;
    public Item fallingItem;
    public bool isEliminateing;
    public bool isGameOver;
    private void Awake()
    {
        THIS = this;
        ItemPre = AssetsLoadManager.Instance.LoadAsset<GameObject>("Item");
        gameOver = transform.Find("Canvas/GameOver").gameObject;
        isGameOver = false;
    }

    private void Start()
    {
        StartCoroutine(LevelBegin());
    }

    public void Restart()
    {
        Debug.Log("==============新游戏");
        gameOver.SetActive(false);
        isGameOver = false;
        isFalling = false;
        isEliminateing = false;
        LevelBgBuilder.THIS.ReStartGame();
        StartCoroutine(LevelBegin());
    }
    
    IEnumerator LevelBegin()
    {
        while (!isGameOver)
        {
            if(!isFalling && !isEliminateing)
            { 
                Shape shape = getARandomShape();
              //Shape shape = Shape.I_Shape;
                if (fallingItem == null)
                {
                    Item item = Instantiate(ItemPre).GetComponent<Item>();
                    fallingItem = item;
                    item.name = "ItemFalling";
                    
                }
                fallingItem.Init(shape);
                isFalling = true;
            }  
            yield return new WaitForSeconds(0.5f);
        }
      
    }

    public Shape getARandomShape()
    {
        Shape range = Shape.B_Shape;
        if (ScoreAndTime.THIS.scroeValue < 500)
        {
            // 正常
            range = (Shape)Random.Range(0, 19);
        }else if (ScoreAndTime.THIS.scroeValue < 1000)
        {
            if (ScoreAndTime.THIS.IsTuPo)
            {
                range = Shape.X_Shape;
            }
            else
            {
                // 解锁星星
                range = (Shape)Random.Range(0, 20);
            }
         
        }else if (ScoreAndTime.THIS.scroeValue <1500)
        {
            if (ScoreAndTime.THIS.IsTuPo)
            {
                range = Shape.M_Shape;
            }
            else
            {
                // 解锁凸字
                range = (Shape)Random.Range(0, 24);
            }
           
        }
        else if (ScoreAndTime.THIS.scroeValue >= 1500)
        {
            if (ScoreAndTime.THIS.IsTuPo)
            {
                range = Shape.B_Shape;
            }
            else
            {
                // 解锁十字
                range = (Shape)Random.Range(0, 25);
            }
           
        }
      
        return range;
    }
    
    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
    }
    
    
    
}
