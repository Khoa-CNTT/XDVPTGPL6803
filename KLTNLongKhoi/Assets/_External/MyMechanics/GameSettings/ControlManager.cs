using System.Collections.Generic;

namespace MBM
{
    public class ControlManager
    {
        private static ControlManager _instance;
        public static ControlManager Instance => _instance ?? (_instance = new ControlManager());

        private string _currentControlScheme;
        private Dictionary<string, Dictionary<string, string>> _controlSchemes;

        private ControlManager()
        {
            _controlSchemes = new Dictionary<string, Dictionary<string, string>>
            {
                { "keyboard", new Dictionary<string, string>
                    {
                        { "move", "WASD" },
                        { "jump", "Space" },
                        { "shoot", "Left Mouse Button" }
                        // Thêm các cặp key-value khác cho sơ đồ điều khiển bằng bàn phím
                    }
                },
                { "gamepad", new Dictionary<string, string>
                    {
                        { "move", "Left Stick" },
                        { "jump", "A Button" },
                        { "shoot", "Right Trigger" }
                        // Thêm các cặp key-value khác cho sơ đồ điều khiển bằng gamepad
                    }
                }
            };

            // Đặt sơ đồ điều khiển mặc định là bàn phím
            _currentControlScheme = "keyboard";
        }

        public void SetScheme(string controlScheme)
        {
            if (_controlSchemes.ContainsKey(controlScheme))
            {
                _currentControlScheme = controlScheme;
            }
            else
            {
                // Xử lý trường hợp sơ đồ điều khiển không tồn tại
                throw new System.Exception("Sơ đồ điều khiển không được hỗ trợ");
            }
        }

        public string GetControl(string action)
        {
            if (_controlSchemes[_currentControlScheme].ContainsKey(action))
            {
                return _controlSchemes[_currentControlScheme][action];
            }
            else
            {
                // Xử lý trường hợp hành động không tồn tại
                return $"[Control mapping missing for '{action}']";
            }
        }
    }
}