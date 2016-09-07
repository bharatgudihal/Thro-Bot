using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Boss : EnemyBase
    {

        public float Health;
        private Color tempColor = Color.Orange;
        public override Color m_Color
        {
            get
            {
                return tempColor;
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
                return 1000;
            }
        }

        public override Type m_Type
        {
            get
            {
                return Type.Boss;
            }
        }

        protected override float m_MovementSpeed
        {
            get
            {
                return 1;
            }
        }

        protected override float m_Scale
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

        private Vector2 stopPosition;

        private BossShield[] Shields;

        public Boss(Vector2 stopPosition)
        {
            this.stopPosition = stopPosition;
            Health = 120;                      
        }

        public override void InitializeBehaviors()
        {
            _movementBehavior = new BossMovementBehaviour(stopPosition, new Vector2(Texture.Width/2,Texture.Height/2));
            _rotationBehavior = null;
        }

        public float getSpeed()
        {
            return m_MovementSpeed;
        }

        public override void Initialize(Texture2D texture, Vector2 position)
        {
            base.Initialize(texture, position);
           m_Origin = m_Center;
        }

        public void SetBossShield(ref BossShield[] shields)
        {
            Shields = shields;
        }

        public void SetColor(Color color)
        {
            tempColor = color;
        }

		public override void SetOpacity (float opacity) {
			base.SetOpacity (opacity);
			for (int i=0; i<Shields.Length; i++) {
				Shields[i].SetOpacity (opacity);
			}
		}
        
    }

    public class BossMovementBehaviour : LinearMovementBehavior
    {
        Vector2 stopPosition;
        Vector2 spriteCenter;

        public BossMovementBehaviour(Vector2 stopPosition, Vector2 spriteCenter)
        {
            this.stopPosition = stopPosition;
            this.spriteCenter = spriteCenter;
        }

        public override Vector2 Move(Vector2 position, float rotation, float speed)
        {
            if (!(position).Equals(stopPosition))
            {
                position.Y += speed;
            }
            return position;
        }
    }
}
