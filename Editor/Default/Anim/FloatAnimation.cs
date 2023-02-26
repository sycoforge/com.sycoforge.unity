using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.AnimatedValues;

namespace ch.sycoforge.Unity.Editor.Anim
{
    public class FloatAnimation : AnimFloat
    {
        public FloatAnimation(float value) : base(value)
        {

        }

        public void Start(float target)
        {
            BeginAnimating(target, value);
        }

        public void Stop()
        {
            StopAnim(value);
        }
    }
}
