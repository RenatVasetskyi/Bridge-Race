using System.Collections.Generic;
using Architecture.Services.Interfaces;
using Audio;
using Data;
using UnityEngine;

namespace Architecture.Services
{
    public class AudioService : IAudioService
    {
        private const string SaveSfxVolumeId = "SfxVolume";
        private const string SaveMusicVolumeId = "MusicVolume";

        private const string SaveMusicStateId = "MusicState";
        private const string SaveSfxStateId = "SfxState";
        
        private const int MaxVolume = 1;
        private const int MinVolume = 0;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IBaseFactory _baseFactory;
        private readonly ISaveService _saveService;

        private readonly List<SfxData> _sfxDataList = new();
        private readonly List<MusicData> _musicDataList = new();

        private AudioSource _sfxAudioSource;
        private AudioSource _musicAudioSource;

        public bool IsMusicOn { get; private set; }
        public bool IsSfxOn { get; private set; }

        public AudioService(IAssetProvider assetProvider, IBaseFactory baseFactory, 
            ISaveService saveService)
        {
            _assetProvider = assetProvider;
            _baseFactory = baseFactory;
            _saveService = saveService;
        }

        public void ChangeVolume(SoundType soundType, SoundVolumeType volumeType)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    switch (volumeType)
                    {
                        case SoundVolumeType.Off:
                            _musicAudioSource.volume = MinVolume;
                            IsMusicOn = false;
                            break;
                        case SoundVolumeType.On:
                            _musicAudioSource.volume = MaxVolume;
                            IsMusicOn = true;
                            break;
                    }
                    break;
                case SoundType.Sound: 
                    switch (volumeType)
                    {
                        case SoundVolumeType.Off:
                            _sfxAudioSource.volume = MinVolume;
                            IsSfxOn = false;
                            break;
                        case SoundVolumeType.On:
                            _sfxAudioSource.volume = MaxVolume;
                            IsSfxOn = true;
                            break;
                    }
                    break;
            }
            
            Save();
        }

        public void PlayMusic(MusicType musicType)
        {
            MusicData musicData = GetMusicData(musicType);
            _musicAudioSource.clip = musicData.Clip;
            _musicAudioSource.Play();
        }

        public void PlaySfx(SfxType sfxType)
        {
            SfxData sfxData = GetSfxData(sfxType);
            _sfxAudioSource.PlayOneShot(sfxData.Clip);
        }

        public void Initialize()
        {
            InitializeSfxDataList();
            InitializeMusicDataList();
            InitializeSfxAudioSource();
            InitializeMusicAudioSource();
            Load();
        }

        private void Save()
        {
            _saveService.SaveFloat(SaveSfxVolumeId, _sfxAudioSource.volume);
            _saveService.SaveFloat(SaveMusicVolumeId, _musicAudioSource.volume);
            _saveService.SaveBool(SaveSfxStateId, IsSfxOn);
            _saveService.SaveBool(SaveMusicStateId, IsMusicOn);
        }

        private void Load()
        {
            _sfxAudioSource.volume = PlayerPrefs.HasKey(SaveSfxVolumeId) 
                ? _saveService.LoadFloat(SaveSfxVolumeId) : MaxVolume;

            _musicAudioSource.volume = PlayerPrefs.HasKey(SaveMusicVolumeId)
                ? _saveService.LoadFloat(SaveMusicVolumeId) : MaxVolume;

            IsSfxOn = !PlayerPrefs.HasKey(SaveSfxStateId) || _saveService.LoadBool(SaveSfxStateId);
            
            IsMusicOn = !PlayerPrefs.HasKey(SaveMusicStateId) || _saveService.LoadBool(SaveMusicStateId);
        }

        public void StopMusic()
        {
            _musicAudioSource.Stop();
        }

        private MusicData GetMusicData(MusicType musicType)
        {
            return _musicDataList.Find(data => data.MusicType == musicType);
        }

        private SfxData GetSfxData(SfxType sfxType)
        {
            return _sfxDataList.Find(data => data.SfxType == sfxType);
        }
        
        private void InitializeSfxDataList()
        {
            SfxHolder sfxHolder = _assetProvider.Initialize<SfxHolder>(AssetPath.SfxHolder);

            _sfxDataList.AddRange(sfxHolder.SoundEffects);
        }

        private void InitializeMusicDataList()
        {
            MusicHolder musicHolder = _assetProvider.Initialize<MusicHolder>(AssetPath.MusicHolder);

            _musicDataList.AddRange(musicHolder.Musics);
        }
        
        private void InitializeSfxAudioSource()
        {
            _sfxAudioSource = _baseFactory.CreateBaseWithContainer<AudioSource>(AssetPath.SfxAudioSource);
        }

        private void InitializeMusicAudioSource()
        {
            _musicAudioSource = _baseFactory.CreateBaseWithContainer<AudioSource>(AssetPath.MusicAudioSource);
        }
    }
}