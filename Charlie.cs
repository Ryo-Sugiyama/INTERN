using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ateam
{
    public class Charlie : BaseBattleAISystem
    {
        /* �ϐ��錾�Ə����� */
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
            //�@�X�e�[�W�f�[�^�̎擾
            StageData = GetStageData();

            //�@�e�A�N�^�[�̃I�u�W�F�N�g�i�[�@
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);


            //�@ID�̊i�[
            for (int i = 0; i < 3; i++)
            {
                playerId[i] = playerList[i].ActorId;
                enemyId[i] = enemyList[i].ActorId;
            }

            //�@�^�[�Q�b�g�ɂȂ�G��ID���i�[
            ///�@:TODO�@�o�u���\�[�g���g�p
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
            //�@�G�f�[�^�̍X�V
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);

            for (int i = 0; i < 3; i++)
            {
                if (enemyList[targetEnemyId].BlockPos.y == playerList[i].BlockPos.y)
                {
                    //�@�^�[�Q�b�g�̓G���E�ɂ���
                    if (enemyList[targetEnemyId].BlockPos.x > playerList[i].BlockPos.x)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                        Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                    //�@�^�[�Q�b�g�̓G�����ɂ���
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
            //�@�����_���ړ�
            /*
            for (int i = 0; i < 3; i++)
            {
                int move = UnityEngine.Random.Range(0, 4);
                switch (move)
                {
                    case 0:
                        //��ړ�
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                        break;

                    case 1:
                        //���ړ�
                        Move(playerId[i], Common.MOVE_TYPE.DOWN);
                        break;

                    case 2:
                        //���ړ�
                        Move(playerId[i], Common.MOVE_TYPE.LEFT);
                        break;

                    case 3:
                        //�E�ړ�
                        Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                        break;
                }
            }
            */
            //�@�������U���ɌŒ�
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