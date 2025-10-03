/// <summary>
/// �I�𒆂̃A�C�R�����z�b�v�A�b�v�����X�N���v�g
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleOnSelection : UIBehaviour
{
    [SerializeField] private float Rate = 2.25f; // �g�嗦
    [SerializeField] private float AnimationDuration = 0.25f; // �A�j���[�V�����̎���


    // �ۊǗp
    //private GameObject fastSelectedObject; // ���ݑI�𒆃I�u�W�F�N�g
    private GameObject currentSelectedObject;
    private Vector3 BaseScale;
    private GameObject previousSelectedObject = null; // ���Ƃ��ƑI�����Ă����I�u�W�F�N�g

    void Update()
    {
        // �v�l���̂݃z�b�v�A�b�v����
        if (JankenManager.Instance.CurrentState == JankenState.Wait)
        {
            // ���ݑI�΂�Ă���I�u�W�F�N�g���擾
            currentSelectedObject = EventSystem.current.currentSelectedGameObject;

            if (currentSelectedObject == previousSelectedObject) return;
            // �X�P�[����ύX���鏈��
            HandleScaleChange(currentSelectedObject);
        }
		else
		{
			CancelScale();
		}
	}


    // �X�P�[���ύX����
    private void HandleScaleChange(GameObject currentSelectedObject)
    {
        // �O��I�΂�Ă����I�u�W�F�N�g������΁A���̃X�P�[���ɖ߂�
        if (previousSelectedObject != null)
        {
            SetScale(previousSelectedObject, BaseScale); // BaseScales�̐������l���Q��
        }

        // �V�����I�΂ꂽ�I�u�W�F�N�g������΁A�X�P�[�����g��
        if (currentSelectedObject != null)
        {
            BaseScale = currentSelectedObject.transform.localScale;
            SetScale(currentSelectedObject, BaseScale * Rate); // BaseScales�̒l�����Ɋg��
        }

        // ���ݑI�΂�Ă���I�u�W�F�N�g���X�V
        previousSelectedObject = currentSelectedObject;
    }

    // �X�P�[����ݒ肷�郁�\�b�h
    private void SetScale(GameObject obj, Vector3 targetScale)
    {
        if (obj != null)
        {
            obj.transform.DOScale(targetScale, AnimationDuration)
                .SetEase(Ease.OutBounce)
                .Play();
        }
    }

    private void CancelScale()
	{
        SetScale(previousSelectedObject, BaseScale); // BaseScales�̐������l���Q��
    }
}
