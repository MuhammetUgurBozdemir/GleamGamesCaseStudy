using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private List<SlotItemData> slotListData;

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
            bool isDark = slotListData.IndexOf(listData) == slotListData.Count - 1;

            if (listData.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.ItemView);
                obj.transform.SetParent(listData.Slot);
                listData.ItemView = obj;
                obj.Init(isDark);
            }
        }
    }

    public bool PutNewItemToSlot(ItemView _item)
    {
        SlotItemData slot = null;

        for (int i = 0; i < 3; i++)
        {
            if (slotListData[i].ItemView == null)
            {
                slot = slotListData[i];
            }
        }

        if (slot == null) return false;

        slot.ItemView = _item;
        _item.transform.SetParent(slot.Slot);
        _item.Init();

        return true;
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
        for (var i = 0; i < 3; i++)
        {
            slotListData[i].ItemView.DestroyAnim(slotListData[1].Slot.position.x);
            slotListData[i].ItemView = null;
        }
    }

    public bool IsGridEmpty()
    {
        return slotListData.All(_x => _x.ItemView == null);
    }

    private int GetRowsItemCount()
    {
        int count = 0;

        for (int i = 0; i < 3; i++)
        {
            if (slotListData[i].ItemView != null) count++;
        }

        return count;
    }


    private bool IsAllItemsAreSame()
    {
        if (GetRowsItemCount() != 3) return false;

        var firstIndex = slotListData[0].ItemView.ItemIndex;

        for (int i = 0; i < 3; i++)
        {
            if (slotListData[i].ItemView.ItemIndex != firstIndex)
            {
                return false;
            }
        }

        return true;
    }
}


[Serializable]
public class SlotItemData
{
    public Transform Slot;
    public ItemView ItemView;
}