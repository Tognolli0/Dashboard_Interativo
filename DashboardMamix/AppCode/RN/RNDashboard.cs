// =====================================================
// RNDashboard.cs
// Pasta: AppCode/RN/RNDashboard.cs
// Regras de negócio do Dashboard — padrão Pekus
// =====================================================
using DashboardPekus.AppCode.DAO;
using DashboardPekus.AppCode.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DashboardPekus.AppCode.RN;

/// <summary>
/// Regras de negócio do Dashboard.
/// Herda RNBase seguindo o padrão Pekus.
/// </summary>
public class RNDashboard : RNBase
{
    // ── Construtores ──────────────────────────────────────────────────────

    /// <summary>Construtor sem usuário — para operações públicas do dashboard.</summary>
    public RNDashboard() : base() { }

    /// <summary>Construtor com usuário logado — para operações auditadas.</summary>
    /// <param name="iCdUsuario">Código do usuário logado.</param>
    public RNDashboard(int iCdUsuario) : base(iCdUsuario) { }

    // ── KPIs ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Retorna os cards de KPI do dashboard.
    /// Padrão Pekus: try/finally com CloseDao/CloseConnection.
    /// Enquanto o banco não está ativo, delega diretamente à DA sem conexão.
    /// </summary>
    public async Task<List<DashboardModel>> RetornaStatsDashboard()
    {
        // ── Sem banco: instancia DA direto (dados estáticos) ──────────────
        var da = new DADashboard();
        return await da.RetornaStatsDashboard();

        // ── Com banco (descomentar quando disponível): ────────────────────
        /*
        MySqlConnection? conn = null;
        DADashboard?     da   = null;

        try
        {
            conn = DAOMySql.GetConnection();
            da   = new DADashboard(conn, null);
            return await da.RetornaStatsDashboard();
        }
        finally
        {
            DAOMySql.CloseDao(da);
            DAOMySql.CloseConnection(conn);
        }
        */
    }

    /// <summary>
    /// Retorna os pedidos recentes para a tabela do dashboard.
    /// </summary>
    public async Task<List<PedidoRecenteModel>> RetornaPedidosRecentes()
    {
        var da = new DADashboard();
        return await da.RetornaPedidosRecentes();

        /*
        MySqlConnection? conn = null;
        DADashboard?     da   = null;

        try
        {
            conn = DAOMySql.GetConnection();
            da   = new DADashboard(conn, null);
            return await da.RetornaPedidosRecentes();
        }
        finally
        {
            DAOMySql.CloseDao(da);
            DAOMySql.CloseConnection(conn);
        }
        */
    }

    /// <summary>
    /// Retorna os indicadores do resumo do mês.
    /// </summary>
    public async Task<List<ResumoMesModel>> RetornaResumoMes()
    {
        var da = new DADashboard();
        return await da.RetornaResumoMes();

        /*
        MySqlConnection? conn = null;
        DADashboard?     da   = null;

        try
        {
            conn = DAOMySql.GetConnection();
            da   = new DADashboard(conn, null);
            return await da.RetornaResumoMes();
        }
        finally
        {
            DAOMySql.CloseDao(da);
            DAOMySql.CloseConnection(conn);
        }
        */
    }

    // ── PDF ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Gera o PDF do relatório do dashboard.
    /// </summary>
    /// <param name="sDsUsuario">Nome do usuário para o cabeçalho do relatório.</param>
    public byte[] GerarPdfDashboard(string sDsUsuario)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);

                page.Header()
                    .Text($"Relatório Dashboard - {sDsUsuario}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Item().Text($"Data de emissão: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    col.Item().LineHorizontal(1);
                    col.Spacing(10);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Element(CelulaTabela).Text("Indicador");
                        table.Cell().Element(CelulaTabela).Text("Valor");
                        table.Cell().Element(CelulaTabela).Text("Vendas Totais");
                        table.Cell().Element(CelulaTabela).Text("32.981");
                        table.Cell().Element(CelulaTabela).Text("Receita");
                        table.Cell().Element(CelulaTabela).Text("R$ 1.432.145");
                    });
                });
            });
        }).GeneratePdf();
    }

    // ── Helpers privados ──────────────────────────────────────────────────

    private static IContainer CelulaTabela(IContainer container)
        => container.Border(1).Padding(5).AlignCenter();
}