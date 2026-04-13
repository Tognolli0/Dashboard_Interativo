# DashboardPekus

> **Versão:** 1.0.0 · **Plataforma:** ASP.NET Core · Blazor Server · .NET 10  
> **Empresa:** Pekus Tecnologia

Painel de controle corporativo web baseado no template **Mamix**, reimplementado em **Blazor Server** com **MudBlazor**. Centraliza KPIs, análise de vendas, pedidos e exportação de relatórios PDF.

> **Status atual:** dados estáticos (mock). A integração com MySQL será habilitada quando as tabelas do banco estiverem disponíveis — basta implementar as queries na `DADashboard` e ajustar a `RNDashboard` conforme o padrão Pekus documentado abaixo.

---

## Stack

| Camada | Tecnologia | Versão |
|---|---|---|
| Runtime | .NET | 10.0 |
| Framework | ASP.NET Core Blazor Server | 10.0 |
| Componentes UI | MudBlazor | 9.3.0 |
| Ícones | Microsoft.FluentUI.AspNetCore.Components.Icons | 4.14.0 |
| Gráficos | Chart.js | 4.4.3 (CDN) |
| PDF | QuestPDF | 2026.2.4 |
| Banco | MySQL | *(pendente — dados estáticos por ora)* |
| i18n | Microsoft.Extensions.Localization + `.resx` | — |

---

## Estrutura de Pastas

```
DashboardPekus/
│
├── App.razor                         # Shell HTML — scripts, fontes, CSS
├── Routes.razor                      # Roteador Blazor + página 404
├── _Imports.razor                    # Usings globais de todos os .razor
├── Program.cs                        # Ponto de entrada — DI, pipeline, culturas
├── DashboardPekus.csproj
├── appsettings.json                  # Logging + Configuracoes (conexão futura, log)
├── appsettings.Development.json
│
├── Properties/
│   └── launchSettings.json           # Perfis http :5123 / https :7139
│
├── wwwroot/
│   ├── css/
│   │   └── app.css                   # Tema Mamix (variáveis, topbar, sidebar, cards)
│   ├── js/
│   │   ├── dashboard.js              # Chart.js — setupDashboardCharts / setupRadarChart
│   │   └── download.js               # downloadFileFromStream (download de PDF)
│   └── lib/
│       └── favicon.png
│
├── Resources/
│   ├── Idiomas.resx                  # Strings pt-BR (cultura padrão)
│   ├── Idiomas.en.resx               # Strings en-US
│   └── Idiomas.Designer.cs           # Classe gerada automaticamente — NÃO EDITAR
│
├── Components/
│   │
│   ├── Dashboard/
│   │   ├── Dashboard.razor           # Página /dashboard — markup
│   │   ├── Dashboard.razor.cs        # Code-behind: ciclo de vida, eventos, helpers
│   │   └── DashboardCard.razor       # Componente card KPI reutilizável
│   │
│   ├── Layout/
│   │   ├── MainLayout.razor          # Layout global: Topbar + Sidebar + Body
│   │   ├── MainLayout.razor.css      # CSS escopado do layout
│   │   ├── ReconnectModal.razor      # Modal de reconexão Blazor Server
│   │   ├── ReconnectModal.razor.css  # CSS escopado do modal
│   │   └── ReconnectModal.razor.js   # JS escopado — listeners de reconexão
│   │
│   └── Pages/
│       └── Home.razor                # Página / — boas-vindas, cards resumo, timeline
│
└── AppCode/                          # Toda a lógica da aplicação (padrão Pekus)
    │
    ├── Controllers/
    │   └── CultureController.cs      # Troca de idioma via cookie + redirect
    │
    ├── DAO/
    │   ├── DAOMySql.cs               # Stub — será implementado quando banco estiver pronto
    │   └── DADashboard.cs            # Retorna dados estáticos (mock dos KPIs)
    │
    ├── Model/
    │   ├── DashboardModel.cs         # Card KPI: título, valor, variação, ícone, cor
    │   ├── PedidoRecenteModel.cs     # Linha da tabela de pedidos recentes
    │   └── ResumoMesModel.cs         # Indicador do resumo mensal com progresso
    │
    └── RN/
        ├── RNBase.cs                 # Classe base das RNs — CdUsuario, Mensagem
        └── RNDashboard.cs            # KPIs (RetornaStatsDashboard) + PDF (GerarPdfDashboard)
```

---

## Arquitetura

```
Apresentação (Blazor Components)
        │  chama
Regras de Negócio (RN*)
        │  chama
Acesso a Dados (DA*)          ← dados estáticos por ora
        │  [futuro] executa SQL parametrizado
Banco de Dados (MySQL)        ← pendente
```

