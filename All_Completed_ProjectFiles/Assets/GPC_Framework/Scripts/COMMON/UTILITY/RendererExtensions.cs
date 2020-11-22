// this script by Michael Garforth in the public domain on http://wiki.unity3d.com/

using UnityEngine;

namespace GPC
{
	public static class RendererExtensions
	{
		public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
		{
			if (camera == null)
				return true;

			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}
}