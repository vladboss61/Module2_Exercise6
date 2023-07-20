using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Xml.Serialization;

namespace Module2_Exercise6;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public class MyAttribute : Attribute
{
    public string Name { get; set; }
}

public interface ISomething
{
    void Update();
}

public class Logic
{
    public ISomething Something { get; set; }

    public Logic(ISomething something)
    {
        Something = something;
    }

    public void Calculate()
    {
        Console.WriteLine("asdadad");
        Something.Update();
    }
}

[MyAttribute(Name = "Test 2")]
internal sealed class Data
{
    [MyAttribute] public int _innerField1;

    private string _innerField2;

    [MyAttribute]
    private static string _innerFieldStatic;

    public int Id { get; set; }

    [MyAttribute]
    [Obsolete]
    public int GetId(string value, int number)
    {
        return Id;
    }

    public Data()
    {

    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        Assembly silk = Assembly.LoadFrom(
            "D:\\MyProjects\\CSharp\\SillkApp\\bin\\Debug\\net6.0\\Silk.NET.Core.dll");

        var configStr = File.ReadAllText("config.json");
        Configuration configs = JsonSerializer.Deserialize<Configuration>(configStr);
        Data data = new Data { Id = 20 };

        data.GetId("asd", 123);
        Type dataType = data.GetType();

        Type dataTypeTypeOf = typeof(Data);

        MethodInfo methodGetId = dataTypeTypeOf.GetMethod("GetId");
        ParameterInfo[] parameters = methodGetId.GetParameters();
        Type proType = typeof(Program);

        if (dataType == dataTypeTypeOf)
        {
            Console.WriteLine("dataType == dataTypeTypeOf, the same");
        }

        if (proType == dataTypeTypeOf)
        {
            Console.WriteLine("proType == dataTypeTypeOf, the same");
        }


        var attrs = Attribute.GetCustomAttributes(dataType);

        foreach (var attr in attrs)
        {
            if (attr is MyAttribute customAttr)
            {
                Console.WriteLine(customAttr.Name);
            }
        }

        PropertyInfo[] props = dataType.GetProperties();

        data.Id = 999;
        props[0].SetValue(data, 999);

        MethodInfo[] methods = dataType.GetMethods();
        FieldInfo[] fields = dataType.GetFields();

        FieldInfo[] privateFields = dataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        Data runtimeData = (Data)Activator.CreateInstance(typeof(Data));
    }
}
