using UnityEngine;

namespace KLTNLongKhoi
{
    public interface ISkillEffect
    {
        void ApplyEffect(GameObject target, float magnitude);
        void RemoveEffect(GameObject target);
    }
}