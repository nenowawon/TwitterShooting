﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private PlayerStateManager playerStateManager;
    private PlayerAnimationManager playerAnimationManager;

    private int skillNumber = 0;
    /// <summary>
    /// スキル番号
    /// </summary>
    public int SkillNumber
    {
        get
        {
            return skillNumber;
        }
        private set
        {
            skillNumber = value;
        }
    }

    [SerializeField]
    private PlayerSkillBase[] skillList = new PlayerSkillBase[PlayerParameter.SKILL_QUANTITY];
    public PlayerSkillBase GetSelectSkill()
    {
        return skillList[SkillNumber];
    }

    /// <summary>
    /// 攻撃中か
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 入力中か
    /// </summary>
    private bool isInput = false;

    /// <summary>
    /// 攻撃角度
    /// </summary>
    private Quaternion attackQuaternion;
    /// <summary>
    /// 攻撃角度に向く速度
    /// </summary>
    const float FACE_SPEED = 1200f;


    void Awake()
    {
        // コンポーネントを取得
        playerStateManager = GetComponent<PlayerStateManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
    }

    public void UpdateSkill(float inputActivateSkill, bool inputSelectSkill1, bool inputSelectSkill2, bool inputSelectSkill3)
    {
        // スキル切り替え
        ChangeSkill(
            inputSelectSkill1,
            inputSelectSkill2,
            inputSelectSkill3);

        if (isAttack)
        {
            // 攻撃方向に向く
            FaceAttack(attackQuaternion);
        }

        // 現在のプレイヤーの状態が行動可能ではなかったら、この先の処理を行わない
        if (playerStateManager.GetPlayerState() != PlayerStateManager.PlayerState.ACTABLE) { return; }

        if (inputActivateSkill >= 1)
        {
            if (isInput) { return; }

            PlayerSkillBase skill = skillList[SkillNumber];

            // スキルがあったら
            if (skill != null)
            {
                // プレイヤーの状態をスキル中に変更
                playerStateManager.SetPlayerState(PlayerStateManager.PlayerState.SKILL);

                Vector3 attackDirection = Vector3.Scale(
                    Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                attackQuaternion = Quaternion.LookRotation(attackDirection);
                isAttack = true;

                // スキル発動アニメーション再生
                playerAnimationManager.ChangeSkillClip(skill.SkillAnimation);
                playerAnimationManager.SetTriggerSkill();

                // スキル生成
                StartCoroutine(CreateSkill(skill.SkillCreationTime, skill));

                // スキル硬直解除
                StartCoroutine(RecoverySkill(skill.SkillRecoveryTime));
            }
            else
            {
                Debug.LogWarning("skillList["+ skillNumber + "] = null");
            }

            isInput = true;
        }
        else
        {
            isInput = false;
        }
    }

    /// <summary>
    /// 攻撃方向に向く
    /// </summary>
    /// <param name="attackQuaternion">攻撃角度</param>
    void FaceAttack(Quaternion attackQuaternion)
    {
        // 攻撃角度に向いていたら、この先の処理を行わない
        if (transform.rotation == attackQuaternion) { return; }

        // 攻撃角度に徐々に向く
        float step = FACE_SPEED * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, attackQuaternion, step);
    }

    /// <summary>
    /// スキル生成
    /// </summary>
    /// <param name="skillCreationTime">スキル生成時間</param>
    /// <param name="skill"></param>
    /// <returns></returns>
    private IEnumerator CreateSkill(float skillCreationTime, PlayerSkillBase skill)
    {
        yield return new WaitForSeconds(skillCreationTime);

        skill.ActivateSkill(transform, skill.SkillCreationPos);
    }

    /// <summary>
    /// スキル硬直解除
    /// </summary>
    /// <param name="recoveryTime">硬直解除時間</param>
    /// <returns></returns>
    private IEnumerator RecoverySkill(float recoveryTime)
    {
        yield return new WaitForSeconds(recoveryTime);

        isAttack = false;

        // プレイヤーの状態を行動可能に変更
        playerStateManager.SetPlayerState(PlayerStateManager.PlayerState.ACTABLE);
    }

    /// <summary>
    /// スキル切り替え
    /// </summary>
    /// <param name="inputSelectNormalAttack">通常攻撃切り替え入力</param>
    /// <param name="inputSelectSkill1">スキル1切り替え入力</param>
    /// <param name="inputSelectSkill2">スキル2切り替え入力</param>
    /// <param name="inputSelectSkill3">スキル3切り替え入力</param>
    void ChangeSkill(bool inputSelectSkill1, bool inputSelectSkill2, bool inputSelectSkill3)
    {
        if (inputSelectSkill1) { SkillNumber = 0; }
        if (inputSelectSkill2) { SkillNumber = 1; }
        if (inputSelectSkill3) { SkillNumber = 2; }
    }
}