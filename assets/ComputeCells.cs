using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeCells : MonoBehaviour
{
    public int buffsize;
    public ComputeShader compute;
    public ComputeShader computeGame;

    public RenderTexture result;
    RenderTexture[] textures;

    public int rule;
    public int i;
    public int prev;
    public int index;

    void Start()
    {   int i = 0;

        textures = new RenderTexture[buffsize];
        for(int j = 0; j < buffsize; j++)
        {
            textures[i] = new RenderTexture(512, 512, 24);
            textures[i].enableRandomWrite = true;
            textures[i].Create();
            i++;
        }
        int kernel = compute.FindKernel("CSMain");
        compute.SetTexture(kernel, "Result", textures[0]);
        //compute.SetTexture(kernelinit, " ", RE)
        compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);

    }
    
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        prev = index;
        i++;
        index = i % buffsize;
        int klife = computeGame.FindKernel("GameOfLife");
        computeGame.SetInt("rule", rule);

        computeGame.SetTexture(klife, "Prev", textures[prev]);
        computeGame.SetTexture(klife, "Result", textures[index]);

    
        computeGame.Dispatch(klife, textures[0].width / 8, textures[0].height / 8, 1);

        Graphics.Blit(textures[index], dest);
    } 
    
    
}
