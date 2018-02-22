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
            //�@�f�[�^�̍X�V
            enemyList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);
            playerList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);


            for (int i = 0; i < 3; i++)
            {
                Debug.Log(SameColumn(playerList[i], enemyList[targetEnemyId]));

                if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.RIGHT)
                {
                    Move(playerId[i], Common.MOVE_TYPE.RIGHT);
                    Debug.Log("RIGHT");
                }

                else if (SameColumn(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.LEFT)
                {
                    Move(playerId[i], Common.MOVE_TYPE.LEFT);
                    Debug.Log("LEFT");
                }
                else
                {
                    if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.UP)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.UP);
                        Debug.Log("UP");
                    }
                    else if (SameRow(playerList[i], enemyList[targetEnemyId]) == ENEMY_POS.DOWN)
                    {
                        Move(playerId[i], Common.MOVE_TYPE.DOWN);
                        Debug.Log("DOWN");
                    }
                    else
                    {
                        Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_SHORT);
                    }
                }

            }

            //�@�������U���ɌŒ�
            for (int i = 0; i < 3; i++)
            {
                Action(playerId[i], Define.Battle.ACTION_TYPE.ATTACK_MIDDLE);
            }
        }

        //---------------------------------------------------
        // ItemSpawnCallback
        //---------------------------------------------------
        override public void ItemSpawnCallback(ItemSpawnData itemData)
        {
        }

        public enum ENEMY_POS
        {
            RIGHT,  // �G�͎��g�̉E����
            LEFT,   // �G�͎��g�̍�����
            UP,     // �G�͎��g�̏���� (��ʌ������ď����)
            DOWN,   // �G�͎��g�̉����� (��ʌ������ĉ�����)
            SAME    // �G�Ɠ����s/��ɂ���
        };

        // �����ƓG��ID���瓯���s�ɂ��邩���m�F����
        // �߂�l�FENEMY_POS
        // 
        public ENEMY_POS SameColumn(CharacterModel.Data playerId, CharacterModel.Data enemyId)
        {

            // ���g�ƓG�͓����s�ɂ���
            if (enemyId.BlockPos.x == playerId.BlockPos.x) { return ENEMY_POS.SAME; }

            // ���g�ƓG�͉E���ɂ���
            if (playerId.BlockPos.x < enemyId.BlockPos.x) { return ENEMY_POS.RIGHT; }

            // ����ȊO�Ȃ̂ŁA�����ɂ���
            return ENEMY_POS.LEFT;
        }

        // �����ƓG��ID���瓯���s�ɂ��邩���m�F����
        // �߂�l�FENEMY_POS
        // 
        public ENEMY_POS SameRow(CharacterModel.Data playerId, CharacterModel.Data enemyId)
        {

            // ���g�ƓG�͓�����ɂ���
            if (enemyId.BlockPos.y == playerId.BlockPos.y) { return ENEMY_POS.SAME; }

            // �G�͎��g��������ɂ���
            if (playerId.BlockPos.y < enemyId.BlockPos.y) { return ENEMY_POS.UP; }

            // ����ȊO�Ȃ̂ŁA�G�͉������ɂ���
            return ENEMY_POS.DOWN;
        }

        // �����ƓG�̋������擾����
        // �߂�l�F���� (float)
        public float Distance(int playerId, int enemyId)
        {
            CharacterModel.Data player = GetCharacterData(playerId);
            CharacterModel.Data enemy = GetCharacterData(enemyId);

            float sx = player.BlockPos.x - enemy.BlockPos.x;
            float sy = player.BlockPos.y - enemy.BlockPos.y;
            return Mathf.Sqrt(sx * sx + sy * sy);
        }
    }
}