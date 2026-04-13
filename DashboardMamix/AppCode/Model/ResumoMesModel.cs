// =====================================================
// ResumoMesModel.cs
// Pasta: AppCode/Model/ResumoMesModel.cs
// =====================================================
namespace DashboardPekus.AppCode.Model;

/// <summary>
/// Modelo dos indicadores de resumo mensal do dashboard.
/// Fluxo: DADashboard → RNDashboard → Dashboard.razor.cs
/// </summary>
public class ResumoMesModel
{
    /// <summary>Rótulo do indicador — ex: "Meta de Vendas"</summary>
    public string DsLabel { get; set; } = string.Empty;

    /// <summary>Valor formatado — ex: "R$ 32.981" ou "64%"</summary>
    public string DsValor { get; set; } = string.Empty;

    /// <summary>Percentual de progresso para a barra (0–100)</summary>
    public double NrProgresso { get; set; }

    /// <summary>Cor MudBlazor da barra de progresso</summary>
    public MudBlazor.Color Cor { get; set; }
}