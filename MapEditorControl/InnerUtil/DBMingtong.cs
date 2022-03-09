using LinqToDB;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOperate
{
    public class DBMingtong : LinqToDB.Data.DataConnection
    {
        public DBMingtong(string connectString) : base("MySql", connectString) { }

        public ITable<MapSection> Map { get { return GetTable<MapSection>(); } }

        public ITable<MonsterSection> Monster { get { return GetTable<MonsterSection>(); } }

        public ITable<SceneMonsterPOJO> SceneMonster { get { return GetTable<SceneMonsterPOJO>(); } }

        public ITable<NpcSection> Npc { get { return GetTable<NpcSection>(); } }

        public ITable<ItemPOJO> Item { get { return GetTable<ItemPOJO>(); } }

        public ITable<MissionPOJO> Mission { get { return GetTable<MissionPOJO>(); } }
    }
}
