// =====================================================
// Dashboard.razor.cs
// Pasta: Components/Dashboard/Dashboard.razor.cs
// =====================================================
using DashboardPekus.AppCode.Model;
using DashboardPekus.AppCode.RN;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DashboardPekus.Components.Dashboard;

public partial class Dashboard : ComponentBase
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    // ── Estado ────────────────────────────────────────
    protected List<DashboardModel>? statsList;
    protected string periodoSelecionado = "Semanal";
    protected List<PedidoRecenteModel> pedidosRecentes = new();
    protected List<ResumoMesModel> resumoMes = new();

    // Campos para os dados do gráfico (para podermos atualizar depois)
    private string[] labels = { "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom" };
    private int[] vendas = { 30, 45, 35, 60, 50, 90, 120 };
    private int[] receita = { 55, 40, 60, 35, 65, 45, 80 };
    private int[] lucro = { 40, 60, 45, 70, 55, 75, 60 };

    // ── Ciclo de Vida ─────────────────────────────────
    protected override async Task OnInitializedAsync()
    {
        // Padrão Pekus: página chama RN → RN chama DAO
        var rn = new RNDashboard();
        statsList = await rn.RetornaStatsDashboard();

        CarregarPedidosRecentes();
        CarregarResumoMes();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        // Renderização inicial do gráfico
        await JS.InvokeVoidAsync("setupDashboardCharts",
            "mainDashboardChart", labels, vendas, receita, lucro);

        // Gráfico Radar
        var radarLabels = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
        var eletronicos = new[] { 90, 60, 75, 55, 80, 70, 85 };
        var roupas = new[] { 50, 80, 45, 90, 60, 55, 70 };
        var moveis = new[] { 70, 45, 85, 60, 75, 90, 50 };

        await JS.InvokeVoidAsync("setupRadarChart",
            "radarChart", radarLabels, eletronicos, roupas, moveis);
    }

    protected async Task OnPeriodoChanged(string novoPeriodo)
    {
        periodoSelecionado = novoPeriodo;

        // Simulação de alteração de dados reais vindo da lógica
        if (periodoSelecionado == "Mensal")
        {
            vendas = new[] { 100, 150, 200, 180, 250, 300, 400 };
            receita = new[] { 80, 120, 160, 140, 210, 260, 350 };
        }
        else
        {
            vendas = new[] { 30, 45, 35, 60, 50, 90, 120 };
            receita = new[] { 55, 40, 60, 35, 65, 45, 80 };
        }

        // Chama o JS para atualizar o gráfico existente com os novos valores
        await JS.InvokeVoidAsync("setupDashboardCharts", "mainDashboardChart", labels, vendas, receita, lucro);
    }

    // ── Helpers ───────────────────────────────────────

    /// <summary>
    /// Converte o IconName do DAO (string) em um ícone MudBlazor.
    /// Sem dependência do FluentUI — resolve os erros CS0426/CS0234.
    /// </summary>
    protected Microsoft.FluentUI.AspNetCore.Components.Icon GetFluentIcon(string name) => name switch
    {
        "Cart" => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Cart(),
        "Money" => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Money(),
        "Layer" => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Layer(),
        "Eye" => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Eye(),
        _ => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Info()
    };

    // ── Dados estáticos (substituir por DAO real futuramente) ─────

    private void CarregarPedidosRecentes()
    {
        pedidosRecentes = new()
        {
            new() { NrPedido="#0001", DsCliente="João Silva",   DsStatus="Concluído",    DsStatusClass="status-success", DsValor="R$ 1.250,00" },
            new() { NrPedido="#0002", DsCliente="Maria Souza",  DsStatus="Pendente",     DsStatusClass="status-warning", DsValor="R$ 840,00"   },
            new() { NrPedido="#0003", DsCliente="Carlos Lima",  DsStatus="Concluído",    DsStatusClass="status-success", DsValor="R$ 3.100,00" },
            new() { NrPedido="#0004", DsCliente="Ana Costa",    DsStatus="Cancelado",    DsStatusClass="status-danger",  DsValor="R$ 520,00"   },
            new() { NrPedido="#0005", DsCliente="Pedro Alves",  DsStatus="Em andamento", DsStatusClass="status-info",    DsValor="R$ 2.780,00" },
        };
    }

    private void CarregarResumoMes()
    {
        resumoMes = new()
        {
            new() { DsLabel="Meta de Vendas",  DsValor="78%", NrProgresso=78, Cor=MudBlazor.Color.Primary },
            new() { DsLabel="Clientes Novos",  DsValor="63%", NrProgresso=63, Cor=MudBlazor.Color.Success },
            new() { DsLabel="Ticket Médio",    DsValor="91%", NrProgresso=91, Cor=MudBlazor.Color.Warning },
            new() { DsLabel="Taxa de Retorno", DsValor="45%", NrProgresso=45, Cor=MudBlazor.Color.Info    },
        };
    }
    protected async Task ExportarRelatorio()
    {
        try
        {
            var rn = new RNDashboard();
            var pdfBytes = rn.GerarPdfDashboard("Diogo Tognolli");

            using var stream = new MemoryStream(pdfBytes);
            using var streamRef = new DotNetStreamReference(stream);

            await JS.InvokeVoidAsync("downloadFileFromStream",
                $"Relatorio_{DateTime.Now:yyyyMMdd}.pdf", streamRef);

            Snackbar.Add("PDF gerado com sucesso!", MudBlazor.Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add("Erro ao gerar PDF: " + ex.Message, MudBlazor.Severity.Error);
        }
    }
}

