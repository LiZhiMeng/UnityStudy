using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MatTool
{
    public static MatData MatToJson(Material mat)
    {
        MatData data = new MatData();
        Shader shader = mat.shader;
        data.shader = shader.name;
        int propertyCount = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < propertyCount; i++)
        {
            Attritubute attr = new Attritubute();
            string propertyName = ShaderUtil.GetPropertyName(shader, i);
            MatShaderPropertyType type = (MatShaderPropertyType) ShaderUtil.GetPropertyType(shader, i);
            switch (type)
            {
                case MatShaderPropertyType.Color:
                    Color color = mat.GetColor(propertyName);
                    attr.attri = new Tuple<string, MatShaderPropertyType, MatVector>(propertyName, type, new MatVector()
                    {
                        x = color.r, y = color.g, z = color.b, w = color.a
                    });
                    break;
                case MatShaderPropertyType.Float:
                case MatShaderPropertyType.Range:
                    double f = mat.GetFloat(propertyName);
                    attr.floatAttri = new Tuple<string, MatShaderPropertyType, double>(propertyName,type,f);
                    break;
                case MatShaderPropertyType.Vector:
                    Vector4 v4 = mat.GetVector(propertyName);
                    attr.attri = new Tuple<string, MatShaderPropertyType, MatVector>(propertyName,type,new MatVector()
                    {
                        x = v4.x,y= v4.y,z=v4.z,w = v4.w
                    });
                    break;
                case  MatShaderPropertyType.TexEnv:
                    Texture t2d = mat.GetTexture(propertyName);
                    string assetPath = AssetDatabase.GetAssetPath(t2d);
                    attr.texAttri = new Tuple<string, MatShaderPropertyType, MatTex>(propertyName,type,new MatTex()
                    {
                        texName = t2d.name,
                        bundleDir = assetPath
                    });
                    break;
            }
            data.attributes.Add(attr);
        }
        return data;
    }


    /// <summary>
    /// json转mat
    /// </summary>
    /// <param name="jsonTxt"></param>
    public static void JsonToMat(string jsonTxt)
    {
    

        return;
    }
}