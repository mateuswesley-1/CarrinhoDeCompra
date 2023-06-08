using System;

namespace CarrinhoComprasAPI
{
    public class Item
    {
        public Item(string nome, decimal peso, decimal valor, string tipo)
        {
            Nome = nome;
            Peso = peso;
            Valor = valor;
            Tipo = tipo;
        }

        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal Peso { get; set; }
        public string Tipo { get; set; }
    }

    public class CarrinhoCompras
    {
        private List<Item> itens;

        public CarrinhoCompras()
        {
            itens = new List<Item>();
        }

        public void AdicionarItem(Item item)
        {
            itens.Add(item);
        }

        public int quantidadeItens()
        {
            return itens.Count;
        }
        public decimal calcularPesoTotal()
        {
            return itens.Sum(item => item.Peso);
        }

        public decimal CalcularFrete()
        {
            decimal pesoTotal = calcularPesoTotal();
            int quantidade = quantidadeItens();
            decimal frete = 0;
            
            if(quantidade >= 6)
            {
                frete += 10;
            }

            if (pesoTotal <= 2)
            {
                frete += 0;
            }
            else if (pesoTotal <= 10)
            {
               frete += (2.00m * pesoTotal);
            }
            else if (pesoTotal <= 50)
            {
                frete += (4.00m * pesoTotal);
            }
            else
            {
                frete += (7.00m * pesoTotal);
            }

            return frete;
        }

        public decimal CalcularDesconto()
        {
            decimal valorProdutos = CalcularValorProdutos();
            decimal frete = CalcularFrete();
            decimal desconto = 0;
            Dictionary<string, int> tiposDic = new Dictionary<string, int>();


            foreach (Item item in itens)
            {
                if (tiposDic.ContainsKey(item.Tipo))
                {
                    tiposDic[item.Tipo] += 1;
                }
                else
                {
                    tiposDic.Add(item.Tipo, 1);
                }
            }

            if(tiposDic.Values.Any(valor => valor > 2))
            {
                desconto += frete * 0.05m;
            }

            if (valorProdutos > 500.00m && valorProdutos <= 1000.00m)
            {
                desconto += valorProdutos * 0.10m;
            }
            else if (valorProdutos > 1000.00m)
            {
                desconto += valorProdutos * 0.20m;
            }

            return desconto;
        }

        public decimal CalcularValorProdutos()
        {
            return itens.Sum(item => item.Valor);
        }

        public decimal CalcularValorTotal()
        {
            decimal valorTotal = CalcularValorProdutos();
            decimal frete = CalcularFrete();
            decimal desconto = CalcularDesconto();

            return valorTotal + frete - desconto;
        }
    }

    public class CarrinhoComprasService
    {
        private CarrinhoCompras carrinho;

        public CarrinhoComprasService()
        {
            carrinho = new CarrinhoCompras();
        }

        public void AdicionarItem(Item item)
        {
            carrinho.AdicionarItem(item);
        }

        public decimal CalcularValorTotal()
        {
            return carrinho.CalcularValorTotal();
        }
    }

    public class CarrinhoComprasController
    {
        private CarrinhoComprasService carrinhoService;

        public CarrinhoComprasController()
        {
            carrinhoService = new CarrinhoComprasService();
        }

        public void AdicionarItem(string nome, decimal valor, decimal peso, string tipo)
        {
            var item = new Item (nome, valor, peso , tipo);
            carrinhoService.AdicionarItem(item);
        }

        public decimal CalcularValorTotal()
        {
            return carrinhoService.CalcularValorTotal();
        }
    }
}

