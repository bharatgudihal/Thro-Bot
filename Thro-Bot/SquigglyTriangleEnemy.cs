using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {
	public class SquigglyTriangleEnemy : EnemyBase {

        private Color m_EnemyColor = Color.Purple;

        public override EnemyBase.Type m_Type { get { return Type.SquigglyTriangle; } }

		protected override float m_Scale { get { return 1f; } }

		protected override float m_MovementSpeed { get { return 1f; } }

		protected override string m_TexturePath { get { return "Graphics/E2"; } }

		public override Color m_Color { get { return m_EnemyColor; } }

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
            m_EnemyColor = ColorCorrector(.05f, m_EnemyColor);
        }

        public override int m_HurtValue { get { return 5; } }

		public override int m_PointValue { get { return 150; } }

		public override void Initialize(Texture2D texture, Vector2 position) {
            UseColorCorrect();
            base.Initialize(texture, position);
		}

		public override void InitializeBehaviors() {
			_movementBehavior = new SquigglyMovementBehavior(0.15f, 5f);
			_rotationBehavior = null;
		}
	}
}
