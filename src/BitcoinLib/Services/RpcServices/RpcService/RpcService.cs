﻿// Copyright (c) 2014 George Kimionis
// Distributed under the GPLv3 software license, see the accompanying file LICENSE or http://opensource.org/licenses/GPL-3.0

using System;
using System.Collections.Generic;
using System.Linq;
using BitcoinLib.RPC.Connector;
using BitcoinLib.Requests.AddNode;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Requests.SignRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.RPC.Specifications;
using BitcoinLib.Services.Coins.Base;
using Newtonsoft.Json.Linq;

namespace BitcoinLib.Services
{
    //   Implementation of API calls list, as found at: https://en.bitcoin.it/wiki/Original_Bitcoin_client/API_Calls_list (note: this list is often out-of-date so call "help" in your bitcoin-cli to get the latest signatures)
    public partial class CoinService : ICoinService
    {
        private readonly IRpcConnector _rpcConnector;

        public CoinService()
        {
            _rpcConnector = new RpcConnector(this);
            Parameters = new CoinParameters(this);
        }

        public CoinService(Boolean useTestnet, bool ignoreConfigValues = false)
            : this()
        {
            Parameters.UseTestnet = useTestnet;
            Parameters.IgnoreConfigValues = ignoreConfigValues;
        }

        public CoinService(String daemonUrl, String rpcUsername, String rpcPassword, String walletPassword = null, bool ignoreConfigValues = false)
            : this()
        {
            Parameters.DaemonUrl = daemonUrl;
            Parameters.UseTestnet = false; //  this will force the CoinParameters.SelectedDaemonUrl dynamic property to automatically pick the daemonUrl defined above
            Parameters.RpcUsername = rpcUsername;
            Parameters.RpcPassword = rpcPassword;

            if (!String.IsNullOrWhiteSpace(walletPassword))
            {
                Parameters.WalletPassword = walletPassword;
            }

            Parameters.IgnoreConfigValues = ignoreConfigValues;
        }

        public String AddMultiSigAddress(Int32 nRquired, List<String> publicKeys, String account)
        {
            return account != null
                       ? _rpcConnector.MakeRequest<String>(RpcMethods.addmultisigaddress, nRquired, publicKeys, account)
                       : _rpcConnector.MakeRequest<String>(RpcMethods.addmultisigaddress, nRquired, publicKeys);
        }

