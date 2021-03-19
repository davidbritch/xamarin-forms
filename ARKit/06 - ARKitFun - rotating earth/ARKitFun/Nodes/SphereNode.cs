using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class SphereNode : SCNNode
    {
        public SphereNode(float size, string filename)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(size, filename),
                Opacity = 0.975f
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float size, string filename)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = UIImage.FromFile(filename);
            material.DoubleSided = true;

            SCNSphere geometry = SCNSphere.Create(size);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
