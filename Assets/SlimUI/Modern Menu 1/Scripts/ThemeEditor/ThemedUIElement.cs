using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	[System.Serializable]
	public class ThemedUIElement : ThemedUI {
		[Header("Parameters")]
		Color outline;
		Image _image;
		TextMeshPro _tmpText;
		public enum OutlineStyle {solidThin, solidThick, dottedThin, dottedThick};
		public bool hasImage = false;
		public bool isText = false;

		public override void Awake(){
			if(hasImage) _image = GetComponent<Image>();
			if(isText) _tmpText = GetComponent<TextMeshPro>();
			base.Awake();
		}

		protected override void OnSkinUI(){
			base.OnSkinUI();

			if(hasImage && _image != null){
				Color c = themeController.currentColor;
				if(_image.color != c)
					_image.color = c;
			}

			if(isText && _tmpText != null){
				Color c = themeController.textColor;
				if(_tmpText.color != c)
					_tmpText.color = c;
			}
		}
	}
}