        public String AddNode(String node, NodeAction action)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.addnode, node, action.ToString());
        }

        public String BackupWallet(String destination)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.backupwallet, destination);
        }

        public CreateMultiSigResponse CreateMultiSig(Int32 nRquired, List<String> publicKeys)
        {
            return _rpcConnector.MakeRequest<CreateMultiSigResponse>(RpcMethods.createmultisig, nRquired, publicKeys);
        }

        public String CreateRawTransaction(CreateRawTransactionRequest rawTransaction)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.createrawtransaction, rawTransaction.Inputs, rawTransaction.Outputs);
        }

        public DecodeRawTransactionResponse DecodeRawTransaction(String rawTransactionHexString)
        {
            return _rpcConnector.MakeRequest<DecodeRawTransactionResponse>(RpcMethods.decoderawtransaction, rawTransactionHexString);
        }

        public DecodeScriptResponse DecodeScript(String hexString)
        {
            return _rpcConnector.MakeRequest<DecodeScriptResponse>(RpcMethods.decodescript, hexString);
        }

        public String DumpPrivKey(String bitcoinAddress)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.dumpprivkey, bitcoinAddress);
        }

        public void DumpWallet(String filename)
        {
            _rpcConnector.MakeRequest<String>(RpcMethods.dumpwallet, filename);
        }

        public String EncryptWallet(String passphrase)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.encryptwallet, passphrase);
        }

        public Decimal EstimateFee(UInt16 nBlocks)
        {
            return _rpcConnector.MakeRequest<Decimal>(RpcMethods.estimatefee, nBlocks);
        }

        public Decimal EstimatePriority(UInt16 nBlocks)
        {
            return _rpcConnector.MakeRequest<Decimal>(RpcMethods.estimatepriority, nBlocks);
        }

        public String GetAccount(String bitcoinAddress)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getaccount, bitcoinAddress);
        }

        public String GetAccountAddress(String account)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getaccountaddress, account);
        }

        public GetAddedNodeInfoResponse GetAddedNodeInfo(String dns, String node)
        {
            return String.IsNullOrWhiteSpace(node)
                       ? _rpcConnector.MakeRequest<GetAddedNodeInfoResponse>(RpcMethods.getaddednodeinfo, dns)
                       : _rpcConnector.MakeRequest<GetAddedNodeInfoResponse>(RpcMethods.getaddednodeinfo, dns, node);
        }

        public List<String> GetAddressesByAccount(String account)
        {
            return _rpcConnector.MakeRequest<List<String>>(RpcMethods.getaddressesbyaccount, account);
        }

        public Decimal GetBalance(String account, Int32 minConf, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<Decimal>(RpcMethods.getbalance, (String.IsNullOrWhiteSpace(account) ? "*" : account), minConf)
                : _rpcConnector.MakeRequest<Decimal>(RpcMethods.getbalance, (String.IsNullOrWhiteSpace(account) ? "*" : account), minConf, includeWatchonly);
        }

        public String GetBestBlockHash()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getbestblockhash);
        }

        public GetBlockResponse GetBlock(String hash, Boolean verbose)
        {
            return _rpcConnector.MakeRequest<GetBlockResponse>(RpcMethods.getblock, hash, verbose);
        }

        public GetBlockchainInfoResponse GetBlockchainInfo()
        {
            return _rpcConnector.MakeRequest<GetBlockchainInfoResponse>(RpcMethods.getblockchaininfo);
        }

        public UInt32 GetBlockCount()
        {
            return _rpcConnector.MakeRequest<UInt32>(RpcMethods.getblockcount);
        }

        public String GetBlockHash(Int64 index)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getblockhash, index);
        }

        public GetBlockTemplateResponse GetBlockTemplate(params object[] parameters)
        {
            return parameters == null
                       ? _rpcConnector.MakeRequest<GetBlockTemplateResponse>(RpcMethods.getblocktemplate)
                       : _rpcConnector.MakeRequest<GetBlockTemplateResponse>(RpcMethods.getblocktemplate, parameters);
        }

        public List<GetChainTipsResponse> GetChainTips()
        {
            return _rpcConnector.MakeRequest<List<GetChainTipsResponse>>(RpcMethods.getchaintips);
        }

        public Int32 GetConnectionCount()
        {
            return _rpcConnector.MakeRequest<Int32>(RpcMethods.getconnectioncount);
        }

        public Double GetDifficulty()
        {
            return _rpcConnector.MakeRequest<Double>(RpcMethods.getdifficulty);
        }

        public String GetGenerate()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getgenerate);
        }

        public Int32 GetHashesPerSec()
        {
            return _rpcConnector.MakeRequest<Int32>(RpcMethods.gethashespersec);
        }

        [Obsolete("Please use calls: GetWalletInfo(), GetBlockchainInfo() and GetNetworkInfo() instead")]
        public GetInfoResponse GetInfo()
        {
            return _rpcConnector.MakeRequest<GetInfoResponse>(RpcMethods.getinfo);
        }

        public GetMemPoolInfoResponse GetMemPoolInfo()
        {
            return _rpcConnector.MakeRequest<GetMemPoolInfoResponse>(RpcMethods.getmempoolinfo);
        }

        public GetMiningInfoResponse GetMiningInfo()
        {
            return _rpcConnector.MakeRequest<GetMiningInfoResponse>(RpcMethods.getmininginfo);
        }

        public GetNetTotalsResponse GetNetTotals()
        {
            return _rpcConnector.MakeRequest<GetNetTotalsResponse>(RpcMethods.getnettotals);
        }

        public UInt64 GetNetworkHashPs(UInt32 blocks, Int64 height)
        {
            return _rpcConnector.MakeRequest<UInt64>(RpcMethods.getnetworkhashps);
        }

        public GetNetworkInfoResponse GetNetworkInfo()
        {
            return _rpcConnector.MakeRequest<GetNetworkInfoResponse>(RpcMethods.getnetworkinfo);
        }

        public String GetNewAddress(String account)
        {
            return String.IsNullOrWhiteSpace(account)
                       ? _rpcConnector.MakeRequest<String>(RpcMethods.getnewaddress)
                       : _rpcConnector.MakeRequest<String>(RpcMethods.getnewaddress, account);
        }

        public List<GetPeerInfoResponse> GetPeerInfo()
        {
            return _rpcConnector.MakeRequest<List<GetPeerInfoResponse>>(RpcMethods.getpeerinfo);
        }

        public GetRawMemPoolResponse GetRawMemPool(Boolean verbose)
        {
            GetRawMemPoolResponse getRawMemPoolResponse = new GetRawMemPoolResponse
                {
                    IsVerbose = verbose
                };

            object rpcResponse = _rpcConnector.MakeRequest<object>(RpcMethods.getrawmempool, verbose);

            if (!verbose)
            {
                JArray rpcResponseAsArray = (JArray)rpcResponse;

                foreach (String txId in rpcResponseAsArray)
                {
                    getRawMemPoolResponse.TxIds.Add(txId);
                }

                return getRawMemPoolResponse;
            }

            IList<KeyValuePair<String, JToken>> rpcResponseAsKvp = (new EnumerableQuery<KeyValuePair<String, JToken>>(((JObject)(rpcResponse)))).ToList();
            IList<JToken> children = JObject.Parse(rpcResponse.ToString()).Children().ToList();

            for (Int32 i = 0; i < children.Count(); i++)
            {
                GetRawMemPoolVerboseResponse getRawMemPoolVerboseResponse = new GetRawMemPoolVerboseResponse
                    {
                        TxId = rpcResponseAsKvp[i].Key
                    };

                getRawMemPoolResponse.TxIds.Add(getRawMemPoolVerboseResponse.TxId);

                foreach (JProperty property in children[i].SelectMany(grandChild => grandChild.OfType<JProperty>()))
                {
                    switch (property.Name)
                    {
                        case "currentpriority":

                            Double currentPriority;

                            if (Double.TryParse(property.Value.ToString(), out currentPriority))
                            {
                                getRawMemPoolVerboseResponse.CurrentPriority = currentPriority;
                            }

                            break;

                        case "depends":

                            foreach (JToken jToken in property.Value)
                            {
                                getRawMemPoolVerboseResponse.Depends.Add(jToken.Value<String>());
                            }

                            break;

                        case "fee":

                            Decimal fee;

                            if (Decimal.TryParse(property.Value.ToString(), out fee))
                            {
                                getRawMemPoolVerboseResponse.Fee = fee;
                            }

                            break;

                        case "height":

                            Int32 height;

                            if (Int32.TryParse(property.Value.ToString(), out height))
                            {
                                getRawMemPoolVerboseResponse.Height = height;
                            }

                            break;

                        case "size":

                            Int32 size;

                            if (Int32.TryParse(property.Value.ToString(), out size))
                            {
                                getRawMemPoolVerboseResponse.Size = size;
                            }

                            break;

                        case "startingpriority":

                            Double startingPriority;

                            if (Double.TryParse(property.Value.ToString(), out startingPriority))
                            {
                                getRawMemPoolVerboseResponse.StartingPriority = startingPriority;
                            }

                            break;

                        case "time":

                            Int32 time;

                            if (Int32.TryParse(property.Value.ToString(), out time))
                            {
                                getRawMemPoolVerboseResponse.Time = time;
                            }

                            break;

                        default:

                            throw new Exception("Unkown property: " + property.Name + " in GetRawMemPool()");
                    }
                }
                getRawMemPoolResponse.VerboseResponses.Add(getRawMemPoolVerboseResponse);
            }
            return getRawMemPoolResponse;
        }

        public String GetRawChangeAddress()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getrawchangeaddress);
        }

        public GetRawTransactionResponse GetRawTransaction(String txId, Int32 verbose)
        {
            if (verbose == 0)
            {
                return new GetRawTransactionResponse { Hex = _rpcConnector.MakeRequest<String>(RpcMethods.getrawtransaction, txId, verbose) };
            }

            if (verbose == 1)
            {
                return _rpcConnector.MakeRequest<GetRawTransactionResponse>(RpcMethods.getrawtransaction, txId, verbose);
            }

            throw new Exception("Invalid verbose value: " + verbose + " in GetRawTransaction()!");
        }

        public String GetReceivedByAccount(String account, Int32 minConf)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getreceivedbyaccount, account, minConf);
        }

        public String GetReceivedByAddress(String bitcoinAddress, Int32 minConf)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.getreceivedbyaddress, bitcoinAddress, minConf);
        }

        public GetTransactionResponse GetTransaction(String txId, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<GetTransactionResponse>(RpcMethods.gettransaction, txId)
                : _rpcConnector.MakeRequest<GetTransactionResponse>(RpcMethods.gettransaction, txId, includeWatchonly);
        }

        public GetTransactionResponse GetTxOut(String txId, Int32 n, Boolean includeMemPool)
        {
            return _rpcConnector.MakeRequest<GetTransactionResponse>(RpcMethods.gettxout, txId, n, includeMemPool);
        }

        public GetTxOutSetInfoResponse GetTxOutSetInfo()
        {
            return _rpcConnector.MakeRequest<GetTxOutSetInfoResponse>(RpcMethods.gettxoutsetinfo);
        }

        public Decimal GetUnconfirmedBalance()
        {
            return _rpcConnector.MakeRequest<Decimal>(RpcMethods.getunconfirmedbalance);
        }

        public GetWalletInfoResponse GetWalletInfo()
        {
            return _rpcConnector.MakeRequest<GetWalletInfoResponse>(RpcMethods.getwalletinfo);
        }

        public String Help(String command)
        {
            return String.IsNullOrWhiteSpace(command)
                       ? _rpcConnector.MakeRequest<String>(RpcMethods.help)
                       : _rpcConnector.MakeRequest<String>(RpcMethods.help, command);
        }

        public void ImportAddress(String address, String label, Boolean rescan)
        {
            _rpcConnector.MakeRequest<String>(RpcMethods.importaddress, address, label, rescan);
        }

        public String ImportPrivKey(String privateKey, String label, Boolean rescan)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.importprivkey, privateKey, label, rescan);
        }

        public void ImportWallet(String filename)
        {
            _rpcConnector.MakeRequest<String>(RpcMethods.importwallet, filename);
        }

        public String KeyPoolRefill(UInt32 newSize)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.keypoolrefill, newSize);
        }

        public Dictionary<String, Decimal> ListAccounts(Int32 minConf, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<Dictionary<String, Decimal>>(RpcMethods.listaccounts, minConf)
                : _rpcConnector.MakeRequest<Dictionary<String, Decimal>>(RpcMethods.listaccounts, minConf, includeWatchonly);
        }

        public List<List<ListAddressGroupingsResponse>> ListAddressGroupings()
        {
            List<List<List<object>>> unstructuredResponse = _rpcConnector.MakeRequest<List<List<List<object>>>>(RpcMethods.listaddressgroupings);
            List<List<ListAddressGroupingsResponse>> structuredResponse = new List<List<ListAddressGroupingsResponse>>(unstructuredResponse.Count);

            for (Int32 i = 0; i < unstructuredResponse.Count; i++)
            {
                for (Int32 j = 0; j < unstructuredResponse[i].Count; j++)
                {
                    if (unstructuredResponse[i][j].Count > 1)
                    {
                        ListAddressGroupingsResponse response = new ListAddressGroupingsResponse
                            {
                                Address = unstructuredResponse[i][j][0].ToString()
                            };

                        Decimal balance;
                        Decimal.TryParse(unstructuredResponse[i][j][1].ToString(), out balance);

                        if (unstructuredResponse[i][j].Count > 2)
                        {
                            response.Account = unstructuredResponse[i][j][2].ToString();
                        }

                        if (structuredResponse.Count < i + 1)
                        {
                            structuredResponse.Add(new List<ListAddressGroupingsResponse>());
                        }

                        structuredResponse[i].Add(response);
                    }
                }
            }
            return structuredResponse;
        }

        public String ListLockUnspent()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.listlockunspent);
        }

        public List<ListReceivedByAccountResponse> ListReceivedByAccount(Int32 minConf, Boolean includeEmpty, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<List<ListReceivedByAccountResponse>>(RpcMethods.listreceivedbyaccount, minConf, includeEmpty)
                : _rpcConnector.MakeRequest<List<ListReceivedByAccountResponse>>(RpcMethods.listreceivedbyaccount, minConf, includeEmpty, includeWatchonly);
        }

        public List<ListReceivedByAddressResponse> ListReceivedByAddress(Int32 minConf, Boolean includeEmpty, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<List<ListReceivedByAddressResponse>>(RpcMethods.listreceivedbyaddress, minConf, includeEmpty)
                : _rpcConnector.MakeRequest<List<ListReceivedByAddressResponse>>(RpcMethods.listreceivedbyaddress, minConf, includeEmpty, includeWatchonly);
        }

        public ListSinceBlockResponse ListSinceBlock(String blockHash, Int32 targetConfirmations, Boolean? includeWatchonly)
        {
            return includeWatchonly == null
                ? _rpcConnector.MakeRequest<ListSinceBlockResponse>(RpcMethods.listsinceblock, (String.IsNullOrWhiteSpace(blockHash) ? "*" : blockHash), targetConfirmations)
                : _rpcConnector.MakeRequest<ListSinceBlockResponse>(RpcMethods.listsinceblock, (String.IsNullOrWhiteSpace(blockHash) ? "*" : blockHash), targetConfirmations, includeWatchonly);
        }

        public List<ListTransactionsResponse> ListTransactions(String account, Int32 count, Int32 from, Boolean? includeWatchonly)
        {
            return includeWatchonly == null 
                ? _rpcConnector.MakeRequest<List<ListTransactionsResponse>>(RpcMethods.listtransactions, (String.IsNullOrWhiteSpace(account) ? "*" : account), count, from)
                : _rpcConnector.MakeRequest<List<ListTransactionsResponse>>(RpcMethods.listtransactions, (String.IsNullOrWhiteSpace(account) ? "*" : account), count, from, includeWatchonly);
        }

        public List<ListUnspentResponse> ListUnspent(Int32 minConf, Int32 maxConf, List<String> addresses)
        {
            return _rpcConnector.MakeRequest<List<ListUnspentResponse>>(RpcMethods.listunspent, minConf, maxConf, (addresses ?? new List<String>()));
        }

        public Boolean LockUnspent(Boolean unlock, IList<ListUnspentResponse> listUnspentResponses)
        {
            IList<Object> transactions = new List<object>();

            foreach (ListUnspentResponse listUnspentResponse in listUnspentResponses)
            {
                transactions.Add(new { txid = listUnspentResponse.TxId, Vout = listUnspentResponse.Vout });
            }

            return _rpcConnector.MakeRequest<Boolean>(RpcMethods.lockunspent, unlock, transactions.ToArray());
        }

        public Boolean Move(String fromAccount, String toAccount, Decimal amount, Int32 minConf, String comment)
        {
            return _rpcConnector.MakeRequest<Boolean>(RpcMethods.move, fromAccount, toAccount, amount, minConf, comment);
        }

        public void Ping()
        {
            _rpcConnector.MakeRequest<String>(RpcMethods.ping);
        }

        public Boolean PrioritiseTransaction(String txId, Decimal priorityDelta, Decimal feeDelta)
        {
            return _rpcConnector.MakeRequest<Boolean>(RpcMethods.prioritisetransaction, txId, priorityDelta, feeDelta);
        }

        public String SendFrom(String fromAccount, String toBitcoinAddress, Decimal amount, Int32 minConf, String comment, String commentTo)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.sendfrom, fromAccount, toBitcoinAddress, amount, minConf, comment, commentTo);
        }

        public String SendMany(String fromAccount, Dictionary<String, Decimal> toBitcoinAddress, Int32 minConf, String comment)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.sendmany, fromAccount, toBitcoinAddress, minConf, comment);
        }

        public String SendRawTransaction(String rawTransactionHexString, Boolean? allowHighFees)
        {
            return allowHighFees == null
                       ? _rpcConnector.MakeRequest<String>(RpcMethods.sendrawtransaction, rawTransactionHexString)
                       : _rpcConnector.MakeRequest<String>(RpcMethods.sendrawtransaction, rawTransactionHexString, allowHighFees);
        }

        public String SendToAddress(String bitcoinAddress, Decimal amount, String comment, String commentTo)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.sendtoaddress, bitcoinAddress, amount, comment, commentTo);
        }

        public String SetAccount(String bitcoinAddress, String account)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.setaccount, bitcoinAddress, account);
        }

        public String SetGenerate(Boolean generate, Int16 generatingProcessorsLimit)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.setgenerate, generate, generatingProcessorsLimit);
        }

        public String SetTxFee(Decimal amount)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.settxfee, amount);
        }

        public String SignMessage(String bitcoinAddress, String message)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.signmessage, bitcoinAddress, message);
        }

        public SignRawTransactionResponse SignRawTransaction(SignRawTransactionRequest request)
        {
            #region default values

            if (request.Inputs.Count == 0)
            {
                request.Inputs = null;
            }

            if (String.IsNullOrWhiteSpace(request.SigHashType))
            {
                request.SigHashType = SigHashType.All;
            }

            if (request.PrivateKeys.Count == 0)
            {
                request.PrivateKeys = null;
            }

            #endregion

            return _rpcConnector.MakeRequest<SignRawTransactionResponse>(RpcMethods.signrawtransaction, request.RawTransactionHex, request.Inputs, request.PrivateKeys, request.SigHashType);
        }

        public String Stop()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.stop);
        }

        public String SubmitBlock(String hexData, params object[] parameters)
        {
            return parameters == null
                       ? _rpcConnector.MakeRequest<String>(RpcMethods.submitblock, hexData)
                       : _rpcConnector.MakeRequest<String>(RpcMethods.submitblock, hexData, parameters);
        }

        public ValidateAddressResponse ValidateAddress(String bitcoinAddress)
        {
            return _rpcConnector.MakeRequest<ValidateAddressResponse>(RpcMethods.validateaddress, bitcoinAddress);
        }

        public Boolean VerifyChain(UInt16 checkLevel, UInt32 numBlocks)
        {
            return _rpcConnector.MakeRequest<Boolean>(RpcMethods.verifychain, checkLevel, numBlocks);
        }

        public String VerifyMessage(String bitcoinAddress, String signature, String message)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.verifymessage, bitcoinAddress, signature, message);
        }

        public String WalletLock()
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.walletlock);
        }

        public String WalletPassphrase(String passphrase, Int32 timeoutInSeconds)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.walletpassphrase, passphrase, timeoutInSeconds);
        }

        public String WalletPassphraseChange(String oldPassphrase, String newPassphrase)
        {
            return _rpcConnector.MakeRequest<String>(RpcMethods.walletpassphrasechange, oldPassphrase, newPassphrase);
        }

        public override String ToString()
        {
            return Parameters.CoinLongName;
        }
    }
}