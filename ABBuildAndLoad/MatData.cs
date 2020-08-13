using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatData
{

    public string shader = "";
    public List<Attritubute> attributes = new List<Attritubute>();
}

public class Attritubute
{
    public Tuple<string, MatShaderPropertyType, MatVector> attri;
    public Tuple<string, MatShaderPropertyType, MatTex> texAttri;
    public Tuple<string, MatShaderPropertyType, double> floatAttri;
}

public struct MatVector
{
    public double x;
    public double y;
    public double z;
    public double w;

    public void Set(double x,double y,double z , double w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}

public struct MatTex
{
    public string texName;
    public string bundleDir;
    public double x;
    public double y;
    public double z;
    public double w;
    public byte[] data;
}



public enum MatShaderPropertyType
{
    Color,
    Vector,
    Float,
    Range,
    TexEnv,
}
