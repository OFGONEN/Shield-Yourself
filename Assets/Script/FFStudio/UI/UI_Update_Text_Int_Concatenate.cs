/* Created by and for usage of FF Studios (2021). */

namespace FFStudio
{
    public class UI_Update_Text_Int_Prefix : UI_Update_Text< SharedIntNotifier, int >
    {
		public string prefix = "";
		public string suffix = "";

		protected override void OnSharedDataChange()
		{
			ui_Text.text = prefix + sharedDataNotifier.SharedValue.ToString() + suffix;
		}
    }
}