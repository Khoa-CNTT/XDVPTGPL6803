using System.Collections.Generic;

namespace MBM
{
    public class LanguageManager
    {
        private static LanguageManager _instance;
        public static LanguageManager Instance => _instance ?? (_instance = new LanguageManager());

        private string _currentLanguage;
        private Dictionary<string, Dictionary<string, string>> _translations;

        private LanguageManager()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                { "en", new Dictionary<string, string>
                    {
                        { "greeting", "Hello" },
                        { "farewell", "Goodbye" }
                        // Thêm các cặp key-value khác cho tiếng Anh
                    }
                },
                { "vi", new Dictionary<string, string>
                    {
                        { "greeting", "Xin chào" },
                        { "farewell", "Tạm biệt" }
                        // Thêm các cặp key-value khác cho tiếng Việt
                    }
                }
            };

            // Đặt ngôn ngữ mặc định là tiếng Anh
            _currentLanguage = "en";
        }

        public void SetLanguage(string languageCode)
        {
            if (_translations.ContainsKey(languageCode))
            {
                _currentLanguage = languageCode;
            }
            else
            {
                // Xử lý trường hợp ngôn ngữ không tồn tại
                throw new System.Exception("Ngôn ngữ không được hỗ trợ");
            }
        }

        public string GetTranslation(string key)
        {
            if (_translations[_currentLanguage].ContainsKey(key))
            {
                return _translations[_currentLanguage][key];
            }
            else
            {
                // Xử lý trường hợp key không tồn tại
                return $"[Translation missing for '{key}']";
            }
        }
    }
}