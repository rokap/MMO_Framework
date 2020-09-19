
using UnityEngine;

namespace Database
{
    [Table("scenery")]
    public class Scenery : ActiveRecord
    {
        public uint id;
        [HideInInspector]
        public new string name;
        public string description;        
        public string prefab;

        public float scale_x = 1f;
        public float scale_y = 1f;
        public float scale_z = 1f;
        public Vector3 Size
        {
            get { return new Vector3(scale_x, scale_y, scale_z); }
            set
            {
                scale_x = value.x;
                scale_y = value.y;
                scale_z = value.z;
            }
        }

        public Scenery() : base() {}

        public Scenery(string name, string description, string prefab, Vector3 scale)
        {
            this.name = name;
            this.description = description;
            this.prefab = prefab;
            
            this.scale_x = scale.x;
            this.scale_y = scale.y;
            this.scale_z = scale.z;

            this.id = this.Create();
        }

        public void Init(Scenery scenery)
        {
            // Update Gameobject Fields
            System.Reflection.FieldInfo[] fields = this.GetType().GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(scenery, field.GetValue(this));
            }

            Debug.Log(scenery.Size);

            // Set Scenery Scale
            scenery.transform.localScale = scenery.Size;

        }

        private void Update()
        {            
            if (gameObject.name != name)
                name = gameObject.name;

            if (transform.localScale.x != scale_x)
                scale_x = transform.localScale.x;

            if (transform.localScale.y != scale_y)
                scale_y = transform.localScale.y;

            if (transform.localScale.z != scale_z)
                scale_z = transform.localScale.z;
        }
    }
}
