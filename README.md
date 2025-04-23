# MSTest x xUnit - Abordando testes "Caixa branca" e "Caixa preta"

# Diferenças entre MSTest e xUnit

Este documento tem como objetivo destacar as principais diferenças entre os frameworks de teste **MSTest** e **xUnit**, com foco em desenvolvedores .NET que desejam escolher a melhor ferramenta para seus testes automatizados.

---

## 📌 Visão Geral

| Característica      | MSTest                      | xUnit                        |
|---------------------|-----------------------------|------------------------------|
| Desenvolvedor       | Microsoft                   | Comunidade (original por James Newkirk) |
| Suporte Oficial     | Sim                         | Parcial (suporte pela comunidade) |
| Popularidade        | Médio                       | Alta                         |
| Integração com .NET | Total                       | Total                        |

---

## ⚙️ Sintaxe de Atributos

| Função                     | MSTest                        | xUnit                      |
|----------------------------|-------------------------------|----------------------------|
| Marcar método de teste     | `[TestMethod]`                | `[Fact]`                   |
| Marcar classe de teste     | `[TestClass]`                 | Nenhum atributo necessário |
| Teste com parâmetros       | `[DataTestMethod]` + `[DataRow]` | `[Theory]` + `[InlineData]` |
| Setup/Teardown por teste   | `[TestInitialize]` / `[TestCleanup]` | Construtor / `IDisposable` |
| Setup/Teardown por classe  | `[ClassInitialize]` / `[ClassCleanup]` | `IClassFixture<T>`         |

---

## 🔁 Ciclo de Vida

- **MSTest** usa atributos dedicados para inicialização e limpeza (como `[TestInitialize]`).
- **xUnit** utiliza o **construtor** da classe para setup e `IDisposable.Dispose()` para cleanup.
- xUnit **instancia uma nova classe de teste por método de teste**, o que isola melhor o estado entre testes.

---

## ✅ Assertivas

- Ambos oferecem uma gama de métodos de verificação (`Assert.Equal`, `Assert.IsTrue`, etc.).
- xUnit promove métodos de extensão mais expressivos e usa tipos estáticos (`Assert.Equal(expected, actual)`).
- MSTest possui assertivas tradicionais do estilo NUnit/Visual Studio.

---

## 🧪 Injeção de Dependência

- **xUnit** suporta injeção de dependência nativamente em testes via `IClassFixture`, ideal para testes com serviços ou mocks.
- **MSTest** não possui suporte nativo à injeção de dependência (requer workarounds).

---

## 📦 Pacotes NuGet

| Framework | Pacote Principal             |
|-----------|------------------------------|
| MSTest    | `Microsoft.NET.Test.Sdk` + `MSTest.TestAdapter` + `MSTest.TestFramework` |
| xUnit     | `xunit` + `xunit.runner.visualstudio` + `Microsoft.NET.Test.Sdk`         |

---

## 🧰 Ferramentas de Execução

- Ambos funcionam com `dotnet test`, Visual Studio Test Explorer e CI/CD como GitHub Actions, Azure DevOps etc.
- xUnit tem runner próprio e integração com `xunit.runner.console`.

---

## 📝 Conclusão

| Critério                      | Melhor Opção     |
|------------------------------|------------------|
| Facilidade para iniciantes   | MSTest           |
| Flexibilidade e extensibilidade | xUnit         |
| Suporte à DI                 | xUnit            |
| Projeto legado (VS integrado) | MSTest          |

Ambos os frameworks são poderosos e capazes. A escolha entre MSTest e xUnit depende do estilo do projeto, requisitos de injeção de dependência e preferência da equipe.

---

## 🔗 Referências

- [Documentação do MSTest](https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-with-mstest)
- [Documentação do xUnit](https://xunit.net/)
