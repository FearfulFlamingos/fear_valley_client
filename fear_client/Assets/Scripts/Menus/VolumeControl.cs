using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Menus
{
    /// <summary>
    /// Controls the volume of sounds within the game. 
    /// </summary>
    /// <remarks>This is not actually used in the game.</remarks>
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer mixer;
        public channel dropdown = channel.MasterVolume;

        /// <summary>
        /// Choose the channel to change.
        /// </summary>
        public enum channel
        {
            /// <summary>Master Volume.</summary>
            MasterVolume,
            /// <summary>Background Music Volume.</summary>
            BGMVolume,
            /// <summary>Sound Effects Volume.</summary>
            SFXVolume
        };
        
        /// <summary>
        /// Set the level of the mixed channel.
        /// </summary>
        /// <remarks>This is only called from the sliders, which input a value.</remarks>
        /// <param name="sliderValue">Value to set it to.</param>
        public void SetLevel(float sliderValue)
        {
            mixer.SetFloat(dropdown.ToString(), Mathf.Log10(sliderValue) * 20);
        }
    }
}