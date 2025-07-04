using UnityEngine;

public class BoxSlotUnit : ItemSlotUnit
{
    [SerializeField] BoxSystem _data;

    public override void Awake()
    {
        _image.color = Color.clear;
        _text.text = "";
    }

    /// <summary>
    /// Update UI.
    /// </summary>
    /// <param name="index"></param>
    public override void UpdateUI(int index)
    {
        
        _item = _data.ReadFromBoxSlot(index, out int stack);
        if (_item == null)
        {
            _image.color = Color.clear;
            _itemStack = 0;
            _text.text = "";
        }
        else
        {
            _image.color = Color.white;
            _image.sprite = _item.Icon;
            _itemStack = stack;
            _text.text = stack > 1 ? stack.ToString() : "";
        }        
    }

    /// <summary>
    /// OnClick event. If Click button, send item to the inventory.
    /// </summary>
    public void OnClick()
    {
        _data.GetCollection(_index);
    }
}