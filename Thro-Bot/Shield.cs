using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Thro_Bot
{

    class Shield : EnemyBase
    {        

        public override Color m_Color
        {
            get
            {
                return Color.White;
            }
        }

        public override int m_HurtValue
        {
            get
            {
                return 0;
            }
        }

        public override int m_PointValue
        {
            get
            {
                return 100;
            }
        }

        public override Type m_Type
        {
            get
            {
                return Type.Hexagon;
            }
        }

        protected override float m_MovementSpeed
        {
            get
            {
                return 1f;
            }
        }

        protected override float m_Scale
        {
            get
            {
                return 1f;
            }
        }

        protected override string m_TexturePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private float radius = 50f;

        private float rotationSpeed = 0.05f;

        private EnemyBase originEnemy;

            public Shield(ref EnemyBase originEnemy)
        {
            this.originEnemy = originEnemy;
        }

        public override void InitializeBehaviors()
        {
            _movementBehavior = null;
            _rotationBehavior = new RotationBehaviorBase(ref originEnemy,radius,rotationSpeed) ;
        }
    }
}
