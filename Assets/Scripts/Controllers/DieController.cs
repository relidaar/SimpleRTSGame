using UnityEngine;

namespace Controllers
{
    public class DieController : MonoBehaviour
    {
        private UnitController _unitController;
        private Animator _animator;

        private void Start()
        {
            _unitController = GetComponent<UnitController>();
            _animator = GetComponent<Animator>();
            _animator.SetBool("isDead", true);
            Destroy(gameObject, 2);
        }
    }
}