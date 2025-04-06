using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace HieuDev
{
    public class Player : MonoBehaviour
    {
        public float speed = 5;

        /// <summary> Cách sử dụng InputSystems bằng cách kéo thả vào </summary>
        [SerializeField] private InputActionReference _moveAction;

        private void Update()
        {
            Vector2 moveDir = _moveAction.action.ReadValue<Vector2>();
            transform.Translate(moveDir * speed * Time.deltaTime);
        }

    }
}
