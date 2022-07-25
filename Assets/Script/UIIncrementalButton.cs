/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using Sirenix.OdinInspector;

public class UIIncrementalButton< Incremental >  : UIEntity where Incremental : struct, IIncrementalData
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] IncrementalSystem< Incremental > incremental_system;
    [ SerializeField ] GameEvent event_incremental_unlock;

  [ Title( "Components" ) ]
	[ SerializeField ] TextMeshProUGUI ui_level;
	[ SerializeField ] TextMeshProUGUI ui_cost;
	[ SerializeField ] Button ui_button;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		DisableInteraction();
	}
#endregion

#region API
	public void ShowButton()
	{
		DisableInteraction();
		Configure();
		GoToTargetPosition( EnableInteraction );
	}

	public void HideButton()
	{
		DisableInteraction();
		Configure();
		GoToTargetPosition();
	}

	public void OnButtonClick()
	{
		incremental_system.Unlock();
		event_incremental_unlock.Raise();
		Configure();
	}
#endregion

#region Implementation
	void Configure()
	{
		ui_button.interactable = incremental_system.CanUnlock();
	}

	void EnableInteraction()
	{
		ui_button.interactable = true;
	}

	void DisableInteraction()
	{
		ui_button.interactable = false;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}