﻿namespace CropCalculator.Render.Filters.Animations
{
    internal abstract class Animation : Filter
    {
        private int _intervalLength;

        public Animation(string key, int intervalLength) : base(key)
        {
            this._intervalLength = intervalLength;
            this.SetKey(key);
            this.StartFilter();
        }

        public override string GetName()
        {
            return "portrait-fire-animation";
        }

        protected float GetProgress()
        {
            float progress = (float)Utilities.Timer.CheckTimer(this._key) / this._intervalLength;

            if (progress > 1f)
            {
                Utilities.Timer.EndTimer(this._key);
                this.StartFilter();
                return 1f;
            }
            return progress;
        }
    }
}
