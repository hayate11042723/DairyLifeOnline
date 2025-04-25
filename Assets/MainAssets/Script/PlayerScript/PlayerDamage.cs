using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [SerializeField] public PlayerStatus charadata; // �v���C���[�̃X�e�[�^�X�f�[�^
    [SerializeField] public Slider Slider; // HP��\������X���C�_�[
    [SerializeField] private string hpTextTag = "HPText"; // HP��\������e�L�X�g�̃^�O
    [SerializeField] GameObject deathEffectPrefab; // ���S���ɕ\������G�t�F�N�g��Prefab
    [SerializeField] private Animator PlayerAnimator; // �v���C���[�̃A�j���[�^�[
    private int playerDamage; // �Z�o���ꂽ�_���[�W��
    private Text hpText; // HP��\������e�L�X�g

    void Start()
    {
        // �^�O���g�p����HP�e�L�X�g���擾
        GameObject hpTextObject = GameObject.FindWithTag(hpTextTag);
        if (hpTextObject != null)
        {
            hpText = hpTextObject.GetComponent<Text>();
        }

        // �v���C���[�̃X�e�[�^�X���ݒ肳��Ă���ꍇ�AHP��������
        if (charadata != null)
        {
            Slider.value = 1; // HP�X���C�_�[�̏����l
            charadata.HP = charadata.MAXHP; // �ő�HP�����݂�HP�ɐݒ�
            UpdateHPText(); // HP�e�L�X�g���X�V
        }
    }

    public void Damage(int value)
    {
        if (charadata != null)
        {
            // �_���[�W���v�Z�i�G�̍U���l - �v���C���[�̖h��l�j
            playerDamage = value - charadata.DEF;
            if (playerDamage <= 0)
            {
                playerDamage = 1; // �_���[�W��0�ȉ��̏ꍇ��1�ɐݒ�
            }
            charadata.HP -= playerDamage; // ���݂�HP����_���[�W������
            Slider.value = (float)charadata.HP / (float)charadata.MAXHP; // HP�X���C�_�[���X�V
            UpdateHPText(); // HP�e�L�X�g���X�V
        }

        if (charadata.HP <= 0) // HP��0�ȉ��ɂȂ����ꍇ
        {
            PlayerAnimator.SetBool("death", true); // Death�A�j���[�V�������Đ�
            StartCoroutine(WaitForDeathAnimation()); // �A�j���[�V�����I����̏������R���[�`���Ŏ��s
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Death�A�j���[�V�������I������܂őҋ@
        yield return new WaitForSeconds(PlayerAnimator.GetCurrentAnimatorStateInfo(0).length);
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity); // ���S�G�t�F�N�g�𐶐�
        yield return new WaitForSeconds(2.0f); // �G�t�F�N�g�������1�b�ҋ@
        HandleRespawn(); // ���X�|�[�����������s
    }

    private void HandleRespawn()
    {
        // ���݂̃V�[�������擾
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "CityScene") // ���݂̃V�[����CityScene�łȂ��ꍇ
        {
            SceneManager.LoadScene("CityScene", LoadSceneMode.Single); // CityScene�ɑJ��
            StartCoroutine(RespawnInCity()); // CityScene�ɑJ�ڌ�A���X�|�[�����������s
        }
        else
        {
            RespawnAtPosition(new Vector3(0, 0.2f, -8)); // CityScene�̏ꍇ�A�w��ʒu�Ƀ��X�|�[��
        }
    }

    private IEnumerator RespawnInCity()
    {
        // CityScene���ǂݍ��܂��܂őҋ@
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "CityScene");
        RespawnAtPosition(new Vector3(0, 0.2f, -8)); // �w��ʒu�Ƀ��X�|�[��
    }

    private void RespawnAtPosition(Vector3 position)
    {
        // �v���C���[���w��ʒu�Ɉړ�
        transform.position = position;
        charadata.HP = charadata.MAXHP; // HP���ő�l�Ƀ��Z�b�g
        Slider.value = 1; // HP�X���C�_�[���ő�l�Ƀ��Z�b�g
        UpdateHPText(); // HP�e�L�X�g���X�V
        PlayerAnimator.SetBool("death", false); // Death�A�j���[�V�����̃t���O������
    }

    public void Death()
    {
        Destroy(gameObject); // �Q�[���I�u�W�F�N�g��j��
    }

    public void UpdateHPText()
    {
        if (hpText != null && charadata != null)
        {
            hpText.text = $"{charadata.HP}/{charadata.MAXHP}"; // HP�e�L�X�g���X�V
        }
    }
}

