using CropCalculator.Enums;
using CropCalculator.Render.Filters;
using Microsoft.Xna.Framework;

namespace CropCalculator.Entities.UI
{
    internal class Tablet : Entity
    {
        public Tablet(
            Origin origin,
            Vector2 anchor,
            float layerDepth,
            IFilter enteringTransition,
            IFilter exitingTransition
        ) : base(
            origin,
            anchor,
            layerDepth,
            enteringTransition,
            exitingTransition
        )
        {

        }

        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override float GetTotalHeight()
        {
            throw new NotImplementedException();
        }

        public override float GetTotalWidth()
        {
            throw new NotImplementedException();
        }
    }
}
