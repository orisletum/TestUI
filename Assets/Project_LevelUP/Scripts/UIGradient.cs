using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class UIGradient : BaseMeshEffect
{
    [SerializeField] private List<Color> colors = new List<Color> { Color.white, Color.white };
    [Range(-180f, 180f)] public float angle = 0f;
    public bool ignoreRatio = true;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || colors.Count < 2)
            return;

        Rect rect = graphic.rectTransform.rect;
        Vector2 dir = UIGradientUtils.RotationDir(angle);

        if (!ignoreRatio)
            dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

        var localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

        UIVertex vertex = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            Vector2 localPosition = localPositionMatrix * vertex.position;
            float t = Mathf.Clamp01(localPosition.y);
            vertex.color *= GetGradientColor(colors, t);
            vh.SetUIVertex(vertex, i);
        }
    }

    private Color GetGradientColor(List<Color> colorList, float t)
    {
        int segmentCount = colorList.Count - 1;
        float segmentSize = 1f / segmentCount;
        int index = Mathf.Min(Mathf.FloorToInt(t * segmentCount), segmentCount - 1);
        float normalizedT = (t - index * segmentSize) / segmentSize;
        return Color.Lerp(colorList[index], colorList[index + 1], normalizedT);
    }
}