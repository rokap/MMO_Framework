
using UnityEngine;

namespace Database
{
    [Table("scenery_spawn_points")]
    public class ScenerySpawnPoint : ActiveRecord
    {
        public uint id;

        public uint scenery_id;
        public Scenery scenery { get; private set; }

        public float position_x;
        public float position_y;
        public float position_z;

        public float rotation_x;
        public float rotation_y;
        public float rotation_z;

        public Vector3 position
        {
            get
            {
                return new Vector3(position_x, position_y, position_z);
            }
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

        public ScenerySpawnPoint() : base() { }

        public ScenerySpawnPoint(Scenery scenery, Vector3 position, Quaternion rotation)
        {
            this.scenery_id = scenery.id;
            this.position = position;
            this.rotation = rotation;
            this.id = this.Create();
        }

        public ScenerySpawnPoint Instantiate()
        {
            // Connect the Scenery
            scenery = ActiveRecord.Load<Scenery>(scenery_id);
            // Spawn Scenery to World;
            ScenerySpawnPoint scenerySpawnPoint = GameObject.Instantiate<ScenerySpawnPoint>(Resources.Load<ScenerySpawnPoint>("Scenery/" + scenery.prefab), position, rotation, GameObject.Find("Scenery").transform);

            // Update Gameobject Fields
            System.Reflection.FieldInfo[] fields = this.GetType().GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(scenerySpawnPoint, field.GetValue(this));
            }

            // Init Scenery
            scenery.Init(scenerySpawnPoint.GetComponent<Scenery>());

            // Set Name & Return 
            scenerySpawnPoint.gameObject.name = scenery.name;

            return scenerySpawnPoint;
        }
        private void Update()
        {
            if (gameObject.transform.position != position)
                position = gameObject.transform.position;

            if (gameObject.transform.rotation != rotation)
                rotation = gameObject.transform.rotation;
        }

    }
}
