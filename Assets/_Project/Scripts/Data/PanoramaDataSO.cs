using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPanorama", menuName = "Vortex/Panorama Data")]
public class PanoramaDataSO : ScriptableObject
{
    [Header("Imagem 360°")]
    public Texture2D panoramaTexture;

    [Header("Posição no Minimapa (0 a 1)")]
    public Vector2 minimapPosition = new Vector2(0.5f, 0.5f);

    [Header("Conexões (Vizinhos)")]
    public List<PanoramaDataSO> neighbors;
}