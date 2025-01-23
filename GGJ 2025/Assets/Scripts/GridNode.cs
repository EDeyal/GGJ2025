using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    bool _isOccupied;
    bool _isHighlighted;
    [SerializeField] GameObject _floor;
    Renderer _renderer;
    public int xPos;
    public int yPos;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = _floor.GetComponent<Renderer>();
    }

    public void HighlightFloor(bool isHighlighted,Color color)
    {

        _renderer.material.color = color;
    }
}
