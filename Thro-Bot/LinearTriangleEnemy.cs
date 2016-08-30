using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {
	public class LinearTriangleEnemy : EnemyBase {
		public override EnemyBase.Type m_Type { get { return Type.LinearTriangle; } }

		protected override float m_Scale { get { return 1f; } }

		protected override float m_MovementSpeed { get { return 1f; } }

		protected override string m_TexturePath { get { return "Graphics/E1"; } }

		public override Color m_Color { get { return Color.YellowGreen; } }

		public override int m_HurtValue { get { return 10; } }

		public override int m_PointValue { get { return 100; } }

		public override void Initialize(Texture2D texture, Vector2 position) {
			base.Initialize(texture, position);
		}

		public override void InitializeBehaviors() {
			_movementBehavior = new LinearMovementBehavior();
			_rotationBehavior = null;
		}
	}
}
