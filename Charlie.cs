using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ateam
{
    public class Charlie : BaseBattleAISystem
    {
        /* 変数宣言と初期化 */
        int[,] stageData;
        int[] playerId = new int[3];
        int[] enemyId = new int[3];
        int targetEnemyId;

        List<CharacterModel.Data> playerList;
        List<CharacterModel.Data> enemyList;

        //---------------------------------------------------
        // InitializeAI
        //---------------------------------------------------
        override public void InitializeAI()
        {
            //　ステージデータの取得
            stageData = GetStageData();

            //　各アクターのオブジェクト格納　
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);


            //　IDの格納
            for (int i = 0; i < 3; i++)
            {
                playerId[i] = playerList[i].ActorId;
                enemyId[i] = enemyList[i].ActorId;
            }

            //　ターゲットになる敵のIDを格納
            ///　:TODO　バブルソートを使用
            if (enemyList[0].Hp < enemyList[1].Hp)
            {
                targetEnemyId = 0;
            }
            if (enemyList[0].Hp > enemyList[2].Hp)
            {
                targetEnemyId = 2;
            }


        }

        //---------------------------------------------------
        // UpdateAI
        //---------------------------------------------------
        override public void UpdateAI()
        {
            //　データの更新
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);


            Moving();

            Shoot();

        }

        //---------------------------------------------------
        // ItemSpawnCallback
        //---------------------------------------------------
        override public void ItemSpawnCallback(ItemSpawnData itemData)
        {
        }

        public void Moving()
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(SameColumn(playerList[i], enemyList[targetEnemyId]));

                if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.RIGHT)
                {
                    if (GetStageDataType(playerId[i], Common.MOVE_TYPE.RIGHT))
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                    else if (GetStageDataType(playerId[i], Common.MOVE_TYPE.UP))
                    {
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                    }
                }

                else if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.LEFT)
                {
                    Move(playerId[i], Common.MOVE_TYPE.LEFT);

                }
                else
                {
                    if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.UP)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                    }
                    else if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.DOWN)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.DOWN);
                    }
                }

                if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.UP)
                {
                    Move(playerId[i], Common.MOVE_TYPE.UP);
                }

                else if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.DOWN)
                {
                    Move(playerId[i], Common.MOVE_TYPE.DOWN);
                }
                else
                {
                    if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.RIGHT)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                    }
                    else if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.LEFT)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.LEFT);
                    }
                }
            }
        }

        public void Shoot()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Distance(playerList[i], enemyList[targetEnemyId]) <= 1)
                {
                    Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_SHORT);
                }
                if (Distance(playerList[i], enemyList[targetEnemyId]) < 3)
                {
                    Action(playerId[i], Define.Battle.ACTION_TYPE.INVINCIBLE);
                }

                Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);

            }
        }

        public enum ENEMY_POS
        {
            RIGHT,  // 敵は自身の右方向
            LEFT,   // 敵は自身の左方向
            UP,     // 敵は自身の上方向 (画面向かって上方向)
            DOWN,   // 敵は自身の下方向 (画面向かって下方向)
            SAME    // 敵と同じ行/列にいる
        };

        // 自分と敵のIDから同じ行にいるかを確認する
        // 戻り値：ENEMY_POS
        // 
        public ENEMY_POS SameColumn(CharacterModel.Data playerId, CharacterModel.Data enemyId)
        {

            // 自身と敵は同じ行にいる
            if (enemyId.BlockPos.x == playerId.BlockPos.x) { return ENEMY_POS.SAME; }

            // 自身と敵は右側にいる
            if (playerId.BlockPos.x < enemyId.BlockPos.x) { return ENEMY_POS.RIGHT; }

            // それ以外なので、左側にいる
            return ENEMY_POS.LEFT;
        }

        // 自分と敵のIDから同じ行にいるかを確認する
        // 戻り値：ENEMY_POS
        // 
        public ENEMY_POS SameRow(CharacterModel.Data playerId, CharacterModel.Data enemyId)
        {

            // 自身と敵は同じ列にいる
            if (enemyId.BlockPos.y == playerId.BlockPos.y) { return ENEMY_POS.SAME; }

            // 敵は自身より上方向にいる
            if (playerId.BlockPos.y < enemyId.BlockPos.y) { return ENEMY_POS.UP; }

            // それ以外なので、敵は下方向にいる
            return ENEMY_POS.DOWN;
        }

        // 自分と敵の距離を取得する
        // 戻り値：距離 (float)
        public float Distance(CharacterModel.Data playerId, CharacterModel.Data enemyId)
        {

            float sx = playerId.BlockPos.x - enemyId.BlockPos.x;
            float sy = playerId.BlockPos.y - enemyId.BlockPos.y;
            return Mathf.Sqrt(sx * sx + sy * sy);
        }

        public bool GetStageDataType(int _playerId, Common.MOVE_TYPE pos)
        {
            CharacterModel.Data playerId = GetCharacterData(_playerId);
            switch (pos)
            {
                case Common.MOVE_TYPE.UP:
                    if (stageData[(int)playerId.BlockPos.y - 1, (int)playerId.BlockPos.x] == (int)Define.Stage.BLOCK_TYPE.OBSTACLE)
                        return false;
                    break;

                case Common.MOVE_TYPE.DOWN:
                    if (stageData[(int)playerId.BlockPos.y + 1, (int)playerId.BlockPos.x] == (int)Define.Stage.BLOCK_TYPE.OBSTACLE)
                        return false;
                    break;

                case Common.MOVE_TYPE.LEFT:
                    if (stageData[(int)playerId.BlockPos.y, (int)playerId.BlockPos.x - 1] == (int)Define.Stage.BLOCK_TYPE.OBSTACLE)
                        return false;
                    break;

                case Common.MOVE_TYPE.RIGHT:
                    if (stageData[(int)playerId.BlockPos.y, (int)playerId.BlockPos.x + 1] == (int)Define.Stage.BLOCK_TYPE.OBSTACLE)
                        return false;
                    break;
            }
            return true;
        }
    }
}