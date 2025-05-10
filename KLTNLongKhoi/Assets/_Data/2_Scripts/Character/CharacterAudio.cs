using UnityEngine;

namespace KLTNLongKhoi
{
    public class CharacterAudio : MonoBehaviour
    {
        // danh saách âm thanh footstep
        public AudioClip[] footstepSounds;
        public float footstepVolume = 0.5f;

        [Space]
        // danh sách âm thanh attack
        public AudioClip[] attackSounds;
        public float attackVolume = 0.5f;

        [Space]
        // danh sách âm thanh hurt
        public AudioClip[] hurtSounds;
        public float hurtVolume = 0.5f;

        [Space]
        // danh sách âm thanh die
        public AudioClip[] dieSounds;
        public float dieVolume = 0.5f;

        [Space]
        // danh sách âm thanh jump
        public AudioClip[] jumpSounds;
        public float jumpVolume = 0.5f;

        [Space]
        // âm thanh dash
        public AudioClip[] dashSounds;
        public float dashVolume = 0.5f;

        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayFootstepSound()
        { 
            if (footstepSounds.Length > 0)
            { 
                int randomIndex = Random.Range(0, footstepSounds.Length);
                audioSource.PlayOneShot(footstepSounds[randomIndex], footstepVolume);
            }
        }

        public void PlayAttackSound()
        {
            if (attackSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, attackSounds.Length);
                audioSource.PlayOneShot(attackSounds[randomIndex], attackVolume);
            }
        }

        public void PlayHurtSound()
        {
            if (hurtSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, hurtSounds.Length);
                audioSource.PlayOneShot(hurtSounds[randomIndex], hurtVolume);
            }
        }

        public void PlayDieSound()
        {
            if (dieSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, dieSounds.Length);
                audioSource.PlayOneShot(dieSounds[randomIndex], dieVolume);
            }
        }

        public void PlayJumpSound()
        {
            if (jumpSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, jumpSounds.Length);
                audioSource.PlayOneShot(jumpSounds[randomIndex], jumpVolume);
            }
        }

        public void PlayDashSound()
        {
            if (dashSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, dashSounds.Length);
                audioSource.PlayOneShot(dashSounds[randomIndex], dashVolume);
            }
        }


    }
}
