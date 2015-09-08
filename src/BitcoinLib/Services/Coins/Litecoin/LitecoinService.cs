// Copyright (c) 2014 George Kimionis
// Distributed under the GPLv3 software license, see the accompanying file LICENSE or http://opensource.org/licenses/GPL-3.0

using System;
using BitcoinLib.CoinParameters.Litecoin;

namespace BitcoinLib.Services.Coins.Litecoin
{
    public class LitecoinService : CoinService, ILitecoinService
    {
        public LitecoinService(Boolean useTestnet = false, bool ignoreConfigValues = false) : base(useTestnet, ignoreConfigValues)
        {
        }

        public LitecoinService(String daemonUrl, String rpcUsername, String rpcPassword, String walletPassword = null, bool ignoreConfigValues = false) : base(daemonUrl, rpcUsername, rpcPassword, walletPassword, ignoreConfigValues)
        {
        }

        public LitecoinConstants.Constants Constants
        {
            get
            {
                return LitecoinConstants.Constants.Instance;
            }
        }
    }
}