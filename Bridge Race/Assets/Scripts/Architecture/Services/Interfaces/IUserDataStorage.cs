using System;

namespace Architecture.Services.Interfaces
{
    public interface IUserDataStorage
    {
        event Action OnNameChanged;
        string UserName { get; }
        void SetUserName(string name);
        void Load();
    }
}