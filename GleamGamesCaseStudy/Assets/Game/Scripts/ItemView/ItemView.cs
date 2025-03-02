using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] private int itemIndex;
    public int ItemIndex => itemIndex;

    public void Init(bool _isDark=false)
    {
        transform.localPosition = Vector3.zero;
        meshRenderer.material.color = _isDark ? Color.black : Color.white;
    }
    
    

    public void DestroyAnim(float _pos)
    {
        transform.DOLocalMoveZ(-1, 0.3f).OnComplete(() =>
        {
            transform.DOMoveX(_pos, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}