/* Created by and for usage of FF Studios (2021). */

namespace FFStudio
{
    public class UI_Update_Text_Float : UI_Update_Text< SharedFloatNotifier, float >
    {
		protected override void OnSharedDataChange()
		{
			ui_Text.text = sharedDataNotifier.SharedValue.ToString( "f" );
		}
    }
}
