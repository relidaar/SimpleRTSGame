using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Selection
{
    public class SelectedElements : MonoBehaviour
    {
        public Dictionary<int, GameObject> SelectedTable { get; } = new Dictionary<int, GameObject>();

        public void Add(GameObject selected)
        {
            if (gameObject == null) return;
            int id = selected.GetInstanceID();
            if (SelectedTable.ContainsKey(id)) return;
            SelectedTable.Add(id, selected);
            selected.GetComponent<ISelectionComponent>().Enable();
            Debug.Log($"Added {selected} to selected elements");
        }

        public void Add(IEnumerable<GameObject> selected)
        {
            foreach (var item in selected)
            {
                Add(item);
            }
        }

        public bool Contains(GameObject entity)
        {
            return SelectedTable.ContainsKey(entity.GetInstanceID());
        }

        public void Remove(int id)
        {
            SelectedTable[id].GetComponent<ISelectionComponent>().Disable();
            SelectedTable.Remove(id);
        }

        public void RemoveAll()
        {
            foreach (var go in SelectedTable.Values.Where(go => go != null))
            {
                go.GetComponent<ISelectionComponent>().Disable();
            }
            SelectedTable.Clear();
        }
    }
}