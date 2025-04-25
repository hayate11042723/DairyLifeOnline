using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowrdEffect : MonoBehaviour
{
    private TrailRenderer trailEffect;

    // Start is called before the first frame update
    void Start()
    {
        trailEffect = GetComponent<TrailRenderer>();
        if (trailEffect != null)
        {
            trailEffect.emitting = false;
        }
    }

    // �A�j���[�V�����C�x���g����Ăяo����郁�\�b�h
    public void AttackFlagON()
    {
        if (trailEffect != null)
        {
            trailEffect.emitting = true;
        }
    }

    // �A�j���[�V�����C�x���g����Ăяo����郁�\�b�h
    public void AttackFlagOFF()
    {
        if (trailEffect != null)
        {
            trailEffect.emitting = false;
        }
    }
}
