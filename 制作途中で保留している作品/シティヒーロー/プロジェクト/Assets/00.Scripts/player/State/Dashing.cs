using UnityEngine;

public partial class PlayerController
{

	public class Dashing : StateBase<PlayerController>
	{
		public Dashing(PlayerController machine) : base(machine) { }

		private float dashForce = 20f;
		private float dashUpwardForce = 3f;
		//private float dashDuration = 0.25f;

		//private float dashCd = 1.5f;
		//private float dashTimer;

		private void Dach()
		{
			Vector3 forceToApply = _machine.transform.forward * dashForce + _machine.transform.up * dashUpwardForce;

			_machine._rigidbody.AddForce(forceToApply, ForceMode.Impulse);

			//Invoke(nameof(ResetDash), dashDuration);
		}

		private void ResetDash()
		{

		}

	}
}
