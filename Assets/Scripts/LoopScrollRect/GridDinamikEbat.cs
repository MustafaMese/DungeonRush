using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( GridLayoutGroup ) )]
public class GridDinamikEbat : MonoBehaviour
{
	private GridLayoutGroup gridLayout;
	private LoopScrollRect scrollView;

	private void OnRectTransformDimensionsChange()
	{
		if( gridLayout == null )
			gridLayout = GetComponent<GridLayoutGroup>();

		if( scrollView == null )
			scrollView = GetComponentInParent<LoopScrollRect>();

		Vector2 contentBoyutu = ( (RectTransform) transform ).rect.size;

		int yeniHucreSayisi;
		if( gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount )
			yeniHucreSayisi = (int) ( ( contentBoyutu.x - gridLayout.padding.horizontal ) / ( gridLayout.cellSize.x + gridLayout.spacing.x ) );
		else
			yeniHucreSayisi = (int) ( ( contentBoyutu.y - gridLayout.padding.vertical ) / ( gridLayout.cellSize.y + gridLayout.spacing.y ) );

		if( yeniHucreSayisi < 1 )
			yeniHucreSayisi = 1;

		if( gridLayout.constraintCount != yeniHucreSayisi )
		{
			gridLayout.constraintCount = yeniHucreSayisi;

			scrollView.m_ContentConstraintCountInit = false;
			scrollView.RefillCells();
		}
	}
}