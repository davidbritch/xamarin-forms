using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class TextNode : SCNNode
    {
        public TextNode(string text, float extrusionDepth, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(text, extrusionDepth, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(string text, float extrusionDepth, UIColor color)
        {
            SCNText geometry = SCNText.Create(text, extrusionDepth);
            geometry.Font = UIFont.FromName("Arial", 0.15f);
            geometry.Flatness = 0;
            geometry.FirstMaterial.DoubleSided = true;
            geometry.FirstMaterial.Diffuse.Contents = color;
            return geometry;
        }
    }
}
