﻿using Microsoft.Xna.Framework;

namespace XnaZal4.Model
{
    public class GripModel : AbstractCuboidModel
    {
        public static readonly float GRIP_1_LENGTH = 0.8f;

        private readonly int _multipier;

        public GripModel(int multipier)
            : base(GRIP_1_LENGTH, 0.05f)
        {
            _multipier = multipier;
        }

        public GripModel()
            : this(1)
        {
        }

        public override Matrix GenerateWorldMatrix(GameController controller)
        {
            return Matrix.CreateRotationY(MathHelper.ToRadians(controller.GripPos.Y * _multipier))
                * Matrix.CreateTranslation(0, 0, Arm2Model.ARM_2_LENGTH)
                * Matrix.CreateRotationZ(MathHelper.ToRadians(controller.Arm2Pos.Z))
                * Matrix.CreateRotationX(MathHelper.ToRadians(controller.Arm2Pos.X))
                * Matrix.CreateTranslation(0, 0, Arm1Model.ARM_1_LENGTH)
                * Matrix.CreateRotationX(MathHelper.ToRadians(controller.Arm1Pos.X))
                * Matrix.CreateRotationY(MathHelper.ToRadians(controller.Arm1Pos.Y));
        }
    }
}
