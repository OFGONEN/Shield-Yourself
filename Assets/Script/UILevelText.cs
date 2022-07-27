/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using TMPro;
using DG.Tweening;

public class UILevelText : MonoBehaviour
{
#region Fields
    [ SerializeField ] TextMeshProUGUI ui_level_text;
    [ SerializeField ] float punchScale;

    RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		SetLevelText();
	}
#endregion

#region API
    public void SetLevelText()
    {
        ui_level_text.text = "Level " + CurrentLevelData.Instance.currentLevel_Shown;
    }

    public void OnLevelCompletePseudo()
    {
		SetLevelText();
		recycledTween.Recycle( ui_level_text.rectTransform.DOPunchScale( Vector3.one * punchScale, GameSettings.Instance.ui_Entity_Scale_TweenDuration, 1, 1 ) );
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
