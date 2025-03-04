using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private List<SlotItemData> slotListData;
    public List<SlotItemData> SlotListData => slotListData;

    #region Injection

    DiContainer container;
    SignalBus signalBus;
    LevelController levelController;

    [Inject]
    private void Construct(DiContainer _container, SignalBus _signalBus, LevelController _levelController)
    {
        container = _container;
        signalBus = _signalBus;
        levelController = _levelController;
    }

    #endregion


    public void Init()
    {
        signalBus.Subscribe<ItemSlotChangedSignal>(MoveSlotToFront);

        foreach (var slotItemData in slotListData)
        {
            bool isDark = slotListData.IndexOf(slotItemData) > 2;

            if (slotItemData.ItemView != null)
            {
                var obj = container.InstantiatePrefabForComponent<ItemView>(slotItemData.ItemView);
                obj.transform.SetParent(slotItemData.Slot);
                slotItemData.ItemView = obj;
                obj.Init(slotItemData, isDark);
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

        _item.SetSlotEmpty();
        slot.ItemView = _item;
        _item.transform.SetParent(slot.Slot);
        _item.Init(slot);

        signalBus.Fire<ItemSlotChangedSignal>();

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
        const int count = 3;

        for (int i = 0; i < count; i++)
        {
            var item = slotListData[i];
            item.ItemView.DestroyAnim(slotListData[1].Slot.position.x);
            item.ItemView = null;
        }

        foreach (var slotItemData in slotListData)
        {
            if (slotItemData.ItemView == null) continue;

            var item = slotItemData.ItemView;
            var newSlot = slotListData[slotListData.IndexOf(slotItemData.ItemView.ParentSlot) - 3];

            bool isDark = slotListData.IndexOf(newSlot) > 2;

            slotItemData.ItemView.transform.SetParent(newSlot.Slot);

            item.SetSlotEmpty();
            newSlot.ItemView = item;

            item.transform.DOLocalMoveZ(0, 0.5f).SetDelay(0.5f).OnComplete(() => { item.Init(newSlot, isDark); });
        }

        DOCheckForLevelEnd().Forget();
    }

    private async UniTask DOCheckForLevelEnd()
    {
        await UniTask.Delay(1000);
        levelController.CheckForLevelEnd();
    }

    private void MoveSlotToFront()
    {
        for (int i = 0; i < 3; i++)
        {
            if (slotListData[i].ItemView != null) return;
        }

        foreach (var slotItemData in slotListData)
        {
            if (slotItemData.ItemView == null) continue;

            var item = slotItemData.ItemView;
            var newSlot = slotListData[slotListData.IndexOf(slotItemData.ItemView.ParentSlot) - 3];

            bool isDark = slotListData.IndexOf(newSlot) > 2;

            slotItemData.ItemView.transform.SetParent(newSlot.Slot);

            item.SetSlotEmpty();
            newSlot.ItemView = item;

            item.transform.DOLocalMoveZ(0, 0.5f).SetDelay(0.5f).OnComplete(() => { item.Init(newSlot, isDark); });
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

    public void Dispose()
    {
        signalBus.Unsubscribe<ItemSlotChangedSignal>(MoveSlotToFront);
    }
}


[Serializable]
public class SlotItemData
{
    public Transform Slot;
    public ItemView ItemView;
}