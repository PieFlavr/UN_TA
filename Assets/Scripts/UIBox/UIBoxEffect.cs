using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.UIBox
{
    [CreateAssetMenu(menuName = "UIBox/BaseEffect")]

    public class UIBoxEffect : ScriptableObject
    {
        public bool isActive = true;
        public bool isApplied = false;
        public string effectName;
        public float duration;
        public float duration_timer;
        public List<ScriptableObject> sub_effect;

        public virtual void Apply(UIBoxController controller)
        {
            if (!isActive) return;
            if (isApplied) return;

            isApplied = true;

            duration_timer = duration;

            foreach (var effect in sub_effect)
            {
                if (effect is UIBoxEffect subEffect && subEffect.isActive)
                {
                    subEffect.Apply(controller);
                }
            }
        }
        public virtual void Process(UIBoxController controller)
        {
            if (!isActive) return;
            if (duration != -1) 
            {
                duration_timer -= Time.deltaTime;
                if (duration_timer <= 0)
                {
                    Clear(controller);
                }
            }

            foreach (var effect in sub_effect)
            {
                if (effect is UIBoxEffect subEffect && subEffect.isActive)
                {
                    subEffect.Process(controller);
                }
            }

            
        }
        public virtual void Clear(UIBoxController controller)
        {
            if (!isActive) return;

            isApplied = false;

            foreach (var effect in sub_effect)
            {
                if (effect is UIBoxEffect subEffect && subEffect.isActive)
                {
                    subEffect.Clear(controller);
                }
            }
        }
    }

}

