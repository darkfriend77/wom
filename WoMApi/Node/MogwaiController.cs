﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WoMApi.Tool;

namespace WoMApi.Node
{
    public class MogwaiController
    {
        private MogwaiWallet Wallet { get; }

        public bool IsWalletUnlocked => Wallet.IsUnlocked;

        public bool IsWalletCreated => Wallet.IsCreated;

        public Block WalletLastBlock => Wallet.LastBlock;

        public string DepositAddress => Wallet.IsUnlocked ? Wallet.Deposit.Address : string.Empty;

        public Dictionary<string, MogwaiKeys> MogwaiKeysDict => Wallet.MogwaiKeyDict;

        public List<MogwaiKeys> TaggedMogwaiKeys { get; set; }

        private int currentMogwayKeys = 0;
        public MogwaiKeys CurrentMogwayKeys
        {
            get
            {
                if (Wallet.MogwaiKeyDict.Count > currentMogwayKeys)
                {
                    return null;
                }
                return Wallet.MogwaiKeyDict.Values.ToList()[currentMogwayKeys];
            }
        }

        public string WalletMnemonicWords => Wallet.MnemonicWords;

        public bool HasMogwayKeys => MogwaiKeysDict.Count > 0;

        private Timer timer;

        public MogwaiController()
        {
            Wallet = new MogwaiWallet();
            TaggedMogwaiKeys = new List<MogwaiKeys>();
        }

        public void Refresh(int minutes)
        {
            Update();
            timer = new Timer(minutes * 60 * 1000);
            timer.Elapsed += OnTimedEventAsync;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async void OnTimedEventAsync(object sender, ElapsedEventArgs e)
        {
            await Blockchain.Instance.CacheBlockhashesAsyncNoProgressAsync();
            Update();
        }

        private void Update()
        {
            Wallet.Update();
            Wallet.Deposit.Update();
            foreach (var mogwaiKey in Wallet.MogwaiKeyDict.Values)
            {
                mogwaiKey.Update();
            }
        }

        public bool Next(out MogwaiKeys mogwayKeys)
        {
            mogwayKeys = null;
            if (currentMogwayKeys + 1 > Wallet.MogwaiKeyDict.Count)
            {
                return false;
            }
            currentMogwayKeys++;
            mogwayKeys = CurrentMogwayKeys;
            return true;
        }

        public bool Previous(out MogwaiKeys mogwayKeys)
        {
            mogwayKeys = null;
            if (currentMogwayKeys == 0)
            {
                return false;
            }
            currentMogwayKeys--;
            mogwayKeys = CurrentMogwayKeys;
            return true;
        }

        public void Tag()
        {
            if (TaggedMogwaiKeys.Contains(CurrentMogwayKeys))
            {
                TaggedMogwaiKeys.Remove(CurrentMogwayKeys);
            }
            else
            {
                TaggedMogwaiKeys.Add(CurrentMogwayKeys);
            }
        }

        public void CreateWallet(string password)
        {
            Wallet.Create(password);
        }

        public void UnlockWallet(string password)
        {
            Wallet.Unlock(password);
        }

        public decimal GetDepositFunds()
        {
            if (!IsWalletUnlocked)
            {
                return -1;
            }
            return Wallet.Deposit.Balance;
        }

        public void PrintMogwaiKeys()
        {
            if (!IsWalletUnlocked)
            {
                return;
            }
            Caching.Persist("mogwaikeys.txt", new { Wallet.Deposit.Address, Wallet.MogwaiKeyDict.Keys });
        }

        public void NewMogwaiKeys()
        {
            if (!IsWalletUnlocked)
            {
                return;
            }
            Wallet.GetNewMogwaiKey(out MogwaiKeys mogwaiKeys);
        }

        public bool SendMog(MogwaiKeys mogwaiKeys)
        {
            if (!IsWalletUnlocked)
            {
                return false;
            }

            if (!Blockchain.Instance.SendMogs(Wallet.Deposit, mogwaiKeys.Address, 5m, 0.0001m))
            {
                return false;
            };

            mogwaiKeys.MogwaiKeysState = MogwaiKeysState.WAIT;
            return true;
        }

        public bool BindMogwai(MogwaiKeys mogwaiKeys)
        {
            if (!IsWalletUnlocked)
            {
                return false;
            }

            if (!Blockchain.Instance.BindMogwai(mogwaiKeys))
            {
                return false;
            };

            mogwaiKeys.MogwaiKeysState = MogwaiKeysState.CREATE;
            return true;
        }
    }
}
