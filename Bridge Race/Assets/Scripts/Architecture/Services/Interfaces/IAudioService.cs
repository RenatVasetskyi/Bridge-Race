using Audio;

namespace Architecture.Services.Interfaces
{
    public interface IAudioService
    {
        public bool IsMusicOn { get; }
        public bool IsSfxOn { get; }
        void Initialize();
        void PlayMusic(MusicType musicType);
        void PlaySfx(SfxType sfxType);
        void StopMusic();
        void ChangeVolume(SoundType soundType, SoundVolumeType volumeType);
    }
}