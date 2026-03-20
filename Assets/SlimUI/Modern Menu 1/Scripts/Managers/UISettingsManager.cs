using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class UISettingsManager : MonoBehaviour {

		public enum Platform {Desktop, Mobile};
		public Platform platform;
		[Header("MOBILE SETTINGS")]
		public GameObject mobileSFXtext;
		public GameObject mobileMusictext;
		public GameObject mobileShadowofftextLINE;
		public GameObject mobileShadowlowtextLINE;
		public GameObject mobileShadowhightextLINE;

		[Header("VIDEO SETTINGS")]
		public GameObject fullscreentext;
		public GameObject ambientocclusiontext;
		public GameObject shadowofftextLINE;
		public GameObject shadowlowtextLINE;
		public GameObject shadowhightextLINE;
		public GameObject aaofftextLINE;
		public GameObject aa2xtextLINE;
		public GameObject aa4xtextLINE;
		public GameObject aa8xtextLINE;
		public GameObject vsynctext;
		public GameObject motionblurtext;
		public GameObject texturelowtextLINE;
		public GameObject texturemedtextLINE;
		public GameObject texturehightextLINE;
		public GameObject cameraeffectstext;

		[Header("GAME SETTINGS")]
		public GameObject showhudtext;
		public GameObject tooltipstext;
		public GameObject difficultynormaltext;
		public GameObject difficultynormaltextLINE;
		public GameObject difficultyhardcoretext;
		public GameObject difficultyhardcoretextLINE;

		[Header("CONTROLS SETTINGS")]
		public GameObject invertmousetext;

		// sliders
		public GameObject musicSlider;
		public GameObject sensitivityXSlider;
		public GameObject sensitivityYSlider;
		public GameObject mouseSmoothSlider;

		private float sliderValue = 0.0f;
		private float sliderValueXSensitivity = 0.0f;
		private float sliderValueYSensitivity = 0.0f;
		private float sliderValueSmoothing = 0.0f;

		// Cached component references — avoids GetComponent every frame / per-event call
		private Slider _musicSliderComp;
		private Slider _sensitivityXSliderComp;
		private Slider _sensitivityYSliderComp;
		private Slider _mouseSmoothSliderComp;
		private TMP_Text _fullscreenTxt;
		private TMP_Text _showhudTxt;
		private TMP_Text _tooltipsTxt;
		private TMP_Text _vsyncTxt;
		private TMP_Text _invertmouseTxt;
		private TMP_Text _motionblurTxt;
		private TMP_Text _ambientocclusionTxt;
		private TMP_Text _cameraeffectsTxt;
		private TMP_Text _mobileSFXTxt;
		private TMP_Text _mobileMusicTxt;

		public void Start(){
			// Cache all component references once
			_musicSliderComp        = musicSlider.GetComponent<Slider>();
			_sensitivityXSliderComp = sensitivityXSlider.GetComponent<Slider>();
			_sensitivityYSliderComp = sensitivityYSlider.GetComponent<Slider>();
			_mouseSmoothSliderComp  = mouseSmoothSlider.GetComponent<Slider>();
			_fullscreenTxt          = fullscreentext.GetComponent<TMP_Text>();
			_showhudTxt             = showhudtext.GetComponent<TMP_Text>();
			_tooltipsTxt            = tooltipstext.GetComponent<TMP_Text>();
			_vsyncTxt               = vsynctext.GetComponent<TMP_Text>();
			_invertmouseTxt         = invertmousetext.GetComponent<TMP_Text>();
			_motionblurTxt          = motionblurtext.GetComponent<TMP_Text>();
			_ambientocclusionTxt    = ambientocclusiontext.GetComponent<TMP_Text>();
			_cameraeffectsTxt       = cameraeffectstext.GetComponent<TMP_Text>();
			_mobileSFXTxt           = mobileSFXtext.GetComponent<TMP_Text>();
			_mobileMusicTxt         = mobileMusictext.GetComponent<TMP_Text>();

			// check difficulty
			if(PlayerPrefs.GetInt("NormalDifficulty") == 1){
				difficultynormaltextLINE.gameObject.SetActive(true);
				difficultyhardcoretextLINE.gameObject.SetActive(false);
			}
			else
			{
				difficultyhardcoretextLINE.gameObject.SetActive(true);
				difficultynormaltextLINE.gameObject.SetActive(false);
			}

			// check slider values
			_musicSliderComp.value        = PlayerPrefs.GetFloat("MusicVolume");
			_sensitivityXSliderComp.value = PlayerPrefs.GetFloat("XSensitivity");
			_sensitivityYSliderComp.value = PlayerPrefs.GetFloat("YSensitivity");
			_mouseSmoothSliderComp.value  = PlayerPrefs.GetFloat("MouseSmoothing");

			// check full screen
			_fullscreenTxt.text = Screen.fullScreen ? "on" : "off";

			// check hud value
			_showhudTxt.text = PlayerPrefs.GetInt("ShowHUD") == 0 ? "off" : "on";

			// check tool tip value
			_tooltipsTxt.text = PlayerPrefs.GetInt("ToolTips") == 0 ? "off" : "on";

			// check shadow distance/enabled
			if(platform == Platform.Desktop){
				if(PlayerPrefs.GetInt("Shadows") == 0){
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 0;
					shadowofftextLINE.gameObject.SetActive(true);
					shadowlowtextLINE.gameObject.SetActive(false);
					shadowhightextLINE.gameObject.SetActive(false);
				}
				else if(PlayerPrefs.GetInt("Shadows") == 1){
					QualitySettings.shadowCascades = 2;
					QualitySettings.shadowDistance = 75;
					shadowofftextLINE.gameObject.SetActive(false);
					shadowlowtextLINE.gameObject.SetActive(true);
					shadowhightextLINE.gameObject.SetActive(false);
				}
				else if(PlayerPrefs.GetInt("Shadows") == 2){
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadowDistance = 500;
					shadowofftextLINE.gameObject.SetActive(false);
					shadowlowtextLINE.gameObject.SetActive(false);
					shadowhightextLINE.gameObject.SetActive(true);
				}
			}else if(platform == Platform.Mobile){
				if(PlayerPrefs.GetInt("MobileShadows") == 0){
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 0;
					mobileShadowofftextLINE.gameObject.SetActive(true);
					mobileShadowlowtextLINE.gameObject.SetActive(false);
					mobileShadowhightextLINE.gameObject.SetActive(false);
				}
				else if(PlayerPrefs.GetInt("MobileShadows") == 1){
					QualitySettings.shadowCascades = 2;
					QualitySettings.shadowDistance = 75;
					mobileShadowofftextLINE.gameObject.SetActive(false);
					mobileShadowlowtextLINE.gameObject.SetActive(true);
					mobileShadowhightextLINE.gameObject.SetActive(false);
				}
				else if(PlayerPrefs.GetInt("MobileShadows") == 2){
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadowDistance = 100;
					mobileShadowofftextLINE.gameObject.SetActive(false);
					mobileShadowlowtextLINE.gameObject.SetActive(false);
					mobileShadowhightextLINE.gameObject.SetActive(true);
				}
			}

			// check vsync
			_vsyncTxt.text = QualitySettings.vSyncCount == 0 ? "off" : "on";

			// check mouse inverse
			_invertmouseTxt.text = PlayerPrefs.GetInt("Inverted") == 0 ? "off" : "on";

			// check motion blur
			_motionblurTxt.text = PlayerPrefs.GetInt("MotionBlur") == 0 ? "off" : "on";

			// check ambient occlusion
			_ambientocclusionTxt.text = PlayerPrefs.GetInt("AmbientOcclusion") == 0 ? "off" : "on";

			// check texture quality
			if(PlayerPrefs.GetInt("Textures") == 0){
				QualitySettings.globalTextureMipmapLimit = 2;
				texturelowtextLINE.gameObject.SetActive(true);
				texturemedtextLINE.gameObject.SetActive(false);
				texturehightextLINE.gameObject.SetActive(false);
			}
			else if(PlayerPrefs.GetInt("Textures") == 1){
				QualitySettings.globalTextureMipmapLimit = 1;
				texturelowtextLINE.gameObject.SetActive(false);
				texturemedtextLINE.gameObject.SetActive(true);
				texturehightextLINE.gameObject.SetActive(false);
			}
			else if(PlayerPrefs.GetInt("Textures") == 2){
				QualitySettings.globalTextureMipmapLimit = 0;
				texturelowtextLINE.gameObject.SetActive(false);
				texturemedtextLINE.gameObject.SetActive(false);
				texturehightextLINE.gameObject.SetActive(true);
			}
		}

		public void Update(){
			// Read cached Slider.value directly — no GetComponent per frame
			sliderValueXSensitivity = _sensitivityXSliderComp.value;
			sliderValueYSensitivity = _sensitivityYSliderComp.value;
			sliderValueSmoothing    = _mouseSmoothSliderComp.value;
		}

		public void FullScreen(){
			Screen.fullScreen = !Screen.fullScreen;
			_fullscreenTxt.text = Screen.fullScreen ? "on" : "off";
		}

		public void MusicSlider(){
			PlayerPrefs.SetFloat("MusicVolume", _musicSliderComp.value);
		}

		public void SensitivityXSlider(){
			PlayerPrefs.SetFloat("XSensitivity", sliderValueXSensitivity);
		}

		public void SensitivityYSlider(){
			PlayerPrefs.SetFloat("YSensitivity", sliderValueYSensitivity);
		}

		public void SensitivitySmoothing(){
			PlayerPrefs.SetFloat("MouseSmoothing", sliderValueSmoothing);
			Debug.Log(PlayerPrefs.GetFloat("MouseSmoothing"));
		}

		public void ShowHUD(){
			if(PlayerPrefs.GetInt("ShowHUD")==0){
				PlayerPrefs.SetInt("ShowHUD",1);
				_showhudTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("ShowHUD")==1){
				PlayerPrefs.SetInt("ShowHUD",0);
				_showhudTxt.text = "off";
			}
		}

		public void MobileSFXMute(){
			if(PlayerPrefs.GetInt("Mobile_MuteSfx")==0){
				PlayerPrefs.SetInt("Mobile_MuteSfx",1);
				_mobileSFXTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("Mobile_MuteSfx")==1){
				PlayerPrefs.SetInt("Mobile_MuteSfx",0);
				_mobileSFXTxt.text = "off";
			}
		}

		public void MobileMusicMute(){
			if(PlayerPrefs.GetInt("Mobile_MuteMusic")==0){
				PlayerPrefs.SetInt("Mobile_MuteMusic",1);
				_mobileMusicTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("Mobile_MuteMusic")==1){
				PlayerPrefs.SetInt("Mobile_MuteMusic",0);
				_mobileMusicTxt.text = "off";
			}
		}

		public void ToolTips(){
			if(PlayerPrefs.GetInt("ToolTips")==0){
				PlayerPrefs.SetInt("ToolTips",1);
				_tooltipsTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("ToolTips")==1){
				PlayerPrefs.SetInt("ToolTips",0);
				_tooltipsTxt.text = "off";
			}
		}

		public void NormalDifficulty(){
			difficultyhardcoretextLINE.gameObject.SetActive(false);
			difficultynormaltextLINE.gameObject.SetActive(true);
			PlayerPrefs.SetInt("NormalDifficulty",1);
			PlayerPrefs.SetInt("HardCoreDifficulty",0);
		}

		public void HardcoreDifficulty(){
			difficultyhardcoretextLINE.gameObject.SetActive(true);
			difficultynormaltextLINE.gameObject.SetActive(false);
			PlayerPrefs.SetInt("NormalDifficulty",0);
			PlayerPrefs.SetInt("HardCoreDifficulty",1);
		}

		public void ShadowsOff(){
			PlayerPrefs.SetInt("Shadows",0);
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0;
			shadowofftextLINE.gameObject.SetActive(true);
			shadowlowtextLINE.gameObject.SetActive(false);
			shadowhightextLINE.gameObject.SetActive(false);
		}

		public void ShadowsLow(){
			PlayerPrefs.SetInt("Shadows",1);
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 75;
			shadowofftextLINE.gameObject.SetActive(false);
			shadowlowtextLINE.gameObject.SetActive(true);
			shadowhightextLINE.gameObject.SetActive(false);
		}

		public void ShadowsHigh(){
			PlayerPrefs.SetInt("Shadows",2);
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500;
			shadowofftextLINE.gameObject.SetActive(false);
			shadowlowtextLINE.gameObject.SetActive(false);
			shadowhightextLINE.gameObject.SetActive(true);
		}

		public void MobileShadowsOff(){
			PlayerPrefs.SetInt("MobileShadows",0);
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0;
			mobileShadowofftextLINE.gameObject.SetActive(true);
			mobileShadowlowtextLINE.gameObject.SetActive(false);
			mobileShadowhightextLINE.gameObject.SetActive(false);
		}

		public void MobileShadowsLow(){
			PlayerPrefs.SetInt("MobileShadows",1);
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 75;
			mobileShadowofftextLINE.gameObject.SetActive(false);
			mobileShadowlowtextLINE.gameObject.SetActive(true);
			mobileShadowhightextLINE.gameObject.SetActive(false);
		}

		public void MobileShadowsHigh(){
			PlayerPrefs.SetInt("MobileShadows",2);
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500;
			mobileShadowofftextLINE.gameObject.SetActive(false);
			mobileShadowlowtextLINE.gameObject.SetActive(false);
			mobileShadowhightextLINE.gameObject.SetActive(true);
		}

		public void vsync(){
			if(QualitySettings.vSyncCount == 0){
				QualitySettings.vSyncCount = 1;
				_vsyncTxt.text = "on";
			}
			else if(QualitySettings.vSyncCount == 1){
				QualitySettings.vSyncCount = 0;
				_vsyncTxt.text = "off";
			}
		}

		public void InvertMouse(){
			if(PlayerPrefs.GetInt("Inverted")==0){
				PlayerPrefs.SetInt("Inverted",1);
				_invertmouseTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("Inverted")==1){
				PlayerPrefs.SetInt("Inverted",0);
				_invertmouseTxt.text = "off";
			}
		}

		public void MotionBlur(){
			if(PlayerPrefs.GetInt("MotionBlur")==0){
				PlayerPrefs.SetInt("MotionBlur",1);
				_motionblurTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("MotionBlur")==1){
				PlayerPrefs.SetInt("MotionBlur",0);
				_motionblurTxt.text = "off";
			}
		}

		public void AmbientOcclusion(){
			if(PlayerPrefs.GetInt("AmbientOcclusion")==0){
				PlayerPrefs.SetInt("AmbientOcclusion",1);
				_ambientocclusionTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("AmbientOcclusion")==1){
				PlayerPrefs.SetInt("AmbientOcclusion",0);
				_ambientocclusionTxt.text = "off";
			}
		}

		public void CameraEffects(){
			if(PlayerPrefs.GetInt("CameraEffects")==0){
				PlayerPrefs.SetInt("CameraEffects",1);
				_cameraeffectsTxt.text = "on";
			}
			else if(PlayerPrefs.GetInt("CameraEffects")==1){
				PlayerPrefs.SetInt("CameraEffects",0);
				_cameraeffectsTxt.text = "off";
			}
		}

		public void TexturesLow(){
			PlayerPrefs.SetInt("Textures",0);
			QualitySettings.globalTextureMipmapLimit = 2;
			texturelowtextLINE.gameObject.SetActive(true);
			texturemedtextLINE.gameObject.SetActive(false);
			texturehightextLINE.gameObject.SetActive(false);
		}

		public void TexturesMed(){
			PlayerPrefs.SetInt("Textures",1);
			QualitySettings.globalTextureMipmapLimit = 1;
			texturelowtextLINE.gameObject.SetActive(false);
			texturemedtextLINE.gameObject.SetActive(true);
			texturehightextLINE.gameObject.SetActive(false);
		}

		public void TexturesHigh(){
			PlayerPrefs.SetInt("Textures",2);
			QualitySettings.globalTextureMipmapLimit = 0;
			texturelowtextLINE.gameObject.SetActive(false);
			texturemedtextLINE.gameObject.SetActive(false);
			texturehightextLINE.gameObject.SetActive(true);
		}
	}
}
