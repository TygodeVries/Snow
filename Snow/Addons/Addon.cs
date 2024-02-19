﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Addons
{
    public abstract class Addon
    {
        protected AddonManager addonManager;
        public AddonManager GetAddonManager()
        {
            return addonManager;
        }

        internal void SetAddonManager(AddonManager addonManager)
        {
            this.addonManager = addonManager;
        }

        public Lobby GetLobby()
        {
            return addonManager.GetLobby();
        }
        public virtual void Start() { }

        public virtual void Stop() { }
    }
}