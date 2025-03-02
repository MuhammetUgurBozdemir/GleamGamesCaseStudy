using System;
using System.Collections;
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