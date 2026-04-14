namespace DashboardPekus.AppCode.DAO;

public class DAOMySql
{
    /// <summary>
    /// String de conexão MySQL descriptografada.
    /// Atribuída em Program.cs via DAOMySql.StringConexao = ...
    /// </summary>
    public static string StringConexao { get; set; } = string.Empty;

    // ── Stubs dos métodos Pekus — serão implementados quando o banco estiver pronto ──

    /// <summary>Fecha uma DA de forma segura.</summary>
    public static void CloseDao(object? da) { }

    /// <summary>Fecha uma conexão de forma segura.</summary>
    public static void CloseConnection(object? conn) { }

    /// <summary>Executa rollback de forma segura.</summary>
    public static void SecureRollback(object? trans)
    {
        System.Diagnostics.Debug.WriteLine("ATENCAO: SecureRollback chamado.");
    }
}