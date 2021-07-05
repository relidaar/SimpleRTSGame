using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthBar : MonoBehaviour
{
    private CastleController _controller;
    [SerializeField] private Slider healthBarUI;
    [SerializeField] private Transform canvas;
    
    private void Start()
    {
        _controller = GetComponent<CastleController>();
        healthBarUI.value = CalculateHealth();
        healthBarUI.gameObject.SetActive(false);
        canvas.rotation = Quaternion.Euler(-45, 0, 0);
    }

    private void Update()
    {
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