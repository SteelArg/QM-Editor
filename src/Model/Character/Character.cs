using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : IPlacedOnGrid {

    private Vector2 _gridPosition;
    private Asset _asset;
    private List<Accessory> _accessories = new List<Accessory>();

    public Character() {}

    public void AddAccessory(Accessory accessory) {
        _accessories.Add(accessory);
    }

    public void RemoveAccessory(Accessory accessory) {
        _accessories.Remove(accessory);
    }

    public void Render(Grid grid) {
        // Render on a grid
        foreach (Accessory acc in _accessories) {
            acc.Render(grid, _gridPosition);
        }
    }

    public void SetGridPosition(Vector2 pos) {
        _gridPosition = pos;
    }
    public Vector2 GetGridPosition() => _gridPosition;

}