using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace Database
{
    public class Table : Attribute
    {
        public string name;

        public Table(string name)
        {
            this.name = name;
        }
    }

    public class Field : Attribute
    {
        public string name;

        public Field(string name)
        {
            this.name = name;
        }
    }

    public class ActiveRecord : MonoBehaviour
    {
        protected Dictionary<int, string> fields = new Dictionary<int, string>();

        public ActiveRecord()
        {

        }

        // Create
        protected uint Create()
        {

            FieldInfo[] properties = GetType().GetFields();
            for (int i = 0; i < properties.Length; i++)
            {
                fields.Add(i, properties[i].Name.ToLower());
            }

            string sql = "INSERT INTO " + GetTableName() + " ( " + GetFields() + " ) values ( " + GetValues() + " ); SELECT LAST_INSERT_ID();";

            return Convert.ToUInt32(Server.database.Query(sql)[0]["id"]);

        }

        // Update 
        public void Save()
        {
            FieldInfo field = GetType().GetField("id");
            int id = Convert.ToInt32(field.GetValue(this));

            if(id == 0)
            {
                Create();
            }
            else
            {
                string table = GetType().GetCustomAttribute<Table>().name;
                string sql = string.Format("UPDATE {0} SET " + GetUpdateFieldValue() + " WHERE id = {1}", table, id);
                Server.database.Query(sql);
            }
        }

        // Load All
        public static List<T> Load<T>() where T : new()
        {
            Type t = typeof(T);
            string table = t.GetCustomAttribute<Table>().name;
            string sql = "SELECT * FROM " + table;

            DataRowCollection rows = Server.database.Query(sql);
            FieldInfo[] properties = t.GetFields();

            List<T> allTs = new List<T>();
            foreach (DataRow row in rows)
            {
                T newT = new T();
                for (int i = 0; i < properties.Length; i++)
                {
                    FieldInfo property = newT.GetType().GetField(properties[i].Name);
                    if (properties[i].FieldType == typeof(int))
                    {
                        property.SetValue(newT, Convert.ToInt32(row[properties[i].Name]));
                    }
                    else if (properties[i].FieldType == typeof(uint))
                    {
                        property.SetValue(newT, Convert.ToUInt32(row[properties[i].Name]));
                    }
                    else
                    {
                        property.SetValue(newT, row[properties[i].Name]);
                    }

                }
                allTs.Add(newT);
            }
            return allTs;

        }

        // Load By Id
        public static T Load<T>(uint id) where T : new()
        {
            Type t = typeof(T);
            string table = t.GetCustomAttribute<Table>().name;
            string sql = "SELECT * FROM " + table + " WHERE id = " + id;

            DataRowCollection rows = Server.database.Query(sql);
            FieldInfo[] properties = t.GetFields();

            T newT = new T();
            foreach (DataRow row in rows)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    FieldInfo property = newT.GetType().GetField(properties[i].Name);
                    if (properties[i].FieldType == typeof(int))
                    {
                        property.SetValue(newT, Convert.ToInt32(row[properties[i].Name]));
                    }
                    else if (properties[i].FieldType == typeof(uint))
                    {
                        property.SetValue(newT, Convert.ToUInt32(row[properties[i].Name]));
                    }
                    else
                    {
                        property.SetValue(newT, row[properties[i].Name]);
                    }

                }
                break;
            }
            return newT;

        }

        // Load By Field
        public static T Load<T>(params (string Key, object Value)[] pairs) where T : new()
        {
            Type t = typeof(T);
            string table = t.GetCustomAttribute<Table>().name;
            string where = "WHERE ";
            foreach (var pair in pairs)
            {
                var (field, value) = pair;
                where += field + " = '" + value + "' && ";
            }
            where = where.TrimEnd('&', ' ');
            string sql = "SELECT * FROM " + table + " " + where;

            DataRowCollection rows = Server.database.Query(sql);
            FieldInfo[] properties = t.GetFields();

            T newT = new T();
            foreach (DataRow row in rows)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    FieldInfo property = newT.GetType().GetField(properties[i].Name);
                    if (properties[i].FieldType == typeof(int))
                    {
                        property.SetValue(newT, Convert.ToInt32(row[properties[i].Name]));
                    }
                    else if (properties[i].FieldType == typeof(uint))
                    {
                        property.SetValue(newT, Convert.ToUInt32(row[properties[i].Name]));
                    }
                    else
                    {
                        property.SetValue(newT, row[properties[i].Name]);
                    }

                }
                break;
            }
            return newT;

        }

        private string GetUpdateFieldValue()
        {
            string fields = null;
            FieldInfo[] properties = GetType().GetFields();
            foreach (FieldInfo property in properties)
            {
                if (property.Name.ToLower() != "id")
                {
                    var value = property.GetValue(this);
                    if (value.ToString() == "True")
                    {
                        fields += property.Name.ToLower() + "='1', ";
                    }
                    else if (value.ToString() == "False")
                    {
                        fields += property.Name.ToLower() + "='0', ";
                    }
                    else
                    {
                        fields += property.Name.ToLower() + "='" + property.GetValue(this) + "', ";
                    }
                }
            }

            return fields.TrimEnd(',', ' ');
        }

        public bool Delete()
        {
            FieldInfo field = GetType().GetField("id");
            int id = Convert.ToInt32(field.GetValue(this));
            string table = GetType().GetCustomAttribute<Table>().name;
            string sql = string.Format("DELETE FROM {0} WHERE id = {1}", table, id);
            Server.database.Query(sql);
            return true;
        }

        public static bool Delete<T>(int id)
        {
            Type t = typeof(T);
            string table = t.GetCustomAttribute<Table>().name;
            string sql = string.Format("DELETE FROM {0} WHERE id = {1}", table, id);
            Server.database.Query(sql);
            return true;
        }

        private string GetTableName()
        {
            return GetType().GetCustomAttribute<Table>().name;
        }

        private string GetFields()
        {
            string fields = null;
            FieldInfo[] properties = GetType().GetFields();
            foreach (FieldInfo property in properties)
            {
                if (property.Name.ToLower() != "id")
                    fields += property.Name.ToLower() + ", ";
            }

            return fields.TrimEnd(',', ' ');
        }

        private string GetValues()
        {
            string fields = null;
            FieldInfo[] properties = GetType().GetFields();
            foreach (FieldInfo property in properties)
            {
                if (property.Name.ToLower() != "id")
                {
                    var value = property.GetValue(this);
                    if (value.ToString() == "True")
                    {
                        fields += "'1', ";
                    }
                    else if (value.ToString() == "False")
                    {
                        fields += "'0', ";
                    }
                    else
                    {
                        fields += "'" + property.GetValue(this) + "', ";
                    }
                }
            }

            return fields.TrimEnd(',', ' ');
        }
    }
}