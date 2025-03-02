using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private List<SlotListData> slotListData;

    #region Injection

    DiContainer container;

    [Inject]
    private void Construct(DiContainer _container)
    {
        container = _container;
    }

    #endregion


    public void Init()
    {
        foreach (var listData in slotListData)
        {
            if (listData.SlotDataList.LeftSlot.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.SlotDataList.LeftSlot.ItemView);
                obj.transform.SetParent(listData.SlotDataList.LeftSlot.Slot);
                listData.SlotDataList.LeftSlot.ItemView = obj;
                obj.Init();
            }

            if (listData.SlotDataList.MiddleSlot.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.SlotDataList.MiddleSlot.ItemView);
                obj.transform.SetParent(listData.SlotDataList.MiddleSlot.Slot);
                listData.SlotDataList.MiddleSlot.ItemView = obj;
                obj.Init();
            }

            if (listData.SlotDataList.RightSlot.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.SlotDataList.RightSlot.ItemView);
                obj.transform.SetParent(listData.SlotDataList.RightSlot.Slot);
                listData.SlotDataList.RightSlot.ItemView = obj;
                obj.Init();
            }
        }
    }

    public bool PutNewItemToSlot(ItemView _item)
    {
        var slot = slotListData[0].SlotDataList;

        if (slot.LeftSlot.ItemView == null)
        {
            slot.LeftSlot.ItemView = _item;
            _item.transform.SetParent(slot.LeftSlot.Slot);
            _item.Init();
            return true;
        }

        if (slot.MiddleSlot.ItemView == null)
        {
            slot.MiddleSlot.ItemView = _item;
            _item.transform.SetParent(slot.MiddleSlot.Slot);
            _item.Init();
            return true;
        }

        if (slot.RightSlot.ItemView == null)
        {
            slot.RightSlot.ItemView = _item;
            _item.transform.SetParent(slot.RightSlot.Slot);
            _item.Init();
            return true;
        }

        return false;
    }

    public void DestroyItems()
    {
        if (IsAllItemsAreSame())
        {
            DestroyFrontRowItems();
        }
    }

    private void DestroyFrontRowItems()
    {
        var frontRow = slotListData[0].SlotDataList;

        var leftItem = frontRow.LeftSlot.ItemView;
        var middleItem = frontRow.MiddleSlot.ItemView;
        var rightItem = frontRow.RightSlot.ItemView;

        var pos = middleItem.transform.position.x;

        leftItem.transform.DOLocalMoveZ(-1, 0.3f).OnComplete(() =>
        {
            leftItem.transform.DOMoveX(pos, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                frontRow.LeftSlot.ItemView = null;
                Destroy(leftItem.gameObject);
            });
        });

        rightItem.transform.DOLocalMoveZ(-1, 0.3f).OnComplete(() =>
        {
            rightItem.transform.DOMoveX(pos, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                frontRow.RightSlot.ItemView = null;
                Destroy(rightItem.gameObject);
            });
        });


        middleItem.transform.DOLocalMoveZ(-1, 0.3f).OnComplete(() =>
        {
            middleItem.transform.DOMoveX(pos, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                frontRow.MiddleSlot.ItemView = null;
                Destroy(middleItem.gameObject);
            });
        });
    }

    public bool IsGridEmpty()
    {
        foreach (var listData in slotListData)
        {
            if (listData.SlotDataList.LeftSlot.ItemView != null ||
                listData.SlotDataList.MiddleSlot.ItemView != null ||
                listData.SlotDataList.RightSlot.ItemView != null)
            {
                return false;
            }
        }

        return true;
    }

    private int GetRowsItemCount()
    {
        int count = 0;

        if (slotListData[0].SlotDataList.LeftSlot.ItemView != null)
            count++;
        if (slotListData[0].SlotDataList.MiddleSlot.ItemView != null)
            count++;
        if (slotListData[0].SlotDataList.RightSlot.ItemView != null)
            count++;

        return count;
    }


    private bool IsAllItemsAreSame()
    {
        if (GetRowsItemCount() != 3) return true;

        var firstIndex = slotListData[0].SlotDataList.LeftSlot.ItemView.ItemIndex;

        var listData = slotListData[0].SlotDataList;

        if (listData.LeftSlot.ItemView.ItemIndex != firstIndex ||
            listData.MiddleSlot.ItemView.ItemIndex != firstIndex ||
            listData.RightSlot.ItemView.ItemIndex != firstIndex)
        {
            return false;
        }

        return true;
    }
}


[Serializable]
public class SlotListData
{
    public SlotData SlotDataList;
}

[Serializable]
public class SlotData
{
    public SlotItemData LeftSlot;
    public SlotItemData MiddleSlot;
    public SlotItemData RightSlot;
}


[Serializable]
public class SlotItemData
{
    public Transform Slot;
    public ItemView ItemView;
}