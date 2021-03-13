namespace BlingIntegrationTagplus.Models
{
    class Produto
    {
        public int NumItem { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fornecedor { get; set; }
        public int Qtd { get; set; }
        public float ValorUnitario { get; set; }
        public float ValorDesconto { get; set; }
    }
}
