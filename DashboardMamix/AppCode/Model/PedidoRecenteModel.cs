namespace DashboardPekus.AppCode.Model;

/// <summary>
/// Modelo de linha da tabela de pedidos recentes do dashboard.
/// Fluxo: DADashboard → RNDashboard → Dashboard.razor.cs
/// </summary>
public class PedidoRecenteModel
{
    /// <summary>Número/código do pedido — ex: "#001"</summary>
    public string NrPedido { get; set; } = string.Empty;

    /// <summary>Nome do cliente</summary>
    public string DsCliente { get; set; } = string.Empty;

    /// <summary>Descrição do status — ex: "Concluído", "Pendente", "Cancelado"</summary>
    public string DsStatus { get; set; } = string.Empty;

    /// <summary>Classe CSS do badge de status — ex: status-success | status-warning | status-danger</summary>
    public string DsStatusClass { get; set; } = string.Empty;

    /// <summary>Valor formatado — ex: "R$ 320,00"</summary>
    public string DsValor { get; set; } = string.Empty;
}