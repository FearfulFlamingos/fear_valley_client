using UnityEngine;

namespace Scripts.Controller
{
    public interface IPlayerSpotlight
    {
        void DeactivateCurrentFocus();
        void SetCharacterSelect(bool state);
        void SpotlightChar(GameObject current);
    }
}