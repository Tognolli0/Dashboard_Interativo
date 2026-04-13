// =====================================================
// RNBase.cs
// Pasta: AppCode/RN/RNBase.cs
// Classe base de todas as RNs — padrão Pekus
// =====================================================

namespace DashboardPekus.AppCode.RN;

/// <summary>
/// Classe base de todas as Regras de Negócio do DashboardPekus.
/// Segue o mesmo padrão do ePed PDV: guarda o código do usuário
/// e expõe Mensagem para retorno de validações.
/// </summary>
public abstract class RNBase
{
    /// <summary>Código do usuário logado — usado para auditoria e permissões.</summary>
    public int CdUsuario { get; private set; }

    /// <summary>Mensagem de retorno para validações e feedback ao componente.</summary>
    public string Mensagem { get; protected set; } = string.Empty;

    /// <summary>
    /// Construtor padrão para uso sem contexto de usuário
    /// (operações públicas ou chamadas internas).
    /// </summary>
    protected RNBase() { }

    /// <summary>
    /// Construtor com código do usuário logado.
    /// </summary>
    /// <param name="iCdUsuario">Código do usuário — deve ser maior que zero.</param>
    protected RNBase(int iCdUsuario)
    {
        if (iCdUsuario <= 0)
            throw new ArgumentException("CdUsuario deve ser maior que zero.", nameof(iCdUsuario));

        CdUsuario = iCdUsuario;
    }
}