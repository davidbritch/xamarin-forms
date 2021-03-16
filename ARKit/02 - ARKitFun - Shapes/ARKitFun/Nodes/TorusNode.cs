using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class TorusNode : SCNNode
    {
        public TorusNode(float ringRadius, float pipeRadius, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(ringRadius, pipeRadius, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float ringRadius, float pipeRadius, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNTorus geometry = SCNTorus.Create(ringRadius, pipeRadius);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
