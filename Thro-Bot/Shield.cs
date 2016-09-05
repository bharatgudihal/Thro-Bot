using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{

    class Shield : EnemyBase
    {

        private Color m_EnemyColor = Color.Orange;

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
                return Type.Shield;
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

        public float radius = 50f;

        private float rotationSpeed = 0.03f;

        private EnemyBase originEnemy;                

        public Shield(ref EnemyBase originEnemy)
        {
            this.originEnemy = originEnemy;            
        }

        public override void Initialize(Texture2D texture, Vector2 position,float rotation)
        {
           // UseColorCorrect();
            base.Initialize(texture, position,rotation);
            m_Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);            
        }

        public override void InitializeBehaviors()
        {
            _movementBehavior = null;
            _rotationBehavior = new RotationBehaviorBase(ref originEnemy,radius,rotationSpeed) ;
        }
    }
}
