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
        public Shield shield1;
        public Shield shield2;

        private Color m_EnemyColor = Color.Orange;

        public void setShield1(ref EnemyBase shield)
        {
            shield1 = (Shield)shield;
        }
        public void setShield2(ref EnemyBase shield)
        {
            shield2 = (Shield)shield;
        }
        public override Color m_Color
        {
            get
            {
                return m_EnemyColor;
            }
        }


        private Color ColorCorrector(float correctionFactor, Color color)
        {
            float red = (255 - color.R) * correctionFactor + color.R;
            float green = (255 - color.G) * correctionFactor + color.G;
            float blue = (255 - color.B) * correctionFactor + color.B;
            color = new Color((int)red, (int)green, (int)blue, 1);
            return color;
        }

        public void SetColor(Color color)
        {
            m_EnemyColor = color;
        }

        private void UseColorCorrect()
        {
            m_EnemyColor = ColorCorrector(0f, m_EnemyColor);
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
           // UseColorCorrect();
            _movementBehavior = new LinearMovementBehavior();
            _rotationBehavior = null;
        }
    }
}
