using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    #endregion // Singleton

    #region Init

    private void Initialize()
    {
        _inventoryItem = new ItemSO[25];
        _inventoryStack = new int[_inventoryItem.Length];
    }

    #endregion

    [SerializeField] DecompositionSystem _decompositionSlotData;

    [SerializeField] HotbarController _hotbarController;
    [SerializeField] GameManager _gameManager;

    private BoxSystem _currentOpenedBox;
    public BoxSystem CurrentOpenedBox => _currentOpenedBox;

    public int InventoryCount => _inventoryItem.Length;

    private ItemSO[] _inventoryItem;
    private int[] _inventoryStack;

    public static event Action OnInventorySlotChanged;

    [SerializeField] InventoryController _inventoryController;

    [SerializeField] CraftingController _craftingController;

    [SerializeField] DecompositionController _decompositionController;

    [SerializeField] RepairController _repairController;

    [SerializeField] BoxController _boxController;
    public BoxController BoxController => _boxController;

    public void OpenInventory()
    {
        if (_inventoryController.gameObject.activeSelf)
        {
            _inventoryController.gameObject.SetActive(false);
        }
        else
        {
            _inventoryController.gameObject.SetActive(true);
        }
    }

    public void OpenCraftingPanel()
    {
        if (_craftingController.gameObject.activeSelf)
        {
            _craftingController.gameObject.SetActive(false);
        }
        else
        {
            _craftingController.gameObject.SetActive(true);
        }
    }

    public void OpenDecompositionPanel()
    {
        if (_decompositionController.gameObject.activeSelf)
        {
            _decompositionController.gameObject.SetActive(false);
        }
        else
        {
            _decompositionController.gameObject.SetActive(true);
        }

    }

    public void OpenRepairPanel()
    {
        if (_repairController.gameObject.activeSelf)
        {
            _repairController.gameObject.SetActive(false);
        }
        else
        {
            _repairController.gameObject.SetActive(true);
        }
    }

    public void OpenBoxPanel()
    {
        if (_boxController.gameObject.activeSelf)
        {
            _boxController.gameObject.SetActive(false);
        }
        else
        {
            _boxController.gameObject.SetActive(true);
        }
    }

    #region Inventory

    /// <summary>
    /// Read Data from inventory.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    public ItemSO ReadFromInventory(int index, out int stack)
    {
        stack = _inventoryStack[index];
        return _inventoryItem[index];
    }

    /// <summary>
    /// Item Moving in Inventory
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public void MoveItemInInventory(int startIndex, int endIndex)
    {
        // if there is no item in start cell.
        if (startIndex == -1)
        {
            Debug.Log("시작칸에 아이템 없음");
            return;
        }

        // if there is item in start cell and drag outside the inventory - Drop Method.
        if (_inventoryItem[startIndex] != null && endIndex == -1)
        {
            Debug.Log("아이템 버리기");
            Vector3 position = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
            for (int i = 0; i < _inventoryStack[startIndex]; i++)
            {
                // TODO : Where to Instantiate item?
                Instantiate(_inventoryItem[startIndex].Prefab, position + new Vector3(1, 0, -5), Quaternion.identity);
            }
            GameManager.Instance.PlayerStats.RemoveInventoryWeight(_inventoryItem[startIndex].Weight * _inventoryStack[startIndex]);
            _inventoryItem[startIndex] = null;
            _inventoryStack[startIndex] = 0;
            OnInventorySlotChanged?.Invoke();
        }

        // if there is item in start cell and drag into empty cell.
        else if (_inventoryItem[startIndex] != null && endIndex != -1 && _inventoryItem[endIndex] == null)
        {
            Debug.Log("아이템 옮기기");
            InventoryTryAdd(_inventoryItem[startIndex], endIndex, _inventoryStack[startIndex]);
            _inventoryItem[startIndex] = null;
            _inventoryStack[startIndex] = 0;
            OnInventorySlotChanged?.Invoke();
        }

        // if there is item both in start and end
        else if (_inventoryItem[startIndex] != null && _inventoryItem[endIndex] != null)
        {
            // if item is same and stack is same, return.
            if (_inventoryItem[startIndex] == _inventoryItem[endIndex] && _inventoryStack[startIndex] == _inventoryStack[endIndex]) return;

            Debug.Log("아이템 위치 바꾸기");
            ItemSO tempItem = _inventoryItem[endIndex];
            int tempStack = _inventoryStack[endIndex];

            _inventoryItem[endIndex] = _inventoryItem[startIndex];
            _inventoryStack[endIndex] = _inventoryStack[startIndex];

            _inventoryItem[startIndex] = tempItem;
            _inventoryStack[startIndex] = tempStack;
            OnInventorySlotChanged?.Invoke();
        }
    }

    /// <summary>
    /// Add item to Inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddItemToInventory(ItemSO item, int amount = 1)
    {
        int remain = amount;
        while (remain > 0)
        {
            for (int i = 0; i < _inventoryItem.Length; i++)
            {
                if (_inventoryItem[i] == item)
                {
                    remain = InventoryTryAdd(item, i, remain);
                    if (remain <= 0) break;
                }
            }

            if (remain <= 0) break;

            for (int i = 0; i < _inventoryItem.Length; i++)
            {
                if (_inventoryItem[i] == null)
                {
                    remain = InventoryTryAdd(item, i, remain);
                    if (remain <= 0) break;
                }
            }

            if (remain <= 0) break;

            else
            {
                GameManager.Instance.PlayerStats.AddInventoryWeight(item.Weight * (amount - remain));
                OnInventorySlotChanged?.Invoke();
                Debug.Log("inventory full"); return false;
            }
        }
        Debug.Log("Weight Up1");
        GameManager.Instance.PlayerStats.AddInventoryWeight(item.Weight * amount);
        OnInventorySlotChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Use Item.
    /// TODO - Item Using Method - Equip/Potions
    /// </summary>
    /// <param name="index"></param>
    public void UseItem(int index)
    {
        if (_inventoryItem[index].Type == ItemType.Usable || _inventoryItem[index].Type == ItemType.Equip)
        {
            GameManager.Instance.PlayerStats.RemoveInventoryWeight(_inventoryItem[index].Weight);
            _inventoryStack[index]--;
            if (_inventoryStack[index] <= 0)
            {
                _inventoryItem[index] = null;
                _inventoryStack[index] = 0;
            }
        }

        OnInventorySlotChanged?.Invoke();
    }

    /// <summary>
    /// Remove item from inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool RemoveItemFromInventory(ItemSO item, int amount = 1)
    {
        int remain = amount;
        while (remain > 0)
        {
            for (int i = 0; i < _inventoryItem.Length; i++)
            {
                if (_inventoryItem[i] == item)
                {
                    remain = InventoryTryRemove(i, amount);
                    if (remain <= 0) break;
                }
            }
            if (remain <= 0) break;

            GameManager.Instance.PlayerStats.RemoveInventoryWeight(item.Weight * (amount - remain));
            OnInventorySlotChanged?.Invoke();
            return false;
        }

        GameManager.Instance.PlayerStats.RemoveInventoryWeight(item.Weight * amount);
        OnInventorySlotChanged?.Invoke();
        return true;
    }

    public void RemoveAllItemsInInventory()
    {
        for (int i = 0; i < _inventoryItem.Length; i++)
        {
            if (_inventoryItem[i] != null)
            {
                GameManager.Instance.PlayerStats.RemoveInventoryWeight(_inventoryItem[i].Weight * _inventoryStack[i]);
                _inventoryItem[i] = null;
                _inventoryStack[i] = 0;
            }
        }
        OnInventorySlotChanged?.Invoke();
    }

    /// <summary>
    /// Try add item to Inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private int InventoryTryAdd(ItemSO item, int index, int amount)
    {
        _inventoryItem[index] = item;
        // If the stack is enough.
        if (amount <= (item.MaxStackSize - _inventoryStack[index]))
        {
            _inventoryStack[index] += amount;
            OnInventorySlotChanged?.Invoke();
            return 0;
        }
        // If the stack is not enough and has space.
        else if (item.MaxStackSize > _inventoryStack[index])
        {
            amount -= (item.MaxStackSize - _inventoryStack[index]);
            _inventoryStack[index] = item.MaxStackSize;
            OnInventorySlotChanged?.Invoke();
            return amount;
        }
        // If the stack is full.
        else
        {
            return amount;
        }
    }

    /// <summary>
    /// Try remove item.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private int InventoryTryRemove(int index, int amount)
    {
        if (amount <= _inventoryStack[index])
        {
            _inventoryStack[index] -= amount;
            if (_inventoryStack[index] == 0)
            {
                _inventoryItem[index] = null;
            }
            OnInventorySlotChanged?.Invoke();
            return 0;
        }
        else if (amount > _inventoryStack[index])
        {
            amount -= _inventoryStack[index];
            _inventoryStack[index] = 0;
            _inventoryItem[index] = null;
        }
        OnInventorySlotChanged?.Invoke();
        return amount;
    }

    #endregion

    #region QuickSlot

    public void AddQuickSlotItem(int targetIndex, int inventoryIndex)
    {
        if (_inventoryItem[inventoryIndex].Type == ItemType.Usable || _inventoryItem[inventoryIndex].Type == ItemType.Equip)
        {
            _hotbarController.SetSlot(targetIndex, inventoryIndex);
            OnInventorySlotChanged?.Invoke();
        }
    }

    public void RemoveQuickSlotItem(int index)
    {
        _hotbarController.SetSlot(index, -1);
        OnInventorySlotChanged?.Invoke();
    }

    #endregion

    #region Decomposition

    /// <summary>
    /// Send item to decomposition slot.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endindex"></param>
    public void SendItemToDecomposition(int startIndex)
    {
        if (_decompositionSlotData.AddItemToDecompositionSlot(_inventoryItem[startIndex], _inventoryStack[startIndex]))
        {
            GameManager.Instance.PlayerStats.RemoveInventoryWeight(_inventoryItem[startIndex].Weight * _inventoryStack[startIndex]);
            _inventoryItem[startIndex] = null;
            _inventoryStack[startIndex] = 0;
            OnInventorySlotChanged?.Invoke();
        }
    }

    /// <summary>
    /// Get Item to InventorySlot from decomposition slot.
    /// </summary>
    /// <param name="startIndex"></param>
    public void ReturnItemFromDecomposition(int startIndex)
    {
        _decompositionSlotData.ReturnItemToInventory(startIndex);
        OnInventorySlotChanged?.Invoke();
    }

    /// <summary>
    /// Move Item in the decomposition slot.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public void MoveItemInDecompositionSlot(int startIndex, int endIndex)
    {
        _decompositionSlotData.MoveItemInDecompositionSlot(startIndex, endIndex);
    }

    #endregion

    #region Box

    public void OpenBox(BoxSystem box)
    {
        _currentOpenedBox = box;
    }

    public void CloseBox()
    {
        _currentOpenedBox = null;
    }

    /// <summary>
    /// Send item to box slot.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endindex"></param>
    public void ReturnItemToBox(int startIndex)
    {
        if (startIndex == -1) return;

        if (_currentOpenedBox.AddItemToBoxSlot(_inventoryItem[startIndex], _inventoryStack[startIndex]))
        {
            GameManager.Instance.PlayerStats.RemoveInventoryWeight(_inventoryItem[startIndex].Weight * _inventoryStack[startIndex]);
            _inventoryItem[startIndex] = null;
            _inventoryStack[startIndex] = 0;
            OnInventorySlotChanged?.Invoke();
        }
    }

    /// <summary>
    /// Move Item in the Box slot.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public void MoveItemInBoxSlot(int startIndex, int endIndex)
    {
        _currentOpenedBox.MoveItemInBoxSlot(startIndex, endIndex);
    }

    #endregion
}