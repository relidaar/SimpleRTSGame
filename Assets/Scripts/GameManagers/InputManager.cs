using System.Linq;
using Controllers;
using Selection;
using UnityEngine;

namespace GameManagers
{
    public class InputManager : MonoBehaviour
    {
        private SelectedElements _selectedElements;
        private RaycastHit _hit;
        private bool _isDragging;
        private Vector3 _mousePosition;

        private void Start()
        {
            _selectedElements = GetComponent<SelectedElements>();
        }

        private void Update()
        {
            SelectWithClick();
            SelectWithBox();
            OperateUnits();
        }

        private void OperateUnits()
        {
            if (!Input.GetMouseButtonDown(1)) return;
            if (!GetHit()) return;

            var units = _selectedElements.SelectedTable.Values
                .Where(x => x.gameObject.CompareTag("PlayerUnit"));

            switch (_hit.transform.tag)
            {
                case "Ground":
                    foreach (var unit in units) unit.GetComponent<UnitController>().Move(_hit.point);
                    break;
                case "EnemyCastle":
                case "EnemyUnit":
                    foreach (var unit in units) unit.GetComponent<UnitController>().Attack(_hit.transform);
                    break;
            }
        }

        private bool GetHit()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out _hit);
            Debug.Log($"{_hit.transform} was hit");
            return hit;
        }

        private void SelectWithClick()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            _mousePosition = Input.mousePosition;
            if (!GetHit()) return;
            
            switch (_hit.transform.gameObject.tag)
            {
                case "PlayerUnit":
                    if (!Input.GetKey(KeyCode.LeftShift))
                        _selectedElements.RemoveAll();
                    _selectedElements.Add(_hit.transform.gameObject);
                    break;
                default:
                    _isDragging = true;
                    break;
            }
        }

        private void SelectWithBox()
        {
            if (!Input.GetMouseButtonUp(0) || !_isDragging) return;
            _selectedElements.RemoveAll();
            var selectables = FindObjectsOfType<UnitController>()
                .Select(x => x.gameObject)
                .Where(x => x.CompareTag("PlayerUnit") && IsWithinSelectionBounds(x.transform));
            _selectedElements.Add(selectables);
            _isDragging = false;
        }

        private bool IsWithinSelectionBounds(Transform transform)
        {
            if (!_isDragging) return false;
            var camera = Camera.main;
            var viewportBounds = ScreenHelper.GetViewportBounds(camera, _mousePosition, Input.mousePosition);
            return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
        }

        private void OnGUI()
        {
            if (!_isDragging) return;
            var rect = ScreenHelper.GetScreenRect(_mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(.8f, .8f, .95f, .1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.gray);
        }
    }
}