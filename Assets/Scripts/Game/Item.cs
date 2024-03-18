using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZXS.Utils;

namespace ZXS.Game
{
    public class Item : MonoBehaviour
    {
        private Shape _type;
        private bool lastisFull; // 用于记录星星下落的上一个位置有没有元素
        private int[,] Itemdata;
        private int midCol = 9;
        private int norMalRow;

        private Coroutine fallingIEtor;
        public class PosInt
        {
            public int x;
            public int y;

            public PosInt(int _x, int _y)
            {
                this.x = _x;
                this.y = _y;
            }
        }
        
        private List<PosInt> _itemSquares = new List<PosInt>();

        private void Awake()
        {
            norMalRow = LevelBgBuilder.THIS.maxRow-1;
        }

        public void Init(Shape type)
        {
            _type = type;
            Itemdata = LevelBgBuilder.THIS.getAShapeItem(type);
            _itemSquares.Clear();
            isVerticaling = false;
            isHorizontalling = false;
            for (int i = 0; i < Itemdata.GetLength(0); i++)
            {
                for (int j = 0; j < Itemdata.GetLength(1); j++)
                {
                    if (Itemdata[i, j] == 1)
                    {
                       ItemSquare itemSquare = LevelBgBuilder.THIS.GetItemSquare(norMalRow - i, midCol + j);
                       itemSquare.SetImageFull(true);
                       _itemSquares.Add(new PosInt(itemSquare.row, itemSquare.col));
                    }
                }
            }
            
            
            
            
            
            if (type == Shape.X_Shape)
            {
                fallingIEtor= StartCoroutine(bbbFalling()); 
            }
            else
            {
                fallingIEtor=  StartCoroutine(aaaFalling()); 
            }
            AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemOut);

        }

        /// <summary>
        /// 变形
        /// </summary>
        public void ChangeShape()
        {
            if(!GameLunch.Instance.isFalling)
            {
                return;
            }

            switch (_type)
            {
                case Shape.X_Shape:
                case Shape.B_Shape:
                case Shape.O_Shpae:
                    return;
            }
            
            int currRow = norMalRow-1;
            int currCol = midCol;
            for (int i = 0; i < _itemSquares.Count; i++)
            {
                if(_itemSquares[i].x < currRow)
                {
                    currRow = _itemSquares[i].x;

                    ItemSquare itemSquare = LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x - 1, _itemSquares[i].y);
                    if (currRow <= 0 ||  (itemSquare._isFull && !IsSelf(itemSquare.row, itemSquare.col)))
                    {
                        return;
                    }
                }
                if(_itemSquares[i].y != currCol)
                {
                    currCol = _itemSquares[i].y;

                    if (currCol >= LevelBgBuilder.THIS.maxCol-4)
                    {
                        currCol = LevelBgBuilder.THIS.maxCol - 4;
                    }
                    if (currCol <= 4)
                    {
                        currCol = 4;
                    }  
                }
            }

            currCol -= 1;
            currRow += 1;
         
            // 判断变现后是否已经有元素在，如果有元素了，就不允许变形
            for (int i = 0; i < Itemdata.GetLength(0); i++)
            {
                for (int j = 0; j < Itemdata.GetLength(1); j++)
                {
                    if (Itemdata[i, j] == 1)
                    {
                        ItemSquare itemSquare = LevelBgBuilder.THIS.GetItemSquare(currRow - i, currCol + j);
                        if (itemSquare._isFull && !IsSelf(currRow - i, currCol + j))
                        {
                            return;
                        }
                       
                    }
                }
            }
            

            for (int i = 0; i < _itemSquares.Count; i++)
            {
             
                LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(false);
            }
            
            Shape nextShape = LevelBgBuilder.THIS.GetNextShape(_type);
            _type = nextShape;
            Itemdata = LevelBgBuilder.THIS.getAShapeItem(_type);
            
            
            
            _itemSquares.Clear();
            
            for (int i = 0; i < Itemdata.GetLength(0); i++)
            {
                for (int j = 0; j < Itemdata.GetLength(1); j++)
                {
                    if (Itemdata[i, j] == 1)
                    {
                        ItemSquare itemSquare = LevelBgBuilder.THIS.GetItemSquare(currRow - i, currCol + j);
                        itemSquare.SetImageFull(true);
                        _itemSquares.Add(new PosInt(itemSquare.row, itemSquare.col));
                    }
                }
            }
            
            AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemChange);
        }
        public void MoveLeft()
        {
            if (!GameLunch.THIS.isFalling || GameLunch.THIS.isEliminateing)
            {
                return;
            }
            foreach (var item in _itemSquares)
            {
                if (item.y - 1 <= -1 || (LevelBgBuilder.THIS.GetItemSquare(item.x, item.y-1)._isFull && !IsSelf(item.x, item.y-1)))
                {
                    return;
                }
            }
            
            for (int i = 0; i < _itemSquares.Count; i++)
            {
                LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(false);
                _itemSquares[i].y--;
            }
            for (int i = 0; i < _itemSquares.Count; i++)
            {
                LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
            }  
            AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemMove);
        }
        public void MoveRight()
        {
            if (!GameLunch.THIS.isFalling || GameLunch.THIS.isEliminateing)
            {
                return;
            }
            foreach (var item in _itemSquares)
            {
                if (item.y+ 1 >= LevelBgBuilder.THIS.maxCol || (LevelBgBuilder.THIS.GetItemSquare(item.x, item.y+1)._isFull && !IsSelf(item.x, item.y+1)))
                {
                    return;
                }
            }
            for (int i = 0; i < _itemSquares.Count; i++)
            {
                LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(false);
                _itemSquares[i].y++;
            }
            for (int i = 0; i < _itemSquares.Count; i++)
            {
                LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
            }  
            AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemMove);
        }

       public bool isVerticaling;   
       public bool isHorizontalling;   
        public void MoveDown()
        {
            isVerticaling = true;
            if (!GameLunch.THIS.isFalling || GameLunch.THIS.isEliminateing || isDown)
            {
                return;
            }

            if (_type == Shape.X_Shape)
            {
                if(LevelBgBuilder.THIS.isDownItemClo(_itemSquares[0].y,_itemSquares[0].x))
                {
                    isDown = true;
                    AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemFall);
                }
            }
            else
            {
                for (int i = 0; i < _itemSquares.Count; i++)
                {
                    if(_itemSquares[i].x-1==-1
                       ||(LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x-1, _itemSquares[i].y)._isFull && !IsSelf(_itemSquares[i].x-1, _itemSquares[i].y))
                      )
                    {
                        isDown = true;
                        AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemFall);
                        break;
                    }
                    
                }
            }
           

            if (isDown)
            {
                isVerticaling = false;
                isHorizontalling = false;
                StopCoroutine(fallingIEtor);
                GameLunch.THIS.isFalling = false;
                AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemFall);
                CheckEliminate(); 
                return;
            }

            if (_type == Shape.X_Shape)
            {
                for (int i = 0; i < _itemSquares.Count; i++)
                {
                       
                    _itemSquares[i].x--;
                }
                for (int i = 0; i < _itemSquares.Count; i++)
                {
                    LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x+1, _itemSquares[i].y).SetImageFull(lastisFull);
                    lastisFull = LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y)._isFull;
                    LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
                } 
            }
            else
            {
                for (int i = 0; i < _itemSquares.Count; i++)
                {
                    LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(false);
                    _itemSquares[i].x--;
                }
                for (int i = 0; i < _itemSquares.Count; i++)
                {
                    LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
                }  
                // AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemMove); 
            }
        }

        public void MoveDownEnd()
        {
            isVerticaling = false;
            isHorizontalling = false;
        }
        bool isDown = false;
        /// <summary>
        /// 普通元素的下落
        /// </summary>
        /// <returns></returns>
        IEnumerator aaaFalling()
        {
            
            isDown = false;
            while (!isDown)
            {
                if (!isVerticaling)
                {
                    for (int i = 0; i < _itemSquares.Count; i++)
                    {
                        if(_itemSquares[i].x-1==-1
                           ||(LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x-1, _itemSquares[i].y)._isFull && !IsSelf(_itemSquares[i].x-1, _itemSquares[i].y))
                          )
                        {
                            isDown = true;
                      
                            break;
                        }
                    
                    }
                    if (!isDown)
                    {
                        for (int i = 0; i < _itemSquares.Count; i++)
                        {
                            LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(false);
                            _itemSquares[i].x--;
                        }
                        for (int i = 0; i < _itemSquares.Count; i++)
                        {
                            LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
                        }  
                    
                    
                    
                    
                        for (int i = 0; i < _itemSquares.Count; i++)
                        {
                            if(_itemSquares[i].x-1==-1
                               ||(LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x-1, _itemSquares[i].y)._isFull && !IsSelf(_itemSquares[i].x-1, _itemSquares[i].y))
                              )
                            {
                                AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemFall);
                                break;
                            }
                    
                        }
                    
                    
                       
                    }
                    
                }
                yield return new WaitForSeconds(ScoreAndTime.THIS.FallSpeed);
            }
            GameLunch.THIS.isFalling = false;
            CheckEliminate(); 
        }

        /// <summary>
        /// 星星元素的下落
        /// </summary>
        /// <returns></returns>
        IEnumerator bbbFalling()
        {
            isDown = false;
            lastisFull = false;
            int donwCount = 0;
            
            while (!isDown)
            {
                if (!isVerticaling)
                {
                    if(LevelBgBuilder.THIS.isDownItemClo(_itemSquares[0].y,_itemSquares[0].x))
                    {
                        isDown = true;
                    }
                    if (!isDown)
                    {
                        for (int i = 0; i < _itemSquares.Count; i++)
                        {
                       
                            _itemSquares[i].x--;
                        }
                        for (int i = 0; i < _itemSquares.Count; i++)
                        {
                            LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x+1, _itemSquares[i].y).SetImageFull(lastisFull);
                            lastisFull = LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y)._isFull;
                            LevelBgBuilder.THIS.GetItemSquare(_itemSquares[i].x, _itemSquares[i].y).SetImageFull(true);
                        } 
                    
                        if(LevelBgBuilder.THIS.isDownItemClo(_itemSquares[0].y,_itemSquares[0].x))
                        {
                            AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemFall);
                        }
                   
                    }
                }
                
                yield return new WaitForSeconds(ScoreAndTime.THIS.FallSpeed);
               
            }
            GameLunch.THIS.isFalling = false;
           
            CheckEliminate();
        }

        /// <summary>
        /// 检查是否达成消除
        /// </summary>
        private void CheckEliminate()
        {
            List<int> wantEliminateRow = new List<int>();
            for (int i = 0; i < LevelBgBuilder.THIS.maxRow; i++)
            {
                for(int j=0;j<LevelBgBuilder.THIS.maxCol;j++)
                {
                    if (!LevelBgBuilder.THIS.GetItemSquare(i, j)._isFull)
                    {
                        break;
                    }
                    if (j == LevelBgBuilder.THIS.maxCol - 1)
                    {
                        wantEliminateRow.Add(i);
                    }
                }
            }

            if (wantEliminateRow.Count == 0)
            {
                if (LevelBgBuilder.THIS.isHasItemRow(LevelBgBuilder.THIS.maxRow - 2))
                {
                    GameLunch.Instance.GameOver();
                }
              
            }
            else
            {
                StartCoroutine(EliminateRow(wantEliminateRow));
            }
        }

        /// <summary>
        /// 消除行
        /// </summary>
        /// <param name="wantEliminateRow"></param>
        /// <returns></returns>
       IEnumerator EliminateRow(List<int> wantEliminateRow)
        {

            if (wantEliminateRow.Count > 0)
            {
               
                GameLunch.THIS.isEliminateing = true;
                for (int z = wantEliminateRow.Count-1; z >=0; z--)
                {
                    AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemEliminate);
                    for (int i = 0; i < LevelBgBuilder.THIS.maxCol; i++)
                    {
                        LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).SetImageFull(false);
                        LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).showBoom();
                       
                    } 
                    yield return new WaitForSeconds(0.4f);

                    for (int x = wantEliminateRow[z]; x < LevelBgBuilder.THIS.maxRow-1; x++)
                    {
                        for (int j = 0; j < LevelBgBuilder.THIS.maxCol; j++)
                        {
                        
                            LevelBgBuilder.THIS.GetItemSquare(x, j).SetImageFull((z+1 >=LevelBgBuilder.THIS.maxRow)?false:LevelBgBuilder.THIS.GetItemSquare(x+1, j)._isFull);
                        }

                    }
                   
                    AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemPostpone);
                    yield return new WaitForSeconds(0.4f);
                    /*if (z % 2 == 0)
                    {
                        for (int i = 0; i < LevelBgBuilder.THIS.maxCol; i++)
                        {
                            LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).SetImageFull(false);
                            LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).showBoom();
                            yield return new WaitForSeconds(0.01f);
                        } 
                    }
                    else
                    {
                        for (int i = LevelBgBuilder.THIS.maxCol-1; i >=0; i--)
                        {
                            LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).SetImageFull(false);
                            LevelBgBuilder.THIS.GetItemSquare(wantEliminateRow[z], i).showBoom();
                            yield return new WaitForSeconds(0.01f);
                        }  
                    }*/
                    
                }
            
            
                
                ScoreAndTime.THIS.ScroeUp(wantEliminateRow.Count);
                GameLunch.THIS.isEliminateing = false;
                
            }
        }

        private bool IsSelf(int row,int col)
        {
            bool isSelf = false;
            for (int i = 0; i < _itemSquares.Count; i++)
            {
              if(_itemSquares[i].x == row && _itemSquares[i].y == col)
              {
                  isSelf = true;
                  break;
              }
            }

            return isSelf;
        }
        
    }

}
