using UnityEngine;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour, ISaveData
    {
        [Header("GAME SETTINGS")]
        [SerializeField] bool _isFullScreen;
        [SerializeField] int _qualityIndex;
        [SerializeField] float _masterVolume;
        [SerializeField] int _currentResolutionIndex;

        private void Awake()
        {
            
        }

        #region SaveData
        public void LoadData<T>(T data)
        {
            if (data is GameSettingsData gsData)
            {

            }
        }

        public T SaveData<T>()
        {
            GameSettingsData data = new GameSettingsData();
            return (T)(object)(data);
        }
        #endregion
    }
}