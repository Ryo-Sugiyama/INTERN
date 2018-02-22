using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ateam
{
    public class Charlie : BaseBattleAISystem
    {
        /* 変数宣言と初期化 */
        int[,] StageData;
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
            StageData = GetStageData();

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
            //　敵データの更新
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);

            for (int i = 0; i < 3; i++)
            {
                if (enemyList[targetEnemyId].BlockPos.y == playerList[i].BlockPos.y)
                {
                    //　ターゲットの敵が右にいる
                    if (enemyList[targetEnemyId].BlockPos.x > playerList[i].BlockPos.x)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                        Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                    //　ターゲットの敵が左にいる
                    if (enemyList[targetEnemyId].BlockPos.x < playerList[i].BlockPos.x)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.LEFT);
                        Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                    else
                    {
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                        Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                }
            }
            //　ランダム移動
            /*
            for (int i = 0; i < 3; i++)
            {
                int move = UnityEngine.Random.Range(0, 4);
                switch (move)
                {
                    case 0:
                        //上移動
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                        break;

                    case 1:
                        //下移動
                        Move(playerId[i], Common.MOVE_TYPE.DOWN);
                        break;

                    case 2:
                        //左移動
                        Move(playerId[i], Common.MOVE_TYPE.LEFT);
                        break;

                    case 3:
                        //右移動
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                        break;
                }
            }
            */
            //　遠距離攻撃に固定
            for (int i = 0; i < 3; i++)
            {
                Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);
            }
        }

        //---------------------------------------------------
        // ItemSpawnCallback
        //---------------------------------------------------
        override public void ItemSpawnCallback(ItemSpawnData itemData)
        {
        }
    }
}