using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BotWars.DAL.Objects;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace BotWars.DAL
{
    public static class Config
    {
        public static void CreateDatabase()
        {
            Configuration().ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true)).BuildConfiguration();
        }

        private static ISessionFactory _factory;
        public static ISessionFactory GetSessionFactory()
        {
            return _factory ?? (_factory = Configuration().BuildSessionFactory());
        }

        private static FluentConfiguration Configuration()
        {
            return Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2008
                                            .ConnectionString(
                                                sb =>
                                                sb.Server(".\\SQLExpress")
                                                  .Database("BotWars")
                                                  .TrustedConnection())
                ).Mappings(m =>
                           m.AutoMappings.Add(
                               // your automapping setup here
                               AutoMap.AssemblyOf<Author>(
                                   type => type.Namespace.EndsWith("Objects"))));
        }
    }
}
