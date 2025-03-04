using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class ItemView : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] private int itemIndex;
    private SlotItemData parentSlot;
    public SlotItemData ParentSlot => parentSlot;
    public int ItemIndex => itemIndex;

    [SerializeField] private Color initialColor;

    private void Awake()
    {
        initialColor = meshRenderer.sharedMaterial.color;
    }


    public void SetSlotEmpty()
    {
        parentSlot.ItemView = null;
    }

    public void Init(SlotItemData _parentSlot, bool _isDark = false)
    {
        parentSlot = _parentSlot;
        parentSlot.ItemView = this;
        transform.localPosition = Vector3.zero;
        meshRenderer.material.color = _isDark ? Color.black : initialColor;
    }


    public void DestroyAnim(float _pos)
    {
        transform.DOLocalMoveZ(-1, 0.3f).OnComplete(() => { transform.DOMoveX(_pos, 0.5f).SetEase(Ease.InOutBack).OnComplete(() => { Destroy(gameObject); }); });
    }
}