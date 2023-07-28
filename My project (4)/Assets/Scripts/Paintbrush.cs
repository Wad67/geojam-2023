using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Paintbrush : MonoBehaviour, ICollectable
{
    public static event HandlePaintBrushCollected OnPaintBrushCollected;
    public delegate void HandlePaintBrushCollected(ItemData itemData);
    public ItemData PaintBrushData;
    public void Collect()
    {
        OnPaintBrushCollected?.Invoke(PaintBrushData);
        Destroy(gameObject);
    }
}



