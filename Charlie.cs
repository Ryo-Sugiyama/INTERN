using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ateam
{
    public class Charlie : BaseBattleAISystem
    {
        /* �ϐ��錾�Ə����� */
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
            //�@�X�e�[�W�f�[�^�̎擾
            stageData = GetStageData();

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