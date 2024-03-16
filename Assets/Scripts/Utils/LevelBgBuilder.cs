using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXS.Config;
using ZXS.Game;

namespace ZXS.Utils
{
    public class LevelBgBuilder : MonoBehaviour
    {
        
        public static LevelBgBuilder THIS;
        
        public Transform bgParent;
        public int maxCol;
        public int maxRow;

        private List<ItemSquare> _squares = new List<ItemSquare>();


        public void Awake()
        {
            THIS = this;
        }

        private void Start()
        {
            LevelBgBuild();
        }

        /// <summary>
        /// 生成背景格子
        /// </summary>
        public void LevelBgBuild()
        {
            GameObject bgPrefab = AssetsLoadManager.Instance.LoadAsset<GameObject>(ResConfig.itemBg);
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    var item = Instantiate(bgPrefab, bgParent);
                    ItemSquare itemSquare = item.GetComponent<ItemSquare>();
                    
                    _squares.Add(itemSquare);
                    Color _color = Color.white;
                    item.GetComponent<RectTransform>().anchoredPosition = new Vector2(-475+(j * 50), -975+(i * 50));
                    /*if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            _color = new Color(0.5f,0.5f,0.5f,0.2f); 
                        }
                        else
                        {
                            _color = new Color(1f,1f,1f,0.2f); 
                        }
                    }
                    else
                    {*/
                        if (j % 2 == 0)
                        {
                            _color = new Color(0.8f,0.8f,0.8f,0.2f); 
                        }
                        else
                        {
                            _color = new Color(0.5f,0.5f,0.5f,0.2f); 
                        } 
                    // }
                    itemSquare.SetRowCol(i,j,_color);
                }
            }
        }

       public ItemSquare GetItemSquare(int row, int col)
       {
            foreach (var itemSquare in _squares)
            {
                if (itemSquare.row == row && itemSquare.col == col)
                {
                    return itemSquare;
                }
            }

            return null;
        }
       
       
       
       private Vector2 fingerDownPosition;
       private Vector2 fingerUpPosition;

       [SerializeField]
       private bool detectSwipeOnlyAfterRelease = false;

       [SerializeField]
       private float minDistanceForSwipe = 20f;

       private bool isMoved;

       private void Update()
       {
           if (GameLunch.THIS.isFalling && !GameLunch.THIS.isGameOver)
           {
#if UNITY_EDITOR
               if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) )
               {
                   GameLunch.THIS.fallingItem.MoveLeft();
               }else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
               {
                   GameLunch.THIS.fallingItem.MoveRight(); 
               }else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
               {
                   GameLunch.THIS.fallingItem.MoveDown();
               }else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
               {
                   if (GameLunch.Instance.isFalling)
                   {
                       GameLunch.THIS.fallingItem.ChangeShape();
                   }
                   
               } else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
               {
                   GameLunch.THIS.fallingItem.MoveDownEnd();
               }    
#endif

#if UNITY_ANDROID
               
               if (Input.GetMouseButtonUp(0) && !isMoved)
               {
                   if (GameLunch.Instance.isFalling)
                   {
                       GameLunch.THIS.fallingItem.ChangeShape();
                   }
               }else if(Input.GetMouseButtonUp(0) && isHandDowning){
                    GameLunch.THIS.fallingItem.MoveDownEnd();
                }
               else
               {
                   foreach (Touch touch in Input.touches)
                   {
                       if (touch.phase == TouchPhase.Began)
                       {
                           fingerUpPosition = touch.position;
                           fingerDownPosition = touch.position;
                           isMoved = false;
                       }

                       if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                       {
                           fingerDownPosition = touch.position;
                           DetectSwipe();
                       }

                       if (touch.phase == TouchPhase.Ended)
                       {
                           fingerDownPosition = touch.position;
                           DetectSwipe();
                       }
                       
                   }    
               }
               
               
