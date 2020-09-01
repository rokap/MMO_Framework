
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

        public float position_x;
        public float position_y;
        public float position_z;

        public float rotation_x;
        public float rotation_y;
        public float rotation_z;


        public Vector3 position
        {
            get { return new Vector3(position_x, position_y, position_z); }
            set
            {
                position_x = value.x;
                position_y = value.y;
                position_z = value.z;
            }
        }

        public Quaternion rotation
        {
            get { return Quaternion.Euler(new Vector3(rotation_x, rotation_y, rotation_z)); }
            set
            {
                rotation_x = value.eulerAngles.x;
                rotation_y = value.eulerAngles.y;
                rotation_z = value.eulerAngles.z;
            }
        }

        public Scenery() : base() { }

        public Scenery(string name, string description, Vector3 position, Quaternion rotation, string prefab)
        {
            this.name = name;
            this.description = description;
            this.position = position;
            this.rotation = rotation;
            this.prefab = prefab;
            this.id = this.Create();
        }

        public Scenery Instantiate()
        {
            Scenery scenery = GameObject.Instantiate<Scenery>(Resources.Load<Scenery>("Scenery/" + prefab), position, rotation, GameObject.Find("Scenery").transform);

            scenery.id = id;
            scenery.name = scenery.gameObject.name = name;
            scenery.description = description;
            scenery.prefab = prefab;
            scenery.position = position;
            scenery.rotation = rotation;

            return scenery;
        }

        private void Update()
        {
            if (position != transform.position)
                position = transform.position;

            if (rotation != transform.rotation)
                rotation = transform.rotation;

            if (gameObject.name != name)
                name = gameObject.name;
        }
    }
}
