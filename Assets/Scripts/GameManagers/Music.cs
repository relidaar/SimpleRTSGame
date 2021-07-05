using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GameManagers
{
    public class Music : MonoBehaviour
    {
        [SerializeField] private AudioClip[] backgroundMusic;

        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private AudioClip defeatMusic;
        
        private Queue<AudioClip> _mix;
        private AudioSource _audio;
        private bool _gameEnd;

        [SerializeField]
        [Range(0f, 1f)]
        private float volume;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _mix = new Queue<AudioClip>();
            Mix();
            Next();

            _gameEnd = false;
        }

        private void Update()
        {
            if (_audio.isPlaying || _gameEnd) return;
            Next();
        }

        private void Next()
        {
            if (_mix.Count <= 0) Mix();
            _audio.clip = _mix.Dequeue();
            StartCoroutine(FadeIn(5, Mathf.SmoothStep));
        }

        private void Mix()
        {
            _mix.Clear();
            var rnd = new Random();
            foreach (var clip in backgroundMusic.OrderBy(x => rnd.Next())) 
                _mix.Enqueue(clip);
        }

        public void PlayVictoryMusic()
        {
            _audio.Pause();
            _audio.clip = victoryMusic;
            _audio.Play();
            _gameEnd = true;
        }

        public void PlayDefeatMusic()
        {
            _audio.Pause();
            _audio.clip = defeatMusic;
            _audio.Play();
            _gameEnd = true;
        }

        public IEnumerator FadeOut(float fadingTime, Func<float, float, float, float> Interpolate)
        {
            float startVolume = _audio.volume;
            float frameCount = fadingTime / Time.deltaTime;
            float framesPassed = 0;

            while (framesPassed <= frameCount)
            {
                var t = framesPassed++ / frameCount;
                _audio.volume = Interpolate(startVolume, 0, t);
                yield return null;
            }

            _audio.volume = 0;
            _audio.Pause();
        }
        
        public IEnumerator FadeIn(float fadingTime, Func<float, float, float, float> Interpolate)
        {
            _audio.Play();
            _audio.volume = 0;

            float resultVolume = volume;
            float frameCount = fadingTime / Time.deltaTime;
            float framesPassed = 0;

            while (framesPassed <= frameCount)
            {
                var t = framesPassed++ / frameCount;
                _audio.volume = Interpolate(0, resultVolume, t);
                yield return null;
            }

            _audio.volume = resultVolume;
        }
    }
}