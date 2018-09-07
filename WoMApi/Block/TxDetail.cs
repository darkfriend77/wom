namespace WoMApi
{
    public class TxDetail
    {
        public string Address { get; set; }
        public string Height { get; set; }
        public string Blockhash { get; set; }
        public int Blocktime { get; set; }
        public int Blockindex { get; set; }
        public int Confirmations { get; set; }
        public string Txid { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public int Amount_satoshi { get; set; }
        public double Fee { get; set; }
        public int Fee_satoshi { get; set; }
    }
}
