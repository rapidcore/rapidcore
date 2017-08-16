using System;
using System.Collections.Generic;
using System.Linq;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.Implementation
{
    public class FuncMigrationDatabase
    {
        #region Migration infos
        private readonly IDictionary<string, MigrationInfo> migrationInfos = new Dictionary<string, MigrationInfo>();

        public MigrationInfo GetInfoByName(string name)
        {
            if (migrationInfos.ContainsKey(name))
            {
                return migrationInfos[name];
            }

            return null;
        }

        public void UpsertMigrationInfo(MigrationInfo info)
        {
            if (migrationInfos.ContainsKey(info.Name))
            {
                migrationInfos[info.Name] = info;
            }
            else
            {
                migrationInfos.Add(info.Name, info);
            }
        }
        
        public IList<MigrationInfo> AllMigrationInfos()
        {
            return migrationInfos.Values.ToList();
        }
        #endregion

        #region KewlEntity
        private readonly IDictionary<string, FuncMigrationKewlEntity> kewlEntities = new Dictionary<string, FuncMigrationKewlEntity>();

        public FuncMigrationKewlEntity GetKewlById(string id)
        {
            if (kewlEntities.ContainsKey(id))
            {
                return kewlEntities[id];
            }

            return null;
        }

        public void UpsertKewl(FuncMigrationKewlEntity kewl)
        {
            if (string.IsNullOrEmpty(kewl.Id))
            {
                kewl.Id = new Guid().ToString();
            }
            
            if (kewlEntities.ContainsKey(kewl.Id))
            {
                kewlEntities[kewl.Id] = kewl;
            }
            else
            {
                kewlEntities.Add(kewl.Id, kewl);
            }
        }

        public IList<FuncMigrationKewlEntity> AllKewl()
        {
            return kewlEntities.Values.ToList();
        }
        #endregion
    }
}