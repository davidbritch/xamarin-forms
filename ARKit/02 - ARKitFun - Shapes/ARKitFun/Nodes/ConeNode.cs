using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class ConeNode : SCNNode
    {
        public ConeNode(float topRadius, float bottomRadius, float height, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(topRadius, bottomRadius, height, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float topRadius, float bottomRadius, float height, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNCone geometry = SCNCone.Create(topRadius, bottomRadius, height);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
