/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Shield : MonoBehaviour
{
#region Fields
  [ Title( "Components" ) ]
    [ SerializeField ] Collider shield_collider;

  [ Title( "Configure" ) ]
    [ SerializeField ] float delta_horizontal;
    [ SerializeField ] float delta_vertical;
    [ SerializeField ] float delta_rotation;
    [ SerializeField ] AnimationCurve delta_curve;

  [ Title( "Default" ) ]
    [ LabelText( "Default Local Position" ), SerializeField ] Vector3 default_position; // Local
    [ LabelText( "Default Local Rotation" ), SerializeField ] Vector3 default_rotation; // Local

    float movement_progress;
	Vector2Message onInputUpdate;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onInputUpdate = ExtensionMethods.EmptyMethod;
	}
#endregion

#region API
    public void OnShieldActivate()
    {
		movement_progress = 0;
		shield_collider.enabled = true;

		RePosition();

		onInputUpdate = InputUpdate;
	}

    public void OnShieldDeactivate()
    {
		shield_collider.enabled = false;
		onInputUpdate = ExtensionMethods.EmptyMethod;
	}

    public void OnTrigger( Collider collider )
    {
        //todo play shield hit vfx
    }

    public void OnInputUpdate( Vector2GameEvent gameEvent )
    {
		onInputUpdate( gameEvent.eventValue );
	}
#endregion

#region Implementation
    void RePosition()
    {
		var cofactor = delta_curve.Evaluate( movement_progress );
		var position = default_position + new Vector3( delta_horizontal * cofactor, delta_vertical * cofactor, 0 );
		var rotation = default_rotation + Vector3.right * ( delta_rotation * cofactor );

		transform.localPosition    = position;
		transform.localEulerAngles = rotation;
	}

    void InputUpdate( Vector2 input )
    {
		movement_progress = Mathf.Clamp( movement_progress + input.y * GameSettings.Instance.shield_movement_speed * Time.deltaTime, 0, 1 );
		RePosition();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
