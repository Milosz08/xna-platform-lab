﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaZal4.Model
{
    public abstract class AbstractCuboidModel
        : AbstractMeshModel<VertexPositionNormalTexture>
    {
        protected readonly float _length;
        protected readonly float _baseSize;

        private readonly short[] _cuboidIndices = new short[36]
        {
            0, 1, 2, 2, 3, 0, // przód
            4, 5, 6, 6, 7, 4, // tył
            3, 2, 6, 6, 7, 3, // góra
            0, 1, 5, 5, 4, 0, // dół
            1, 5, 6, 6, 2, 1, // prawo
            0, 4, 7, 7, 3, 0  // lewo
        };

        protected AbstractCuboidModel(float length, float baseSize)
            : base(8)
        {
            _length = length;
            _baseSize = baseSize;
            InitMeshStructure();
        }

        protected override void InitMeshStructure()
        {
            Vector3 normal = Vector3.Normalize(new Vector3(0, 0, -1));
            _vertices = new VertexPositionNormalTexture[8]
            {
                new VertexPositionNormalTexture(new Vector3(0, -_baseSize, -_baseSize), normal, new Vector2(0, 1)),           // lewy dolny tył
                new VertexPositionNormalTexture(new Vector3(_length, -_baseSize, -_baseSize), normal, new Vector2(0, 0)),     // lewy dolny przód
                new VertexPositionNormalTexture(new Vector3(_length, _baseSize, -_baseSize), normal, new Vector2(1, 0)),      // lewy górny przód
                new VertexPositionNormalTexture(new Vector3(0, _baseSize, -_baseSize), normal, new Vector2(1, 1)),            // lewy górny tył
                new VertexPositionNormalTexture(new Vector3(0, -_baseSize, _baseSize), normal, new Vector2(0, 1)),            // prawy dolny tył
                new VertexPositionNormalTexture(new Vector3(_length, -_baseSize, _baseSize), normal, new Vector2(0, 0)),      // prawy dolny przód
                new VertexPositionNormalTexture(new Vector3(_length, _baseSize, _baseSize), normal, new Vector2(1, 0)),       // prawy górny przód
                new VertexPositionNormalTexture(new Vector3(0, _baseSize, _baseSize), normal, new Vector2(1, 1)),             // prawy górny tył
            };
        }

        public abstract Matrix GenerateWorldMatrix(GameController controller);

        public short[] CuboidIndices
        {
            get => _cuboidIndices;
        }
    }
}