using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Thro_Bot
{
    class BossShield:EnemyBase
    {
        private EnemyBase OriginEnemy;

        protected override float m_Scale
        {
            get
            {
                return 1f;
            }
        }

        public override Type m_Type
        {
            get
            {
                return Type.BossShield;
            }
        }

        protected override float m_MovementSpeed
        {
            get
            {
                return 1;
            }
        }

        protected override string m_TexturePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Color m_Color
        {
            get
            {
                return Color.Orange;
            }
        }

        public override int m_HurtValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int m_PointValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float Radius { get; private set; }
        public float RotationSpeed { get; private set; }

        public BossShield(ref EnemyBase originEnemy)
        {
            OriginEnemy = originEnemy;
            Radius = 0;
            RotationSpeed = .05f;           
        }

        public override void Initialize(Texture2D texture, Vector2 position, float rotation)
        {
            base.Initialize(texture, position, rotation);
            m_Origin = m_Center;
        }

        public override void InitializeBehaviors()
        {
            _movementBehavior = null;
            _rotationBehavior = new BossShieldRotationBehaviour(ref OriginEnemy, Radius, RotationSpeed);
        }

        public void startRotation()
        {
            ((BossShieldRotationBehaviour)_rotationBehavior).StartRotation(RotationSpeed);
        }
    }

    public class BossShieldRotationBehaviour : RotationBehaviorBase
    {
        TimeSpan deltaTime = TimeSpan.Zero;
        TimeSpan stopDelay = TimeSpan.FromSeconds(7);

        public BossShieldRotationBehaviour(ref EnemyBase originEnemy, float radius, float rotationSpeed) : base(ref originEnemy, radius, rotationSpeed)
        {        
        }

        public override float Rotate(float rotation,GameTime gameTime)
        {
            rotation -= rotationSpeed;
            if(rotation <= -2 * Math.PI * 5)
            {
                rotation = 0f;
                rotationSpeed = 0;
            }
            return rotation;
        }

        public override Vector2 Move(Vector2 position, float rotation)
        {
            position.X = originEnemy.m_Position.X;
            position.Y = originEnemy.m_Position.Y;
            return position;
        }

        public void StartRotation(float rotationSpeed)
        {
            this.rotationSpeed = rotationSpeed;
        }
    }
}