#endif      
              

           }
       }

       public bool isHasItemRow(int row)
       {
           bool isHas = false;
           if (row >= maxRow)
           {
               return false;
           }
           
           _squares.FindAll(item => item.row == row).ForEach((curr) =>
           {
               if (curr._isFull)
               {
                   isHas= true; 
               }
           });
           
          
           return isHas;
       }
       /// <summary>
       /// 是否到列底
       /// </summary>
       /// <param name="clo">目标列</param>
       /// <param name="row">当前行</param>
       /// <returns></returns>
       public bool isDownItemClo(int clo,int _row)
       {
           bool isDown = true;
           if (clo < 0 || clo >= maxCol)
           {
               return true;
           }
        
           
           _squares.FindAll(item => item.col == clo && item.row < _row).ForEach((curr) =>
           {
               if (!curr._isFull)
               {
                   isDown= false; 
               }
           });
           
          
           return isDown;
       }
       
       /// <summary>
       /// 获取一个形状的变形的下一个形状
       /// </summary>
       /// <param name="_shape"></param>
       /// <returns></returns>
       public Shape GetNextShape(Shape _shape)
       {
          switch (_shape)
            {
                case Shape.I_Shape:
                    return Shape.I_Shape_1;
                    break;
                case Shape.I_Shape_1:
                    return Shape.I_Shape;
                    break;
                
                case Shape.J_Shape:
                    return Shape.J_Shape_1;
                    break;
                case Shape.J_Shape_1:
                    return Shape.J_Shape_2;
                    break;
                case Shape.J_Shape_2:
                    return Shape.J_Shape_3;
                    break;
                case Shape.J_Shape_3:
                    return Shape.J_Shape;
                    break;
                
                
                
                case Shape.L_Shape:
                    return Shape.L_Shape_1;
                    break;
                case Shape.L_Shape_1:
                    return Shape.L_Shape_2;
                    break;
                case Shape.L_Shape_2:
                    return Shape.L_Shape_3;
                    break;
                case Shape.L_Shape_3:
                    return Shape.L_Shape;
                    break;
                case Shape.O_Shpae:
                    return Shape.O_Shpae;
                    break;
                case Shape.S_Shape:
                    return Shape.S_Shape_1;
                    break;
                case Shape.S_Shape_1:
                    return Shape.S_Shape;
                    break;
                case Shape.Z_Shape:
                    return Shape.Z_Shape_1;
                case Shape.Z_Shape_1:
                    return Shape.Z_Shape;
                    break;
                case Shape.T_Shape:
                    return Shape.T_Shape_1;
                    break;
                case Shape.T_Shape_1:
                    return Shape.T_Shape_2;
                    break;
                case Shape.T_Shape_2:
                    return Shape.T_Shape_3;
                    break;
                case Shape.T_Shape_3:
                    return Shape.T_Shape;
                    break;
                
                case Shape.M_Shape:
                    return Shape.M_Shape_1;
                    break;
                case Shape.M_Shape_1:
                    return Shape.M_Shape_2;
                    break;
                case  Shape.M_Shape_2:
                    return Shape.M_Shape_3;
                    break;
                case Shape.M_Shape_3:
                    return Shape.M_Shape;
                    break;
                
                case Shape.X_Shape:
                    return Shape.X_Shape;
                    break;
                case Shape.B_Shape:
                    return Shape.B_Shape;
                    break;
            }

          return _shape;
       }

       /// <summary>
       /// 获取一个形状的数据
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       public int[,] getAShapeItem(Shape type)
        {
            switch (type)
            {
                case Shape.I_Shape:
                    return new int[,] { { 1, 1, 1, 1 } };
                    break;
                case Shape.I_Shape_1:
                    return new int[,] { { 1 }, { 1 }, { 1 }, { 1 } };
                    break;
                
                case Shape.J_Shape:
                    return new int[,] { { 1, 1, 1 }, { 0, 0, 1 } };
                    break;
                case Shape.J_Shape_1:
                    return new int[,] { { 0, 0, 1 }, { 0, 0, 1 }, { 0, 1, 1 } };
                    break;
                case Shape.J_Shape_2:
                    return new int[,] { { 1, 0, 0 },  { 1, 1, 1 } };
                    break;
                case Shape.J_Shape_3:
                    return new int[,] { { 1, 1, 0 }, { 1, 0, 0 }, { 1, 0, 0 } };
                    break;
                
                
                
                case Shape.L_Shape:
                    return new int[,] { { 1, 1, 1 }, { 1, 0, 0 } };
                    break;
                case Shape.L_Shape_1:
                    return new int[,] { { 1, 1 }, { 0, 1 }, { 0, 1 } };
                    break;
                case Shape.L_Shape_2:
                    return new int[,] { { 0, 0, 1 }, { 1,1, 1 } };
                    break;
                case Shape.L_Shape_3:
                    return new int[,] { { 1, 0 }, { 1, 0 },{ 1, 1 } };
                    break;
                case Shape.O_Shpae:
                    return new int[,] { { 1, 1 }, { 1, 1 } };
                    break;
                case Shape.S_Shape:
                    return new int[,] { { 0, 1, 1 }, { 1, 1, 0 } };
                    break;
                case Shape.S_Shape_1:
                    return new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 } };
                    break;
                case Shape.Z_Shape:
                    return new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
                case Shape.Z_Shape_1:
                    return new int[,] { { 0, 1 }, {  1, 1 }, {  1, 0 } };
                    break;
                case Shape.T_Shape:
                    return new int[,] { { 1, 1, 1 }, { 0, 1, 0 } };
                    break;
                case Shape.T_Shape_1:
                    return new int[,] { { 0, 1 }, { 1, 1 },{ 0, 1 } };
                    break;
                case Shape.T_Shape_2:
                    return new int[,] { { 0, 1, 0 }, { 1, 1, 1 } };
                    break;
                case Shape.T_Shape_3:
                    return new int[,] { { 1, 0 }, { 1, 1 }, { 1, 0 }  };
                    break;
                
                case Shape.M_Shape:
                    return new int[,] { { 0,1, 0 }, { 1, 1,1 }, { 1, 0,1 }  };
                    break;
                case Shape.M_Shape_1:
                    return new int[,] { { 1,1, 0 }, { 0, 1,1 }, { 1, 1,0 }  };
                    break;
                case Shape.M_Shape_2:
                    return new int[,] { { 1,0, 1 }, { 1, 1,1 }, { 0, 1,0 }  };
                    break;
                case Shape.M_Shape_3:
                    return new int[,] { { 0,1, 1 }, { 1, 1,0 }, { 0, 1,1 }  };
                    break;
                case Shape.X_Shape:
                    return new int[,] { { 1 }  };
                    break;
                case Shape.B_Shape:
                    return new int[,] { { 0,1, 0 },{ 1,1, 1 },{ 0,1, 0 }  };
                    break;
            }

            return new int[,]{};
        }
        
       /// <summary>
       /// Android 拖动控制方向
       /// </summary>
       private void DetectSwipe()
       {
           if (SwipeDistanceCheckMet())
           {
               isMoved = true;
               if (IsVerticalSwipe())
               {
                   var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? "Up" : "Down";
                   Debug.Log("Vertical Swipe Detected. Direction: " + direction);
                   if ("Down".Equals(direction))
                   {
                       GameLunch.THIS.fallingItem.MoveDown();
                   }
               }
               else
               {
                   var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? "Right" : "Left";
                   Debug.Log("Horizontal Swipe Detected. Direction: " + direction);

                   if ("Right".Equals(direction))
                   {
                       GameLunch.THIS.fallingItem.MoveRight();
                   }
                   else if ("Left".Equals(direction))
                   {
                         GameLunch.THIS.fallingItem.MoveLeft();
                   }
                   
               }
               fingerUpPosition = fingerDownPosition;
           }
       }
       
       
       private bool SwipeDistanceCheckMet()
       {
           return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
       }

       private bool IsVerticalSwipe()
       {
           return VerticalMovementDistance() > HorizontalMovementDistance();
       }

       private float VerticalMovementDistance()
       {
           return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
       }

       private float HorizontalMovementDistance()
       {
           return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
       }
    } 
}

