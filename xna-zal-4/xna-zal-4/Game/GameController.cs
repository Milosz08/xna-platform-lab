﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaZal4.Model;

namespace XnaZal4
{
    public class GameController : AbstractGame
    {
        public static readonly float ANGLE_ROTATION_SPEED = 0.02f;                 // szybkość poruszania kamery
        public static readonly Vector3 INIT_CAMERA_ANGLE = new(0.1f, 0.6f, 1.8f);  // początkowy kąt kamery
        public static readonly float BOTTOM_TOLERANCE = 0.1f;                      // blokada zmniejszania siatki
        public static readonly float TOP_TOLERANCE = 15f;                          // blokada zwiększania siatki
        public static readonly float KEYPAD_ROTATE_SPEED = 1f;                     // szybkość obracania ramieniem
        public static readonly float ARM2_MAX_ANGLE = 60f;                         // maksymalne wychylenie ramienia 2
        public static readonly float GRIP_MAX_ANGLE = 90f;                         // maksymalne otwarcie chwytaka

        private Matrix _viewMatrix, _projection;
        private Vector3 _arm1Pos = Vector3.Zero, _arm2Pos = Vector3.Zero;
        private Vector2 _gripPos = Vector2.Zero;
        private Vector2 _grip2Pos = Vector2.Zero;

        private float _angleX = INIT_CAMERA_ANGLE.X;
        private float _angleY = INIT_CAMERA_ANGLE.Y;
        private float _angleZ = INIT_CAMERA_ANGLE.Z;

        public GameController(GameWindow game, GameState state)
            : base(game, state)
        {
        }

        public void Interact()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            ChangeAxisLinesAngle(keyboardState);
            RotateArm1(keyboardState);
            RotateArm2(keyboardState);
            OpenCloseGrips(keyboardState);
            OpenCloseSecondGrips(keyboardState);

            _viewMatrix = Matrix.CreateLookAt(new Vector3(
                _angleZ * 0.1f, _angleZ * 1.0f, _angleZ * 6.0f), Vector3.Zero, Vector3.Up);

            _viewMatrix = Matrix.CreateRotationX(_angleX) *
                Matrix.CreateRotationY(_angleY) * _viewMatrix;

            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50),
                _game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000.0f);

            GenerateEffectMatrix(_state.Arm1);
            GenerateEffectMatrix(_state.Arm2);
            GenerateEffectMatrix(_state.GripLeft);
            GenerateEffectMatrix(_state.GripRight);
            GenerateEffectMatrix(_state.Grip2Left);
            GenerateEffectMatrix(_state.Grip2Right);

            _state.AxisLines.Effect.View = _viewMatrix;
            _state.AxisLines.Effect.Projection = _projection;
        }

        private void GenerateEffectMatrix<T>(ModelEffectBinder<T> binder)
            where T : AbstractCuboidModel
        {
            binder.Effect.View = _viewMatrix;
            binder.Effect.World = binder.Model.GenerateWorldMatrix(this);
            binder.Effect.Projection = _projection;
        }

        private void OpenCloseSecondGrips(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.U))
            {
                if (_grip2Pos.Y < GRIP_MAX_ANGLE)
                {
                    _grip2Pos.Y += KEYPAD_ROTATE_SPEED;
                }
            }
            if (keyboardState.IsKeyDown(Keys.J))
            {
                if (_grip2Pos.Y > 0)
                {
                    _grip2Pos.Y -= KEYPAD_ROTATE_SPEED;
                }
            }
        }

        private void OpenCloseGrips(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Y)) // otwieranie 
            {
                if (_gripPos.Y < GRIP_MAX_ANGLE)
                {
                    _gripPos.Y += KEYPAD_ROTATE_SPEED;
                }
            }
            if (keyboardState.IsKeyDown(Keys.H)) // zamykanie
            {
                if (_gripPos.Y > 0)
                {
                    _gripPos.Y -= KEYPAD_ROTATE_SPEED;
                }
            }
        }

        private void RotateArm1(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W)) // prawo
            {
                IncreaseOvalRotation(ref _arm1Pos.Y);
            }
            if (keyboardState.IsKeyDown(Keys.S)) // lewo
            {
                DecreaseOvalRotation(ref _arm1Pos.Y);
            }
            if (keyboardState.IsKeyDown(Keys.E)) // góra
            {
                IncreaseOvalRotation(ref _arm1Pos.X);
            }
            if (keyboardState.IsKeyDown(Keys.D)) // dół
            {
                DecreaseOvalRotation(ref _arm1Pos.X);
            }
        }

        private void RotateArm2(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.T) && _arm2Pos.X < ARM2_MAX_ANGLE) // góra
            {
                _arm2Pos.X += KEYPAD_ROTATE_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.G) && _arm2Pos.X > -ARM2_MAX_ANGLE) // dół
            {
                _arm2Pos.X -= KEYPAD_ROTATE_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.R)) // obrót prawo
            {
                IncreaseOvalRotation(ref _arm2Pos.Z);
            }
            if (keyboardState.IsKeyDown(Keys.F)) // obrót lewo
            {
                DecreaseOvalRotation(ref _arm2Pos.Z);
            }
        }

        private static void IncreaseOvalRotation(ref float rotation)
        {
            if (rotation >= 359)
            {
                rotation = 0;
            }
            else
            {
                rotation += KEYPAD_ROTATE_SPEED;
            }
        }

        private static void DecreaseOvalRotation(ref float rotation)
        {
            if (rotation <= 0)
            {
                rotation = 359;
            }
            else
            {
                rotation -= KEYPAD_ROTATE_SPEED;
            }
        }

        private void ChangeAxisLinesAngle(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Q) && _angleZ >= BOTTOM_TOLERANCE)
            {
                _angleZ -= ANGLE_ROTATION_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.A) && _angleZ <= TOP_TOLERANCE)
            {
                _angleZ += ANGLE_ROTATION_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _angleY += ANGLE_ROTATION_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _angleY -= ANGLE_ROTATION_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _angleX += ANGLE_ROTATION_SPEED;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                _angleX -= ANGLE_ROTATION_SPEED;
            }
        }

        public void ExitOnEsc()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.Exit();
            }
        }

        public Vector3 Arm1Pos
        {
            get => _arm1Pos;
        }

        public Vector3 Arm2Pos
        {
            get => _arm2Pos;
        }

        public Vector2 GripPos
        {
            get => _gripPos;
        }

        public Vector2 Grip2Pos
        {
            get => _grip2Pos;
        }
    }
}
