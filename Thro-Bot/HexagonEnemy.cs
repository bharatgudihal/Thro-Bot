using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Thro_Bot
{
      
    class HexagonEnemy : EnemyBase
    {
        public Shield shield;
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
                return 10;
            }
        }

        public override int m_PointValue
        {
            get
            {
                return 300;
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

        public override void InitializeBehaviors()
        {
            _movementBehavior = new LinearMovementBehavior();
            _rotationBehavior = null;
        }
    }
}
