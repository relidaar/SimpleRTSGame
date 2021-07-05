using System;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    private UnitController _controller;
    [SerializeField] private Slider healthBarUI;
    [SerializeField] private Transform canvas;
    private Quaternion _rotation;
    
    private void Start()
    {
        _controller = GetComponent<UnitController>();
        healthBarUI.value = CalculateHealth();
        _rotation = Quaternion.Euler(-45, 0, 0);
        healthBarUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        canvas.rotation = _rotation;
        healthBarUI.value = CalculateHealth();
        if (_controller.Health < _controller.Stats.health)
        {
            healthBarUI.gameObject.SetActive(true);
        }
    }

    private float CalculateHealth()
    {
        return _controller.Health / _controller.Stats.health;
    }
}
