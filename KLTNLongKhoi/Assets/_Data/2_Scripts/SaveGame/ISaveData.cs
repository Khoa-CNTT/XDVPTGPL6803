using UnityEngine;

using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public interface ISaveData
    {
        public void LoadData<T>(T data);
        public T SaveData<T>(); /// <summary> T: Kiểu dữ liệu trả về, D: Kiểu dữ liệu muốn lấy </summary>
    }
}
