﻿using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Eco.Plugins.EcoApiExportMod
{
    public class previous_run_data
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class config
    {
        public bool debug { get; set; }
        public string api_access_token { get; set; }
        public int timeout { get; set; }
        public int db_query_limit { get; set; }
        public string base_api_url { get; set; }
        public string server_dir { get; set; }
        public previous_run_data[] previous_run_data { get; set; }
    }


    public class Initializer : IModKitPlugin, IInitializablePlugin
    {
        private string status = "initializing";
        public string base_mod_path = "";
        protected Collector collector;
        protected DatabaseCollector database_collector;

        public Initializer()
        {
            collector = new Collector();
            database_collector = new DatabaseCollector();
        }

        public override string ToString()
        {
            return "EcoApiExportMod";
        }

        public string GetStatus()
        {
            return status;
        }

        public void Initialize(TimedTask timer)
        {
            base_mod_path = string.Format("{0}Mods/EcoApiExportMod/", AppDomain.CurrentDomain.BaseDirectory);
            Logger.DebugVerbose("initializing");
            try
            {
                new Thread(() => { collector.collect(); })
                {
                    Name = "Collector"
                }.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            try
            {
                new Thread(() => { database_collector.collect(); })
                {
                    Name = "DatabaseCollector"
                }.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
