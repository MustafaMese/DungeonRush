using UnityEngine;

namespace UnityEngine.UI
{
	public abstract class LoopScrollDataSource
	{
		public abstract void ProvideData( Transform transform, int index );
	}

	public class LoopScrollSendIndexSource : LoopScrollDataSource
	{
		private readonly LoopScrollRect scrollView;

		public LoopScrollSendIndexSource( LoopScrollRect scrollView )
		{
			this.scrollView = scrollView;
		}

		public override void ProvideData( Transform transform, int index )
		{
			LoopScrollRectItem item = transform.GetComponent<LoopScrollRectItem>();
			item.CurrentRow = index;
			if( scrollView.ItemCallback != null )
				scrollView.ItemCallback( item, index );
		}
	}

	public class LoopScrollArraySource<T> : LoopScrollDataSource
	{
		private readonly T[] objectsToFill;

		public LoopScrollArraySource( T[] objectsToFill )
		{
			this.objectsToFill = objectsToFill;
		}

		public override void ProvideData( Transform transform, int idx )
		{
			transform.SendMessage( "ScrollCellContent", objectsToFill[idx] );
		}
	}
}