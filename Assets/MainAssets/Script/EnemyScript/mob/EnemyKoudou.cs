using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyKoudou : MonoBehaviour, IEnemyAction
{
    //�V���A�������Ă���BCharaStatus�̓G���w��B
    [SerializeField] CharaStatus charadata;
    private GameObject Player;
    PlayerDamage script;
    Vector3 distance; //Player�Ƃ̋���
    int State;

    void Update()
    {
        FindClosestPlayer();
    }

    void FindClosestPlayer()
    {
        // �^�O "Player" �����S�ẴI�u�W�F�N�g���擾
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        // �ePlayer�Ƃ̋������v�Z���čŒZ���������߂�
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPlayer = player;
            }
        }

        Player = nearestPlayer;
    }

    public int EnemyAIkoudou()
    {
        if (Player == null)
        {
            return 0;
        }

        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();
        script = Player.GetComponent<PlayerDamage>();

        if (script == null)
        {
            return 0;
        }

        //�L���������v�Z�̏���
        //�G�̈ʒu����Player�̈ʒu�����������Mathf.Abs�Ő�Βl���o�����Ƃŋ������킩��B

        distance = transform.position - Player.transform.position;

        float distanceX = Mathf.Abs(distance.x);
        float distanceZ = Mathf.Abs(distance.z);

        //X���W��Z���W�̋����̂ǂ��炪�傫�������ׁA�傫���ق��̋������G��ShortAttackRange�ȉ��ł����State��1�Ƃ��ĕԂ��B�U�����s���B
        if (charadata.MAXHP > enemyDamage.HP && script.charadata.HP > 0)
        {
            if (distanceX > distanceZ)
            {
                if (charadata.ShortAttackRange >= distanceX)
                {
                    State = 1;
                    return State;
                }
                else if (charadata.ShortAttackRange < distanceX)
                {
                    State = 0;
                    return State;
                }
            }
            else if (distanceX < distanceZ)
            {
                if (charadata.ShortAttackRange >= distanceZ)
                {
                    State = 1;
                    return State;
                }
                else if (charadata.ShortAttackRange < distanceZ)
                {
                    State = 0;
                    return State;
                }
            }
        }

        //�����𖞂����Ȃ��ꍇ��State��0�Ƃ��ĕԂ��B�������Ȃ��B
        State = 0;
        return State;
    }
}
