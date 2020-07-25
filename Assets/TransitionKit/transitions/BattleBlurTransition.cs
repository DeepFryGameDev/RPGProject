using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.SceneManagement;


namespace Prime31.TransitionKit
{
	public class BattleBlurTransition : TransitionKitDelegate
	{
		public float duration = 0.5f;
		public int nextScene = -1;
		public float blurMin = 0.0f;
		public float blurMax = 0.01f;

        float crossfadeTransitionTime = 2f;
        Animator crossfadeTransition;

        AudioSource BGS;
        AudioManager AM;

        #region TransitionKitDelegate implementation

        public Shader shaderForTransition()
		{
			return Shader.Find( "prime[31]/Transitions/Blur" );
		}


		public Mesh meshForDisplay()
		{
			return null;
		}


		public Texture2D textureForDisplay()
		{
			return null;
		}


		public IEnumerator onScreenObscured( TransitionKit transitionKit )
		{
            BGS = GameObject.Find("GameManager/BGS").GetComponent<AudioSource>();
            AM = GameObject.Find("GameManager").GetComponent<AudioManager>();

			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;

			if( nextScene >= 0 )
				SceneManager.LoadSceneAsync( nextScene );

            BGS.clip = AM.battleTransition;
            BGS.Play();

            var elapsed = 0f;
			while( elapsed < duration )
			{
				elapsed += transitionKit.deltaTime;
				var step = Mathf.Pow( elapsed / duration, 2f );
				var blurAmount = Mathf.Lerp( blurMin, blurMax, step );

				transitionKit.material.SetFloat( "_BlurSize", blurAmount );

				yield return null;
			}

            // we dont transition back to the new scene unless it is loaded
            if (nextScene >= 0)
            {
                //modify between here

                crossfadeTransition = GameObject.Find("GameManager/SceneLoader/Crossfade").GetComponent<Animator>();

                crossfadeTransition.SetTrigger("Start");

                yield return new WaitForSeconds(crossfadeTransitionTime);

                //and here

                yield return transitionKit.StartCoroutine( transitionKit.waitForLevelToLoad( nextScene ) );

                //here is where to enable battle start

                GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().alpha = 1;
                GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().interactable = true;
                GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().blocksRaycasts = true;

                GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject hero in heroes)
                {
                    HeroStateMachine HSM = hero.GetComponent<HeroStateMachine>();
                    HSM.currentState = HeroStateMachine.TurnState.PROCESSING;
                }

                foreach (GameObject enemy in enemies)
                {
                    EnemyStateMachine ESM = enemy.GetComponent<EnemyStateMachine>();
                    ESM.currentState = TurnState.PROCESSING;
                }
            }
        }

		#endregion

	}
}