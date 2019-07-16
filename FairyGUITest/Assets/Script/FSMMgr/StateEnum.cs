using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态ID
/// </summary>
public enum StateID {
    STATE_NULL  =   0,
    STATE_WALK  =   1,
    STATE_JUMP  =   2,

    ////////// enemy巡逻测试相关
    STATE_CRUISE=   103, //巡逻
    STATE_CHASE =   104, //追击
    STATE_BACK  =   105, //返回原位
    /////////////////////
    /////////  玩家控制相关
    STATE_PLAYER_IDLE   = 201,    //站立状态
    STATE_PLAYER_RUN    = 202,
    STATE_PLAYER_ATTACK1 = 203,    //攻击1
    STATE_PLAYER_ATTACK2 = 204,    //攻击2
    STATE_PLAYER_ATTACK3 = 205,    //攻击3

    //////////////////////
    /////////   新IK玩家控制
    NEW_PLAYER_IDLE     =   301,
    NEW_PLAYER_WALK     =   302,    //行走、跑步动作，包含了左右转向
    NEW_PLAYER_JUMP     =   303,    //跳跃
    NEW_PLAYER_SLIDE    =   304,    //滑行
    NEW_PLAYER_PICK     =   305,    //捡东西

}

/// <summary>
/// 转化条件ID
/// </summary>
public enum TransConditionID
{
    CONDITION_NULL      =   0,
    C_KEY_UP              =   1,  //键盘上键按下
    C_KEY_DOWN            =   2,  //键盘下键按下
    C_KEY_LEFT            =   3,  //键盘上键按下
    C_KEY_RIGHT           =   4,  //键盘下键按下

    ////////// enemy巡逻测试相关
    C_CRUISE_T_CHASE      =   103, //巡逻变成追击
    C_CHASE_T_CRUISE      =   104, //追击编程巡逻
    C_BACK_T_INIT         =   105, //返回原位
    C_INIT_TO_CRUISE      = 106,    //返回原位编程巡逻
    C_BACK_T_CHASE        = 107,    //返回原位变成追击
    /////////////////////
    /////////  玩家控制相关
    C_PLAYER_IDLE       =   201,    //站立状态
    C_PLAYER_RUN        =   202,    
    C_PLAYER_ATTACK1    =   203,    //攻击1
    C_PLAYER_ATTACK2    =   204,    //攻击2
    C_PLAYER_ATTACK3    =   205,    //攻击3
    ////////////////////
    ////////    新玩家控制相关
    NEW_PLAYER_IDLE     =   301,
    NEW_PLAYER_WALK     =   302,
    NEW_PLAYER_JUMP     =   303,
    NEW_PLAYER_SLIDE    =   304,
    NEW_PLAYER_PICK     =   305,

}