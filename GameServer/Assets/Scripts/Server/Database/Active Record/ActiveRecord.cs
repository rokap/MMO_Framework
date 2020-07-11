using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Table : Attribute
{
    private string name;

    public Table(string name)
    {
        this.name = name;
    }
}
public class Field : Attribute
{
    private string name;

    public Field(string name)
    {
        this.name = name;
    }
}

public class ActiveRecord
{
    public Dictionary<string, string> fields = new Dictionary<string, string>();

    public void Save()
    {
        foreach (KeyValuePair<string,string> field in fields)
        {

        }
    }
}