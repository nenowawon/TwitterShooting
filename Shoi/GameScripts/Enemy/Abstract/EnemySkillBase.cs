﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// 敵のスキルの基底クラス
/// </summary>
[System.Serializable]
public abstract class EnemySkillBase : ScriptableObject
{

    [Inject]
    protected MainGameManager gameManager;

    public MainGameManager setGameManager
    {
        set { gameManager = value; }
    }

    [Header("スキル名")]
    [SerializeField]
    private string skillName;
    /// <summary>
    /// スキル名
    /// </summary>
    public string getSkillName
    {
        get { return skillName; }
    }


    [Header("攻撃力")]
    [SerializeField]
    private int skillAtackPower;
    /// <summary>
    /// スキル発動アニメーション
    /// </summary>
    public int getAtackPower
    {
        get { return skillAtackPower; }
    }

    [Header("スキル詠唱時間")]
    [SerializeField]
    private int skillChantFrame;
    /// <summary>
    /// スキル詠唱時間
    /// </summary>
    public int getSkillChantFrame
    {
        get { return skillChantFrame; }
    }

    [Header("スキルアニメーション")]
    [SerializeField]
    private AnimationClip skillAnimation;
    /// <summary>
    /// スキル発動アニメーション
    /// </summary>
    public AnimationClip getSkillAnimation
    {
        get { return skillAnimation; }
    }


    [Header("スキルアニメーション速度")]
    [SerializeField]
    private float skillAnimationSpeed;
    /// <summary>
    /// スキル発動アニメーション速度
    /// </summary>
    public float getSkillAnimationSpeed
    {
        get { return skillAnimationSpeed; }
    }

    [Header("スキルプレハブ")]
    /// <summary>
    /// スキルプレハブ
    /// </summary>
    [SerializeField]
    protected AttackCollision skillPrefab;

    [Header("スキル硬直解除時間")]
    [SerializeField]
    private float skillRecoveryTime;
    /// <summary>
    /// スキル硬直解除時間
    /// </summary>
    public float getSkillRecoveryTime
    {
        get { return skillRecoveryTime; }
    }

    /// <summary>
    /// スキル発動処理
    /// </summary>
    public abstract void ActivateSkill(Transform thisTransform);


    protected GameObject hitEffect;
    public void SetHitEffect(GameObject effect)
    {
        hitEffect = effect;
    }
}