### Padrão de Camadas (Pekus)

- **Componentes** nunca acessam DAs diretamente — sempre via RN
- **RNs** herdam de `RNBase` e orquestram DA + regras de negócio
- **DAs** herdam de `DAOMySql` quando o banco estiver ativo

---

## Padrão de Código nas RNs (hoje — dados estáticos)

```csharp
public async Task<List<DashboardModel>> RetornaStatsDashboard()
{
    var da = new DADashboard();
    return await da.GetDashboardStats();
}
```

## Padrão de Código nas RNs (futuro — com banco)

```csharp
public async Task<List<DashboardModel>> RetornaStatsDashboard()
{
    MySqlConnection? conn = null;
    DADashboard?     da   = null;

    try
    {
        conn = DAOMySql.GetConnection();
        da   = new DADashboard(conn, null);
        return await da.GetDashboardStats();
    }
    finally
    {
        DAOMySql.CloseDao(da);
        DAOMySql.CloseConnection(conn);
    }
}
```

Para operações com **múltiplas escritas** (transação):

```csharp
MySqlTransaction? trans = null;
try
{
    conn  = DAOMySql.GetConnection();
    trans = conn.BeginTransaction();
    da    = new DADashboard(conn, trans);
    // ... operações de escrita ...
    trans.Commit();
}
catch
{
    DAOMySql.SecureRollback(trans);
    throw;
}
finally
{
    DAOMySql.CloseDao(da);
    DAOMySql.CloseConnection(conn);
}
```

---

## Mapeamento Banco → Model (futuro)

Use o atributo `[DAOMySqlCampoToModel]` nas propriedades do modelo:

```csharp
public class DashboardModel
{
    [DAOMySqlCampoToModel("DS_TITULO")]
    public string Title { get; set; } = "";

    [DAOMySqlCampoToModel("NR_PERCENTUAL")]
    public double Percentage { get; set; }
}
```

---

## Configuração

### `appsettings.json`

```json
{
  "Configuracoes": {
    "StringConexaoDatabase": "",
    "PathLog": "C:/temp/DashboardPekus/logs",
    "PrefixoArqLog": "DashboardPekus"
  }
}
```

> `StringConexaoDatabase` fica vazio enquanto os dados são estáticos.  
> Quando o banco estiver pronto: `"Server=127.0.0.1;Port=3306;Database=nome;Uid=root;Pwd=senha;"`

### Culturas Suportadas

| Cultura | Arquivo | Padrão |
|---|---|---|
| `pt-BR` | `Resources/Idiomas.resx` | ✅ |
| `en-US` | `Resources/Idiomas.en.resx` | — |

---

## Executar

```bash
# Desenvolvimento
dotnet run --project DashboardPekus/DashboardPekus.csproj

# Publicar
dotnet publish DashboardPekus/DashboardPekus.csproj -c Release -o ./publish
```

Acesso: `http://localhost:5123` · `https://localhost:7139`

---

## Convenções de Nomenclatura (Pekus)

| Prefixo | Camada | Exemplo |
|---|---|---|
| `RN` | Regra de Negócio | `RNDashboard` |
| `DA` | Acesso a Dados | `DADashboard`, `DAOMySql` |
| `_` | Campo privado de instância | `_isDarkMode`, `_vendas` |
| `s` | string local | `_periodoSelecionado` |
| `i` | int local/parâmetro | `iCdUsuario` |

---

## Adicionando um Novo Módulo

1. **Model** → `AppCode/Model/NovoModel.cs` · namespace `DashboardPekus.AppCode.Model`
2. **DA** → `AppCode/DAO/DANovo.cs` · retorna dados estáticos até banco estar pronto
3. **RN** → `AppCode/RN/RNNovo.cs` · herda `RNBase` · chama `DANovo`
4. **Página** → `Components/Pages/Novo/Novo.razor` + `Novo.razor.cs` · `@page "/novo"` · `@rendermode InteractiveServer`
5. **Strings** → adicionar chaves em `Idiomas.resx` e `Idiomas.en.resx`
6. **Menu** → adicionar `MudNavLink` no `MainLayout.razor`

---

## Ativando o Banco MySQL (quando disponível)

1. Instalar pacote NuGet: `MySql.Data`
2. Preencher `StringConexaoDatabase` no `appsettings.json`
3. Implementar `DAOMySql.cs` com `GetConnection()`, `LoadRows()`, `ToObjectList<T>()`
4. Implementar as queries reais na `DADashboard` (ver comentários `TODO` no arquivo)
5. Atualizar `RNDashboard` para o padrão com `try/finally` documentado acima