// =====================================================
// DashboardModel.cs
// Pasta: AppCode/Model/DashboardModel.cs
// =====================================================
namespace DashboardPekus.AppCode.Model;

/// <summary>
/// Modelo dos cards de KPI do dashboard.
/// Fluxo: DADashboard → RNDashboard → Dashboard.razor.cs
/// </summary>
public class DashboardModel
{
    /// <summary>Título exibido no card — ex: "Vendas Totais"</summary>
    public string DsTitulo { get; set; } = string.Empty;

    /// <summary>Valor formatado — ex: "32.981" ou "R$ 645"</summary>
    public string DsValor { get; set; } = string.Empty;

    /// <summary>Percentual de variação em relação ao período anterior</summary>
    public double NrVariacao { get; set; }

    /// <summary>true = crescimento (verde ▲) · false = queda (vermelho ▼)</summary>
    public bool FlPositivo { get; set; }

    /// <summary>Classe CSS do card: bg-purple | bg-green | bg-blue | bg-orange</summary>
    public string DsCorClass { get; set; } = "bg-purple";

    /// <summary>
    /// Chave do ícone — usada pelo GetMudIcon() no code-behind.
    /// Valores aceitos: Cart | Money | Layer | Eye
    /// </summary>
    public string DsIcone { get; set; } = string.Empty;
}