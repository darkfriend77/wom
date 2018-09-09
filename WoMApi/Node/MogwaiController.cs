using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WoMApi.Tool;

namespace WoMApi.Node
{
    public class MogwaiController
    {
        private MogwaiWallet Wallet { get; }

        public bool IsWalletUnlocked => Wallet.IsUnlocked;

        public bool IsWalletCreated => Wallet.IsCreated;

        public string DepositAddress => Wallet.IsUnlocked ? Wallet.Deposit.Address : string.Empty;

        public Dictionary<string, MogwaiKeys> MogwaiKeysDict => Wallet.MogwaiKeyDict;

        private int selectedIndex = 0;
        public MogwaiKeys SelectedMogwayKeys
        {
            get
            {
                if (Wallet.MogwaiKeyDict.Count >  selectedIndex)
                {
                    return null;
                }
                return Wallet.MogwaiKeyDict.Values.ToList()[selectedIndex];
            }
        }

        public string WalletMnemonicWords => Wallet.MnemonicWords;

        private Timer timer;

        public MogwaiController()
        {
            Wallet = new MogwaiWallet();
        }

        public void Refresh(int seconds)
        {
            timer = new Timer(seconds * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Wallet.Deposit.Update();
            foreach(var mogwaiKey in Wallet.MogwaiKeyDict.Values)
            {
                mogwaiKey.Update();
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
            Caching.Persist("mogwaikeys.txt", Wallet.MogwaiKeyDict);
        }

        public void NewMogwaiKeys()
        {
            Wallet.GetNewMogwaiKey(out MogwaiKeys mogwaiKeys);
        }

        public void SendMog(MogwaiKeys mogwaiKeys)
        {
            Blockchain.Instance.SendMogs(Wallet.Deposit, mogwaiKeys.Address, 5m, 0.0001m);
        }

        public void BindMogwai(MogwaiKeys mogwaiKeys)
        {
            Blockchain.Instance.BindMogwai(mogwaiKeys);
        }
    }
}
