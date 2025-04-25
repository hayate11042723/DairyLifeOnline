using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamageble�̃C���^�[�t�F�[�X�̌p�����s��
public class EnemyDamageBoss : MonoBehaviour, IDamageable
{
    //�V���A�������Ă���Bcharadata�̊e�G�l�~�[��Status���w��B
    [SerializeField] private CharaStatus charadata;
    //�V���A�������Ă���Bplayerdata�̊e�G�l�~�[��Status���w��B
    [SerializeField] private PlayerStatus playerdata;
    // �V���A�������Ă���BLvdata�̎Q�Ƃ��w��B
    [SerializeField] private Lvdata lvdata;
    //�V���A�����BSlider��HP�Q�[�W�w��
    [SerializeField] Slider Slider;
    [SerializeField] int HP;
    [SerializeField] int DestroyTime;
    int enemyDamage;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] GameObject Effect;

    //�V���A�����B�X�e�[�^�X�|�C���g�̎w��
    [SerializeField] private int statusPoint;
    //�V���A�����B�o���l�̑��ʒl�̎w��
    [SerializeField] private int maxexp;

    // �V���A�����Bhit�A�j���[�V�������g���K�[����_���[�W�ʂ̎w��
    [SerializeField] private int hitTriggerDamage;

    void Start()
    {
        //charadata��null�łȂ����Ƃ��m�F
        if (charadata != null)
        {
            // value��HP�Q�[�W�̃X���C�_�[�̍ő��1��
            Slider.value = 1;

            //charadata�̍ő�HP�����B
            HP = charadata.MAXHP;
        }
    }

    // HP��S�񕜂��郁�\�b�h
    public void isSleep()
    {
        HP = charadata.MAXHP;
        Slider.value = 1;
    }

    // �_���[�W�����̃��\�b�h�@value�ɂ�Player1��ATK�̒l�������Ă�
    public void Damage(int value)
    {
        // charadata��null�łȂ������`�F�b�N
        if (charadata != null)
        {
            // Player��ATK����Enemy��DEF���������l����_���[�W���Z�o
            enemyDamage = value - charadata.DEF;
            // �_���[�W�ʂ�0�ȉ��ɂȂ�����1�_���[�W�ɂ���
            if (enemyDamage <= 0)
            {
                enemyDamage = 1;
            }
            // HP����Z�o���ꂽ�_���[�W������
            HP -= enemyDamage;
            // HP�Q�[�W�ɔ��f
            Slider.value = (float)HP / (float)charadata.MAXHP;

            // ����̃_���[�W�ʂɒB�����ꍇ�ɃA�j���[�V�����t���O��TRUE�ɂ���
            if (enemyDamage >= hitTriggerDamage)
            {
                EnemyAnimator.SetBool("hit", true);
                // �A�j���[�V�������Đ����I�������t���O��FALSE�ɖ߂�
                StartCoroutine(ResetHitFlag());
                enemyDamage = 0;
            }
        }

        // HP��0�ȉ��Ȃ�hit�A�j���[�V�������Đ����I��������Death�t���O���Z�b�g
        if (HP <= 0)
        {
            // hit�A�j���[�V�������Đ����I�������Death�t���O���Z�b�g
            StartCoroutine(ResetHitFlag(true));
        }
    }

    // �A�j���[�V�����I�����hit�t���O��FALSE�ɖ߂��A�K�v�ɉ�����death�t���O��TRUE�ɂ���R���[�`��
    private IEnumerator ResetHitFlag(bool setDeathFlag = false)
    {
        // �A�j���[�V�������I������܂ő҂�
        float animationLength = EnemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // hit�t���O��FALSE�ɖ߂�
        EnemyAnimator.SetBool("hit", false);

        // death�t���O��TRUE�ɂ���
        if (setDeathFlag)
        {
            EnemyAnimator.SetBool("death", true);
            // �R���[�`�����J�n���ăA�j���[�V�����I�����Death()�����s
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // �A�j���[�V�������I������܂ő҂�
        float animationLength = EnemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // �����1�b�҂�
        yield return new WaitForSeconds(1f);

        // ���S����
        Death();
    }

    // ���S�����̃��\�b�h
    public void Death()
    {
        // ���ŃG�t�F�N�g
        var effect = Instantiate(Effect);
        effect.transform.position = gameObject.transform.position;

        // �Q�[���I�u�W�F�N�g��j��
        Destroy(gameObject);
        Destroy(effect, DestroyTime);

        // �l���o���l������Ȃ�o���l����
        if (charadata.GETEXP > 0)
        {
            playerdata.EXP = playerdata.EXP + charadata.GETEXP;

            while (playerdata.LV < lvdata.playerExpTable.Count && playerdata.EXP >= lvdata.playerExpTable[playerdata.LV].exp)
            {
                playerdata.EXP -= lvdata.playerExpTable[playerdata.LV].exp;
                playerdata.LV += 1;
                playerdata.StatusPoint += statusPoint;
                playerdata.MAXEXP *= maxexp;
            }
        }

        // �l���S�[���h������Ȃ�S�[���h����
        if (charadata.GETGOLD > 0)
        {
            playerdata.HAVEGOLD = playerdata.HAVEGOLD + charadata.GETGOLD;
        }
    }
}

