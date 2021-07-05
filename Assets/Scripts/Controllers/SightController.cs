using System;
using System.Collections.Generic;
using Selection;
using UnityEngine;

namespace Controllers
{
    public class SightController : MonoBehaviour
    {
        [SerializeField] private GameObject targetGameObject;
        private UnitController _unitController;
        private List<Transform> _targets;
        private SelectedElements _selected;
        [SerializeField] private float sightRange;

        private void Start()
        {
            _unitController = GetComponent<UnitController>();
            _targets = new List<Transform>();
            _selected = GameObject.FindWithTag("GameManager").GetComponent<SelectedElements>();
            GetComponent<SphereCollider>().radius = sightRange;
        }

        private void Update()
        {
            ChangeTargetIfOutOfRange();
            ChangeTarget();
        }

        private void ChangeTarget()
        {
            if (_unitController.Target != null || _targets.Count <= 0 || _selected.Contains(gameObject)) return;
            _unitController.Attack(_targets[0]);
            _targets.RemoveAt(0);
        }

        private void ChangeTargetIfOutOfRange()
        {
            if (_unitController.Target == null || _targets.Count <= 0) return;
            if ((_unitController.Target.position - transform.position).magnitude < sightRange) return;
            _unitController.Attack(_targets[0]);
            _targets.RemoveAt(0);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetGameObject.tag) && other != null) 
                _targets.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(targetGameObject.tag) && other != null && _targets.Contains(other.transform)) 
                _targets.Remove(other.transform);
        }
    }
}
