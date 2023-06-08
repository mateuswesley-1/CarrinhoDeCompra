using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.IO;
using Xunit;
using CarrinhoComprasAPI;

namespace CarrinhoDeCompra
{
    public class CarrinhoDeCompraProcessorTest
    {
        [Theory]
        [MemberData(nameof(GetCarrinhoComprasTestCases))]
        public void Should_Return_Valor_Total_Correto(List<Item> itens, decimal valorProduto, decimal pesoTotal, decimal valorFrete, decimal desconto, decimal valorTotal)
        {
            // Arrange
            var carrinho = new CarrinhoCompras();

            // Act
            foreach (var item in itens)
            {
                carrinho.AdicionarItem(item);
            }

            var valorTotalCalculado = carrinho.CalcularValorTotal();
            var valorFreteCalculado = carrinho.CalcularFrete();
            var valorProdutoCalculado = carrinho.CalcularValorProdutos();
            var pesoTotalCalculado = carrinho.calcularPesoTotal();

            // Assert
            Assert.Equal(valorTotal, valorTotalCalculado);

        }

        [Theory]
        [MemberData(nameof(GetCarrinhoComprasTestCases))]
        public void Should_Return_Frete_Correto(List<Item> itens, decimal valorProduto, decimal pesoTotal, decimal valorFrete, decimal desconto, decimal valorTotal)
        {
            // Arrange
            var carrinho = new CarrinhoCompras();

            // Act
            foreach (var item in itens)
            {
                carrinho.AdicionarItem(item);
            }

            var valorFreteCalculado = carrinho.CalcularFrete();

            // Assert
            Assert.Equal(valorFrete, valorFreteCalculado);

        }

        [Theory]
        [MemberData(nameof(GetCarrinhoComprasTestCases))]
        public void Should_Return_Peso_Total_Correto(List<Item> itens, decimal valorProduto, decimal pesoTotal, decimal valorFrete, decimal desconto, decimal valorTotal)
        {
            // Arrange
            var carrinho = new CarrinhoCompras();

            // Act
            foreach (var item in itens)
            {
                carrinho.AdicionarItem(item);
            }

            var pesoTotalCalculado = carrinho.calcularPesoTotal();

            // Assert
            Assert.Equal(pesoTotal, pesoTotalCalculado);

        }

        [Theory]
        [MemberData(nameof(GetCarrinhoComprasTestCases))]
        public void Should_Return_Valor_Produtos_Correto(List<Item> itens, decimal valorProduto, decimal pesoTotal, decimal valorFrete, decimal desconto, decimal valorTotal)
        {
            // Arrange
            var carrinho = new CarrinhoCompras();

            // Act
            foreach (var item in itens)
            {
                carrinho.AdicionarItem(item);
            }

            var valorProdutoCalculado = carrinho.CalcularValorProdutos();


            // Assert
            Assert.Equal(valorProduto, valorProdutoCalculado);

        }
        [Theory]
        [MemberData(nameof(GetCarrinhoComprasTestCases))]
        public void Should_Return_Valor_Desconto_Correto(List<Item> itens, decimal valorProduto, decimal pesoTotal, decimal valorFrete, decimal desconto, decimal valorTotal)
        {
            // Arrange
            var carrinho = new CarrinhoCompras();

            // Act
            foreach (var item in itens)
            {
                carrinho.AdicionarItem(item);
            }

            var valorDescotoCalculado = carrinho.CalcularDesconto();


            // Assert
            Assert.Equal(desconto, valorDescotoCalculado);

        }
        public static IEnumerable<object[]> GetCarrinhoComprasTestCases()
        {
            var testCases = ReadCsvFile("C:/Users/mateu/OneDrive - vrstd/Área de Trabalho/Residencia TCE/Estudos/.NET/CarrinhoDeCompra/carrinho.csv");
            
             return testCases.Select(testCase => new object[] { testCase.Itens, testCase.valorProduto, testCase.pesoTotal, testCase.valorFrete, testCase.desconto, testCase.valorTotal });
        }

        private static List<TestCase> ReadCsvFile(string filePath)
        {
            var testCases = new List<TestCase>();

            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                string line;
                TestCase currentTestCase = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Caso"))
                    {
                        if (currentTestCase != null)
                        {
                            testCases.Add(currentTestCase);
                        }

                        currentTestCase = new TestCase();
                        currentTestCase.Itens = new List<Item>();

                        var values = line.Split(';');
                        currentTestCase.Nome = values[0];
                        currentTestCase.valorProduto = decimal.Parse(values[5]);
                        currentTestCase.pesoTotal = decimal.Parse(values[6]);
                        currentTestCase.valorFrete = decimal.Parse(values[7]);
                        currentTestCase.desconto = decimal.Parse(values[8]);
                        currentTestCase.valorTotal = decimal.Parse(values[9]);
                    }
                    if (currentTestCase != null)
                    {
                        var values = line.Split(';');

                        var nome = values[1];
                        var peso = decimal.Parse(values[2]);
                        var valor = decimal.Parse(values[3]);
                        var tipo = values[4];

                        var item = new Item(nome, peso, valor, tipo);
                        currentTestCase.Itens.Add(item);

                    }
                }
                testCases.Add(currentTestCase);

            }
            return testCases;
        }

        public class TestCase
        {
            public string Nome { get; set; }
            public List<Item> Itens { get; set; }

            public decimal valorProduto { get; set; }
            public decimal pesoTotal { get; set; }
            public decimal valorFrete { get; set; }
            public decimal desconto { get; set; }
            public decimal valorTotal { get; set; }

        }
    }
}
