using UnityEngine;

namespace SlimUI.ModernMenu{
	[ExecuteInEditMode()]
	[System.Serializable]
	public class ThemedUI : MonoBehaviour {

		public ThemedUIData themeController;

		protected virtual void OnSkinUI(){

		}

		public virtual void Awake(){
			OnSkinUI();
		}

		// OnSkinUI is applied once in Awake at runtime.
		// In the editor it runs every frame so theme changes preview instantly.
		public virtual void Update(){
#if UNITY_EDITOR
			OnSkinUI();
#endif
		}
	}
}
