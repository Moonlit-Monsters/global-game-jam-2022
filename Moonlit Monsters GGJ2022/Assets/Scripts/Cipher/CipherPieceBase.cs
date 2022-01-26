using UnityEngine;

public abstract class CipherPieceBase : MonoBehaviour
{
	/** The group this cipher piece is part of */
	public CipherGroup Group {get; set;} = null;

	#if UNITY_EDITOR
		private void OnValidate()
		{
			if (this.Group != null && !this.Group.DoesContain(this))
			{
				this.Group = null;
			}
		}

		/** Draw a link gizmo between this and the given global point
		\param pos The global position to draw a link to
		*/
		private void DrawLinkGizmo(Vector3 pos)
		{
			Gizmos.DrawWireSphere(pos, 1f);
			Gizmos.DrawLine(this.transform.position, pos);
		}

		/** Draw a green link gizmo to the password item of the group */
		private void DrawPasswordGizmo()
		{
			CipherPassword pass = this.Group?.PasswordItem;
			if (pass != null && pass != this)
			{
				Gizmos.color = Color.green;
				this.DrawLinkGizmo(pass.transform.position);
			}
		}

		/** Draw a blue link gizmo to the key item of the group */
		private void DrawKeyGizmo()
		{
			CipherKey key = this.Group?.KeyItem;
			if (key != null && key != this)
			{
				Gizmos.color = Color.blue;
				this.DrawLinkGizmo(key.transform.position);
			}
		}

		/** Draw a red link gizmo to every gate in the same group */
		private void DrawGateGizmos()
		{
			if (this.Group != null)
			{
				Gizmos.color = Color.red;
				foreach (CipherGate gate in this.Group.Gates)
				{
					if (gate != this)
					{
						this.DrawLinkGizmo(gate.transform.position);
					}
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			this.DrawPasswordGizmo();
			this.DrawKeyGizmo();
			this.DrawGateGizmos();
		}
	#endif
}