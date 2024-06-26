using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Internal
{
    [Serializable]
    public struct ShaderGraphRequirements
    {
        [SerializeField] List<NeededTransform> m_RequiresTransforms;
        [SerializeField] NeededCoordinateSpace m_RequiresNormal;
        [SerializeField] NeededCoordinateSpace m_RequiresBitangent;
        [SerializeField] NeededCoordinateSpace m_RequiresTangent;
        [SerializeField] NeededCoordinateSpace m_RequiresViewDir;
        [SerializeField] NeededCoordinateSpace m_RequiresPosition;
        [SerializeField] NeededCoordinateSpace m_RequiresPositionPredisplacement;
        [SerializeField] bool m_RequiresScreenPosition;
        [SerializeField] bool m_RequiresNDCPosition;
        [SerializeField] bool m_RequiresPixelPosition;
        [SerializeField] bool m_RequiresVertexColor;
        [SerializeField] bool m_RequiresFaceSign;
        [SerializeField] List<UVChannel> m_RequiresMeshUVs;
        [SerializeField] bool m_RequiresDepthTexture;
        [SerializeField] bool m_RequiresCameraOpaqueTexture;
        [SerializeField] bool m_RequiresTime;
        [SerializeField] bool m_RequiresVertexSkinning;
        [SerializeField] bool m_RequiresVertexID;
        [SerializeField] List<UVChannel> m_RequiresMeshUVDerivatives;

        internal static ShaderGraphRequirements none
        {
            get
            {
                return new ShaderGraphRequirements
                {
                    m_RequiresTransforms = new List<NeededTransform>(),
                    m_RequiresMeshUVs = new List<UVChannel>(),
                    m_RequiresMeshUVDerivatives = new List<UVChannel>()
                };
            }
        }

        public List<NeededTransform> requiresTransforms
        {
            get { return m_RequiresTransforms; }
            internal set { m_RequiresTransforms = value; }
        }

        public NeededCoordinateSpace requiresNormal
        {
            get { return m_RequiresNormal; }
            internal set { m_RequiresNormal = value; }
        }

        public NeededCoordinateSpace requiresBitangent
        {
            get { return m_RequiresBitangent; }
            internal set { m_RequiresBitangent = value; }
        }

        public NeededCoordinateSpace requiresTangent
        {
            get { return m_RequiresTangent; }
            internal set { m_RequiresTangent = value; }
        }

        public NeededCoordinateSpace requiresViewDir
        {
            get { return m_RequiresViewDir; }
            internal set { m_RequiresViewDir = value; }
        }

        public NeededCoordinateSpace requiresPosition
        {
            get { return m_RequiresPosition; }
            internal set { m_RequiresPosition = value; }
        }

        public NeededCoordinateSpace requiresPositionPredisplacement
        {
            get { return m_RequiresPositionPredisplacement; }
            internal set { m_RequiresPositionPredisplacement = value; }
        }

        public bool requiresScreenPosition
        {
            get { return m_RequiresScreenPosition; }
            internal set { m_RequiresScreenPosition = value; }
        }

        public bool requiresNDCPosition
        {
            get { return m_RequiresNDCPosition; }
            internal set { m_RequiresNDCPosition = value; }
        }

        public bool requiresPixelPosition
        {
            get { return m_RequiresPixelPosition; }
            internal set { m_RequiresPixelPosition = value; }
        }

        public bool requiresVertexColor
        {
            get { return m_RequiresVertexColor; }
            internal set { m_RequiresVertexColor = value; }
        }

        public bool requiresFaceSign
        {
            get { return m_RequiresFaceSign; }
            internal set { m_RequiresFaceSign = value; }
        }

        public List<UVChannel> requiresMeshUVs
        {
            get { return m_RequiresMeshUVs; }
            internal set { m_RequiresMeshUVs = value; }
        }

        public List<UVChannel> requiresMeshUVDerivatives
        {
            get { return m_RequiresMeshUVDerivatives; }
            internal set { m_RequiresMeshUVDerivatives = value; }
        }

        public bool requiresDepthTexture
        {
            get { return m_RequiresDepthTexture; }
            internal set { m_RequiresDepthTexture = value; }
        }

        public bool requiresCameraOpaqueTexture
        {
            get { return m_RequiresCameraOpaqueTexture; }
            internal set { m_RequiresCameraOpaqueTexture = value; }
        }

        public bool requiresTime
        {
            get { return m_RequiresTime; }
            internal set { m_RequiresTime = value; }
        }

        public bool requiresVertexSkinning
        {
            get { return m_RequiresVertexSkinning; }
            internal set { m_RequiresVertexSkinning = value; }
        }

        public bool requiresVertexID
        {
            get { return m_RequiresVertexID; }
            internal set { m_RequiresVertexID = value; }
        }

        internal bool NeedsTangentSpace()
        {
            var compoundSpaces = m_RequiresBitangent | m_RequiresNormal | m_RequiresPosition
                | m_RequiresTangent | m_RequiresViewDir | m_RequiresPosition
                | m_RequiresNormal;

            return (compoundSpaces & NeededCoordinateSpace.Tangent) > 0;
        }

        internal ShaderGraphRequirements Union(ShaderGraphRequirements other)
        {
            var newReqs = new ShaderGraphRequirements();
            newReqs.m_RequiresNormal = other.m_RequiresNormal | m_RequiresNormal;
            newReqs.m_RequiresTangent = other.m_RequiresTangent | m_RequiresTangent;
            newReqs.m_RequiresBitangent = other.m_RequiresBitangent | m_RequiresBitangent;
            newReqs.m_RequiresViewDir = other.m_RequiresViewDir | m_RequiresViewDir;
            newReqs.m_RequiresPosition = other.m_RequiresPosition | m_RequiresPosition;
            newReqs.m_RequiresPositionPredisplacement = other.m_RequiresPositionPredisplacement | m_RequiresPositionPredisplacement;
            newReqs.m_RequiresScreenPosition = other.m_RequiresScreenPosition | m_RequiresScreenPosition;
            newReqs.m_RequiresNDCPosition = other.m_RequiresNDCPosition | m_RequiresNDCPosition;
            newReqs.m_RequiresPixelPosition = other.m_RequiresPixelPosition | m_RequiresPixelPosition;
            newReqs.m_RequiresVertexColor = other.m_RequiresVertexColor | m_RequiresVertexColor;
            newReqs.m_RequiresFaceSign = other.m_RequiresFaceSign | m_RequiresFaceSign;
            newReqs.m_RequiresDepthTexture = other.m_RequiresDepthTexture | m_RequiresDepthTexture;
            newReqs.m_RequiresCameraOpaqueTexture = other.m_RequiresCameraOpaqueTexture | m_RequiresCameraOpaqueTexture;
            newReqs.m_RequiresTime = other.m_RequiresTime | m_RequiresTime;
            newReqs.m_RequiresVertexSkinning = other.m_RequiresVertexSkinning | m_RequiresVertexSkinning;
            newReqs.m_RequiresVertexID = other.m_RequiresVertexID | m_RequiresVertexID;

            newReqs.m_RequiresMeshUVs = new List<UVChannel>();
            if (m_RequiresMeshUVs != null)
                newReqs.m_RequiresMeshUVs.AddRange(m_RequiresMeshUVs);
            if (other.m_RequiresMeshUVs != null)
                newReqs.m_RequiresMeshUVs.AddRange(other.m_RequiresMeshUVs);

            newReqs.m_RequiresMeshUVDerivatives = new List<UVChannel>();
            if (m_RequiresMeshUVDerivatives != null)
                newReqs.m_RequiresMeshUVDerivatives.AddRange(m_RequiresMeshUVDerivatives);
            if (other.m_RequiresMeshUVDerivatives != null)
                newReqs.m_RequiresMeshUVDerivatives.AddRange(other.m_RequiresMeshUVDerivatives);

            return newReqs;
        }

        internal static ShaderGraphRequirements FromNodes<T>(List<T> nodes, ShaderStageCapability stageCapability = ShaderStageCapability.All, bool includeIntermediateSpaces = true, bool[] texCoordNeedsDerivs = null)
            where T : AbstractMaterialNode
        {
            var requiresTransforms = nodes.OfType<IMayRequireTransform>().SelectMany(o => o.RequiresTransform()).Distinct().ToList();
            NeededCoordinateSpace requiresNormal = nodes.OfType<IMayRequireNormal>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresNormal(stageCapability));
            NeededCoordinateSpace requiresBitangent = nodes.OfType<IMayRequireBitangent>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresBitangent(stageCapability));
            NeededCoordinateSpace requiresTangent = nodes.OfType<IMayRequireTangent>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresTangent(stageCapability));
            NeededCoordinateSpace requiresViewDir = nodes.OfType<IMayRequireViewDirection>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresViewDirection(stageCapability));
            NeededCoordinateSpace requiresPosition = nodes.OfType<IMayRequirePosition>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresPosition(stageCapability));
            NeededCoordinateSpace requiresPredisplacement = nodes.OfType<IMayRequirePositionPredisplacement>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresPositionPredisplacement(stageCapability));
            bool requiresScreenPosition = nodes.OfType<IMayRequireScreenPosition>().Any(x => x.RequiresScreenPosition(stageCapability));
            bool requiresNDCPosition = nodes.OfType<IMayRequireNDCPosition>().Any(x => x.RequiresNDCPosition(stageCapability));
            bool requiresPixelPosition = nodes.OfType<IMayRequirePixelPosition>().Any(x => x.RequiresPixelPosition(stageCapability));
            bool requiresVertexColor = nodes.OfType<IMayRequireVertexColor>().Any(x => x.RequiresVertexColor(stageCapability));
            bool requiresFaceSign = nodes.OfType<IMayRequireFaceSign>().Any(x => x.RequiresFaceSign());
            bool requiresDepthTexture = nodes.OfType<IMayRequireDepthTexture>().Any(x => x.RequiresDepthTexture());
            bool requiresCameraOpaqueTexture = nodes.OfType<IMayRequireCameraOpaqueTexture>().Any(x => x.RequiresCameraOpaqueTexture());
            bool requiresTime = nodes.Any(x => x.RequiresTime());
            bool requiresVertexSkinning = nodes.OfType<IMayRequireVertexSkinning>().Any(x => x.RequiresVertexSkinning(stageCapability));
            bool requiresVertexID = nodes.OfType<IMayRequireVertexID>().Any(x => x.RequiresVertexID(stageCapability));

            var meshUV = new List<UVChannel>();
            var meshUVDerivatives = new List<UVChannel>();
            for (int uvIndex = 0; uvIndex < ShaderGeneratorNames.UVCount; ++uvIndex)
            {
                var channel = (UVChannel)uvIndex;
                if (nodes.OfType<IMayRequireMeshUV>().Any(x => x.RequiresMeshUV(channel)))
                {
                    meshUV.Add(channel);
                    if (texCoordNeedsDerivs is not null &&
                        uvIndex < texCoordNeedsDerivs.Length &&
                        texCoordNeedsDerivs[uvIndex])
                    {
                        meshUVDerivatives.Add(channel);
                    }
                }
            }

            // if anything needs tangentspace we have make
            // sure to have our othonormal basis!
            if (includeIntermediateSpaces)
            {
                var compoundSpaces = requiresBitangent | requiresNormal | requiresPosition
                    | requiresTangent | requiresViewDir | requiresPosition
                    | requiresNormal;

                var needsTangentSpace = (compoundSpaces & NeededCoordinateSpace.Tangent) > 0;
                if (needsTangentSpace)
                {
                    requiresBitangent |= NeededCoordinateSpace.World;
                    requiresNormal |= NeededCoordinateSpace.World;
                    requiresTangent |= NeededCoordinateSpace.World;
                }
            }

            var reqs = new ShaderGraphRequirements()
            {
                m_RequiresTransforms = requiresTransforms,
                m_RequiresNormal = requiresNormal,
                m_RequiresBitangent = requiresBitangent,
                m_RequiresTangent = requiresTangent,
                m_RequiresViewDir = requiresViewDir,
                m_RequiresPosition = requiresPosition,
                m_RequiresPositionPredisplacement = requiresPredisplacement,
                m_RequiresScreenPosition = requiresScreenPosition,
                m_RequiresNDCPosition = requiresNDCPosition,
                m_RequiresPixelPosition = requiresPixelPosition,
                m_RequiresVertexColor = requiresVertexColor,
                m_RequiresFaceSign = requiresFaceSign,
                m_RequiresMeshUVs = meshUV,
                m_RequiresMeshUVDerivatives = meshUVDerivatives,
                m_RequiresDepthTexture = requiresDepthTexture,
                m_RequiresCameraOpaqueTexture = requiresCameraOpaqueTexture,
                m_RequiresTime = requiresTime,
                m_RequiresVertexSkinning = requiresVertexSkinning,
                m_RequiresVertexID = requiresVertexID,
            };

            return reqs;
        }

        internal static ShaderGraphRequirements FromUvDerivativeList(List<UVChannel> meshUVDerivatives)
        {
            var reqs = new ShaderGraphRequirements()
            {
                m_RequiresMeshUVDerivatives = meshUVDerivatives,
            };

            return reqs;
        }
    }
}
