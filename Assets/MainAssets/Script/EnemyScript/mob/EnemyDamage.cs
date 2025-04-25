using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamageble�̃C���^�[�t�F�[�X�̌p�����s��
public class EnemyDamage : MonoBehaviour, IDamageable
{
    //�V���A�������Ă���Bcharadata�̊e�G�l�~�[��Status���w��B
    [SerializeField] private CharaStatus charadata;
    //�V���A�������Ă���Bplayerdata�̊e�G�l�~�[��Status���w��B
    [SerializeField] private PlayerStatus playerdata;
    // �V���A�������Ă���BLvdata�̎Q�Ƃ��w��B
    [SerializeField] private Lvdata lvdata;
    //�V���A�����BSlider��HP�Q�[�W�w��
    [SerializeField] Slider Slider;
    public int HP;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] GameObject Effect;
    [SerializeField] GameObject Enemy;

    int enemyDamage;

    //�V���A�����B�X�e�[�^�X�|�C���g�̎w��
    [SerializeField] private int statusPoint;
    //�V���A�����B�o���l�̑��ʒl�̎w��
    [SerializeField] private int maxexp;

    private bool isDead = false;

    void Start()
    {
        //charadata��null�łȂ����Ƃ��m�F
        if (charadata != null)
        {
            // value��HP�Q�[�W�̃X���C�_�[�̍ő��1��
            Slider.value = 1;

            //charadata�̍ő�HP�����B
            HP = charadata.MAXHP;

            // �X���C�_�[���\���ɂ���
            Slider.gameObject.SetActive(false);
        }

        // DontDestroyOnLoad���Ăяo����Ă��Ȃ����m�F
        if (gameObject.scene.name == null)
        {
            Debug.LogError("DontDestroyOnLoad has been called on this object");
        }
    }

    // �_���[�W�����̃��\�b�h�@value�ɂ�Player1��ATK�̒l�������Ă�
    public void Damage(int value)
    {
        // charadata��null�łȂ������`�F�b�N
        if (charadata != null && !isDead)
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

            // HP����������X���C�_�[��\������
            if (!Slider.gameObject.activeSelf)
            {
                Slider.gameObject.SetActive(true);
            }

            // HP�����������ɃA�j���[�V�����t���O��TRUE�ɂ���
            EnemyAnimator.SetBool("hit", true);

            // �A�j���[�V�������Đ����I�������t���O��FALSE�ɖ߂�
            StartCoroutine(ResetHitFlag());

            // HP��0�ȉ��Ȃ�Death�A�j���[�V�������Đ�
            if (HP <= 0)
            {
                isDead = true;
                // Death�t���O���Z�b�g���ăA�j���[�V�������Đ�
                EnemyAnimator.SetBool("death", true);

                // �R���[�`�����J�n���ăA�j���[�V�����I�����Death()�����s
                StartCoroutine(WaitForDeathAnimation());
            }
        }
    }

    // �A�j���[�V�����I����Ɏ��S�������s���R���[�`��
    private IEnumerator WaitForDeathAnimation()
    {
        Debug.Log("WaitForDeathAnimation started");
        // �A�j���[�V�������I������܂ő҂�
        yield return new WaitForSeconds(EnemyAnimator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("WaitForDeathAnimation ended");

        // ���S����
        Death();
    }

    // �A�j���[�V�����I�����hit�t���O��FALSE�ɖ߂��R���[�`��
    private IEnumerator ResetHitFlag()
    {
        // �A�j���[�V�������I������܂ő҂�
        yield return new WaitForSeconds(EnemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        // hit�t���O��FALSE�ɖ߂�
        EnemyAnimator.SetBool("hit", false);
    }

    // ���S�����̃��\�b�h
    public void Death()
    {
        Debug.Log("Death method called");

        // ���ŃG�t�F�N�g
        var effect = Instantiate(Effect);
        effect.transform.position = Enemy.transform.position;

        // �l���o���l������Ȃ�o���l����
        if (charadata.GETEXP > 0)
        {
            playerdata.EXP = playerdata.EXP + charadata.GETEXP;

            if (playerdata.LV < lvdata.playerExpTable.Count)
            {
                var a = lvdata.playerExpTable[playerdata.LV];

                if (playerdata.EXP >= a.exp)
                {
                    playerdata.LV += 1;
                    playerdata.StatusPoint += statusPoint;
                    playerdata.EXP = 0;
                    playerdata.MAXEXP *= maxexp;
                    playerdata.HP = playerdata.MAXHP; // ���x���A�b�v����HP��S��

                    // HP�e�L�X�g�ƃX���C�_�[���X�V
                    var playerDamage = FindObjectOfType<PlayerDamage>();
                    if (playerDamage != null)
                    {
                        playerDamage.UpdateHPText();
                        playerDamage.Slider.value = (float)playerdata.HP / playerdata.MAXHP; // HP�X���C�_�[���ő�l�ɐݒ�
                    }
                }
            }
            else
            {
                Debug.LogError("Player level is out of range of the experience table.");
            }
        }

        // �l���S�[���h������Ȃ�S�[���h����
        if (charadata.GETGOLD > 0)
        {
            playerdata.HAVEGOLD = playerdata.HAVEGOLD + charadata.GETGOLD;
        }

        // �Q�[���I�u�W�F�N�g��j��
        Debug.Log("Destroying game object: " + Enemy.name);
        Destroy(Enemy);
        Debug.Log("Destroying effect: " + effect.name);
        Destroy(effect, 5);
    }
}

