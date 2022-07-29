/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class UI_Update_Text_Float_Suffix : UI_Update_Text_Float
{
	public string suffix;

	protected override void OnSharedDataChange()
	{
		ui_Text.text = sharedDataNotifier.SharedValue.ToString( "f" ) + suffix;
	}
}
