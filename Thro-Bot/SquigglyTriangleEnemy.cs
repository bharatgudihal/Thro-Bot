using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {
	public class SquigglyTriangleEnemy : EnemyBase {
		public override EnemyBase.Type m_Type { get { return Type.SquigglyTriangle; } }

		protected override float m_Scale { get { return 1f; } }

		protected override float m_MovementSpeed { get { return 1f; } }

		protected override string m_TexturePath { get { return "Graphics/E2"; } }

		public override Color m_Color { get { return Color.White; } }

		public override int m_HurtValue { get { return 5; } }

		public override int m_PointValue { get { return 150; } }

		public override void Initialize(Texture2D texture, Vector2 position) {
			base.Initialize(texture, position);
		}

		public override void InitializeBehaviors() {
			_movementBehavior = new SquigglyMovementBehavior(0.15f, 2f);
			_rotationBehavior = null;
		}
	}
}
