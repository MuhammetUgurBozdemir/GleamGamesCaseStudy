using System;
using System.Collections.Generic;
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
                obj.Init();
            }

            if (listData.SlotDataList.MiddleSlot.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.SlotDataList.MiddleSlot.ItemView);
                obj.transform.SetParent(listData.SlotDataList.MiddleSlot.Slot);
                obj.Init();
            }

            if (listData.SlotDataList.RightSlot.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(listData.SlotDataList.RightSlot.ItemView);
                obj.transform.SetParent(listData.SlotDataList.RightSlot.Slot);
                obj.Init();
            }
        }
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