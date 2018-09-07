﻿using DearMogwai.Application.Engine.Subsystems;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DearMogwai.Application
{
    public class MogwaiEngine : Engine.Engine
    {
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public MogwaiEngine()
            : base()
        {
            Name = "DearMogwai - A crossplatform World of Mogwai client";
            new FrameTimer(this);
            new ResourceLoader(this);
            new Sdl2Renderer(this);
            new Input(this);
            new Ui(this);
            new SceneManager(this, "sad.binary");
        }
    }

    //public class DearMogwai : SampleApplication
    //{
    //    private readonly ProcessedTexture _stoneTexData;

    //    private readonly VertexPositionTexture[] _vertices;
    //    private readonly ushort[] _indices;
    //    private DeviceBuffer _projectionBuffer;
    //    private DeviceBuffer _viewBuffer;
    //    private DeviceBuffer _worldBuffer;
    //    private DeviceBuffer _vertexBuffer;
    //    private DeviceBuffer _indexBuffer;
    //    private CommandList _cl;
    //    private Texture _surfaceTexture;
    //    private TextureView _surfaceTextureView;
    //    private ResourceSet _projViewSet;
    //    private ResourceSet _worldTextureSet;

    //    public DearMogwai(IApplicationWindow window) : base(window)
    //    {
    //        _stoneTexData = LoadEmbeddedAsset<ProcessedTexture>("spnza_bricks_a_diff.binary");
    //        _vertices = GetCubeVertices();
    //        _indices = GetCubeIndices();
    //    }

    //    protected override unsafe void CreateResources(ResourceFactory factory)
    //    {
    //        _projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
    //        _viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
    //        _worldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

    //        _vertexBuffer =
    //            factory.CreateBuffer(new BufferDescription(
    //                (uint)(VertexPositionTexture.SizeInBytes * _vertices.Length), BufferUsage.VertexBuffer));
    //        GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, _vertices);

    //        _indexBuffer =
    //            factory.CreateBuffer(new BufferDescription(sizeof(ushort) * (uint)_indices.Length,
    //                BufferUsage.IndexBuffer));
    //        GraphicsDevice.UpdateBuffer(_indexBuffer, 0, _indices);

    //        _surfaceTexture = _stoneTexData.CreateDeviceTexture(GraphicsDevice, ResourceFactory, TextureUsage.Sampled);
    //        _surfaceTextureView = factory.CreateTextureView(_surfaceTexture);

    //        ResourceLayout projViewLayout = factory.CreateResourceLayout(
    //            new ResourceLayoutDescription(
    //                new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
    //                new ResourceLayoutElementDescription("View", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

    //        ResourceLayout worldTextureLayout = factory.CreateResourceLayout(
    //            new ResourceLayoutDescription(
    //                new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex),
    //                new ResourceLayoutElementDescription("SurfaceTexture", ResourceKind.TextureReadOnly,
    //                    ShaderStages.Fragment),
    //                new ResourceLayoutElementDescription("SurfaceSampler", ResourceKind.Sampler,
    //                    ShaderStages.Fragment)));

    //        _projViewSet = factory.CreateResourceSet(new ResourceSetDescription(
    //            projViewLayout,
    //            _projectionBuffer,
    //            _viewBuffer));

    //        _worldTextureSet = factory.CreateResourceSet(new ResourceSetDescription(
    //            worldTextureLayout,
    //            _worldBuffer,
    //            _surfaceTextureView,
    //            GraphicsDevice.Aniso4xSampler));

    //        _cl = factory.CreateCommandList();
    //    }

    //    protected override void OnDeviceDestroyed()
    //    {
    //        base.OnDeviceDestroyed();
    //    }

    //    protected override unsafe void Draw(float deltaSeconds)
    //    {
    //        _cl.UpdateBuffer(_projectionBuffer, 0, Matrix4x4.CreatePerspectiveFieldOfView(
    //            1.0f,
    //            (float)Window.Width / Window.Height,
    //            0.5f,
    //            100f));

    //        _cl.UpdateBuffer(_viewBuffer, 0, Matrix4x4.CreateLookAt(Vector3.UnitZ * 2.5f, Vector3.Zero, Vector3.UnitY));

    //        Matrix4x4 rotation = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (_ticks / 1000f)) * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, (_ticks / 3000f));
    //        _cl.UpdateBuffer(_worldBuffer, 0, ref rotation);

    //        _cl.SetVertexBuffer(0, _vertexBuffer);
    //        _cl.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
    //        _cl.SetGraphicsResourceSet(0, _projViewSet);
    //        _cl.SetGraphicsResourceSet(1, _worldTextureSet);
    //        _cl.DrawIndexed(36, 1, 0, 0, 0);
    //    }

    //    private static VertexPositionTexture[] GetCubeVertices()
    //    {
    //        VertexPositionTexture[] vertices = new VertexPositionTexture[]
    //        {
    //            // Top
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(0, 1)),
    //            // Bottom                                                             
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, +0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, +0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
    //            // Left                                                               
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, +0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
    //            // Right                                                              
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, +0.5f), new Vector2(0, 1)),
    //            // Back                                                               
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, -0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, -0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
    //            // Front                                                              
    //            new VertexPositionTexture(new Vector3(-0.5f, +0.5f, +0.5f), new Vector2(0, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, +0.5f, +0.5f), new Vector2(1, 0)),
    //            new VertexPositionTexture(new Vector3(+0.5f, -0.5f, +0.5f), new Vector2(1, 1)),
    //            new VertexPositionTexture(new Vector3(-0.5f, -0.5f, +0.5f), new Vector2(0, 1)),
    //        };

    //        return vertices;
    //    }

    //    private static ushort[] GetCubeIndices()
    //    {
    //        ushort[] indices =
    //        {
    //            0, 1, 2, 0, 2, 3,
    //            4, 5, 6, 4, 6, 7,
    //            8, 9, 10, 8, 10, 11,
    //            12, 13, 14, 12, 14, 15,
    //            16, 17, 18, 16, 18, 19,
    //            20, 21, 22, 20, 22, 23,
    //        };

    //        return indices;
    //    }
    //}

    public struct VertexPositionTexture
    {
        public const uint SizeInBytes = 20;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float TexU;
        public float TexV;

        public VertexPositionTexture(Vector3 pos, Vector2 uv)
        {
            PosX = pos.X;
            PosY = pos.Y;
            PosZ = pos.Z;
            TexU = uv.X;
            TexV = uv.Y;
        }
    }
}
