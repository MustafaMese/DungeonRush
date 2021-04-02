using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class LoopScrollRectItem : MonoBehaviour
	{
		[System.NonSerialized]
		public int CurrentRow;
	}

	[System.Serializable]
	public class LoopScrollPrefabSource
	{
		public Transform prefab;
		public int havuzIlkBoyutu = 5;

		private Stack<Transform> havuz;

		public Transform GetObject()
		{
			if( havuz == null )
			{
				havuz = new Stack<Transform>( havuzIlkBoyutu );

				for( int i = 0; i < havuzIlkBoyutu; i++ )
				{
					Transform instance = Object.Instantiate( prefab, null, false );
					instance.gameObject.SetActive( false );

					havuz.Push( instance );
				}
			}

			if( havuz.Count == 0 )
				return Object.Instantiate( prefab, null, false );
			else
			{
				Transform instance = havuz.Pop();
				instance.gameObject.SetActive( true );
				return instance;
			}
		}

		public void ReturnObject( Transform go )
		{
			go.gameObject.SetActive( false );
			go.SetParent( null, false );

			havuz.Push( go );
		}
	}
}