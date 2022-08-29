using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTheme", menuName = "SkillsComp/Theme Data")]
    public class ThemeData : ScriptableObject
    {
        public Color color;
        public Texture2D logo;
        public StyleSheet styleSheet;
    }
}
