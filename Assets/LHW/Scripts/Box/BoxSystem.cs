using System;
using UnityEngine;

public class BoxSystem : MonoBehaviour
{
    [SerializeField] private ItemSO[] _boxItem;
    [SerializeField] private int[] _boxStack;

    [SerializeField] private CollectionSO[] _boxCollection;

    public ItemSO[] BoxItem => _boxItem;
    public int[] BoxStack => _boxStack;

    public CollectionSO[] BoxCollection => _boxCollection;

    public event Action OnBoxSlotUpdated;

    /// <summary>
    /// Read Data of box slot.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    public ItemSO ReadFromBoxSlot(int index, out int stack)
    {
        stack = _boxStack[index];
        return _boxItem[index];
    }

    public void RemoveAllItem()
    {
        for(int i = 0; i < _boxItem.Length; i++)
        {
            _boxItem[i] = null;
            _boxStack[i] = 0;
        }
        for (int i = 0; i < _boxCollection.Length; i++)
        {
            _boxCollection[i] = null;
        }
    }

    /// <summary>
    /// Used as Onclick event.
    /// Get all items in the box to inventory.
    /// </summary>
    public void GetAllBoxItemIntoInventory()
    {
        for(int i = 0; i < _boxItem.Length; i++)
        {
            InventoryManager.Instance.GetItemFromBox(i);
        }
        for (int i = 0; i < _boxCollection.Length; i++)
        {
            GetCollection(i);
        }
    }

    /// <summary>
    /// Add item to box Slot.(From Inventory)
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    public bool AddItemToBoxSlot(ItemSO item, int stack = 1)
    {
        int remain = stack;
        while (remain > 0)
        {
            for (int i = 0; i < _boxItem.Length; i++)
            {
                if (_boxItem[i] == item)
                {
                    remain = BoxSlotTryAdd(item, i, remain);
                    if (remain <= 0) break;
                }
            }

            if (remain <= 0) break;

            for (int i = 0; i < _boxItem.Length; i++)
            {
                if (_boxItem[i] == null)
                {
                    remain = BoxSlotTryAdd(item, i, remain);
                    if (remain <= 0) break;
                }
            }

            if (remain <= 0) break;

            else
            {
                Debug.Log("slot is full"); return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Return Item to the Inventory.
    /// </summary>
    /// <param name="index"></param>
    public void SendItemToInventory(int index)
    {
        if (index == -1) return;

        if (_boxItem[index] != null)
        {
            InventoryManager.Instance.AddItemToInventory(_boxItem[index], _boxStack[index]);

            _boxItem[index] = null;
            _boxStack[index] = 0;
            OnBoxSlotUpdated?.Invoke();
        }
    }

    public void AddCollection(CollectionSO collection, int index)
    {
        _boxCollection[index] = collection;
    }

    public void GetCollection(int index)
    {
        if(_boxCollection[index] != null)
        {
            ItemCollectionManager.Instance.TryCollectItem(_boxCollection[index]);
            _boxCollection[index] = null;
        }
    }

    /// <summary>
    /// Move item in the box slot.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public void MoveItemInBoxSlot(int startIndex, int endIndex)
    {
        // if there is no item in start cell.
        if (startIndex == -1 || endIndex == -1)
        {
            return;
        }

        // if there is item in start cell and drag into empty cell.
        else if (_boxItem[startIndex] != null && endIndex != -1 && _boxItem[endIndex] == null)
        {
            BoxSlotTryAdd(_boxItem[startIndex], endIndex, _boxStack[startIndex]);
            _boxItem[startIndex] = null;
            _boxStack[startIndex] = 0;
            OnBoxSlotUpdated?.Invoke();
        }

        // if there is item both in start and end
        else if (_boxItem[startIndex] != null && _boxItem[endIndex] != null)
        {
            // if item is same and stack is same, return.
            if (_boxItem[startIndex] == _boxItem[endIndex] && _boxStack[startIndex] == _boxStack[endIndex]) return;

            ItemSO tempItem = _boxItem[endIndex];
            int tempStack = _boxStack[endIndex];

            _boxItem[endIndex] = _boxItem[startIndex];
            _boxStack[endIndex] = _boxStack[startIndex];

            _boxItem[startIndex] = tempItem;
            _boxStack[startIndex] = tempStack;
            OnBoxSlotUpdated?.Invoke();
        }
    }

    /// <summary>
    /// Try add item in the box slot.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private int BoxSlotTryAdd(ItemSO item, int index, int amount)
    {
        _boxItem[index] = item;
        // If the stack is enough.
        if (amount <= (item.MaxStackSize - _boxStack[index]))
        {
            _boxStack[index] += amount;
            OnBoxSlotUpdated?.Invoke();
            return 0;
        }
        // If the stack is not enough and has space.
        else if (item.MaxStackSize > _boxStack[index])
        {
            amount -= (item.MaxStackSize - _boxStack[index]);
            _boxStack[index] = item.MaxStackSize;
            OnBoxSlotUpdated?.Invoke();
            return amount;
        }
        // If the stack is full.
        else
        {
            return amount;
        }
    }
}