using UnityEngine;

namespace Architecture.Services.Interfaces
{
    public interface ISaveService
    {
        void SaveInt(string saveId, int value);
        void SaveBool(string saveId, bool value);
        void SaveFloat(string saveId, float value);
        void SaveString(string saveId, string value);
        void SaveSprite(string saveId, Sprite value);
        int LoadInt(string saveId);
        bool LoadBool(string saveId);
        float LoadFloat(string saveId);
        string LoadString(string saveId);
        Sprite LoadSprite(string saveId);
        bool HasKey(string key);
        void DeleteKey(string key);
    }
}
