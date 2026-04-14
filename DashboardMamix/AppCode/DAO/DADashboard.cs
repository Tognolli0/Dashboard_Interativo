using DashboardPekus.AppCode.Model;

namespace DashboardPekus.AppCode.DAO;

/// <summary>
/// Acesso a dados do Dashboard.
/// Retorna dados estáticos enquanto o banco não está disponível.
/// </summary>
public class DADashboard
{
    // ── Construtor futuro (banco ativo) ───────────────────────────────────
    // public DADashboard(IDbConnection cnDados, IDbTransaction? trans) { ... }

    // ── KPIs principais ───────────────────────────────────────────────────

    /// <summary>
    /// Retorna os cards de KPI do dashboard.
    /// TODO: substituir por query real quando o banco estiver disponível.
    /// </summary>
    public Task<List<DashboardModel>> RetornaStatsDashboard()
    {
        var lista = new List<DashboardModel>
        {
            new()
            {
                DsTitulo   = "Vendas Totais",
                DsValor    = "32.981",
                NrVariacao = 4.5,
                FlPositivo = true,
                DsCorClass = "bg-purple",
                DsIcone    = "Cart"
            },
            new()
            {
                DsTitulo   = "Lucro",
                DsValor    = "R$ 645",
                NrVariacao = 0.18,
                FlPositivo = true,
                DsCorClass = "bg-green",
                DsIcone    = "Money"
            },
            new()
            {
                DsTitulo   = "Receita",
                DsValor    = "R$ 1.432.145",
                NrVariacao = 0.29,
                FlPositivo = true,
                DsCorClass = "bg-blue",
                DsIcone    = "Layer"
            },
            new()
            {
                DsTitulo   = "Visualizações",
                DsValor    = "4.678",
                NrVariacao = 0.05,
                FlPositivo = false,
                DsCorClass = "bg-orange",
                DsIcone    = "Eye"
            }
        };

        return Task.FromResult(lista);
    }

    /// <summary>
    /// Retorna os pedidos recentes para a tabela do dashboard.
    /// TODO: substituir por query real quando o banco estiver disponível.
    /// </summary>
    public Task<List<PedidoRecenteModel>> RetornaPedidosRecentes()
    {
        var lista = new List<PedidoRecenteModel>
        {
            new() { NrPedido = "#001", DsCliente = "João Silva",    DsStatus = "Concluído",  DsStatusClass = "status-success", DsValor = "R$ 320,00" },
            new() { NrPedido = "#002", DsCliente = "Maria Souza",   DsStatus = "Pendente",   DsStatusClass = "status-warning", DsValor = "R$ 150,00" },
            new() { NrPedido = "#003", DsCliente = "Carlos Lima",   DsStatus = "Cancelado",  DsStatusClass = "status-danger",  DsValor = "R$ 98,50"  },
            new() { NrPedido = "#004", DsCliente = "Ana Pereira",   DsStatus = "Concluído",  DsStatusClass = "status-success", DsValor = "R$ 540,00" },
            new() { NrPedido = "#005", DsCliente = "Pedro Alves",   DsStatus = "Em Preparo", DsStatusClass = "status-info",    DsValor = "R$ 210,00" }
        };

        return Task.FromResult(lista);
    }

    /// <summary>
    /// Retorna os indicadores do resumo do mês.
    /// TODO: substituir por query real quando o banco estiver disponível.
    /// </summary>
    public Task<List<ResumoMesModel>> RetornaResumoMes()
    {
        var lista = new List<ResumoMesModel>
        {
            new() { DsLabel = "Meta de Vendas",   DsValor = "R$ 32.981",   NrProgresso = 72, Cor = MudBlazor.Color.Primary  },
            new() { DsLabel = "Novos Clientes",   DsValor = "248",          NrProgresso = 58, Cor = MudBlazor.Color.Success  },
            new() { DsLabel = "Ticket Médio",     DsValor = "R$ 132,90",   NrProgresso = 85, Cor = MudBlazor.Color.Warning  },
            new() { DsLabel = "Taxa de Retorno",  DsValor = "64%",          NrProgresso = 64, Cor = MudBlazor.Color.Info     }
        };

        return Task.FromResult(lista);
    }

    // ── Implementação real (para quando o banco estiver pronto) ────────────
    /*
    public Task<List<DashboardModel>> RetornaStatsDashboardReal()
    {
        _cmdDados!.CommandText = @"
            SELECT
                DS_TITULO     AS DsTitulo,
                DS_VALOR      AS DsValor,
                NR_VARIACAO   AS NrVariacao,
                FL_POSITIVO   AS FlPositivo,
                DS_COR_CLASS  AS DsCorClass,
                DS_ICONE      AS DsIcone
            FROM DASH_KPI
            WHERE FL_ATIVO = 1
            ORDER BY NR_ORDEM";

        _cmdDados.Parameters.Clear();
        LoadRows();
        return Task.FromResult(ToObjectList<DashboardModel>());
    }
    */
}